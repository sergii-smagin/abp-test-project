namespace ConferenceRoomApi.Domain.Pricing;

public interface IPricingRule
{
    int Priority { get; }

    bool IsApplicable(TimeInterval interval);

    decimal GetMultiplier();
}