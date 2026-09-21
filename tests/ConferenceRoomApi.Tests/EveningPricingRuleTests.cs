using ConferenceRoomApi.Domain.Pricing;

namespace ConferenceRoomApi.Tests;

public class EveningPricingRuleTests
{
    [Theory]
    [InlineData(18, 19, true)]
    [InlineData(22, 23, true)]
    [InlineData(17, 19, false)]
    public void ShouldCheckEveningInterval(
        int startHour,
        int endHour,
        bool expected)
    {
        // Arrange
        var rule = new EveningPricingRule();

        var interval = new TimeInterval(
            new DateTime(2026, 9, 17, startHour, 0, 0),
            new DateTime(2026, 9, 17, endHour, 0, 0));

        // Act
        var result = rule.IsApplicable(interval);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ShouldReturnEightyPercentMultiplier()
    {
        // Arrange
        var rule = new EveningPricingRule();

        // Act
        var result = rule.GetMultiplier();

        // Assert
        Assert.Equal(0.80m, result);
    }
}