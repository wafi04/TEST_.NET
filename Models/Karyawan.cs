using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CrudApi.Models;

public enum JenisKelamin
{
    LakiLaki,
    Perempuan
}

public class Karyawan
{
    [Key]
    [MaxLength(20)]
    public string Nik { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [Column(TypeName = "date")]
    public DateTime TanggalLahir { get; set; }

    [Required]
    [MaxLength(255)]
    public string Alamat { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string Negara { get; set; } = string.Empty;

    [Required]
    public JenisKelamin JenisKelamin { get; set; }

    // Foreign key untuk User
    [Required]
    public int UserId { get; set; }

    // Navigation property ke User
    [ForeignKey("UserId")]
    public virtual User User { get; set; } = null!;
}