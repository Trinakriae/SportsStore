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
        Mock<IProductRepository> _mockProductRepository;
        Mock<IOrderProcessor> _mockOrderProcessor;
        Cart _cart;

        public CartControllerTest()
        {
            //Arrange
            //-- create Product Mock repository
            _mockProductRepository = new Mock<IProductRepository>();
            _mockProductRepository.Setup(m => m.Products).Returns(new Product[]
            {
                new Product {ProductID = 1, Name = "P1", Category="Cat1"},
            }.AsQueryable());

            // Arrange create an order processor
            _mockOrderProcessor = new Mock<IOrderProcessor>();

            //Arrange - create Cart
            _cart = new Cart();
        }

        [TestMethod]
        public void Can_Add_To_Cart()
        {
            //Arrange - create the controller
            CartController target = new CartController(_mockProductRepository.Object, null);

            // Act - add a product to the cart
            target.AddToCart(_cart, 1, null);

            Assert.AreEqual(_cart.Lines.Count(), 1);
            Assert.AreEqual(_cart.Lines.ToArray()[0].Product.ProductID, 1);
        }

        [TestMethod]
        public void Adding_Product_To_Cart_Goes_To_Cart_Screen()
        {
            //Arrange - create the controller
            CartController target = new CartController(_mockProductRepository.Object, null);

            // Act - add a product to the cart
            RedirectToRouteResult result = target.AddToCart(_cart, 2, "myUrl");

            Assert.AreEqual(result.RouteValues["action"], "Index");
            Assert.AreEqual(result.RouteValues["returnUrl"], "myUrl");
        }

        [TestMethod]
        public void Can_View_Cart_Contents()
        { 
            //Arrange - create the controller
            CartController target = new CartController(null, null);

            // Act - call the Index action method
            CartIndexViewModel result = (CartIndexViewModel)target.Index(_cart, "myUrl").ViewData.Model;

            Assert.AreEqual(result.Cart, _cart);
            Assert.AreEqual(result.ReturnUrl, "myUrl");
        }

        [TestMethod]
        public void Can_Checkout_Empty_Cart()
        {
            //Arrange - create shipping details
            ShippingDetails shippingDetails = new ShippingDetails();

            //Arrange - create the controller
            CartController target = new CartController(null, _mockOrderProcessor.Object);

            // Act
            ViewResult result = target.Checkout(_cart, shippingDetails);

            //Assert check that the order hasn't been passed on to the processor - it does not matter the actual value of Cart or ShippingDetails passed
            _mockOrderProcessor.Verify(m => m.ProcessOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()), Times.Never());
           
            //Assert - check that the method is returning the default view
            Assert.AreEqual("", result.ViewName);

            //Assert - check that we are passing an invalid model to the view
            Assert.AreEqual(false, result.ViewData.ModelState.IsValid);
        }

        [TestMethod]
        public void Cannot_Checkout_Invalid_ShippingDetails()
        {
            _cart.AddItem(new Product(), 1);

            //Arrange - create the controller
            CartController target = new CartController(null, _mockOrderProcessor.Object);

            //Arrange - add an error to the model
            target.ModelState.AddModelError("error", "error");

            //Act - try to checkout
            ViewResult result = target.Checkout(_cart, new ShippingDetails());

            //Assert check that the order hasn't been passed on to the processor - it does not matter the actual value of Cart or ShippingDetails passed
            _mockOrderProcessor.Verify(m => m.ProcessOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()), Times.Never());

            //Assert - check that the method is returning the default view
            Assert.AreEqual("", result.ViewName);

            //Assert - check that we are passing an invalid model to the view
            Assert.AreEqual(false, result.ViewData.ModelState.IsValid);
        }

        [TestMethod]
        public void Can_Checkout_And_Submit_Order()
        {
            _cart.AddItem(new Product(), 1);

            //Arrange - create the controller
            CartController target = new CartController(null, _mockOrderProcessor.Object);

            //Act - try to checkout
            ViewResult result = target.Checkout(_cart, new ShippingDetails());

            //Assert check that the order has been passed on to the processor
            _mockOrderProcessor.Verify(m => m.ProcessOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()), Times.Once());

            //Assert - check that the method is returning the default view
            Assert.AreEqual("Completed", result.ViewName);

            //Assert - check that we are passing an invalid model to the view
            Assert.AreEqual(true, result.ViewData.ModelState.IsValid);
        }
    }
}
