using FCG.Catalog.Application.UseCases.Category.GetAll;
using FCG.Catalog.Application.UseCases.Category.Register;
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
    }

}
