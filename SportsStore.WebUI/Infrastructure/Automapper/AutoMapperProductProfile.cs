using AutoMapper;
using SportsStore.Domain.Entities;
using SportsStore.WebUI.ViewModels;

namespace SportsStore.WebUI.Infrastructure.Automapper
{
    public class AutoMapperProductProfile : Profile
    {
        public AutoMapperProductProfile()
        {
            CreateMap<Product, ProductEditViewModel>();
            CreateMap<Product, ProductDisplayViewModel>();
        }
    }
}