namespace ConferenceRoomApi.Domain.Pricing;

public class EveningPricingRule : IPricingRule
{
    public int Priority => 50;
    
    public bool IsApplicable(TimeInterval interval)
    {
        var start = interval.Start.TimeOfDay;
        var end = interval.End.TimeOfDay;

        return start >= TimeSpan.FromHours(18)
            && end <= TimeSpan.FromHours(23);
    }

    public decimal GetMultiplier()
    {
        return 0.80m;
    }
}