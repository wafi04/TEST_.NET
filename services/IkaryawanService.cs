using CrudApi.Models;
using CrudApi.Data;
using Microsoft.EntityFrameworkCore;


namespace CrudApi.Services;

public interface IKaryawanService
{
    Task<IEnumerable<Karyawan>> GetAllKaryawanAsync();  // Nama method di interface
    Task<Karyawan?> GetKaryawanByNikAsync(string nik);
    Task<Karyawan> CreateKaryawanAsync(Karyawan karyawan);
    Task<Karyawan?> UpdateKaryawanAsync(string nik, Karyawan karyawan);
    Task<bool> DeleteKaryawanAsync(string nik);
}

public class KaryawanService : IKaryawanService
{
    private readonly ApplicationDbContext _context;

    public KaryawanService(ApplicationDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

   public async Task<IEnumerable<Karyawan>> GetAllKaryawanAsync()
    {
        return await _context.Karyawan
            .Include(k => k.User) 
            .OrderBy(k => k.Name)
            .ToListAsync();
    }

   public async Task<Karyawan?> GetKaryawanByNikAsync(string nik)
    {
        return await _context.Karyawan
            .Include(k => k.User)
            .FirstOrDefaultAsync(k => k.Nik == nik);
    }

    public async Task<Karyawan> CreateKaryawanAsync(Karyawan karyawan)
    {
        var user = await _context.Users.FindAsync(karyawan.UserId);
        if (user == null)
        {
            throw new InvalidOperationException($"User with ID {karyawan.UserId} not found");
        }

        var existingKaryawan = await _context.Karyawan
            .FirstOrDefaultAsync(k => k.Nik == karyawan.Nik);
            
        if (existingKaryawan != null)
        {
            throw new InvalidOperationException($"Karyawan dengan NIK {karyawan.Nik} sudah ada");
        }

        _context.Karyawan.Add(karyawan);
        await _context.SaveChangesAsync();
        return karyawan;
    }

    public async Task<Karyawan?> UpdateKaryawanAsync(string nik, Karyawan karyawan)
    {
        var existingKaryawan = await _context.Karyawan
            .FirstOrDefaultAsync(k => k.Nik == nik);

        if (existingKaryawan == null)
        {
            return null;
        }

        // Update properti
        existingKaryawan.Name = karyawan.Name;
        existingKaryawan.TanggalLahir = karyawan.TanggalLahir;
        existingKaryawan.Alamat = karyawan.Alamat;
        existingKaryawan.Negara = karyawan.Negara;
        existingKaryawan.JenisKelamin = karyawan.JenisKelamin;

        await _context.SaveChangesAsync();
        return existingKaryawan;
    }

    public async Task<bool> DeleteKaryawanAsync(string nik)
    {
        var karyawan = await _context.Karyawan
            .FirstOrDefaultAsync(k => k.Nik == nik);

        if (karyawan == null)
        {
            return false;
        }

        _context.Karyawan.Remove(karyawan);
        await _context.SaveChangesAsync();
        return true;
    }
}