namespace ConferenceRoomApi.Domain.Pricing;

public class PeakPricingRule : IPricingRule
{
    public int Priority => 100;

    public bool IsApplicable(TimeInterval interval)
    {
        var start = interval.Start.TimeOfDay;
        var end = interval.End.TimeOfDay;

        return start >= TimeSpan.FromHours(12)
            && end <= TimeSpan.FromHours(14);
    }

    public decimal GetMultiplier()
    {
        return 1.15m;
    }
}