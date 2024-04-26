using NSubstitute;
using Test.TestInterfaces;

public class CalculatorTests
{
    [Fact]
    public void Add_ReturnsSumOfArguments()
    {
        // Arrange
        var calculatorMock = Substitute.For<ICalculator>();
        calculatorMock.Add(2, 3).Returns(5); // Setup the behavior of the mock

        // Act
        var result = calculatorMock.Add(2, 3);

        // Assert
        Assert.Equal(5, result);
    }

    [Fact]
    public void Subtract_ReturnsDifferenceOfArguments()
    {
        // Arrange
        var calculatorMock = Substitute.For<ICalculator>();
        calculatorMock.Subtract(5, 3).Returns(2); // Setup the behavior of the mock

        // Act
        var result = calculatorMock.Subtract(5, 3);

        // Assert
        Assert.Equal(2, result);
    }
}