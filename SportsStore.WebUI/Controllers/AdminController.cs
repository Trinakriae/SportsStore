using AutoMapper;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using SportsStore.WebUI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SportsStore.WebUI.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private IProductRepository _repository;

        public AdminController(IProductRepository repository)
        {
            _repository = repository;
        }

        // GET: Admin
        public ViewResult Index()
        {
            return View(_repository.Products);
        }

        // GET: Admin/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Admin/Create
        public ActionResult Create()
        {
            return View("Edit", new ProductEditViewModel());
        }

        // POST: Admin/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/Edit/5
        public ViewResult Edit(int productId)
        {
            Product product = _repository.Products.FirstOrDefault(p => p.ProductID == productId);

            ProductEditViewModel viewModel = Mapper.Map<Product, ProductEditViewModel>(product);

            return View(viewModel);
        }

        //// POST: Admin/Edit/5
        [HttpPost]
        public ActionResult Edit(ProductEditViewModel productViewModel, HttpPostedFileBase image)
        {
            if (ModelState.IsValid)
            {
                if(image != null)
                {
                    productViewModel.ImageMimeType = image.ContentType;
                    productViewModel.ImageData = new byte[image.ContentLength];
                    image.InputStream.Read(productViewModel.ImageData, 0, image.ContentLength);
                }

                Product product = Mapper.Map<ProductEditViewModel, Product>(productViewModel);
                _repository.SaveProduct(product);
                TempData["message"] = string.Format("{0} has been saved", product.Name);
                return RedirectToAction("Index");
            }
            else
            {
                //There is something wrong with the data values
                return View(productViewModel);
            }
        }

        // POST: Admin/Delete/5
        [HttpPost]
        public ActionResult Delete(int productId)
        {
            Product product = _repository.Products.FirstOrDefault(p => p.ProductID == productId);
            if(product != null)
            {
                _repository.DeleteProduct(product);
                TempData["message"] = string.Format("{0} was deleted", product.Name);
            }
            return RedirectToAction("Index");
        } 
    }
}
