using SportsStore.WebUI.Infrastructure.Automapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsStore.UnitTests
{
    public static class TestProjectInitialize
    {
        private static bool _automapperConfigured;

        //Method to configure Automapper just once for all the classes
        public static void ConfigureAutoMapper()
        {
            if(!_automapperConfigured)
            {
                AutoMapperWebUIConfiguration.Configure();
                _automapperConfigured = true;
            }
        }
    }
}
