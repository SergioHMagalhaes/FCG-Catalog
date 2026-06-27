using FCG.Catalog.Application.UseCases.Category.Delete;
using FCG.Catalog.Application.UseCases.Category.GetAll;
using FCG.Catalog.Application.UseCases.Category.GetById;
using FCG.Catalog.Application.UseCases.Category.Register;
using FCG.Catalog.Application.UseCases.Category.Update;
using FCG.Catalog.Communication.Requests;
using FCG.Catalog.Communication.Responses;
using FCG.Catalog.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

    [HttpGet]
    [Route("{id}")]
    [ProducesResponseType(typeof(ResponseCategoryJson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(
        [FromServices] IGetCategoryByIdUseCase useCase,
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
        [FromServices] IUpdateCategoryUseCase useCase,
        [FromRoute] long id,
        [FromBody] RequestCategoryJson request)
    {
        await useCase.Execute(id, request);

        return NoContent();
    }

    [HttpDelete]
    [Authorize(Roles = Roles.ADMIN)]
    [Route("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(
    [FromServices] IDeleteCategoryUseCase useCase,
    [FromRoute] long id)
    {
        await useCase.Execute(id);

        return NoContent();
    }
}