using ConferenceRoomApi.Domain.Pricing;

namespace ConferenceRoomApi.Tests;

public class PricingRulePriorityTests
{
    [Fact]
    public void PeakShouldHaveHigherPriorityThanStandard()
    {
        // Arrange
        var peakRule = new PeakPricingRule();
        var standardRule = new StandardPricingRule();

        // Act
        var peakPriority = peakRule.Priority;
        var standardPriority = standardRule.Priority;

        // Assert
        Assert.True(peakPriority > standardPriority);
    }
}