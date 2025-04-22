using FluentResults;
using LAHJAAPI.Utilities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace LAHJAAPI.Services2
{

    public class TokenService(IOptions<AppSettings> options)
    {
        private readonly ILogger _logger;
        private AppSettings AppSettings => options.Value;


        public static string GenerateSecureToken(int length = 32)
        {
            byte[] randomBytes = new byte[length];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
            }
            return Convert.ToBase64String(randomBytes);
        }

        public string GenerateTemporaryToken(string secret, List<Claim> claims, DateTime? expirees = null)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: AppSettings.Jwt.validIssuer,
                audience: AppSettings.Jwt.ValidAudience,
                claims: claims,
                expires: expirees ?? DateTime.UtcNow.AddDays(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateTemporaryToken(string requestId, string eventId, string clientId, DateTime? expirees = null)
        {
            var claims = new[]
            {
            new Claim("RequestId", requestId),
            new Claim("EventId", eventId),
            new Claim("ClientId", clientId),
            //new Claim("tokenService", tokenService),
            new Claim(JwtRegisteredClaimNames.Exp, DateTimeOffset.UtcNow.AddDays(1).ToUnixTimeSeconds().ToString())
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AppSettings.Jwt.Secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: AppSettings.Jwt.validIssuer,
                audience: AppSettings.Jwt.ValidAudience,
                claims: claims,
                expires: expirees ?? DateTime.UtcNow.AddDays(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateToken(List<Claim>? claims = null, DateTime? expires = null)
        {
            claims = claims ?? [];
            //claims.Add(new Claim(JwtRegisteredClaimNames.Exp, DateTimeOffset.UtcNow.AddMinutes(30).ToUnixTimeSeconds().ToString()));
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AppSettings.Jwt.Secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: AppSettings.Jwt.validIssuer,
                audience: AppSettings.Jwt.ValidAudience,
                claims: claims,
                expires: expires ?? DateTime.UtcNow.AddDays(30),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public Result<ClaimsPrincipal> ValidateToken(string token, string secret, string? audience = null)
        {
            if (string.IsNullOrEmpty(token))
            {
                return Result.Fail("Invalid token");
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(secret);

            try
            {
                var claimsPrincipal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {

                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = AppSettings.Jwt.validIssuer,
                    ValidAudience = audience ?? AppSettings.Jwt.ValidAudience,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateLifetime = true // تحقق من انتهاء الصلاحية
                }, out SecurityToken validatedToken);


                return Result.Ok(claimsPrincipal);
            }
            catch (SecurityTokenExpiredException ex)
            {
                return Result.Fail(new Error($"The token is expired.").CausedBy(ex));
            }
            catch (Exception ex)
            {
                return Result.Fail(new Error($"Token validation failed: {ex.Message}").CausedBy(ex));
            }
        }

        public Result<ClaimsPrincipal> ValidateToken(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return Result.Fail(new Error("Invalid token"));
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(AppSettings.Jwt.Secret);

            try
            {
                var claimsPrincipal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = AppSettings.Jwt.validIssuer,
                    ValidAudience = AppSettings.Jwt.ValidAudience,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateLifetime = true // تحقق من انتهاء الصلاحية
                }, out SecurityToken validatedToken);



                return Result.Ok(claimsPrincipal);
            }
            catch (Exception ex)
            {
                return Result.Fail(new Error($"Token validation failed: {ex.Message}").CausedBy(ex));
            }
        }
    }

}
