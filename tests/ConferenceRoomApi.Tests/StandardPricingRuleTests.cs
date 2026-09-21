using ConferenceRoomApi.Domain.Pricing;

namespace ConferenceRoomApi.Tests;

public class StandardPricingRuleTests
{
    [Fact]
    public void ShouldApplyStandardPrice()
    {
        // Arrange
        var rule = new StandardPricingRule();

        var interval = new TimeInterval(
            new DateTime(2026, 9, 17, 9, 0, 0),
            new DateTime(2026, 9, 17, 12, 0, 0));

        // Act
        var result = rule.IsApplicable(interval);

        // Assert
        Assert.True(result);
    }
}