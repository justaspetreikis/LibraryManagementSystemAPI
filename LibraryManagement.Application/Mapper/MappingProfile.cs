using AutoMapper;
using LibraryManagement.Application.Dto.Requests;
using LibraryManagement.Application.Dto.Response;
using LibraryManagement.Application.Dto.Responses;
using LibraryManagement.Domain.Entities;

namespace LibraryManagement.Application.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            Guid userId = Guid.NewGuid();

            CreateMap<SignUpRequest, User>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => userId));

            CreateMap<AddInformationRequest, Person>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.PersonalCode, opt => opt.MapFrom(src => src.PersonalCode))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));

            CreateMap<AddInformationRequest, Address>()
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City))
                .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Street))
                .ForMember(dest => dest.HouseNumber, opt => opt.MapFrom(src => src.HouseNumber))
                .ForMember(dest => dest.FlatNumber, opt => opt.MapFrom(src => src.FlatNumber));

            CreateMap<ProfileImage, UpdateImageResponse>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.ImageBytes, opt => opt.MapFrom(src => src.ImageBytes))
                .ForMember(dest => dest.ContentType, opt => opt.MapFrom(src => src.ContentType));

            CreateMap<User, UserSignUpResponse>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => userId))
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username));

            CreateMap<User, AddInformationResponse>()
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.Person.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.Person.LastName))
                .ForMember(dest => dest.PersonalCode, opt => opt.MapFrom(src => src.Person.PersonalCode))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.Person.PhoneNumber))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Person.Email))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Person.Address.City))
                .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Person.Address.Street))
                .ForMember(dest => dest.HouseNumber, opt => opt.MapFrom(src => src.Person.Address.HouseNumber))
                .ForMember(dest => dest.FlatNumber, opt => opt.MapFrom(src => src.Person.Address.FlatNumber));

            CreateMap<User, UserInformationResponse>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username))
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role))
                .ForPath(dest => dest.Person.Id, opt => opt.MapFrom(src => src.Person.Id))
                .ForPath(dest => dest.Person.FirstName, opt => opt.MapFrom(src => src.Person.FirstName))
                .ForPath(dest => dest.Person.LastName, opt => opt.MapFrom(src => src.Person.LastName))
                .ForPath(dest => dest.Person.PersonalCode, opt => opt.MapFrom(src => src.Person.PersonalCode))
                .ForPath(dest => dest.Person.PhoneNumber, opt => opt.MapFrom(src => src.Person.PhoneNumber))
                .ForPath(dest => dest.Person.Email, opt => opt.MapFrom(src => src.Person.Email))
                .ForPath(dest => dest.Person.AddressId, opt => opt.MapFrom(src => src.Person.AddressId))
                .ForPath(dest => dest.Person.ProfileImageId, opt => opt.MapFrom(src => src.Person.ProfileImageId))
                .ForPath(dest => dest.Person.Address.Id, opt => opt.MapFrom(src => src.Person.Address.Id))
                .ForPath(dest => dest.Person.Address.City, opt => opt.MapFrom(src => src.Person.Address.City))
                .ForPath(dest => dest.Person.Address.Street, opt => opt.MapFrom(src => src.Person.Address.Street))
                .ForPath(dest => dest.Person.Address.HouseNumber, opt => opt.MapFrom(src => src.Person.Address.HouseNumber))
                .ForPath(dest => dest.Person.Address.FlatNumber, opt => opt.MapFrom(src => src.Person.Address.FlatNumber))
                .ForPath(dest => dest.Person.ProfileImage.Id, opt => opt.MapFrom(src => src.Person.ProfileImage.Id))
                .ForPath(dest => dest.Person.ProfileImage.Name, opt => opt.MapFrom(src => src.Person.ProfileImage.Name))
                .ForPath(dest => dest.Person.ProfileImage.ImageBytes, opt => opt.MapFrom(src => src.Person.ProfileImage.ImageBytes))
                .ForPath(dest => dest.Person.ProfileImage.ContentType, opt => opt.MapFrom(src => src.Person.ProfileImage.ContentType));
        }
    }
}
