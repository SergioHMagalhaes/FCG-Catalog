using FCG.Catalog.Application.UseCases.GameOrders.Place;
using FCG.Catalog.Communication.Requests;
using FCG.Catalog.Communication.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FCG.Catalog.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GameOrdersController : ControllerBase
{
    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(ResponseRegisterdGameJson), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PlaceOrder(
    [FromServices] IPlaceGameOrderUseCase useCase,
    [FromBody] RequestPlaceGameOrderJson request)
    {
        var response = await useCase.Execute(request);

        return Created(string.Empty, response);
    }
}
