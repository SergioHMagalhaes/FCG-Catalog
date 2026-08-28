using Bogus;
using FCG.Catalog.Domain.Services.LoggedUser;
using Moq;

namespace CommonTestUtilities.Services;

public class LoggedUserBuilder
{
    public static ILoggedUser Build(Guid userId, string? userName = null)
    {
        var mock = new Mock<ILoggedUser>();

        mock.Setup(loggedUser => loggedUser.GetId()).Returns(userId);
        mock.Setup(loggedUser => loggedUser.GetName()).Returns(userName ?? new Faker().Person.FullName);

        return mock.Object;
    }
}
