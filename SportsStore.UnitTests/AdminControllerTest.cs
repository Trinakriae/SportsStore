using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using SportsStore.WebUI.Controllers;
using SportsStore.WebUI.Infrastructure.Automapper;
using SportsStore.WebUI.ViewModels;

namespace SportsStore.UnitTests
{
    [TestClass]
    public class AdminControllerTest
    {
        Mock<IProductRepository> _mock;

        public AdminControllerTest()
        {
            //Arrange
            //-- create Product Mock repository
            _mock = new Mock<IProductRepository>();
            _mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product {ProductID = 1, Name = "P1", Category="Cat1"},
                new Product {ProductID = 2, Name = "P2", Category="Cat2"},
                new Product {ProductID = 3, Name = "P3", Category="Cat1"},
                new Product {ProductID = 4, Name = "P4", Category="Cat2"},
                new Product {ProductID = 5, Name = "P5", Category="Cat3"},
            }.AsQueryable());
        }

        [ClassInitialize]
        public static void Initialize(TestContext context)
        {
            AutoMapperWebConfiguration.Configure();
        }

        [TestMethod]
        public void Index_Contains_All_Products()
        {
            //Arrange - create a controller
            AdminController target = new AdminController(_mock.Object);

            //Action
            Product[] result = ((IEnumerable<Product>)target.Index().ViewData.Model).ToArray();

            //Assert
            Assert.IsTrue(result.Length == 5);
            Assert.AreEqual(result[0].Name, "P1");
            Assert.AreEqual(result[1].Name, "P2");
            Assert.AreEqual(result[2].Name, "P3");
            Assert.AreEqual(result[3].Name, "P4");
            Assert.AreEqual(result[4].Name, "P5");
        }

        [TestMethod]
        public void Can_Edit_Product()
        {
            // Arrange - create the controller
            AdminController target = new AdminController(_mock.Object);
            
            // Act
            ProductEditViewModel p1 = target.Edit(1).ViewData.Model as ProductEditViewModel;
            ProductEditViewModel p2 = target.Edit(2).ViewData.Model as ProductEditViewModel;
            ProductEditViewModel p3 = target.Edit(3).ViewData.Model as ProductEditViewModel;

            // Assert
            Assert.AreEqual(1, p1.ProductID);
            Assert.AreEqual(2, p2.ProductID);
            Assert.AreEqual(3, p3.ProductID);
        }

        [TestMethod]
        public void Cannot_Edit_Nonexistent_Product()
        {
            // Arrange - create the controller
            AdminController target = new AdminController(_mock.Object);

            // Act
            ProductEditViewModel result = target.Edit(7).ViewData.Model as ProductEditViewModel;

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void Can_Save_Valid_Changes()
        {   
            // Arrange - create the controller
            AdminController target = new AdminController(_mock.Object);
            
            // Arrange - create a product
            ProductEditViewModel productViewModel = new ProductEditViewModel { Name = "Test" };

            // Act - try to save the product
            ActionResult result = target.Edit(productViewModel, null);
            
            // Assert - check that the repository was called
            _mock.Verify(m => m.SaveProduct(It.IsAny<Product>()));
            // Assert - check the method result type
            Assert.IsNotInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Cannot_Save_Invalid_Changes()
        {
            // Arrange - create the controller
            AdminController target = new AdminController(_mock.Object);
            // Arrange - create a product
            ProductEditViewModel productViewModel = new ProductEditViewModel { Name = "Test" };
            // Arrange - add an error to the model state
            target.ModelState.AddModelError("error", "error");

            // Act - try to save the product
            ActionResult result = target.Edit(productViewModel, null);

            // Assert - check that the repository was not called
            _mock.Verify(m => m.SaveProduct(It.IsAny<Product>()), Times.Never());
            // Assert - check the method result type
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }


        [TestMethod]
        public void Can_Delete_Valid_Products()
        {
            // Arrange - create the controller
            AdminController target = new AdminController(_mock.Object);
            Product product = _mock.Object.Products.FirstOrDefault(p => p.ProductID == 2);

            // Act - delete the product
            target.Delete(2);

            // Assert - ensure that the repository delete method was 
            // called with the correct Product 
            _mock.Verify(m => m.DeleteProduct(product));
        }

        [TestMethod]
        public void Cannot_Delete_Invalid_Products()
        {
            // Arrange - create the controller
            AdminController target = new AdminController(_mock.Object);

            // Act - delete using an ID that doesn't exist
            target.Delete(100);

            // Assert - ensure that the repository delete method was 
            // called with the correct Product 
            _mock.Verify(m => m.DeleteProduct(It.IsAny<Product>()), Times.Never());
        }
    }
}
