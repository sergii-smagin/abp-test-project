using ConferenceRoomApi.Domain.Pricing;

namespace ConferenceRoomApi.Tests;

public class PricingCalculatorTests
{
    [Fact]
    public void ShouldSplitBookingAndSelectPricingRules()
    {
        // Arrange
        var calculator = new PricingCalculator(
            new List<IPricingRule>
            {
                new MorningPricingRule(),
                new StandardPricingRule(),
                new PeakPricingRule(),
                new EveningPricingRule()
            });

        var bookingInterval = new TimeInterval(
            new DateTime(2026, 9, 17, 10, 0, 0),
            new DateTime(2026, 9, 17, 20, 0, 0));

        // Act
        var result = calculator.SplitIntoPricingIntervals(bookingInterval);

        // Assert
        Assert.Equal(4, result.Count);

        Assert.Equal(
            new TimeInterval(
                new DateTime(2026, 9, 17, 10, 0, 0),
                new DateTime(2026, 9, 17, 12, 0, 0)),
            result[0].Interval);
        Assert.IsType<StandardPricingRule>(result[0].Rule);

        Assert.Equal(
            new TimeInterval(
                new DateTime(2026, 9, 17, 12, 0, 0),
                new DateTime(2026, 9, 17, 14, 0, 0)),
            result[1].Interval);
        Assert.IsType<PeakPricingRule>(result[1].Rule);

        Assert.Equal(
            new TimeInterval(
                new DateTime(2026, 9, 17, 14, 0, 0),
                new DateTime(2026, 9, 17, 18, 0, 0)),
            result[2].Interval);
        Assert.IsType<StandardPricingRule>(result[2].Rule);

        Assert.Equal(
            new TimeInterval(
                new DateTime(2026, 9, 17, 18, 0, 0),
                new DateTime(2026, 9, 17, 20, 0, 0)),
            result[3].Interval);
        Assert.IsType<EveningPricingRule>(result[3].Rule);
    }

    [Fact]
    public void ShouldCalculateTotalPrice()
    {
        // Arrange
        var calculator = new PricingCalculator(
            new List<IPricingRule>
            {
                new MorningPricingRule(),
                new StandardPricingRule(),
                new PeakPricingRule(),
                new EveningPricingRule()
            });

        var bookingInterval = new TimeInterval(
            new DateTime(2026, 9, 17, 10, 0, 0),
            new DateTime(2026, 9, 17, 20, 0, 0));

        var baseHourlyRate = 1000m;

        // Act
        var result = calculator.Calculate(
            bookingInterval,
            baseHourlyRate);

        // Assert
        Assert.Equal(9900m, result);
    }

    [Fact]
    public void ShouldCalculatePriceForPartialHour()
    {
        // Arrange
        var calculator = new PricingCalculator(
            new List<IPricingRule>
            {
                new MorningPricingRule(),
                new StandardPricingRule(),
                new PeakPricingRule(),
                new EveningPricingRule()
            });

        var bookingInterval = new TimeInterval(
            new DateTime(2026, 9, 17, 10, 30, 0),
            new DateTime(2026, 9, 17, 11, 45, 0));

        var baseHourlyRate = 1000m;

        // Act
        var result = calculator.Calculate(
            bookingInterval,
            baseHourlyRate);

        // Assert
        Assert.Equal(1250m, result);
    }

    [Fact]
    public void ShouldCalculateDifferentPricesWhenBookingCrossesPricingBoundary()
    {
        // Arrange
        var calculator = new PricingCalculator(
            new List<IPricingRule>
            {
                new MorningPricingRule(),
                new StandardPricingRule(),
                new PeakPricingRule(),
                new EveningPricingRule()
            });

        var bookingInterval = new TimeInterval(
            new DateTime(2026, 9, 17, 11, 30, 0),
            new DateTime(2026, 9, 17, 12, 30, 0));

        var baseHourlyRate = 1000m;

        // Act
        var result = calculator.Calculate(
            bookingInterval,
            baseHourlyRate);

        // Assert
        Assert.Equal(1075m, result);
    }
}