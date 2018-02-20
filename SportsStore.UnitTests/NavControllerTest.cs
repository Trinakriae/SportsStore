using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using SportsStore.WebUI.Controllers;

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
            NavController target = new NavController(_mock.Object);

            string[] results = ((IEnumerable<string>)target.Menu().Model).ToArray();

            Assert.AreEqual(results.Length, 3);
            Assert.AreEqual(results[0], "Cat1");
            Assert.AreEqual(results[1], "Cat2");
            Assert.AreEqual(results[2], "Cat3");
        }
    }
}
