using aliment_backend;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace unit_test.UnitOfWork_test
{
    public class UnitOfWorkTest
    {
        private readonly DbConnection _connection;
        private readonly DbContextOptions<ApplicationDbContext> _contextOptions;
        public ApplicationDbContext Context { get; }
        private UnitOfWorkClass _unitOfWork;

        public UnitOfWorkTest()
        {
            // Setup Mock ApplicationDbContext
            _connection = new SqliteConnection("Filename=:memory:");
            _connection.Open();

            // Configure les options du contexte de base de données
            _contextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                                .UseSqlite(_connection)
                                .Options;

            // Initialise le contexte de base de données avec les options configurées
            Context = new ApplicationDbContext(_contextOptions);

            // Setup UnitOfWorkClass with the Mock ApplicationDbContext
            _unitOfWork = new UnitOfWorkClass(Context);
        }

        [Fact]
        public void GetRepository_ThrowsArgumentException_ForUnsupportedType()
        {
            // Arrange
            Type unsupportedType = typeof(SomeUnsupportedEntity);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => _unitOfWork.GetRepository<SomeUnsupportedEntity>());
        }

        // Define a class that is not supported by your UnitOfWorkClass
        private class SomeUnsupportedEntity
        {
            public int Id { get; set; }
            public string? Name { get; set; }
        }
    }
}
