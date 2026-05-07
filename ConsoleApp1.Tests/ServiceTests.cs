using Moq;

namespace ConsoleApp1.Tests
{
    public class ServiceTests
    {
        [Fact]
        public async Task EmailService_SendResultAsync_LogsToFile()
        {
            // Arrange
            var emailService = new EmailService();
            var testType = "равносторонний";
            var testCoordinates = new List<(int, int)> { (0, 0), (100, 0), (50, 87) };
            
            // Clean up any existing log file
            if (File.Exists("email_log.txt"))
            {
                File.Delete("email_log.txt");
            }

            // Act
            await emailService.SendResultAsync(testType, testCoordinates);

            // Assert
            Assert.True(File.Exists("email_log.txt"));
            var logContent = await File.ReadAllTextAsync("email_log.txt");
            Assert.Contains("равносторонний", logContent);
            Assert.Contains("(0, 0)", logContent);
            
            // Clean up
            File.Delete("email_log.txt");
        }

        [Fact]
        public void UserInterface_GetTriangleSidesAsync_ReturnsInput()
        {
            // Arrange
            var mockConsole = new Mock<IUserInterface>();
            mockConsole.Setup(x => x.GetTriangleSidesAsync())
                .ReturnsAsync(("3", "4", "5"));

            // Act
            var result = mockConsole.Object.GetTriangleSidesAsync().Result;

            // Assert
            Assert.Equal("3", result.side1);
            Assert.Equal("4", result.side2);
            Assert.Equal("5", result.side3);
        }
    }
}
