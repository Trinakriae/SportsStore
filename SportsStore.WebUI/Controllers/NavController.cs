using SportsStore.Domain.Abstract;
using SportsStore.WebUI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SportsStore.WebUI.Controllers
{
    public class NavController : Controller
    {
        private IProductRepository _repository;
        
        public NavController(IProductRepository repository)
        {
            _repository = repository;
        }

        // GET: Nav
        public PartialViewResult Menu(string category = null)
        {
            CategoriesListViewModel viewmodel = new CategoriesListViewModel
            {
                Categories = _repository.Products
                                        .Select(x => x.Category)
                                        .Distinct()
                                        .OrderBy(x => x),
                SelectedCategory = category
            }; 

            return PartialView(viewmodel);
        }
    }
}