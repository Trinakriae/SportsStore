using System;
using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using SportsStore.WebUI.Controllers;
using SportsStore.WebUI.ViewModels;

namespace SportsStore.UnitTests
{
    [TestClass]
    public class CartControllerTest
    {
        Mock<IProductRepository> _mock;

        public CartControllerTest()
        {
            //Arrange
            //-- create Product Mock repository
            _mock = new Mock<IProductRepository>();
            _mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product {ProductID = 1, Name = "P1", Category="Cat1"},
            }.AsQueryable());
        }

        [TestMethod]
        public void Can_Add_To_Cart()
        {
            //Arrange - create Cart
            Cart cart = new Cart();

            //Arrange - create the controller
            CartController target = new CartController(_mock.Object);

            // Act - add a product to the cart
            target.AddToCart(cart, 1, null);

            Assert.AreEqual(cart.Lines.Count(), 1);
            Assert.AreEqual(cart.Lines.ToArray()[0].Product.ProductID, 1);
        }

        [TestMethod]
        public void Adding_Product_To_Cart_Goes_To_Cart_Screen()
        {
            //Arrange - create Cart
            Cart cart = new Cart();

            //Arrange - create the controller
            CartController target = new CartController(_mock.Object);

            // Act - add a product to the cart
            RedirectToRouteResult result = target.AddToCart(cart, 2, "myUrl");

            Assert.AreEqual(result.RouteValues["action"], "Index");
            Assert.AreEqual(result.RouteValues["returnUrl"], "myUrl");
        }

        [TestMethod]
        public void Can_View_Cart_Contents()
        {
            //Arrange - create Cart
            Cart cart = new Cart();

            //Arrange - create the controller
            CartController target = new CartController(null);

            // Act - call the Index action method
            CartIndexViewModel result = (CartIndexViewModel)target.Index(cart, "myUrl").ViewData.Model;

            Assert.AreEqual(result.Cart, cart);
            Assert.AreEqual(result.ReturnUrl, "myUrl");
        }
    }
}
