using book_store.Areas.Admin.Services;
using book_store.Areas.Admin.Services.IServices;
using book_store.DataAccess.Repository.IRepository;
using book_store.Models;
using book_store.Models.ViewModels;
using book_store.Utility;
using Microsoft.AspNetCore.Hosting;
using Microsoft.SqlServer.Server;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace book_store.Tests.Admin
{
    public class ProductServiceTests
    {
        private readonly ProductService _productService;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork = new Mock<IUnitOfWork>();
        private readonly Mock<IWebHostEnvironment> _mockWebHostEnvironment = new Mock<IWebHostEnvironment>();

        public ProductServiceTests()
        {
            _mockWebHostEnvironment.Setup(m => m.WebRootPath)
                .Returns(Path.Combine(Path.GetTempPath(), "TestWebRoot"));

            _productService = new ProductService(_mockUnitOfWork.Object, _mockWebHostEnvironment.Object);
        }

        [Fact]
        public void GetAllProducts_ReturnAllProducts()
        {
            // Arrange
            var productList = new List<Product>
            {
                new Product { Id = 1, Title = "Book 1", Author = "Author 1", Description = "This is Book 1, written by Author 1", ISBN = "ABCD123EFG", Price = 10, CategoryId = 2 },
                new Product { Id = 2, Title = "Book 2", Author = "Author 2", Description = "This is Book 2, written by Author 2", ISBN = "HIJK456LMN", Price = 20, CategoryId = 2 },
                new Product { Id = 3, Title = "Book 3", Author = "Author 3", Description = "This is Book 3, written by Author 3", ISBN = "OPQR789TUV", Price = 30, CategoryId = 1 },
            };
            _mockUnitOfWork.Setup(x => x.Product.GetAll(null, "Category")).Returns(productList);

            // Act
            List<Product> result = _productService.GetAllProducts().ToList();

            // Assert
            Assert.Equal("Book 1", result[0].Title);
            Assert.Equal("Book 2", result[1].Title);
            Assert.Equal("Book 3", result[2].Title);

            Assert.Equal("Author 1", result[0].Author);
            Assert.Equal("Author 2", result[1].Author);
            Assert.Equal("Author 3", result[2].Author);

            Assert.Equal("This is Book 1, written by Author 1", result[0].Description);
            Assert.Equal("This is Book 2, written by Author 2", result[1].Description);
            Assert.Equal("This is Book 3, written by Author 3", result[2].Description);

            Assert.Equal("ABCD123EFG", result[0].ISBN);
            Assert.Equal("HIJK456LMN", result[1].ISBN);
            Assert.Equal("OPQR789TUV", result[2].ISBN);

            Assert.Equal(10, result[0].Price);
            Assert.Equal(20, result[1].Price);
            Assert.Equal(30, result[2].Price);

            Assert.Equal(2, result[0].CategoryId);
            Assert.Equal(2, result[1].CategoryId);
            Assert.Equal(1, result[2].CategoryId);
        }

        [Fact]
        public void CreateProduct_ReturnServiceResult_Success()
        {
            var productList = new List<Product>();

            // Arrange
            var productVM_1 = new ProductVM
            {
                Product = new Product { Id = 1, Title = "Book 1", Author = "Author 1", Description = "This is Book 1, written by Author 1", ISBN = "ABCD123EFG", Price = 10, CategoryId = 2 }
            };

            var productVM_2 = new ProductVM
            {
                Product = new Product { Id = 2, Title = "Book 2", Author = "Author 2", Description = "This is Book 2, written by Author 2", ISBN = "HIJK456LMN", Price = 20, CategoryId = 1 }
            };

            // mock of add data
            _mockUnitOfWork.Setup(x => x.Product.Add(It.IsAny<Product>()))
                           .Callback<Product>(p => productList.Add(p));

            // mock for Get() for check data that added into the list
            _mockUnitOfWork.Setup(x => x.Product.Get(It.IsAny<Expression<Func<Product, bool>>>(),
                                                      null,
                                                      false))
                            .Returns<Expression<Func<Product, bool>>, string, bool>((expression, include, tracked) =>
                            {
                                var func = expression.Compile(); // แปลง Expression เป็น Func
                                return productList.FirstOrDefault(func); // ค้นหาใน list
                            });

            // Act
            var result1 = _productService.CreateProduct(productVM_1, null);
            var result2 = _productService.CreateProduct(productVM_2, null);

            // Assert
            Assert.True(result1.Success);
            Assert.Equal("Created product successfully.", result1.Message);

            Assert.True(result2.Success);
            Assert.Equal("Created product successfully.", result2.Message);
        }

        [Fact]
        public void UpdateProduct_ReturnServiceResult_Success()
        {
            // Arrange
            var productList = new List<Product>();

            _mockUnitOfWork.Setup(x => x.Product.Add(It.IsAny<Product>()))
               .Callback<Product>(p => productList.Add(p));

            _mockUnitOfWork.Setup(x => x.Product.Get(It.IsAny<Expression<Func<Product, bool>>>(),
                                          null,
                                          true))
                .Returns<Expression<Func<Product, bool>>, string, bool>((expression, include, tracked) =>
                {
                    var func = expression.Compile();
                    return productList.FirstOrDefault(func);
                });

            var existingProduct = new ProductVM
            {
                Product = new Product { Id = 1, Title = "Book 1", Author = "Author 1", Description = "This is Book 1, written by Author 1", ISBN = "ABCD123EFG", Price = 10, CategoryId = 2 }
            };
            var updatedProduct = new ProductVM
            {
                Product = new Product { Id = 1, Title = "Think Again", Author = "Adam", Description = "This is Book is Think Again, written by Adam", ISBN = "HIJK456LMN", Price = 30, CategoryId = 3 }
            };

            // Act
            _productService.CreateProduct(existingProduct, null);
            ServiceResult result = _productService.UpdateProduct(updatedProduct, null);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Update product successfully.", result.Message);
        }

        [Fact]
        public void DeleteProduct_ReturnServiceResult_Success()
        {
            // Arrange
            var product = new Product { Id = 1, Title = "Think Again", Author = "Adam", Description = "This is Book is Think Again, written by Adam", ISBN = "HIJK456LMN", Price = 30, CategoryId = 3 };
            _mockUnitOfWork.Setup(p => p.Product.Get(It.IsAny<Expression<Func<Product, bool>>>(), null, true))
                .Returns(product);

            // Act
            ServiceResult result = _productService.DeleteProduct(product.Id);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Delete product successfully.", result.Message);
        }

    }
}
