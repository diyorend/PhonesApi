using AutoMapper;
using PhoneApi.Dto;
using PhoneApi.Models;

namespace PhoneApi.Helper
{
    public class MappingProfiles: Profile
    {
        public MappingProfiles()
        {
            CreateMap<Phone, PhoneDto>();
            CreateMap<Category, CategoryDto>();
            CreateMap<Company, CompanyDto>();
            CreateMap<Model, ModelDto>();
            CreateMap<Review, ReviewDto>();
            CreateMap<Reviewer, ReviewerDto>();
            CreateMap<CategoryDto, Category>();
            CreateMap<CompanyDto, Company>();
            CreateMap<ModelDto, Model>();
            CreateMap<ReviewDto, Review>();
            CreateMap<ReviewerDto, Reviewer>();
            CreateMap<PhoneDto, Phone>();
        }
    }
}
