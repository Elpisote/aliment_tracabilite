using aliment_backend;
using aliment_backend.Controllers;
using aliment_backend.DTOs;
using aliment_backend.Entities;
using aliment_backend.Mappers;
using aliment_backend.Utils;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace unit_test.Controller_test
{
    public class StockControllerTest
    {
        private readonly ApplicationDbContextMoq _contextMock = new();
        private readonly UnitOfWorkClass _unitOfWork;
        private readonly IMapper _mapper;
        private readonly StockController _controller;

        public StockControllerTest()
        {
            MapperConfiguration mapperConfig = new(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfile());
            });

            _mapper = mapperConfig.CreateMapper();
            _unitOfWork = new UnitOfWorkClass(_contextMock.Context);
            _controller = new StockController(_unitOfWork, _mapper);
        }

        [Fact]
        public async Task GetAll_AllStocks_ReturnAll()
        {
            // Act
            IActionResult result = await _controller.GetAllAsync();

            // Assert
            OkObjectResult okResult = Assert.IsType<OkObjectResult>(result);
            Assert.IsAssignableFrom<IEnumerable<Stock>>(okResult.Value);

            IEnumerable<Stock>? stocks = okResult.Value as IEnumerable<Stock>;
            Stock? returnedStock = stocks?.FirstOrDefault();
            Assert.NotNull(returnedStock);
            Assert.NotNull(returnedStock?.ExpirationDate);

            if (_contextMock.Context.Stocks != null)
            {
                int expectedStockCount = _contextMock.Context.Stocks.Count(s => s.Statuts == Statuts.Inprogess);
                int actualStockCount = stocks?.Count() ?? 0;
                Assert.Equal(expectedStockCount, actualStockCount);
            }           
        }

        [Fact]
        public async Task GetAllAsync_ForStocksWithDurationConservation_ReturnsCorrectExpirationDate()
        {
            // Act
            IActionResult result = await _controller.GetAllAsync();

            // Assert
            OkObjectResult okResult = Assert.IsType<OkObjectResult>(result);
            IEnumerable<Stock> returnedStocks = Assert.IsAssignableFrom<IEnumerable<Stock>>(okResult.Value);

            foreach (Stock stock in returnedStocks)
            {
                Assert.NotNull(stock?.ExpirationDate);

                if (stock.Product != null && stock?.Product.DurationConservation != null)
                {
                    DateTime expectedExpirationDate = stock.OpeningDate.AddDays(stock.Product.DurationConservation);
                    Assert.Equal(expectedExpirationDate, stock.ExpirationDate);
                }
                else
                {
                    Assert.Null(stock?.ExpirationDate);
                }
            }
        }

        [Fact]
        public async Task GetByIdAsync_ExistingId_ReturnsProduct()
        {
            // Arrange
            int stockId = 1;
            string? expectedProductName = _contextMock?.Context?.Stocks?.FirstOrDefault(c => c.Id == stockId)?.Product?.Name;

            // Act
            IActionResult result = await _controller.GetByIdAsync(stockId);

            // Assert
            OkObjectResult okResult = Assert.IsType<OkObjectResult>(result);
            Stock? stock = Assert.IsType<Stock>(okResult.Value);

            Assert.Equal(stockId, stock.Id);
            Assert.Equal(expectedProductName, stock.Product?.Name);
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
        public async Task AddOne_AttemptsToAddStock_ReturnsBadRequest()
        {
            // Arrange
            StockDTO stockDto = new() { Statuts = Statuts.Error, ProductId = 1 };

            // Act
            IActionResult result = await _controller.AddOne(stockDto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Delete_AttemptsToDeleteStock_ReturnsBadRequest()
        {
            // Act 
            IActionResult result = await _controller.Delete(1);

            // Assert - Vérifiez si le résultat est Ok et que l'entité a été supprimée avec succès
            BadRequestObjectResult okResult = Assert.IsType<BadRequestObjectResult>(result);
            string message = Assert.IsType<string>(okResult.Value);
            Assert.Equal("La suppression des stocks n'est pas autorisée.", message);
        }

        [Fact]
        public async Task Update_Stock_Success()
        {
            // Arrange
            int stockId = 1;
            OkObjectResult? originalStockResult = await _controller.GetByIdAsync(stockId) as OkObjectResult;

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                    {              
                        new("Username", "username"),
                    }, "mock"))
                }
            };
            Assert.NotNull(originalStockResult);

            Stock? originalStock = originalStockResult.Value as Stock;
            StockDTO dto = new() { Statuts = Statuts.Consumes, ProductId = 1 };
            Statuts? originalStatuts = originalStock?.Statuts;

            // Act
            OkObjectResult? updateResult = await _controller.Update(stockId, dto) as OkObjectResult;

            // Assert
            Assert.NotNull(updateResult);
            Assert.Equal(200, updateResult.StatusCode);
            Stock? updatedStock = updateResult.Value as Stock;
            Assert.NotNull(updatedStock);
            Assert.Equal(stockId, updatedStock.Id);
            Assert.Equal(dto.Statuts, updatedStock.Statuts);
            Assert.Equal(dto.ProductId, updatedStock.ProductId);
            Assert.NotEqual(originalStatuts, updatedStock.Statuts);
            Assert.Equal("username", updatedStock.UserModification);
        }

        [Fact]
        public async Task Update_InvalidId_NotFound()
        {
            //Arrange
            int id = 1000;
            StockDTO dto = new() { Statuts = Statuts.Consumes, ProductId = 1 };
            // Act
            IActionResult result = await _controller.Update(id, dto);

            // Assert 
            NotFoundObjectResult? notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal($"L'entité n°{id} n'a pas été trouvée.", notFoundResult.Value);
        }

        [Fact]
        public async Task AddMany_AllProductsValid_ReturnsOk()
        {
            // Arrange           
            List<int> productIds = new() { 1, 2, 3 };

            // Simuler un utilisateur authentifié
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                    {
                        new("Username", "username"),
                    }, "mock"))
                }
            };

            // Act
            IActionResult result = await _controller.AddMany(productIds);

            // Assert
            OkObjectResult okResult = Assert.IsType<OkObjectResult>(result);
            List<Stock> addedStocks = Assert.IsType<List<Stock>>(okResult.Value);
            Assert.Equal(productIds.Count, addedStocks.Count);

            // Vérifier que la date d'expiration est correcte pour chaque stock ajouté
            foreach (Stock stock in addedStocks)
            {
                if(_contextMock.Context.Products != null)
                {
                    Product? product = _contextMock.Context.Products.FirstOrDefault(p => p.Id == stock.ProductId);
                    Assert.NotNull(product);

                    DateTime expectedExpirationDate = stock.OpeningDate.AddDays(product.DurationConservation);
                    Assert.Equal(expectedExpirationDate, stock.ExpirationDate);
                }               
            }

            // Vérifier que le champ UserCreation est correct pour chaque stock ajouté
            foreach (Stock stock in addedStocks)
            {
                Assert.Equal("username", stock.UserCreation);
            }
        }

        [Fact]
        public async Task AddMany_AllProductsInvalid_NotFound()
        {
            // Arrange           
            List<int> nonExistingProductIds = new() { 1000, 2000, 3000 };

            // Act & Assert
            foreach (int productId in nonExistingProductIds)
            {
                IActionResult result = await _controller.AddMany(new List<int> { productId });

                NotFoundObjectResult notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
                string errorMessage = Assert.IsType<string>(notFoundResult.Value);
                Assert.Equal($"Le produit avec l'identifiant {productId} n'a pas été trouvé.", errorMessage);
            }
        }

        [Fact]
        public void Stock_Countdown_ReturnsCorrectValue()
        {
            // Arrange
            DateTime openingDate = DateTime.Now;
            DateTime expirationDate = openingDate.AddDays(2);

            Stock stock = new()
            {
                OpeningDate = openingDate,
                ExpirationDate = expirationDate
            };

            // Act
            string? countdown = stock.Countdown;

            // Convertir la différence en jours, heures et minutes
            TimeSpan difference = expirationDate - DateTime.Now;
            int expectedDays = (int)difference.TotalDays;
            int expectedHours = difference.Hours;

            // Assert
            Assert.Equal($"{expectedDays}j {expectedHours}h", countdown); // Vérifie le compte à rebours
        }

        [Fact]
        public void Stock_ExpiredCountdown_ReturnsCorrectValue()
        {
            // Arrange         
            Stock stock = new()
            {
                OpeningDate = DateTime.Now,
                ExpirationDate = DateTime.Now.AddDays(-2),
            };

            // Assert
            Assert.Equal("Expiré", stock.Countdown);
        }
    }
}
