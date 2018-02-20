using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using SportsStore.WebUI.Controllers;
using SportsStore.WebUI.ViewModels;

namespace SportsStore.UnitTests
{
    [TestClass]
    public class NavControllerTest
    {
        private Mock<IProductRepository> _mock;

        public NavControllerTest()
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


        [TestMethod]
        public void Can_Create_Categories()
        {
            //Arrange - Create the controller
            NavController target = new NavController(_mock.Object);

            string[] results = ((CategoriesListViewModel)target.Menu().Model).Categories.ToArray();

            Assert.AreEqual(results.Length, 3);
            Assert.AreEqual(results[0], "Cat1");
            Assert.AreEqual(results[1], "Cat2");
            Assert.AreEqual(results[2], "Cat3");
        }

        [TestMethod]
        public void Indicates_Selected_Category()
        {
            //Arrange - Create the controller
            NavController target = new NavController(_mock.Object);

            //Arrange - Define the category to select
            string categoryToSelect = "Cat1";

            string result = ((CategoriesListViewModel)target.Menu(categoryToSelect).Model).SelectedCategory;

            Assert.AreEqual(categoryToSelect, result);
        }
    }
}
