namespace ConferenceRoomApi.Domain.Pricing;

public class StandardPricingRule : IPricingRule
{
    public int Priority => 10;
    
    public bool IsApplicable(TimeInterval interval)
    {
        var start = interval.Start.TimeOfDay;
        var end = interval.End.TimeOfDay;

        return start >= TimeSpan.FromHours(9)
            && end <= TimeSpan.FromHours(18);
    }

    public decimal GetMultiplier()
    {
        return 1.00m;
    }
}