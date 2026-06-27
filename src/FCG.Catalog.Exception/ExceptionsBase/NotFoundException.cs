using System.Net;

namespace FCG.Catalog.Exception.ExceptionsBase;

public class NotFoundException : FCGCatalogException
{
    public NotFoundException(string message) : base(message)
    {
    }

    public override int StatusCode => (int)HttpStatusCode.NotFound;

    public override List<string> GetErrors()
    {
        return [Message];
    }
}
