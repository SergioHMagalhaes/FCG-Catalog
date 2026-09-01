namespace FCG.Catalog.Domain.Services.LoggedUser;

public interface ILoggedUser
{
    Guid GetId();
    string GetName();
    bool IsAdmin();
}
