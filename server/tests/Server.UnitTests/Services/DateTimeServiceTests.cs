using FluentAssertions;
using Server.API.Services;

namespace Server.UnitTests.Services;

public class DateTimeServiceTests
{
  [Fact]
  public void Now_ReturnsValidResponse()
  {
    // Arrange
    var dateTimeService = new DateTimeService();

    // Act
    var response = dateTimeService.Now();

    // Assert
    response.Should().NotBe(null);
  }
}
