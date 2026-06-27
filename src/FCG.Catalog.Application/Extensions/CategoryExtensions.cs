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

    public static List<ResponseCategoryJson> MapToResponse(this List<Category> categories)
    {
        return [.. categories.Select(c => new ResponseCategoryJson
        {
            Id = c.Id,
            Name = c.Name,
        })];
    }
}
