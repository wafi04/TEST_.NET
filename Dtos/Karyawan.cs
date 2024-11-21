using System.ComponentModel.DataAnnotations;
using CrudApi.Models;

namespace CrudApi.Dtos
{
    public class KaryawanCreateDto
    {
        [Required(ErrorMessage = "NIK is required")]
        [StringLength(20, ErrorMessage = "NIK cannot be longer than 20 characters")]
        public string Nik { get; set; } = string.Empty;

        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Tanggal Lahir is required")]
        public DateTime TanggalLahir { get; set; }

        [Required(ErrorMessage = "Alamat is required")]
        [StringLength(255, ErrorMessage = "Alamat cannot be longer than 255 characters")]
        public string Alamat { get; set; } = string.Empty;

        [Required(ErrorMessage = "Negara is required")]
        [StringLength(50, ErrorMessage = "Negara cannot be longer than 50 characters")]
        public string Negara { get; set; } = string.Empty;

        [Required(ErrorMessage = "Jenis Kelamin is required")]
        [EnumDataType(typeof(JenisKelamin), ErrorMessage = "Invalid Jenis Kelamin")]
        public JenisKelamin JenisKelamin { get; set; }

        [Required(ErrorMessage = "User ID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "User ID must be a positive number")]
        public int UserId { get; set; }
    }

    public class KaryawanUpdateDto
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Tanggal Lahir is required")]
        public DateTime TanggalLahir { get; set; }

        [Required(ErrorMessage = "Alamat is required")]
        [StringLength(255, ErrorMessage = "Alamat cannot be longer than 255 characters")]
        public string Alamat { get; set; } = string.Empty;

        [Required(ErrorMessage = "Negara is required")]
        [StringLength(50, ErrorMessage = "Negara cannot be longer than 50 characters")]
        public string Negara { get; set; } = string.Empty;

        [Required(ErrorMessage = "Jenis Kelamin is required")]
        [EnumDataType(typeof(JenisKelamin), ErrorMessage = "Invalid Jenis Kelamin")]
        public JenisKelamin JenisKelamin { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "User ID must be a positive number")]
        public int UserId { get; set; }
    }

    public class KaryawanDto
    {
        public string Nik { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public DateTime TanggalLahir { get; set; }
        public string Alamat { get; set; } = string.Empty;
        public string Negara { get; set; } = string.Empty;
        public JenisKelamin JenisKelamin { get; set; }

        public List<UserDetailDto>   user  {get;set;}  = new List<UserDetailDto>();
    }

   

    public class KaryawanDetailDto
    {
        public string Nik { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public DateTime TanggalLahir { get; set; }
        public string Alamat { get; set; } = string.Empty;
        public JenisKelamin JenisKelamin { get; set; }
    }
}