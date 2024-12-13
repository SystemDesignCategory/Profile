using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FakeIDP;

public class AuthService
{
    public string CreateToken(User user)
    {

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.UserName),
            new Claim(ClaimTypes.Sid, user.UserId.ToString())
        };

        foreach (var item in user.UserData)
        {
            claims.Add(new Claim(item.Key, item.Value));
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("myFakeIdpService@SymmetricSecretKey"));
        var signingCredentials = new SigningCredentials(key , SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: "https://localhost:7124",
            audience: "System-Design-FakeIDP", 
            claims: claims,
            expires: DateTime.Now.AddHours(7),
            signingCredentials: signingCredentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}