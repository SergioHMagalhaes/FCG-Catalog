using FCG.Catalog.Application.Extensions;
using FCG.Catalog.Communication.Responses;
using FCG.Catalog.Domain.Repositories;
using FCG.Catalog.Exception.ExceptionsBase;

namespace FCG.Catalog.Application.UseCases.Category.GetById;

public class GetCategoryByIdUseCase : IGetCategoryByIdUseCase
{
    private readonly ICategoryRepository _repository;
    public GetCategoryByIdUseCase(ICategoryRepository repository)
    {
        _repository = repository;
    }
    public async Task<ResponseCategoryJson> Execute(long id)
    {
        var result = await _repository.GetById(id);

        if (result == null)
            throw new NotFoundException("Categoria não encontrada");

        return result.MapToResponse();
    }
}
