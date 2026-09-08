using FCG.Catalog.Domain.Enums;
using FCG.Catalog.Domain.Services.LoggedUser;
using FCG.Catalog.Domain.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace FCG.Catalog.Infrastructure.Services.LoggedUser;

public class LoggedUser : ILoggedUser
{
    private readonly ITokenProvider _tokenProvider;
    public LoggedUser(ITokenProvider tokenProvider)
    {
        _tokenProvider = tokenProvider;
    }
    public Guid GetId()   
    {
        string token = _tokenProvider.TokenOnRequest();

        var tokenHandler = new JwtSecurityTokenHandler();

        var jwtSecurityToken = tokenHandler.ReadJwtToken(token);

        var identifier = jwtSecurityToken.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value;

        return Guid.Parse(identifier);
    }

    public string GetName()
    {
        string token = _tokenProvider.TokenOnRequest();

        var tokenHandler = new JwtSecurityTokenHandler();

        var jwtSecurityToken = tokenHandler.ReadJwtToken(token);

        return jwtSecurityToken.Claims.First(claim => claim.Type == ClaimTypes.Name).Value;
    }

    public bool IsAdmin()
    {
        string token = _tokenProvider.TokenOnRequest();
        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtSecurityToken = tokenHandler.ReadJwtToken(token);

        var roleClaim = jwtSecurityToken.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Role);

        return roleClaim != null && roleClaim.Value == Roles.ADMIN;
    }


}
