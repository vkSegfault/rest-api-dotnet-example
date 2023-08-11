using Microsoft.AspNetCore.Mvc;
using BuberBreakfast.Contracts.Breakfasts;
using BuberBreakfast.Models;
using BuberBreakfast.Services.Breakfasts;
using ErrorOr;
using BuberBreakfast.ServiceErrors;

namespace BuberBreakfast.Controllers;

[ApiController]
[Route("[controller]")]
public class BreakfastsController: ApiController
{

    // dependency injection
    private readonly IBreakfastService? _breakfastService;
    public BreakfastsController(IBreakfastService breakfastService)
    {
        _breakfastService = breakfastService;
    }

    [HttpPost()]
    public IActionResult CreateBreakfast(CreateBreakfastRequest request)
    {
        // map Request (defined by contract) to our internal Model representation
        var breakfast = new Breakfast(
            Guid.NewGuid(),
            request.Name,
            request.Description,
            request.StartDateTime,
            request.EndDateTime,
            DateTime.UtcNow,
            request.Savory,
            request.Sweet
        );

        // do whataver we want with MODEL and then...
        // this should call Service, Controller is not suppose to handle such intrinsics
        // TODO: save to DB
        _breakfastService!.CreateBreakfast(breakfast);

        // map our Model representation to Response (defined by contract) so we can return it
        var response = new BreakfastResponse(
            breakfast.Id,
            breakfast.Name!,
            breakfast.Description!,
            breakfast.StartDateTime,
            breakfast.LastModifiedTime,
            breakfast.Savory!,
            breakfast.Sweet!
        );

        return CreatedAtAction(
            actionName: nameof(GetBreakfast),
            routeValues: new { id = breakfast.Id },  // id = is for {id:guid} from GET
            value: response
            );
    }
 
    // id is of type guid (can't be e.g.: string)
    [HttpGet("api/{id:guid}")]
    public IActionResult GetBreakfast(Guid id)
    {
        ErrorOr<Breakfast> getBreakfastResult = _breakfastService!.GetBreakfast(id);
        
        return getBreakfastResult.Match(
            breakfast => Ok(MapBreakfastResponse(breakfast)),
            errors => Problem(errors)
        );

        // if( getBreakfastResult.IsError && getBreakfastResult.FirstError == Errors.Breakfast.NotFound )
        // {
        //     return NotFound();
        // }
        
        // var breakfast = getBreakfastResult.Value; 
        // var response = MapBreakfastResponse(breakfast);
        // return Ok(response);
    }

    private static BreakfastResponse MapBreakfastResponse(Breakfast breakfast)
    {
        return new BreakfastResponse(
            breakfast.Id,
            breakfast.Name!,
            breakfast.Description!,
            breakfast.StartDateTime,
            breakfast.EndDateTime,
            breakfast.Savory!,
            breakfast.Sweet!
        );
    }

    [HttpPut("api/{id:guid}")]
    public IActionResult UpsertBreakfast(Guid id, UpsertBreakfastRequest request)
    {
        var breakfast = new Breakfast(
            id,
            request.Name,
            request.Description,
            request.StartDateTime,
            request.EndDateTime,
            DateTime.UtcNow,
            request.Savory,
            request.Sweet
        );

        _breakfastService!.UpsertBreakfast( id, breakfast );

        return Ok(request);
    }

    [HttpDelete("api/{id:guid}")]
    public IActionResult DeleteBreakfast(Guid id)
    {
        _breakfastService!.DeleteBreakfast( id );
        return NoContent();
    }
}