using FCG.Catalog.Application.UseCases.GameOrders.GetById;
using FCG.Catalog.Application.UseCases.GameOrders.GetUserOrders;
using FCG.Catalog.Application.UseCases.GameOrders.Place;
using FCG.Catalog.Application.UseCases.Games.GetById;
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

    [HttpGet]
    [ProducesResponseType(typeof(ResponsePlaceGameOrderJson), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> getUserOrders(
        [FromServices] IGetUserGamerOrderUseCase useCase)
    {
        var response = await useCase.Execute();

        if (response.GameOrders.Count != 0)
            return Ok(response);

        return NoContent();
    }

    [HttpGet]
    [Route("{id}")]
    [ProducesResponseType(typeof(ResponseGameOrdersJson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(
        [FromServices] IGetGameOrderByIdUseCase useCase,
        [FromRoute] long id)
    {
        var response = await useCase.Execute(id);

        return Ok(response);
    }
}
