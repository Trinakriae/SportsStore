using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportsStore.WebUI.ViewModels
{
    public class CategoriesListViewModel
    {
        public IEnumerable<string> Categories { get; set; }

        public string SelectedCategory { get; set; }
    }
}