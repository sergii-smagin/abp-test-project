namespace ConferenceRoomApi.Domain.Pricing;

public class PricingCalculator
{
    private readonly IEnumerable<IPricingRule> _pricingRules;

    public PricingCalculator(IEnumerable<IPricingRule> pricingRules)
    {
        _pricingRules = pricingRules;
    }

    public List<PricedInterval> SplitIntoPricingIntervals(TimeInterval bookingInterval)
    {
        var result = new List<PricedInterval>();

        var current = bookingInterval.Start;

        while (current < bookingInterval.End)
        {
            var nextBoundary = GetNextBoundary(current);

            var intervalEnd = nextBoundary < bookingInterval.End
                ? nextBoundary
                : bookingInterval.End;

            var interval = new TimeInterval(current, intervalEnd);
            var rule = GetApplicableRule(interval);

            result.Add(new PricedInterval(interval, rule));

            current = intervalEnd;
        }

        return result;
    }

    public decimal Calculate(
        TimeInterval bookingInterval,
        decimal baseHourlyRate)
    {
        var pricedIntervals = SplitIntoPricingIntervals(bookingInterval);

        return pricedIntervals
            .Sum(interval => CalculateIntervalPrice(interval, baseHourlyRate));
    }

    private decimal CalculateIntervalPrice(
        PricedInterval pricedInterval,
        decimal baseHourlyRate)
    {
        var duration = pricedInterval.Interval.End
            - pricedInterval.Interval.Start;

        var hours = (decimal)duration.TotalHours;

        return hours
            * baseHourlyRate
            * pricedInterval.Rule.GetMultiplier();
    }

    private DateTime GetNextBoundary(DateTime current)
    {
        var boundaries = new[]
        {
            TimeSpan.FromHours(9),
            TimeSpan.FromHours(12),
            TimeSpan.FromHours(14),
            TimeSpan.FromHours(18),
            TimeSpan.FromHours(23)
        };

        foreach (var boundary in boundaries)
        {
            if (current.TimeOfDay < boundary)
            {
                return current.Date.Add(boundary);
            }
        }

        return current.Date.AddDays(1).AddHours(6);
    }

    private IPricingRule GetApplicableRule(TimeInterval interval)
    {
        return _pricingRules
            .Where(rule => rule.IsApplicable(interval))
            .OrderByDescending(rule => rule.Priority)
            .First();
    }
}