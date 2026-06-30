using FCG.Catalog.Application.UseCases.Category.GetAll;
using FCG.Catalog.Application.UseCases.Category.GetById;
using FCG.Catalog.Application.UseCases.Category.Update;
using FCG.Catalog.Application.UseCases.Game.GetAll;
using FCG.Catalog.Application.UseCases.Game.GetById;
using FCG.Catalog.Application.UseCases.Game.Register;
using FCG.Catalog.Application.UseCases.Game.Update;
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
    [ProducesResponseType(typeof(ResponseGamesJson), StatusCodes.Status200OK)]
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

    [HttpGet]
    [Route("{id}")]
    [ProducesResponseType(typeof(ResponseGameJson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(
        [FromServices] IGetGameByIdUseCase useCase,
        [FromRoute] long id)
    {
        var response = await useCase.Execute(id);

        return Ok(response);
    }

    [HttpPut]
    [Authorize(Roles = Roles.ADMIN)]
    [Route("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(
    [FromServices] IUpdateGameUseCase useCase,
    [FromRoute] long id,
    [FromBody] RequestGameJson request)
    {
        await useCase.Execute(id, request);

        return NoContent();
    }
}
