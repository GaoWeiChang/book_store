using book_store.Areas.Admin.Services;
using book_store.DataAccess.Repository.IRepository;
using book_store.Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace book_store.Tests.Admin
{
    public class CategoryServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<ICategoryRepository> _mockCategoryRepo;
        private readonly CategoryService _categoryService;
        public CategoryServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockCategoryRepo = new Mock<ICategoryRepository>();

            _mockUnitOfWork.Setup(u => u.Category).Returns(_mockCategoryRepo.Object);
            
            _categoryService = new CategoryService(_mockUnitOfWork.Object);
        }

        [Fact]
        public void GetAllCategories_ReturnOrderedList()
        {
            // Arrange
            var categories = new List<Category>
            {
                new Category { Id=3, Name="History", DisplayOrder=3 },
                new Category { Id=1, Name="Action", DisplayOrder=1 },
                new Category { Id=2, Name="Drama", DisplayOrder=2 }
            };
            _mockCategoryRepo.Setup(c=>c.GetAll(null, null)).Returns(categories);

            // Act
            List<Category> result = _categoryService.GetAllCategories().ToList();

            // Assert
            Assert.Equal(3, result.Count);

            Assert.Equal("Action", result[0].Name);
            Assert.Equal("Drama", result[1].Name);
            Assert.Equal("History", result[2].Name);

            Assert.Equal(1, result[0].DisplayOrder);
            Assert.Equal(2, result[1].DisplayOrder);
            Assert.Equal(3, result[2].DisplayOrder);
        }

        [Fact]
        public void CreateCategory_ReturnTrue()
        {
            // Arrange
            Category category1 = new Category { Id = 1, Name = "Action", DisplayOrder = 1 };
            Category category2 = new Category { Id = 2, Name = "History", DisplayOrder = 2 };


            _mockCategoryRepo.Setup(c => c.Get(It.IsAny<Expression<Func<Category, bool>>>(), null, false))
                .Returns((Category)null);

            // Act
            bool result1 = _categoryService.CreateCategory(category1);
            bool result2 = _categoryService.CreateCategory(category2);

            // Assert
            Assert.True(result1);
            Assert.True(result2);
        }

        [Fact]
        public void UpdateCategory_ReturnTrue()
        {
            // Arrange
            var existingCategory = new Category { Id = 1, Name = "Denny", DisplayOrder = 1 };
            var updatedCategory = new Category { Id = 1, Name = "Tom", DisplayOrder = 3 };

            _mockCategoryRepo.Setup(r => r.Get(It.Is<Expression<Func<Category, bool>>>(c => c.ToString().Contains("Id")), null, true)).
                Returns(existingCategory);

            // Act
            bool result = _categoryService.UpdateCategory(updatedCategory);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void DeleteCategory_ReturnTrue()
        {
            // Arrange
            var category = new Category { Id = 1, Name = "Action", DisplayOrder = 1 };

            _mockCategoryRepo.Setup(c => c.Get(It.IsAny<Expression<Func<Category, bool>>>(), null, false))
                .Returns(category);

            // Act
            bool result = _categoryService.DeleteCategory(category.Id);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void DeleteCategory_CategoryNotFound_ReturnsFalse()
        {
            // Arrange
            _mockCategoryRepo.Setup(c => c.Get(It.IsAny<Expression<Func<Category, bool>>>(), null, false))
                .Returns((Category)null);

            // Act
            bool result = _categoryService.DeleteCategory(2);

            // Assert
            Assert.False(result);
        }
    }
}
