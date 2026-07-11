namespace FCG.Catalog.Domain.Repositories;

public interface IUnitOfWork
{
    Task Commit();
}
