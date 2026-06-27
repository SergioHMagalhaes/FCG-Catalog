using FCG.Catalog.Application.UseCases.Category.GetAll;
using FCG.Catalog.Application.UseCases.Category.Register;
using FCG.Catalog.Communication.Requests;
using FCG.Catalog.Communication.Responses;
using FCG.Catalog.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace FCG.Catalog.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoryController : ControllerBase
{
    [HttpPost]
    [Authorize(Roles = Roles.ADMIN)]
    [ProducesResponseType(typeof(ResponseRegisterdCategoryJson), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register(
        [FromServices] IRegisterCategoryUseCase useCase,
        [FromBody] RequestCategoryJson request)
    {
        var response = await useCase.Execute(request);
        
        return Created(string.Empty, response);
    }

    [HttpGet]
    [ProducesResponseType(typeof(ResponseCategoriesJson), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetAllCategories([FromServices] IGetAllCategoryUseCase useCase)
    {
        var response = await useCase.Execute();

        if (response.Categories.Count != 0)
            return Ok(response);

        return NoContent();
    }
}