using FCG.Catalog.Application.UseCases.GameOrders.GetUserOrders;
using FCG.Catalog.Application.UseCases.Libraries.GetUserLibrary;
using FCG.Catalog.Communication.Responses;
using Microsoft.AspNetCore.Mvc;

namespace FCG.Catalog.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LibraryController : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(ResponsePlaceGameOrderJson), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetUserLibrary(
        [FromServices] IGetUserLibraryUseCase useCase)
    {
        var response = await useCase.Execute();

        return Ok(response);
    }
}
