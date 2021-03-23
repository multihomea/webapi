using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductManager;
using ProductManager.Controllers;
using Xunit;

namespace ProductManagerTest
{
    public class ProductSeedDataFixture : IDisposable
    {
        public List<Product> Products { get; private set; }
        public ProductManagerContext Context { get; private set; }
        public ProductManagerController Controller { get; private set; }

        public ProductSeedDataFixture()
        {
            Product produt_pomme = new Product()
            {
                ProductId = new Guid("815accac-fd5b-478a-a9d6-f171a2f6ae7f"),
                Name = "Pomme",
                StartDate = new DateTime(2021, 01, 11, 3, 5, 6),
                EndDate = new DateTime(2023, 05, 11, 3, 5, 6)
            };
            Product product_apple = new Product
            {
                ProductId = new Guid("33704c4a-5b87-464c-bfb6-51971b4d18ad"),
                Name = "Apple",
                StartDate = new DateTime(2011, 03, 11, 3, 5, 6),
                EndDate = new DateTime(2013, 05, 11, 3, 8, 1)
            };
            Product product_orange = new Product()
            {
                ProductId = new Guid("ab2bd817-98cd-4cf3-a80a-53ea0cd9c200"),
                Name = "Orange",
                StartDate = new DateTime(2001, 05, 11, 3, 5, 6),
                EndDate = new DateTime(2003, 05, 11, 3, 5, 6)
            };

            Products = new List<Product>() { produt_pomme, product_apple, product_orange };

            var options = new DbContextOptionsBuilder<ProductManagerContext>()
            .UseInMemoryDatabase(databaseName: "ProductsDB")
            .Options;

            Context = new ProductManagerContext(options);
            Context.Products.AddRange(produt_pomme, product_apple, product_orange);
            Context.SaveChanges();


            Controller = new ProductManagerController(Context);

        }

        public void Dispose()
        {
            Context.Dispose();
        }
    }

    public class ProductManagerControllerTest : IClassFixture<ProductSeedDataFixture>
    {

        ProductSeedDataFixture fixture;

        public ProductManagerControllerTest(ProductSeedDataFixture fixture)
        {

            this.fixture = fixture;

        }

        [Fact]
        public async void Get_WhenCalled_ReturnsOkResult()
        {
            // Act
            var okResult = await fixture.Controller.GetProducts();

            // Assert
            Assert.IsType<ActionResult<IEnumerable<Product>>>(okResult);

        }

        [Fact]
        public async void Get_WhenCalled_ReturnsAllItems()
        {
            // Act
            var okResult = await fixture.Controller.GetProducts();

            // Assert
            var items = Assert.IsType<List<Product>>(okResult.Value);
            Assert.Equal(3, items.Count);
        }

        [Fact]
        public async void Get_WhenCalled_AllItemsTheExcpectedOnes()
        {
            // Act
            var okResult = await fixture.Controller.GetProducts();// as ActionResult<IEnumerable<Product>>;

            // Assert
            var items = Assert.IsType<List<Product>>(okResult.Value);
            for (int i = 0; i < fixture.Products.Count; i++)
            {
                Assert.Equal(fixture.Products[i].Name, items[i].Name);
                Assert.Equal(fixture.Products[i].StartDate, items[i].StartDate);
                Assert.Equal(fixture.Products[i].EndDate, items[i].EndDate);
            }
        }

        [Fact]
        public async void Post_WhenCalled_SavesTheNewObjectInTheDB()
        {
            Product produt_tomate = new Product()
            {
                Name = "tomate",
                StartDate = new DateTime(2021, 01, 11, 3, 5, 6),
                EndDate = new DateTime(4023, 05, 11, 3, 5, 6)
            };
            // Act
            var okResult = await fixture.Controller.PostProduct(produt_tomate);// as ActionResult<IEnumerable<Product>>;

            // Assert
            var dbItem = fixture.Context.Products.LastOrDefaultAsync().Result as Product;
            Assert.Equal(dbItem.Name,produt_tomate.Name);
            Assert.Equal(dbItem.StartDate,produt_tomate.StartDate);
            Assert.Equal(dbItem.EndDate,produt_tomate.EndDate);
        }
    }
}