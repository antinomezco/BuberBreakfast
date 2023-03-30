using BuberBreakfast.Contracts.Breakfast;
using BuberBreakfast.Models;
using BuberBreakfast.ServiceErrors;
using BuberBreakfast.Services.Breakfasts;
using ErrorOr;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BuberBreakfast.Controllers
{
    
    public class BreakfastController : ApiController
    {
        private readonly IBreakfastService _breakfastService;
        private readonly Breakfast[] = 

        public BreakfastController(IBreakfastService breakfastService)
        {
            _breakfastService = breakfastService;
        }


        [HttpPost("")]
        public IActionResult CreateBreakFast(CreateBreakfastRequest request)
        {
            ErrorOr<Breakfast> requestToBreakfastResult = Breakfast.From(request);

            if(requestToBreakfastResult.IsError)
            {
                return Problem(requestToBreakfastResult.Errors);
            }

            var breakfast = requestToBreakfastResult.Value;

            //TODO: Save breakfast to database
            ErrorOr<Created> createBreakfastResult = _breakfastService.CreateBreakfast(breakfast);

            return createBreakfastResult.Match(
                created => CreatedAtGetBreakfast(breakfast), errors => Problem(errors));
        }

        private IActionResult CreatedAtGetBreakfast(Breakfast breakfast)
        {
            return CreatedAtAction(
                actionName: nameof(GetBreakFast),
                routeValues: new { id = breakfast.Id },
                value: MapBreakfastResponse(breakfast));
        }

        [HttpGet("{id:guid}")]
        public IActionResult GetBreakFast(Guid id)
        {
            ErrorOr<Breakfast> getBreakfastResult = _breakfastService.GetBreakfast(id);

            return getBreakfastResult.Match(
                breakfast => Ok(MapBreakfastResponse(breakfast)),
                errors => Problem(errors));
        }


        [HttpPut("{id:guid}")]
        public IActionResult UpsertBreakFast(Guid id, UpsertBreakfastRequest request)
        {
            ErrorOr<Breakfast> requestToBreakfastResult = Breakfast.From(id, request);

            if (requestToBreakfastResult.IsError)
            {
                return Problem(requestToBreakfastResult.Errors);    
            }

            var breakfast = requestToBreakfastResult.Value;

            ErrorOr<UpsertedBreakfast> upsertedBreakfastResult = _breakfastService.UpsertBreakfast(breakfast);
            // return 201 if a new breakfast was created
            return upsertedBreakfastResult.Match(upserted=> upserted.IsNewlyCreated ? CreatedAtGetBreakfast(breakfast):NoContent(), errors => Problem(errors));
        }
        [HttpDelete("{id:guid}")]
        public IActionResult DeleteBreakFast(Guid id)
        {
            ErrorOr<Deleted> deleteBreakfastResult = _breakfastService.DeleteBreakfast(id);
            return deleteBreakfastResult.Match(
                deleted => NoContent(),
                errors => Problem(errors));
        }
        private static BreakfastResponse MapBreakfastResponse(Breakfast breakfast)
        {
            return new BreakfastResponse(breakfast.Id, breakfast.Name, breakfast.Description, breakfast.StartDateTime, breakfast.EndDateTime, breakfast.LastModifiedDateTime, breakfast.Savory, breakfast.Sweet);
        }
    }
}
