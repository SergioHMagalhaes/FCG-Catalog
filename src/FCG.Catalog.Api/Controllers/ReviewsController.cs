using FCG.Catalog.Application.UseCases.Reviews.Register;
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
}
