// File: Dtos/User/UserResponseDto.cs
namespace CrudApi.Dtos
{
    public class UserResponseDto
    {
        public int Id { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
    }

     public class UserDetailDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public List<KaryawanDetailDto> Karyawan { get; set; } = new List<KaryawanDetailDto>();
    }

    public class CreateUserDto
    {
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
}

// File: Dtos/Karyawan/KaryawanDto.cs
namespace CrudApi.Dtos.Karyawan
{
    public class KaryawanResponseDto
    {
        public string? Nik { get; set; }
        public string? Name { get; set; }
        public string? JenisKelamin { get; set; }
    }

    public class CreateKaryawanDto
    {
        public string?Nik { get; set; }
        public string? Name { get; set; }
        public string? JenisKelamin { get; set; }
    }
}