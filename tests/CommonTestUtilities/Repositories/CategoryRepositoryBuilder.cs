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

    public CategoryRepositoryBuilder GetById(Category category)
    {
        _repository.Setup(repository => repository.GetById(category.Id)).ReturnsAsync(category);

        return this;
    }

    public CategoryRepositoryBuilder GetByIdTracked(Category category)
    {
        _repository
            .Setup(repository => repository.GetByIdTracked(category.Id))
            .ReturnsAsync(category);

        return this;
    }

    public CategoryRepositoryBuilder GetByIdTrackedNotFound()
    {
        _repository
            .Setup(repository => repository.GetByIdTracked(It.IsAny<long>()))
            .ReturnsAsync((Category?)null);

        return this;
    }

    public CategoryRepositoryBuilder GetByIdNotFound()
    {
        _repository
            .Setup(repository => repository.GetById(It.IsAny<long>()))
            .Returns(Task.FromResult<Category?>(null));

        return this;
    }
    public ICategoryRepository Build() => _repository.Object;
}
