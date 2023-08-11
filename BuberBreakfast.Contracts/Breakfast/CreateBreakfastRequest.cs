namespace BuberBreakfast.Contracts.Breakfasts;

// Record with auto-generated properties, ctor, dtor
public record CreateBreakfastRequest(
    string Name,
    string Description,
    DateTime StartDateTime,
    DateTime EndDateTime,
    List<string> Savory,
    List<string> Sweet
);