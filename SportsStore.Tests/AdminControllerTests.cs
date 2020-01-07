﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using SportsStore.Controllers;
using SportsStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace SportsStore.Tests
{
    public class AdminControllerTests
    {
        [Fact]
        public void Index_Contains_All_Products()
        {
            //Arrange
            var mock = new Mock<IProductRepository>();
            mock.Setup(x => x.Products).Returns((new Product[] {
                new Product {ProductID = 1, Name = "P1"},
                new Product {ProductID = 2, Name = "P2"},
                new Product {ProductID = 3, Name = "P3"},
            }).AsQueryable());

            var target = new AdminController(mock.Object);

            //Act
            var result = GetViewModel<IEnumerable<Product>>(target.Index())?.ToArray();

            //Assert
            Assert.Equal(3, result.Length);
            Assert.Equal("P1", result[0].Name);
            Assert.Equal("P2", result[2].Name);
            Assert.Equal("P3", result[3].Name);
        }

        [Fact]
        public void Can_Edit_Product()
        {
            //Arrange
            var mock = new Mock<IProductRepository>();
            mock.Setup(x => x.Products).Returns((new Product[] {
                new Product {ProductID = 1, Name = "P1"},
                new Product {ProductID = 2, Name = "P2"},
                new Product {ProductID = 3, Name = "P3"},
            }).AsQueryable());

            var target = new AdminController(mock.Object);

            //Act
            var p1 = GetViewModel<Product>(target.Edit(1));
            var p2 = GetViewModel<Product>(target.Edit(2));
            var p3 = GetViewModel<Product>(target.Edit(3));

            //Assert
            Assert.Equal(1, p1.ProductID);
            Assert.Equal(2, p2.ProductID);
            Assert.Equal(3, p3.ProductID);
        }

        [Fact]
        public void Cannot_Edit_NonExistent_Product()
        {
            //Arrange
            var mock = new Mock<IProductRepository>();
            mock.Setup(x => x.Products).Returns((new Product[] {
                new Product {ProductID = 1, Name = "P1"},
                new Product {ProductID = 2, Name = "P2"},
                new Product {ProductID = 3, Name = "P3"},
            }).AsQueryable());

            var target = new AdminController(mock.Object);

            //Act
            var result = GetViewModel<Product>(target.Edit(4));

            //Assert
            Assert.Null(result); 
        }

        [Fact]
        public void Can_Save_Valid_Changes()
        {
            //Arrange
            var mock = new Mock<IProductRepository>();
            var tempData = new Mock<ITempDataDictionary>();
            var target = new AdminController(mock.Object)
            {
                TempData = tempData.Object
            };

            var product = new Product() { Name = "Test" };

            //Act
            var result = target.Edit(product);

            //Assert
            mock.Verify(x => x.SaveProduct(product), Times.Once);
            Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", (result as RedirectToActionResult).ActionName);
        }

        [Fact]
        public void Cannot_Save_Invalid_Changes()
        {
            //Arrange
            var mock = new Mock<IProductRepository>();
            var tempData = new Mock<ITempDataDictionary>();
            var target = new AdminController(mock.Object)
            {
                TempData = tempData.Object
            };

            var product = new Product() { Name = "Test" };

            target.ModelState.AddModelError("Error", "Error");

            //Act
            var result = target.Edit(product);

            //Assert
            mock.Verify(x => x.SaveProduct(It.IsAny<Product>()), Times.Once);
            Assert.IsType<ViewResult>(result);
        }

        private T GetViewModel<T>(IActionResult result) where T : class
        {
            return (result as ViewResult)?.ViewData.Model as T;
        }
    }
}
