using aliment_backend.Controllers;
using aliment_backend;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using aliment_backend.Mappers;
using aliment_backend.Entities;
using aliment_backend.DTOs;

namespace unit_test.Controller_test
{
    public class ProductControllerTest
    {
        private readonly ApplicationDbContextMoq _contextMock = new();
        private readonly UnitOfWorkClass _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ProductController _controller;

        public ProductControllerTest()
        {
            MapperConfiguration mapperConfig = new(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfile());
            });

            _mapper = mapperConfig.CreateMapper();
            _unitOfWork = new UnitOfWorkClass(_contextMock.Context);
            _controller = new ProductController(_unitOfWork, _mapper);

            List<Claim> claims = new()
            {
                new Claim(ClaimTypes.Name, "TestUser"),
                new Claim(ClaimTypes.Role, "Admin")
            };

            ClaimsIdentity identity = new(claims, "TestAuthType");
            ClaimsPrincipal claimsPrincipal = new(identity);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = claimsPrincipal }
            };
        }

        [Fact]
        public async Task GetAll_AllProducts_ReturnAll()
        {
            // Act
            IActionResult result = await _controller.GetAllAsync();

            // Assert
            OkObjectResult okResult = Assert.IsType<OkObjectResult>(result);
            Assert.IsAssignableFrom<IEnumerable<Product>>(okResult.Value);

            IEnumerable<Product>? products = okResult.Value as IEnumerable<Product>;

            if(_contextMock.Context.Products != null)
            {
                int expectedProductCount = _contextMock.Context.Products.Count();
                int actualProductCount = products?.Count() ?? 0;
                Assert.Equal(expectedProductCount, actualProductCount);
            }
        }

        [Fact]
        public async Task GetAll_WithCategory_ReturnAll()
        {
            // Act
            IActionResult result = await _controller.GetAllAsync();

            // Assert
            OkObjectResult okResult = Assert.IsType<OkObjectResult>(result);
            Assert.IsAssignableFrom<IEnumerable<Product>>(okResult.Value);

            IEnumerable<Product>? products = okResult.Value as IEnumerable<Product>;

            if (products != null)
            {
                foreach (Product product in products)
                {
                    Assert.NotNull(product.Category);
                    Assert.NotNull(product.Category.Products);
                }
            }
        }

        [Fact]
        public async Task GetAll_WithStocks_ReturnAll()
        {
            // Act
            IActionResult result = await _controller.GetAllAsync();

            // Assert
            OkObjectResult okResult = Assert.IsType<OkObjectResult>(result);
            Assert.IsAssignableFrom<IEnumerable<Product>>(okResult.Value);

            IEnumerable<Product>? products = okResult.Value as IEnumerable<Product>;

            if (products != null)
            {
                foreach (Product product in products)
                {
                    // Si la catégorie a des produits, vérifiez qu'elle n'est pas vide
                    if (product.Stocks != null && product.Stocks.Any())
                    {
                        Assert.NotEmpty(product.Stocks);
                    }
                }
            }
        }

        [Fact]
        public async Task GetByIdAsync_ExistingId_ReturnsProduct()
        {
            // Arrange
            int productId = 1;

            // Act
            IActionResult result = await _controller.GetByIdAsync(productId);
            OkObjectResult okResult = Assert.IsType<OkObjectResult>(result);
            Product? product = Assert.IsType<Product>(okResult.Value);

            if(_contextMock.Context.Products != null)
            {
                string? expectedCategoryName = _contextMock.Context.Products.FirstOrDefault(p => p.Id == productId)?.Category?.Name;

                // Assert
                Assert.Equal(productId, product.Id);
                Assert.NotNull(product.Category);
                Assert.Equal(expectedCategoryName, product.Category.Name);
            }           
        }

        [Fact]
        public async Task GetTaskAsync_NonExistingId_NotFound()
        {
            // Arrange
            int notExistingId = 1000;

            // Act
            IActionResult result = await _controller.GetByIdAsync(notExistingId);

            // Assert
            NotFoundObjectResult notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            string message = Assert.IsType<string>(notFoundResult.Value);
            Assert.Equal($"L'entité n°{notExistingId} n'a pas été trouvée.", message);
        }

        [Fact]
        public async Task AddOne_ValidProduct_ReturnsOk()
        {
            // Arrange
            ProductDTO dto = new() { Name = "Agneau", Description = "", DurationConservation = 3, CategoryId = 1 };

            // Act
            IActionResult result = await _controller.AddOne(dto);

            if(_contextMock.Context.Products != null)
            {
                string? expectedCategoryName = _contextMock.Context.Products.FirstOrDefault(p => p.Id == dto.CategoryId)?.Category?.Name;
                OkObjectResult okResult = Assert.IsType<OkObjectResult>(result);
                Product? product = okResult.Value as Product;

                // Assert
                Assert.NotNull(product);
                Assert.Equal(dto.Name, product.Name);
                Assert.Equal(dto.DurationConservation, product.DurationConservation);
                Assert.Equal(expectedCategoryName, product?.Category?.Name);
            }           
        }

        [Fact]
        public async Task AddOne_BadDurationConservation_ReturnsBadRequest()
        {
            // Arrange
            ProductDTO invalidDTO = new() { Name = "Essai", Description = "", DurationConservation = 0, CategoryId = 2 };
            _controller.ModelState.AddModelError("DurationConservation", "La durée de conservation doit être comprise entre 1 et 25.");

            // Act
            IActionResult result = await _controller.AddOne(invalidDTO);

            // Assert
            Assert.False(_controller.ModelState.IsValid);
            BadRequestObjectResult badResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badResult.StatusCode);
        }

        [Fact]
        public async Task Delete_DeletedSuccessfully_ReturnsOk()
        {
            // Act 
            IActionResult result = await _controller.Delete(1);

            // Assert - Vérifiez si le résultat est Ok et que l'entité a été supprimée avec succès
            OkObjectResult okResult = Assert.IsType<OkObjectResult>(result);
            bool? isSuccess = okResult.Value as bool?;
            Assert.NotNull(isSuccess);
            Assert.True(isSuccess.GetValueOrDefault());
            if (_contextMock.Context.Products != null)
            {
                Product? deletedProduct = _contextMock.Context.Products.FirstOrDefault(p => p.Id == 1);
                Assert.Null(deletedProduct);
            }               
        }


        [Fact]
        public async Task Delete_DeletedNotSuccessfully_NotFound()
        {
            // Act 
            IActionResult result = await _controller.Delete(1000);

            // Assert - Vérifiez si le résultat est NotFound
            NotFoundObjectResult notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal($"L'entité avec l'ID {1000} n'a pas été trouvée.", notFoundResult.Value);
        }

        [Fact]
        public async Task Update_Product_Success()
        {
            // Arrange
            int productId = 1;
            OkObjectResult? originalProductResult = await _controller.GetByIdAsync(productId) as OkObjectResult;
            Assert.NotNull(originalProductResult);
            Product? originalProduct = originalProductResult.Value as Product;
            ProductDTO dto = new() { Name = "NewName", Description = "", DurationConservation = 3, CategoryId = 1 };
            string? originalName = originalProduct?.Name;
            int originalDurationConservation = originalProduct?.DurationConservation ?? 0;

            // Act
            OkObjectResult? updateResult = await _controller.Update(productId, dto) as OkObjectResult;

            // Assert
            Assert.NotNull(updateResult);
            Assert.Equal(200, updateResult.StatusCode);

            if (updateResult.StatusCode == 200)
            {
                OkObjectResult? updatedProductResult = await _controller.GetByIdAsync(productId) as OkObjectResult;

                // Assert: Vérifiez si la récupération de la catégorie mise à jour a réussi
                Assert.NotNull(updatedProductResult);

                Product? updatedProduct = updatedProductResult.Value as Product;

                // Assert: Vérifier si le nom du produit a été correctement mis à jour
                Assert.Equal(dto.Name, updatedProduct?.Name);
                Assert.Equal(dto.DurationConservation, updatedProduct?.DurationConservation);
                // Vérifiez également que d'autres propriétés ne sont pas modifiées si ce n'est pas attendu
                if (originalName != null) // Si originalCategoryName n'est pas null, alors vérifiez le changement
                {
                    Assert.NotEqual(originalName, updatedProduct?.Name);
                }
                Assert.NotEqual(originalDurationConservation, updatedProduct?.DurationConservation);
            }
        }

        [Fact]
        public async Task Update_InvalidId_NotFound()
        {
            //Arrange
            ProductDTO productDto = new()
            {
                Name = "NewName",
                Description = "",
                DurationConservation = 2,
                CategoryId = 1
            };

            // Act
            IActionResult result = await _controller.Update(1000, productDto);

            // Assert - Vérifiez si le résultat est NotFound
            NotFoundObjectResult? notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal($"L'entité n°{1000} n'a pas été trouvée.", notFoundResult.Value);
        }

        [Fact]
        public async Task Update_InvalidDTO_BadRequest()
        {
            //Arrange
            int productId = 1;
            ProductDTO productDto = new() { Name = "", Description = "", DurationConservation = 2, CategoryId = 1 };
            _controller.ModelState.AddModelError("Name", "Le nom est requis.");

            // Act
            IActionResult result = await _controller.Update(productId, productDto);

            // Assert
            Assert.False(_controller.ModelState.IsValid);
            BadRequestObjectResult badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);
        }

        [Fact]
        public async Task NbProductStock_Returns_CorrectCountAsync()
        {
            // Arrange            
            int productId = 2;

            // Act
            IActionResult result = await _controller.GetByIdAsync(productId);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            OkObjectResult okResult = (OkObjectResult)result;
            Product? product = okResult.Value as Product;

            if(_contextMock.Context.Stocks != null)
            {
                int expectedNbProducts = _contextMock.Context.Stocks.Count(s => s.ProductId == productId);
                int actualNbProducts = product?.NbProductStock ?? 0;
                Assert.Equal(expectedNbProducts, actualNbProducts);
            }            
        }

        [Fact]
        public void GetProductsByCategoryIds_ValidCategoryIds_ReturnsProducts()
        {
            // Arrange
            List<int> validCategoryIds = new() { 1, 2 };
            List<Product> expectedProducts = GetExpectedProducts(validCategoryIds);

            // Act
            IActionResult result = _controller.GetProductsByCategoryIds(validCategoryIds);

            // Assert
            OkObjectResult okResult = Assert.IsType<OkObjectResult>(result);
            IEnumerable<Product> products = Assert.IsAssignableFrom<IEnumerable<Product>>(okResult.Value);

            Assert.Equal(expectedProducts.Count, products.Count());

            foreach (var product in expectedProducts)
            {
                Assert.Contains(product, products);
            }
        }

        [Fact]
        public void GetProductsByCategoryIds_NullOrEmptyCategoryIds_ReturnsBadRequest()
        {
            // Arrange
            List<int> categoryIds = new(); // Liste vide
            string expectedErrorMessage = "La liste d'IDs de catégorie ne peut pas être vide.";

            // Act
            IActionResult result = _controller.GetProductsByCategoryIds(categoryIds);

            // Assert
            BadRequestObjectResult badRequestResult = Assert.IsType<BadRequestObjectResult>(result);

            Assert.Equal(expectedErrorMessage, badRequestResult.Value);
        }

        [Fact]
        public void GetProductsByCategoryIds_NonexistentCategoryIds_ReturnsBadRequestWithMessage()
        {
            // Arrange
            int nonexistentCategoryId = 999;
            List<int> categoryIds = new() { 1, 2, nonexistentCategoryId };
            string expectedErrorMessage = $"L'ID de catégorie {nonexistentCategoryId} n'existe pas.";

            // Act
            IActionResult  result = _controller.GetProductsByCategoryIds(categoryIds);

            // Assert
            BadRequestObjectResult badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.NotNull(result);
            Assert.Equal(expectedErrorMessage, badRequestResult.Value);
        }

        private List<Product> GetExpectedProducts(List<int> categoryIds)
        {
            if (_contextMock.Context.Products != null)
            {
                return _contextMock.Context.Products
                                                   .Where(p => categoryIds.Contains(p.CategoryId))
                                                   .ToList();
            }
            else
            {
                return new List<Product>();
            }                
        }
    }
}
