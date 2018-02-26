using AutoMapper;

namespace SportsStore.WebUI.Infrastructure.Automapper
{
    public static class AutoMapperWebConfiguration
    {
        public static void Configure()
        {
            ConfigureProductMapping();
        }

        private static void ConfigureProductMapping()
        {
            Mapper.Initialize(cfg => cfg.AddProfile<AutoMapperProductProfile>());
        }
    }
}