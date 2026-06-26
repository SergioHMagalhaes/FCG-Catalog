using FCG.Catalog.Domain.Repositories;
using Moq;

namespace CommonTestUtilities.Repositories;
public class CategoryRepositoryBuilder
{
    private readonly Mock<ICategoryRepository> _repository;

    public CategoryRepositoryBuilder()
    {
        _repository = new Mock<ICategoryRepository>();
    }

    public void ExistCategoryWithName(string name)
    {
        _repository.Setup(userReadOnly => userReadOnly.ExistsByName(name)).ReturnsAsync(true);
    }
    public ICategoryRepository Build() => _repository.Object;
}
