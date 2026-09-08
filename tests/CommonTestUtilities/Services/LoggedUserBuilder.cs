using Bogus;
using FCG.Catalog.Domain.Services.LoggedUser;
using Moq;

namespace CommonTestUtilities.Services;

public class LoggedUserBuilder
{
    public static ILoggedUser Build(Guid userId, string? userName = null, bool isAdmin = false)
    {
        var mock = new Mock<ILoggedUser>();

        mock.Setup(loggedUser => loggedUser.GetId()).Returns(userId);
        mock.Setup(loggedUser => loggedUser.GetName()).Returns(userName ?? new Faker().Person.FullName);
        mock.Setup(loggedUser => loggedUser.IsAdmin()).Returns(isAdmin);

        return mock.Object;
    }
}
