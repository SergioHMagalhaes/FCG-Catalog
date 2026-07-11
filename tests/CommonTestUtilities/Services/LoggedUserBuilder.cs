using FCG.Catalog.Domain.Services.LoggedUser;
using Moq;

namespace CommonTestUtilities.Services;

public class LoggedUserBuilder
{
    public static ILoggedUser Build(Guid userId)
    {
        var mock = new Mock<ILoggedUser>();

        mock.Setup(loggedUser => loggedUser.GetId()).Returns(userId);

        return mock.Object;
    }
}
