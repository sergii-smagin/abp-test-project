using ConferenceRoomApi.Domain.Pricing;

namespace ConferenceRoomApi.Tests;

public class PeakPricingRuleTests
{
    [Theory]
    [InlineData(12, 13, true)]
    [InlineData(13, 14, true)]
    [InlineData(11, 13, false)]
    [InlineData(14, 15, false)]
    public void ShouldCheckPeakInterval(
        int startHour,
        int endHour,
        bool expected)
    {
        // Arrange
        var rule = new PeakPricingRule();

        var interval = new TimeInterval(
            new DateTime(2026, 9, 17, startHour, 0, 0),
            new DateTime(2026, 9, 17, endHour, 0, 0));

        // Act
        var result = rule.IsApplicable(interval);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ShouldReturnOneHundredFifteenPercentMultiplier()
    {
        // Arrange
        var rule = new PeakPricingRule();

        // Act
        var result = rule.GetMultiplier();

        // Assert
        Assert.Equal(1.15m, result);
    }
}