using ConferenceRoomApi.Domain.Pricing;

namespace ConferenceRoomApi.Tests;

public class MorningPricingRuleTests
{
    [Theory]
    [InlineData(6, 8, true)]
    [InlineData(8, 9, true)]
    [InlineData(9, 10, false)]
    [InlineData(5, 7, false)]
    public void ShouldCheckMorningInterval(
        int startHour,
        int endHour,
        bool expected)
    {
        // Arrange
        var rule = new MorningPricingRule();

        var interval = new TimeInterval(
            new DateTime(2026, 9, 17, startHour, 0, 0),
            new DateTime(2026, 9, 17, endHour, 0, 0));

        // Act
        var result = rule.IsApplicable(interval);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ShouldReturnNinetyPercentMultiplier()
    {
        // Arrange
        var rule = new MorningPricingRule();

        // Act
        var result = rule.GetMultiplier();

        // Assert
        Assert.Equal(0.90m, result);
    }
}