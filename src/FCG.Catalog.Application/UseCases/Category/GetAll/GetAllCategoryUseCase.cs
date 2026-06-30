using FCG.Catalog.Application.Extensions;
using FCG.Catalog.Communication.Responses;
using FCG.Catalog.Domain.Repositories;

namespace FCG.Catalog.Application.UseCases.Category.GetAll;

public class GetAllCategoryUseCase : IGetAllCategoryUseCase
{
    private readonly ICategoryRepository _repository;
    public GetAllCategoryUseCase(ICategoryRepository repository)
    {
        _repository = repository;
    }
    public async Task<ResponseCategoriesJson> Execute()
    {
        var result = await _repository.GetAll();

        return new ResponseCategoriesJson
        {
            Categories = result.MapToResponse()
        };
    }
}
