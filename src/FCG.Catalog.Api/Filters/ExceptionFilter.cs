using FCG.Catalog.Communication.Responses;
using FCG.Catalog.Exception.ExceptionsBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FCG.Catalog.Api.Filters;

public class ExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is FCGCatalogException)
        {
            HandleProjectException(context);
        }
        else
        {
            ThrowUnkowError(context);
        }
    }

    private void HandleProjectException(ExceptionContext context)
    {
        var fcgCatalogException = (FCGCatalogException)context.Exception;
        var errorResponse = new ResponseErrorJson(fcgCatalogException.GetErrors());

        context.HttpContext.Response.StatusCode = fcgCatalogException.StatusCode;
        context.Result = new ObjectResult(errorResponse);
    }

    private void ThrowUnkowError(ExceptionContext context)
    {
        var errorResponse = new ResponseErrorJson("Erro desconhecido.");

        context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Result = new ObjectResult(errorResponse);
    }
}
