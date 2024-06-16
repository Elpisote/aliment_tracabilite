using aliment_backend.Entities;

namespace unit_test.Entities_test
{
    public class HistoricalTest : IClassFixture<ApplicationDbContextMoq>
    {
        private readonly ApplicationDbContextMoq _dbContextMoq;

        public HistoricalTest(ApplicationDbContextMoq dbContextMoq)
        {
            _dbContextMoq = dbContextMoq;
        }

        [Fact]
        public void Historical_Id_IsGenerated()
        {
            // Arrange
            Historical historical = new(DateTime.Now, "Création", 1);

            // Act
            _dbContextMoq?.Context?.Historical?.Add(historical);
            _dbContextMoq?.Context.SaveChanges();

            // Assert
            Assert.NotEqual(0, historical.Id); 
        }
    }
}
