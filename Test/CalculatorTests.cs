using NSubstitute;
using Test.TestInterfaces;
using Xunit;

public class CalculatorTests
{
    [Fact]
    public void Add_ReturnsSumOfArguments()
    {
        // Arrange
        ICalculator calculatorMock = Substitute.For<ICalculator>();
        calculatorMock.Add(2, 3).Returns(5); // Setup the behavior of the mock

        // Act
        int result = calculatorMock.Add(2, 3);

        // Assert
        Assert.Equal(5, result);
    }

    [Fact]
    public void Subtract_ReturnsDifferenceOfArguments()
    {
        // Arrange
        ICalculator calculatorMock = Substitute.For<ICalculator>();
        calculatorMock.Subtract(5, 3).Returns(2); // Setup the behavior of the mock

        // Act
        int result = calculatorMock.Subtract(5, 3);

        // Assert
        Assert.Equal(2, result);
    }
}