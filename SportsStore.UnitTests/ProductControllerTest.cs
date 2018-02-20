using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using SportsStore.WebUI.Controllers;
using SportsStore.WebUI.HtmlHelpers;
using SportsStore.WebUI.ViewModels;

namespace SportsStore.UnitTests
{
    [TestClass]
    public class ProductControllerTest
    {
        private Mock<IProductRepository> _mock;

        public ProductControllerTest()
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
        public void Can_Paginate()
        {
            //Arrange

            ProductController controller = new ProductController(_mock.Object);
            controller._pageSize = 3;

            //Action
            ProductsListViewModel result = (ProductsListViewModel)controller.List(null, 2).Model;

            //Assert
            Product[] prodArray = result.Products.ToArray();
            Assert.IsTrue(prodArray.Length == 2);
            Assert.AreEqual(prodArray[0].Name, "P4");
            Assert.AreEqual(prodArray[1].Name, "P5");
        }

        [TestMethod]
        public void Can_Generate_Page_Links()
        {
            HtmlHelper myHelper = null;

            PagingInfo paginInfo = new PagingInfo
            {
                CurrentPage = 2,
                TotalItems = 28,
                ItemsPerPage = 10
            };

            Func<int, string> pageUrlDelegate = i => "Page" + i;

            // Act
            MvcHtmlString result = myHelper.PageLinks(paginInfo, pageUrlDelegate);

            // Assert
            Assert.AreEqual(result.ToString(), @"<a href=""Page1"">1</a><a class=""selected"" href=""Page2"">2</a><a href=""Page3"">3</a>");
;        }

        [TestMethod]
        public void Can_Send_Pagination_View_Model()
        {
            //Arrange
            ProductController controller = new ProductController(_mock.Object);
            controller._pageSize = 3;

            //Action
            ProductsListViewModel result = (ProductsListViewModel)controller.List(null, 2).Model;

            //Assert
            PagingInfo pageInfo = result.PagingInfo;
            Assert.AreEqual(pageInfo.CurrentPage, 2);
            Assert.AreEqual(pageInfo.ItemsPerPage, 3);
            Assert.AreEqual(pageInfo.TotalItems, 5);
            Assert.AreEqual(pageInfo.TotalPages, 2);
        }

        [TestMethod]
        public void Can_Filter_Products()
        {
            //Arrange 

            ProductController controller = new ProductController(_mock.Object);
            controller._pageSize = 3;

            //Action
            ProductsListViewModel result = (ProductsListViewModel)controller.List("Cat2", 1).Model;

            //Assert
            Product[] prodArray = result.Products.ToArray();
            Assert.IsTrue(prodArray.Length == 2);
            Assert.IsTrue(prodArray[0].Name == "P2" && prodArray[0].Category == "Cat2");
            Assert.IsTrue(prodArray[1].Name == "P4" && prodArray[1].Category == "Cat2");
        }

        [TestMethod]
        public void Generate_Category_Specific_Product_Count()
        {
            //Arrange 

            ProductController controller = new ProductController(_mock.Object);
            controller._pageSize = 3;

            //Action
            int result1 = ((ProductsListViewModel)controller.List("Cat1").Model).PagingInfo.TotalItems;
            int result2 = ((ProductsListViewModel)controller.List("Cat2").Model).PagingInfo.TotalItems;
            int result3 = ((ProductsListViewModel)controller.List("Cat3").Model).PagingInfo.TotalItems;
            int resultAll = ((ProductsListViewModel)controller.List(null).Model).PagingInfo.TotalItems;

            //Assert
            Assert.AreEqual(result1, 2);
            Assert.AreEqual(result2, 2);
            Assert.AreEqual(result3, 1);
            Assert.AreEqual(resultAll, 5);
        }
    }
}
