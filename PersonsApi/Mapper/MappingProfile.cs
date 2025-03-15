using AutoMapper;
using PersonsApi.Dtos;
using PersonsApi.DTOs;
using PersonsApi.Models;

namespace PersonsApi.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Person, PersonDTO>().ReverseMap();

            CreateMap<PhoneNumber, PhoneNumberDTO>().ReverseMap();

            CreateMap<RelatedIndividual, RelatedIndividualDTO>().ReverseMap();

            CreateMap<Person, PersonResponseDto>();
            CreateMap<PersonRequestDTO, Person>()
            .ForMember(dest => dest.RelatedIndividuals, opt => opt.Ignore())
            .ForMember(dest => dest.PhoneNumbers, opt => opt.MapFrom(src => src.PhoneNumbers));

        }
    }
}
