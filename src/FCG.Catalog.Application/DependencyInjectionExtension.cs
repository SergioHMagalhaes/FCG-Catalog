using FCG.Catalog.Application.UseCases.Categories.Delete;
using FCG.Catalog.Application.UseCases.Categories.GetAll;
using FCG.Catalog.Application.UseCases.Categories.GetById;
using FCG.Catalog.Application.UseCases.Categories.Register;
using FCG.Catalog.Application.UseCases.Categories.Update;
using FCG.Catalog.Application.UseCases.GameOrders.GetById;
using FCG.Catalog.Application.UseCases.GameOrders.GetUserOrders;
using FCG.Catalog.Application.UseCases.GameOrders.Place;
using FCG.Catalog.Application.UseCases.Games.Delete;
using FCG.Catalog.Application.UseCases.Games.GetAll;
using FCG.Catalog.Application.UseCases.Games.GetById;
using FCG.Catalog.Application.UseCases.Games.Register;
using FCG.Catalog.Application.UseCases.Games.Update;
using FCG.Catalog.Application.UseCases.Libraries.GetUserLibrary;
using FCG.Catalog.Application.UseCases.Reviews.Register;
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
        services.AddScoped<IGetGameByIdUseCase, GetGameByIdUseCase>();
        services.AddScoped<IUpdateGameUseCase, UpdateGameUseCase>();
        services.AddScoped<IDeleteGameUseCase, DeleteGameUseCase>();
        services.AddScoped<IPlaceGameOrderUseCase, PlaceGameOrderUseCase>();
        services.AddScoped<IGetUserGamerOrderUseCase, GetUserGamerOrderUseCase>();
        services.AddScoped<IGetGameOrderByIdUseCase, GetGameOrderByIdUseCase>();
        services.AddScoped<IGetUserLibraryUseCase, GetUserLibraryUseCase>();
        services.AddScoped<IRegisterReviewUseCase, RegisterReviewUseCase>();
    }
}
