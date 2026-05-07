using Xunit;

namespace ConsoleApp1.Tests
{
    public class TriangleCalculatorTests
    {
        [Fact]
        public void CalculateTriangle_ValidEquilateralTriangle_ReturnsCorrectResult()
        {
            // Arrange
            string side1 = "3";
            string side2 = "3";
            string side3 = "3";

            // Act
            var result = TriangleCalculator.CalculateTriangle(side1, side2, side3);

            // Assert
            Assert.Equal("равносторонний", result.type);
            Assert.Empty(result.error);
            Assert.Equal(3, result.coordinates.Count);
        }

        [Fact]
        public void CalculateTriangle_ValidIsoscelesTriangle_ReturnsCorrectResult()
        {
            // Arrange
            string side1 = "5";
            string side2 = "5";
            string side3 = "8";

            // Act
            var result = TriangleCalculator.CalculateTriangle(side1, side2, side3);

            // Assert
            Assert.Equal("равнобедренный", result.type);
            Assert.Empty(result.error);
            Assert.Equal(3, result.coordinates.Count);
        }

        [Fact]
        public void CalculateTriangle_ValidScaleneTriangle_ReturnsCorrectResult()
        {
            // Arrange
            string side1 = "3";
            string side2 = "4";
            string side3 = "5";

            // Act
            var result = TriangleCalculator.CalculateTriangle(side1, side2, side3);

            // Assert
            Assert.Equal("разносторонний", result.type);
            Assert.Empty(result.error);
            Assert.Equal(3, result.coordinates.Count);
        }

        [Fact]
        public void CalculateTriangle_InvalidSides_ReturnsError()
        {
            // Arrange
            string side1 = "1";
            string side2 = "2";
            string side3 = "10";

            // Act
            var result = TriangleCalculator.CalculateTriangle(side1, side2, side3);

            // Assert
            Assert.Equal("не треугольник", result.type);
            Assert.Empty(result.error);
            Assert.Equal(3, result.coordinates.Count);
            Assert.Equal((-1, -1), result.coordinates[0]);
        }

        [Fact]
        public void CalculateTriangle_NonNumericInput_ReturnsError()
        {
            // Arrange
            string side1 = "abc";
            string side2 = "4";
            string side3 = "5";

            // Act
            var result = TriangleCalculator.CalculateTriangle(side1, side2, side3);

            // Assert
            Assert.Empty(result.type);
            Assert.Equal("Некорректные числа", result.error);
            Assert.Equal(3, result.coordinates.Count);
            Assert.Equal((-2, -2), result.coordinates[0]);
        }
    }
}
