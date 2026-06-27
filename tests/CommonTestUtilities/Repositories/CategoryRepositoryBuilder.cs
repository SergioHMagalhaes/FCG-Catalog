using FCG.Catalog.Domain.Entities;
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

    public CategoryRepositoryBuilder GetAll(List<Category> categories)
    {
        _repository.Setup(repository => repository.GetAll()).ReturnsAsync(categories);

        return this;
    }

    public ICategoryRepository Build() => _repository.Object;
}
