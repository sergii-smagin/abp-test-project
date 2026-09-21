namespace ConferenceRoomApi.Domain.Pricing;

public record PricedInterval(
    TimeInterval Interval,
    IPricingRule Rule
);