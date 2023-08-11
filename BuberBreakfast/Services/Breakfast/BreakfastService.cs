using BuberBreakfast.Services.Breakfasts;
using BuberBreakfast.Models;
using ErrorOr;
using BuberBreakfast.ServiceErrors;

namespace BuberBreakfast.Services.Breakfasts;
public class BreakfastService : IBreakfastService
{
    // temp in-memory DB
    private static readonly Dictionary<Guid, Breakfast> _breakfast = new();
    public void CreateBreakfast( Breakfast breakfast )
    {
        _breakfast.Add( breakfast.Id, breakfast );
    }

    public ErrorOr<Breakfast> GetBreakfast( Guid id )
    {
        if( _breakfast.TryGetValue(id, out var breakfast) )
        {
            return breakfast;
        }
        //return _breakfast[id];
        return Errors.Breakfast.NotFound;
    }

    public void UpsertBreakfast( Guid id, Breakfast breakfast )
    {
        _breakfast[breakfast.Id] = breakfast;
    }

    public void DeleteBreakfast( Guid id )
    {
        _breakfast.Remove( id );
    }
}