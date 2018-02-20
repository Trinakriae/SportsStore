using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;

namespace SportsStore.UnitTests
{
    [TestClass()]
    public class CartTest
    {
        private Mock<IProductRepository> _mock;

        public CartTest()
        {
            //Arrange
            //-- create Product Mock repository
            _mock = new Mock<IProductRepository>();
            _mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product {ProductID = 1, Name = "P1", Category="Cat1", Price=100M},
                new Product {ProductID = 2, Name = "P2", Category="Cat2", Price=50M},
                new Product {ProductID = 3, Name = "P3", Category="Cat1"},
                new Product {ProductID = 4, Name = "P4", Category="Cat2"},
                new Product {ProductID = 5, Name = "P5", Category="Cat3"},
            }.AsQueryable());
        }

        [TestMethod]
        public void Can_Add_New_Lines()
        {
            // Arrange - get the products in array
            Product[] products = _mock.Object.Products.ToArray();

            // Arrange - create a new cart
            Cart target = new Cart();

            // Act
            target.AddItem(products[0], 1);
            target.AddItem(products[1], 1);
            CartLine[] results = target.Lines.ToArray();

            // Assert
            Assert.AreEqual(results.Length, 2);
            Assert.AreEqual(results[0].Product, products[0]);
            Assert.AreEqual(results[1].Product, products[1]);
        }

        [TestMethod]
        public void Can_Add_Existing_Lines()
        {
            // Arrange - get the products in array
            Product[] products = _mock.Object.Products.ToArray();

            // Arrange - create a new cart
            Cart target = new Cart();

            // Act
            target.AddItem(products[0], 1);
            target.AddItem(products[1], 1);
            target.AddItem(products[0], 10);
            CartLine[] results = target.Lines.OrderBy(c => c.Product.ProductID).ToArray();

            // Assert
            Assert.AreEqual(results.Length, 2);
            Assert.AreEqual(results[0].Quantity, 11);
            Assert.AreEqual(results[1].Quantity, 1);
        }

        [TestMethod]
        public void Can_Remove_Line()
        {
            // Arrange - get the products in array
            Product[] products = _mock.Object.Products.ToArray();

            // Arrange - create a new cart
            Cart target = new Cart();
            // Arrange - add some products to the cart
            target.AddItem(products[0], 1);
            target.AddItem(products[1], 3);
            target.AddItem(products[2], 5);
            target.AddItem(products[1], 1);

            // Act
            target.RemoveLine(products[1]);

            // Assert
            Assert.AreEqual(target.Lines.Where(c => c.Product == products[1]).Count(), 0);
            Assert.AreEqual(target.Lines.Count(), 2);
        }


        [TestMethod]
        public void Calculator_Cart_Total()
        {
            // Arrange - get the products in array
            Product[] products = _mock.Object.Products.ToArray();

            // Arrange - create a new cart
            Cart target = new Cart();

            // Act
            target.AddItem(products[0], 1);
            target.AddItem(products[1], 1);
            target.AddItem(products[0], 3);
            decimal result = target.ComputeTotalValue();

            // Assert
            Assert.AreEqual(result, 450M);
        }

        [TestMethod]
        public void Can_Clear_Contents()
        {
            // Arrange - get the products in array
            Product[] products = _mock.Object.Products.ToArray();

            // Arrange - create a new cart
            Cart target = new Cart();

            // Arrange - add some items
            target.AddItem(products[0], 1);
            target.AddItem(products[1], 1);

            // Act - reset the cart
            target.Clear();

            // Assert
            Assert.AreEqual(target.Lines.Count(), 0);
        }
    }
}
