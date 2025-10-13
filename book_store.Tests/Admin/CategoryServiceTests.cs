using book_store.Areas.Admin.Services;
using book_store.DataAccess.Repository.IRepository;
using book_store.Models;
using book_store.Utility;
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
        private readonly CategoryService _categoryService;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork = new Mock<IUnitOfWork>();

        public CategoryServiceTests()
        {
            _categoryService = new CategoryService(_mockUnitOfWork.Object);
        }

        [Fact]
        public void GetAllCategories_ReturnOrderedList()
        {
            // Arrange
            List<Category> categoryList = new List<Category>
            {
                new Category { Id=3, Name="History", DisplayOrder=3 },
                new Category { Id=1, Name="Action", DisplayOrder=1 },
                new Category { Id=2, Name="Drama", DisplayOrder=2 }
            };

            _mockUnitOfWork.Setup(x => x.Category.GetAll(null, null)).Returns(categoryList);

            // Act
            List<Category> result = _categoryService.GetAllCategories().ToList();

            // Assert
            Assert.Equal("Action", result[0].Name);
            Assert.Equal("Drama", result[1].Name);
            Assert.Equal("History", result[2].Name);

            Assert.Equal(1, result[0].DisplayOrder);
            Assert.Equal(2, result[1].DisplayOrder);
            Assert.Equal(3, result[2].DisplayOrder);
        }

        [Fact]
        public void CreateCategory_ReturnServiceResult_Success()
        {
            // Arrange
            Category category1 = new Category { Id = 1, Name = "Action", DisplayOrder = 1 };
            Category category2 = new Category { Id = 2, Name = "History", DisplayOrder = 2 };

                        // blank database
            _mockUnitOfWork.Setup(c => c.Category.Get(It.IsAny<Expression<Func<Category, bool>>>(), null, false))
                .Returns(() => null);

            // Act
            ServiceResult result1 = _categoryService.CreateCategory(category1);
            ServiceResult result2 = _categoryService.CreateCategory(category2);

            // Assert
            Assert.True(result1.Success);
            Assert.Equal("Created category successfully.", result1.Message);

            Assert.True(result2.Success);
            Assert.Equal("Created category successfully.", result2.Message);
        }

        [Fact]
        public void CreateCategory_ReturnServiceResult_Fail()
        {
            var categoryList = new List<Category>();

            // Arrange
            Category category1 = new Category { Id = 1, Name = "Action", DisplayOrder = 1 };
            Category category2 = new Category { Id = 2, Name = "Action", DisplayOrder = 2 };
            Category category3 = new Category { Id = 3, Name = "War", DisplayOrder = 1 };
            Category category4 = new Category { Id = 4, Name = "History", DisplayOrder = -1 };
            Category category5 = new Category { Id = 2, Name = "Romantic", DisplayOrder = 3 };
                
                // mock of add data
            _mockUnitOfWork.Setup(x => x.Category.Add(It.IsAny<Category>()))
                           .Callback<Category>(c => categoryList.Add(c));

                // mock for Get() for check data that added into the list
            _mockUnitOfWork.Setup(x => x.Category.Get(It.IsAny<Expression<Func<Category, bool>>>(),
                                                      null,
                                                      false))
                            .Returns<Expression<Func<Category, bool>>, string, bool>((expression, include, tracked) =>
                            {
                                var func = expression.Compile(); // แปลง Expression เป็น Func
                                return categoryList.FirstOrDefault(func); // ค้นหาใน list
                            });

            // Act
            ServiceResult result1 = _categoryService.CreateCategory(category1);
            ServiceResult result2 = _categoryService.CreateCategory(category2);
            ServiceResult result3 = _categoryService.CreateCategory(category3);
            ServiceResult result4 = _categoryService.CreateCategory(category4);
            ServiceResult result5 = _categoryService.CreateCategory(category5);


            // Assert
            Assert.True(result1.Success);
            Assert.Equal("Created category successfully.", result1.Message);

            Assert.False(result2.Success);
            Assert.Equal("This category name already exist.", result2.Message);

            Assert.False(result3.Success);
            Assert.Equal("This display order already exist.", result3.Message);

            Assert.False(result4.Success);
            Assert.Equal("Display order must be positive number.", result4.Message);

            Assert.True(result5.Success);
            Assert.Equal("Created category successfully.", result5.Message);
        }


        [Fact]
        public void UpdateCategory_ReturnServiceResult_Success()
        {
            // Arrange
            var categoryList = new List<Category>();

            _mockUnitOfWork.Setup(x => x.Category.Add(It.IsAny<Category>()))
               .Callback<Category>(c => categoryList.Add(c));

            _mockUnitOfWork.Setup(x => x.Category.Get(It.IsAny<Expression<Func<Category, bool>>>(),
                                          null,
                                          true))
                .Returns<Expression<Func<Category, bool>>, string, bool>((expression, include, tracked) =>
                {
                    var func = expression.Compile();
                    return categoryList.FirstOrDefault(func); 
                });

            var existingCategory = new Category { Id = 1, Name = "Action", DisplayOrder = 1 };
            var updatedCategory = new Category { Id = 1, Name = "Horror", DisplayOrder = 3 };

            // Act
            _categoryService.CreateCategory(existingCategory);
            ServiceResult result = _categoryService.UpdateCategory(updatedCategory);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Update category successfully.", result.Message);
        }

        [Fact]
        public void DeleteCategory_ReturnTrue()
        {
            // Arrange
            var category = new Category { Id = 1, Name = "Action", DisplayOrder = 1 };

            _mockUnitOfWork.Setup(c => c.Category.Get(It.IsAny<Expression<Func<Category, bool>>>(), null, true))
                .Returns(category);

            // Act
            ServiceResult result = _categoryService.DeleteCategory(category.Id);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Delete category successfully.", result.Message);
        }
    }
}
