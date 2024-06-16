using aliment_backend;
using aliment_backend.Controllers;
using aliment_backend.DTOs;
using aliment_backend.Entities;
using aliment_backend.Mappers;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace unit_test.Controller_test
{
    public class CategoryControllerTest
    {
        private readonly ApplicationDbContextMoq _contextMock = new();
        private readonly UnitOfWorkClass _unitOfWork;
        private readonly IMapper _mapper;
        private readonly CategoryController _controller;

        public CategoryControllerTest()
        {
            MapperConfiguration mapperConfig = new(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfile());
            });

            _mapper = mapperConfig.CreateMapper();
            _unitOfWork = new UnitOfWorkClass(_contextMock.Context);
            _controller = new CategoryController(_unitOfWork, _mapper);
        }

        [Fact]
        public async Task GetAll_AllCategories_ReturnAll()
        {
            // Act
            IActionResult result = await _controller.GetAllAsync();

            // Assert
            OkObjectResult okResult = Assert.IsType<OkObjectResult>(result);
            Assert.IsAssignableFrom<IEnumerable<Category>>(okResult.Value);

            IEnumerable<Category>? categories = okResult.Value as IEnumerable<Category>;

            if (_contextMock.Context.Categories != null)
            {
                int expectedCategoryCount = _contextMock.Context.Categories.Count();
                int actualCategoryCount = categories?.Count() ?? 0;

                Assert.Equal(expectedCategoryCount, actualCategoryCount);
            }
        }

        [Fact]
        public async Task GetAll_WithProducts_ReturnAll()
        {
            // Act
            IActionResult result = await _controller.GetAllAsync();

            // Assert
            OkObjectResult okResult = Assert.IsType<OkObjectResult>(result);
            Assert.IsAssignableFrom<IEnumerable<Category>>(okResult.Value);
            IEnumerable<Category>? categories = okResult.Value as IEnumerable<Category>;
            Assert.NotNull(categories);

            if (categories != null)
            {
                foreach (var category in categories)
                {
                    if (category.Products != null && category.Products.Any())
                    {
                        Assert.NotEmpty(category.Products);
                    }
                }
            }    
        }

        [Fact]
        public async Task GetByIdAsync_ExistingId_ReturnsCategory()
        {
            // Arrange
            int categoryId = 1;
            string? expectedCategoryName = _contextMock?.Context?.Categories?.FirstOrDefault(c => c.Id == categoryId)?.Name;

            // Act
            IActionResult result = await _controller.GetByIdAsync(categoryId);

            // Assert
            OkObjectResult okResult = Assert.IsType<OkObjectResult>(result);
            Category category = Assert.IsType<Category>(okResult.Value);
            Assert.NotNull(category);
            Assert.Equal(categoryId, category.Id);
            Assert.Equal(expectedCategoryName, category.Name);
        }

        [Fact]
        public async Task GetCategoryById_NonExistingId_ReturnsNotFound()
        {
            // Arrange
            int nonExistingId = 1000;

            // Act
            IActionResult result = await _controller.GetByIdAsync(nonExistingId);

            // Assert
            NotFoundObjectResult notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            string message = Assert.IsType<string>(notFoundResult.Value);
            Assert.Equal($"L'entité n°{nonExistingId} n'a pas été trouvée.", message);
        }

        [Fact]
        public async Task AddOne_ValidCategory_ReturnsOk()
        {
            // Arrange
            CategoryDTO dto = new() { Name = "Fruit", Description = "rouge" };

            // Act
            IActionResult result = await _controller.AddOne(dto);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            OkObjectResult? okResult = result as OkObjectResult;
            Assert.NotNull(okResult);

            Category? category = okResult.Value as Category;
            Assert.NotNull(category);
            Assert.Equal(dto.Name, category.Name);
            Assert.Equal(dto.Description, category.Description);
        }

        [Fact]
        public async Task AddOne_EmptyName_ReturnsBadRequest()
        {
            // Arrange
            CategoryDTO cat = new() { Name = "", Description = "" };
            _controller.ModelState.AddModelError("Name", "Le nom de la catégorie est requis.");

            // Act
            IActionResult result = await _controller.AddOne(cat);

            // Assert
            Assert.False(_controller.ModelState.IsValid);
            BadRequestObjectResult badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);
        }

        [Fact]
        public async Task Delete_DeletedSuccessfully_ReturnsOk()
        {
            // Act 
            IActionResult result = await _controller.Delete(1);

            // Assert
            OkObjectResult okResult = Assert.IsType<OkObjectResult>(result);
            bool? isSuccess = okResult.Value as bool?;
            Assert.NotNull(isSuccess);
            Assert.True(isSuccess.GetValueOrDefault());
        }


        [Fact]
        public async Task Delete_DeletedNotSuccessfully_NotFound()
        {
            // Act 
            IActionResult result = await _controller.Delete(1000);

            // Assert 
            NotFoundObjectResult notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal($"L'entité avec l'ID {1000} n'a pas été trouvée.", notFoundResult.Value);
        }

        [Fact]
        public async Task Update_Category_Success()
        {
            // Arrange
            int categoryId = 1;
            OkObjectResult? originalCategoryResult = await _controller.GetByIdAsync(categoryId) as OkObjectResult;

            // Assert
            Assert.NotNull(originalCategoryResult);
            Assert.Equal(200, originalCategoryResult.StatusCode);

            Category? originalCategory = originalCategoryResult.Value as Category;
            string? originalCategoryName = originalCategory?.Name;

            CategoryDTO categoryDto = new()
            {
                Name = "NewName",
                Description = ""
            };

            // Act
            OkObjectResult? updateResult = await _controller.Update(categoryId, categoryDto) as OkObjectResult;

            // Assert
            Assert.NotNull(updateResult);
            Assert.Equal(200, updateResult.StatusCode);

            if (updateResult.StatusCode == 200)
            {
                OkObjectResult? updatedCategoryResult = await _controller.GetByIdAsync(categoryId) as OkObjectResult;

                Assert.NotNull(updatedCategoryResult);
                Assert.Equal(200, updatedCategoryResult.StatusCode);

                Category? updatedCategory = updatedCategoryResult.Value as Category;
                string? updatedCategoryName = updatedCategory?.Name;

                Assert.Equal(categoryDto.Name, updatedCategoryName);
                if (originalCategoryName != null) 
                {
                    Assert.NotEqual(originalCategoryName, updatedCategoryName);
                }
            }
        }

        [Fact]
        public async Task Update_InvalidId_NotFound()
        {
            //Arrange
            CategoryDTO categoryDto = new()
            {
                Name = "NewName",
                Description = ""
            };

            // Act
            IActionResult result = await _controller.Update(1000, categoryDto);

            // Assert    
            NotFoundObjectResult? notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal($"L'entité n°{1000} n'a pas été trouvée.", notFoundResult.Value);
        }

        [Fact]
        public async Task Update_InvalidDTO_BadRequest()
        {
            //Arrange
            int categoryId = 1;
            CategoryDTO categoryDto = new() { Name = "N", Description = "" };
            _controller.ModelState.AddModelError("Name", "Le nom de la catégorie doit comporter au moins 3 caractères.");

            // Act
            IActionResult result = await _controller.Update(categoryId, categoryDto);

            // Assert
            Assert.False(_controller.ModelState.IsValid);
            BadRequestObjectResult badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);
        }

        [Fact]
        public async Task NbProduct_Returns_CorrectCountAsync()
        {
            // Arrange
            int categoryId = 1;

            // Act
            IActionResult result = await _controller.GetByIdAsync(categoryId);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            OkObjectResult okResult = (OkObjectResult)result;
            Category? category = okResult.Value as Category;

            if(_contextMock.Context.Products != null)
            {
                int expectedNbProducts = _contextMock.Context.Products.Count(p => p.CategoryId == categoryId);
                int actualNbProducts = category?.NbProduct ?? 0;
                Assert.Equal(expectedNbProducts, actualNbProducts);
            }
        }
    }
}
