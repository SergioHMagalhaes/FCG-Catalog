using FCG.Catalog.Domain.Repositories;
using Moq;

namespace CommonTestUtilities.Repositories;
public class UnitOfWorkBuilder
{
    private readonly Mock<IUnitOfWork> _mock;

    public UnitOfWorkBuilder()
    {
        _mock = new Mock<IUnitOfWork>();

        _mock
            .Setup(unitOfWork => unitOfWork.Commit())
            .Returns(Task.CompletedTask);
    }

    public IUnitOfWork Build()
    {
        return _mock.Object;
    }

    public void VerifyCommitOnce()
    {
        _mock.Verify(unitOfWork => unitOfWork.Commit(), Times.Once);
    }

    public void VerifyCommitNever()
    {
        _mock.Verify(unitOfWork => unitOfWork.Commit(), Times.Never);
    }
}
