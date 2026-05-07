using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace ConsoleApp1.Tests
{
    public class IntegrationTests
    {
        [Fact]
        public async Task TriangleController_ProcessTriangle_NewTriangle_AddsToDatabase()
        {
            // Arrange
            var services = new ServiceCollection();
            
            // In-memory database
            services.AddDbContext<Lab8DbContext>(options =>
                options.UseInMemoryDatabase("TestDb_NewTriangle"));

            // Mock dependencies
            var mockUserInterface = new Mock<IUserInterface>();
            mockUserInterface.Setup(x => x.GetTriangleSidesAsync())
                .ReturnsAsync(("3", "4", "5"));

            var mockEmailService = new Mock<IEmailService>();

            services.AddScoped<IDatabaseService, DatabaseService>();
            services.AddScoped(_ => mockUserInterface.Object);
            services.AddScoped(_ => mockEmailService.Object);
            services.AddScoped<TriangleController>();

            var serviceProvider = services.BuildServiceProvider();

            // Act
            using (var scope = serviceProvider.CreateScope())
            {
                var controller = scope.ServiceProvider.GetRequiredService<TriangleController>();
                await controller.ProcessTriangleAsync();
            }

            // Assert
            using (var scope = serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<Lab8DbContext>();
                var record = await context.TriangleRecords.FirstOrDefaultAsync();
                
                Assert.NotNull(record);
                Assert.Equal(3f, record.Side1);
                Assert.Equal(4f, record.Side2);
                Assert.Equal(5f, record.Side3);
                Assert.Equal("разносторонний", record.TriangleType);
                Assert.Empty(record.ErrorMessage);
                
                // Verify email was sent
                mockEmailService.Verify(x => x.SendResultAsync("разносторонний", It.IsAny<List<(int, int)>>()), Times.Once);
            }
        }

        [Fact]
        public async Task TriangleController_ProcessTriangle_ExistingTriangle_RetrievesFromDatabase()
        {
            // Arrange
            var services = new ServiceCollection();
            
            // In-memory database with existing record
            services.AddDbContext<Lab8DbContext>(options =>
                options.UseInMemoryDatabase("TestDb_ExistingTriangle"));

            var mockUserInterface = new Mock<IUserInterface>();
            mockUserInterface.Setup(x => x.GetTriangleSidesAsync())
                .ReturnsAsync(("3", "3", "3"));

            var mockEmailService = new Mock<IEmailService>();

            services.AddScoped<IDatabaseService, DatabaseService>();
            services.AddScoped(_ => mockUserInterface.Object);
            services.AddScoped(_ => mockEmailService.Object);
            services.AddScoped<TriangleController>();

            var serviceProvider = services.BuildServiceProvider();

            // Add existing record
            using (var scope = serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<Lab8DbContext>();
                var existingRecord = new TriangleRecord
                {
                    Id = 12345,
                    Side1 = 3f,
                    Side2 = 3f,
                    Side3 = 3f,
                    TriangleType = "равносторонний",
                    ErrorMessage = "",
                    Coordinates = "0,0;100,0;50,87"
                };
                context.TriangleRecords.Add(existingRecord);
                await context.SaveChangesAsync();
            }

            // Act
            using (var scope = serviceProvider.CreateScope())
            {
                var controller = scope.ServiceProvider.GetRequiredService<TriangleController>();
                await controller.ProcessTriangleAsync();
            }

            // Assert
            using (var scope = serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<Lab8DbContext>();
                var records = await context.TriangleRecords.ToListAsync();
                
                // Should still be only one record
                Assert.Single(records);
                Assert.Equal("равносторонний", records[0].TriangleType);
                
                // Verify email was sent
                mockEmailService.Verify(x => x.SendResultAsync("равносторонний", It.IsAny<List<(int, int)>>()), Times.Once);
            }
        }

        [Fact]
        public async Task DatabaseService_CRUDOperations_WorksCorrectly()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<Lab8DbContext>()
                .UseInMemoryDatabase("TestDb_CRUD")
                .Options;

            using (var context = new Lab8DbContext(options))
            {
                var databaseService = new DatabaseService(context);
                
                var testRecord = new TriangleRecord
                {
                    Side1 = 5f,
                    Side2 = 5f,
                    Side3 = 8f,
                    TriangleType = "равнобедренный",
                    ErrorMessage = "",
                    Coordinates = "0,0;80,0;40,60"
                };

                // Act & Assert - Add
                await databaseService.AddTriangleRecordAsync(testRecord);
                
                var retrieved = await databaseService.GetTriangleRecordAsync(5f, 5f, 8f);
                Assert.NotNull(retrieved);
                Assert.Equal("равнобедренный", retrieved.TriangleType);

                // Act & Assert - Delete
                await databaseService.DeleteTriangleRecordAsync(5f, 5f, 8f);
                
                var deleted = await databaseService.GetTriangleRecordAsync(5f, 5f, 8f);
                Assert.Null(deleted);
            }
        }
    }
}
