namespace ConferenceRoomApi.Domain.Pricing;

public class MorningPricingRule : IPricingRule
{
    public int Priority => 50;

    public bool IsApplicable(TimeInterval interval)
    {
        var start = interval.Start.TimeOfDay;
        var end = interval.End.TimeOfDay;

        return start >= TimeSpan.FromHours(6)
            && end <= TimeSpan.FromHours(9);
    }

    public decimal GetMultiplier()
    {
        return 0.90m;
    }
}