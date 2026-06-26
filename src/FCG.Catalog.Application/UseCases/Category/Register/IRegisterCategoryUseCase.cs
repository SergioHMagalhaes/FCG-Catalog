using FCG.Catalog.Communication.Requests;
using FCG.Catalog.Communication.Responses;

namespace FCG.Catalog.Application.UseCases.Category.Register;

public interface IRegisterCategoryUseCase
{
    Task<ResponseRegisterdCategoryJson> Execute(RequestCategoryJson request);
}
