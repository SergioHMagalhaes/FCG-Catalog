using FCG.Catalog.Application.UseCases.Category.GetAll;
using FCG.Catalog.Application.UseCases.Game.GetAll;
using FCG.Catalog.Application.UseCases.Game.Register;
using FCG.Catalog.Communication.Requests;
using FCG.Catalog.Communication.Responses;
using FCG.Catalog.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FCG.Catalog.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GameController : ControllerBase
{
    [HttpPost]
    [Authorize(Roles = Roles.ADMIN)]
    [ProducesResponseType(typeof(ResponseRegisterdGameJson), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register(
    [FromServices] IRegisterGameUseCase useCase,
    [FromBody] RequestGameJson request)
    {
        var response = await useCase.Execute(request);

        return Created(string.Empty, response);
    }

    [HttpGet]
    [ProducesResponseType(typeof(ResponseCategoriesJson), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetAllGames(
        [FromServices] IGetAllGamesUseCase useCase,
        [FromQuery] RequestGetAllGamesJson request)
    {
        var response = await useCase.Execute(request);

        if (response.Games.Count != 0)
            return Ok(response);

        return NoContent();
    }
}
