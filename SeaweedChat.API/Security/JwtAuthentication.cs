using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;    
using Microsoft.AspNetCore.Authentication.JwtBearer;
namespace SeaweedChat.API.Security;
public class JwtAuthentication : IAuthentication
{
    private readonly IConfiguration _config;
    public JwtAuthentication(IConfiguration config)
    {
        _config = config ?? throw new ArgumentNullException(nameof(config));
    }
    public void Configure(IServiceCollection services) => services
        .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options => 
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = _config["Jwt:Issuer"],
                ValidateLifetime = true,
                ValidateAudience = true,
                ValidAudience =  _config["Jwt:Audience"],
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(_config["Jwt:Key"] ?? "the most secret key"))
            };
        });
    public string Authenticate(IEnumerable<Claim> claims)
    {
        var claimsIdentity = new ClaimsIdentity(claims, "Token");

        var securityKey = new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(_config["Jwt:Key"] ?? "the most secret key"));    
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);    

        var time = DateTime.UtcNow;
        var jwt = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            notBefore: time,
            expires: time.Add(TimeSpan.FromMinutes(_config.GetValue<double>("Jwt:Lifetime"))),
            claims: claimsIdentity.Claims,
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }
}