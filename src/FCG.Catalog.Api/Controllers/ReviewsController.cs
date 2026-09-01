using FCG.Catalog.Application.UseCases.Reviews.Delete;
using FCG.Catalog.Application.UseCases.Reviews.GetByGameId;
using FCG.Catalog.Application.UseCases.Reviews.MarkHelpfulVotes;
using FCG.Catalog.Application.UseCases.Reviews.Register;
using FCG.Catalog.Application.UseCases.Reviews.Update;
using FCG.Catalog.Communication.Requests;
using FCG.Catalog.Communication.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FCG.Catalog.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ReviewsController : ControllerBase
{
    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(ResponseRegisterdReviewJson), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register(
    [FromServices] IRegisterReviewUseCase useCase,
    [FromBody] RequestReviewJson request)
    {
        var response = await useCase.Execute(request);

        return Created(string.Empty, response);
    }

    [HttpGet]
    [ProducesResponseType(typeof(ResponseReviewsJson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByGameId(
        [FromServices] IGetReviewsByGameIdUseCase useCase,
        [FromQuery] RequestGetReviewByGameIdJson request)
    {
        var response = await useCase.Execute(request);

        return Ok(response);
    }

    [HttpPost]
    [Authorize]
    [Route("{reviewId}/helpful-votes")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> MarkHelpfulVotes(
        [FromServices] IMarkHelpfulVotesUseCase useCase,
        [FromRoute] Guid reviewId)
    {
        await useCase.Execute(reviewId);

        return NoContent();
    }

    [HttpPut]
    [Authorize]
    [Route("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(
        [FromServices] IUpdateReviewUseCase useCase,
        [FromRoute] Guid id,
        [FromBody] RequestReviewUpdateJson request)
    {
        await useCase.Execute(id, request);

        return NoContent();
    }

    [HttpDelete]
    [Authorize]
    [Route("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(
        [FromServices] IDeleteReviewUseCase useCase,
        [FromRoute] Guid id)
    {
        await useCase.Execute(id);

        return NoContent();
    }
}
