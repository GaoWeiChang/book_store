using book_store.Areas.Admin.Services;
using book_store.DataAccess.Repository.IRepository;
using book_store.Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
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
            _mockCategoryRepo.Setup(u=>u.GetAll(null, null)).Returns(categories);

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
    }
}
