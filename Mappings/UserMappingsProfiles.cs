// File: Mappings/UserMappingProfile.cs
using AutoMapper;
using CrudApi.Models;
using  CrudApi.Dtos;
// using CrudApi.Dtos; // Pastikan Anda punya folder Dtos
namespace CrudApi.Mappings
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            // Mapping dari User ke berbagai DTO
            CreateMap<User, UserResponseDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));

            CreateMap<User, UserDetailDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));


            // Mapping dari CreateUserDto ke User
            CreateMap<CreateUserDto, User>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Username))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore()); // Password hash akan dihandle di service
            CreateMap<Karyawan, KaryawanDto>()
            .ForMember(dest => dest.user, opt => opt.MapFrom(src => new List<UserDetailDto>())); // Adjust as needed

        // Create a map from User to UserDetailDto
        CreateMap<User, UserDetailDto>()
            .ForMember(dest => dest.Karyawan, opt => opt.MapFrom(src => new List<KaryawanDto>())); // Adjust as needed
        }
    }

    // Mapping untuk Karyawan
    public class KaryawanMappingProfile : Profile
    {
        public class KaryawanResponseDto
    {
        public string? Nik { get; set; }
        public string? Name { get; set; }
        public string? JenisKelamin { get; set; }
    }

        public KaryawanMappingProfile()
        {
            CreateMap<Karyawan, KaryawanResponseDto>()
                .ForMember(dest => dest.Nik, opt => opt.MapFrom(src => src.Nik))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.JenisKelamin, opt => opt.MapFrom(src => src.JenisKelamin));

            CreateMap<KaryawanCreateDto, Karyawan>()
                .ForMember(dest => dest.Nik, opt => opt.MapFrom(src => src.Nik))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.JenisKelamin, opt => opt.MapFrom(src => src.JenisKelamin));
            CreateMap<Karyawan, KaryawanDto>()
            .ForMember(dest => dest.user, opt => opt.MapFrom(src => new List<UserDetailDto>())); // Adjust as needed

            CreateMap<User, UserDetailDto>()
                .ForMember(dest => dest.Karyawan, opt => opt.MapFrom(src => new List<KaryawanDto>())); // Adjust as needed
            }

    }
}