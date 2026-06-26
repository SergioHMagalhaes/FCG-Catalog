using FCG.Catalog.Communication.Requests;
using FCG.Catalog.Communication.Responses;
using FCG.Catalog.Domain.Entities;

namespace FCG.Catalog.Application.Extensions;

public static class CategoryExtensions
{
    public static Category MapToDomain(this RequestCategoryJson request)
    {
        return new Category
        {
            Name = request.Name,
        };
    }
}
