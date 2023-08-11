namespace BuberBreakfast.Contracts.Breakfasts;

// Record with auto-generated properties, ctor, dtor
public record BreakfastResponse(
    Guid Id,
    string Name,
    string Description,
    DateTime StartDateTime,
    DateTime LastModifiedDateTime,
    List<string> Savory,
    List<string> Sweet
);