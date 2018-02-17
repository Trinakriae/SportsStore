﻿using SportsStore.Domain.Abstract;
using SportsStore.WebUI.ViewModels;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SportsStore.WebUI.Controllers
{
    public class ProductController : Controller
    {
        public int _pageSize = 4; // We will change this later
        private IProductRepository _repository;

        public ProductController(IProductRepository productRepository)
        {
            _repository = productRepository;
        }

        //Returns a View with the list of Products
        public ViewResult List(int page = 1)
        {
            ProductsListViewModel viewModel = new ProductsListViewModel
            {
                Products = _repository.Products.OrderBy(p => p.ProductID)
                                               .Skip((page - 1) * _pageSize)
                                               .Take(_pageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = _pageSize,
                    TotalItems = _repository.Products.Count()
                }
            };

            return View(viewModel);
        }
    }
}