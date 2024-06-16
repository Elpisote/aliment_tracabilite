using aliment_backend.Controllers;
using aliment_backend.Entities;
using aliment_backend;
using Microsoft.AspNetCore.Mvc;

namespace unit_test.Controller_test
{
    public class HistoricalControllerTest
    {
        private readonly ApplicationDbContextMoq _contextMock = new();
        private readonly UnitOfWorkClass _unitOfWork;
        private readonly HistoricalController _controller;

        public HistoricalControllerTest()
        {
            _unitOfWork = new UnitOfWorkClass(_contextMock.Context);
            _controller = new HistoricalController(_unitOfWork);
        }

        [Fact]
        public async Task GetAllAsync_AllHistorical_ReturnAll()
        {
            // Act
            IActionResult actionResult = await _controller.GetAllAsync();

            // Assert
            OkObjectResult okObjectResult = Assert.IsType<OkObjectResult>(actionResult);
            IEnumerable<Historical> historicals = Assert.IsAssignableFrom<IEnumerable<Historical>>(okObjectResult.Value);

            if(_contextMock.Context.Historical != null)
            {
                int expectedHistoricalCount = _contextMock.Context.Historical.Count();
                int actualHistoricalCount = historicals?.Count() ?? 0;
                Assert.Equal(expectedHistoricalCount, actualHistoricalCount);
            }            
        }

        [Fact]
        public async Task GetAllAsync_EachStockHasAssociatedProduct_ReturnNotnull()
        {
            // Act
            IActionResult actionResult = await _controller.GetAllAsync();

            // Assert
            OkObjectResult okObjectResult = Assert.IsType<OkObjectResult>(actionResult);
            IEnumerable<Historical> historicals = Assert.IsAssignableFrom<IEnumerable<Historical>>(okObjectResult.Value);

            foreach (Historical historical in historicals)
            {
                Assert.NotNull(historical.Stock);
                Assert.NotNull(historical?.Stock?.Product);
            }
        }
    }
}
