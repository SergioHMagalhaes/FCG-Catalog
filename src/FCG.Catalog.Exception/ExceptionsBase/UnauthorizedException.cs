using System.Net;

namespace FCG.Catalog.Exception.ExceptionsBase;

public class UnauthorizedException(string message) : FCGCatalogException(message)
{
    public override int StatusCode => (int)HttpStatusCode.Unauthorized;

    public override List<string> GetErrors()
    {
        return [Message];
    }
}
