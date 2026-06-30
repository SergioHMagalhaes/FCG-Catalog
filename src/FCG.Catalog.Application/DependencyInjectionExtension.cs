using FCG.Catalog.Application.UseCases.Category.Delete;
using FCG.Catalog.Application.UseCases.Category.GetAll;
using FCG.Catalog.Application.UseCases.Category.GetById;
using FCG.Catalog.Application.UseCases.Category.Register;
using FCG.Catalog.Application.UseCases.Category.Update;
using FCG.Catalog.Application.UseCases.Game.GetAll;
using FCG.Catalog.Application.UseCases.Game.Register;
using Microsoft.Extensions.DependencyInjection;

namespace FCG.Catalog.Application;

public static class DependencyInjectionExtension
{
    public static void AddApplication(this IServiceCollection services)
    {
        AddUseCases(services);
    }

    private static void AddUseCases(IServiceCollection services)
    {
        services.AddScoped<IRegisterCategoryUseCase, RegisterCategoryUseCase>();
        services.AddScoped<IGetAllCategoryUseCase, GetAllCategoryUseCase>();
        services.AddScoped<IGetCategoryByIdUseCase, GetCategoryByIdUseCase>();
        services.AddScoped<IUpdateCategoryUseCase, UpdateCategoryUseCase>();
        services.AddScoped<IDeleteCategoryUseCase, DeleteCategoryUseCase>();
        services.AddScoped<IRegisterGameUseCase, RegisterGameUseCase>();
        services.AddScoped<IGetAllGamesUseCase, GetAllGamesUseCase>();
    }

}
