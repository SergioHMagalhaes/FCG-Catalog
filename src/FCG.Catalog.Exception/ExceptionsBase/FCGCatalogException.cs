namespace FCG.Catalog.Exception.ExceptionsBase;

public abstract class FCGCatalogException(string message) : SystemException(message)
{
    public abstract int StatusCode { get; }
    public abstract List<string> GetErrors();
}
