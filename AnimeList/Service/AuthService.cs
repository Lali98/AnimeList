using AnimeList.Dtos.Auth;
using AnimeList.Models;
using AnimeList.Service.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AnimeList.Service
{
    public class AuthService (UserManager<ApplicationUser> userManager, IConfiguration configuration) : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IConfiguration _configuration = configuration;

        public async Task<(bool Success, string? Error)> RegisterAsync(RegisterDto dto)
        {
            var existingUser = await _userManager.FindByNameAsync(dto.UserName);

            if (existingUser is not null)
                return (false, "Ez a felhasználó már foglalt.");

            var existingEmail = await _userManager.FindByEmailAsync(dto.Email);

            if (existingEmail is not null)
                return (false, "Ez a email cím már foglalt.");

            var user = new ApplicationUser
            {
                UserName = dto.UserName,
                Email = dto.Email,
            };

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
            {
                var error = string.Join(", ", result.Errors.Select(x => x.Description));
                return (false, error);
            }
            return (true, null);
        }

        public async Task<AuthResponseDto?> LoginAsync(LoginDto dto)
        {
            var user = await _userManager.FindByNameAsync(dto.UserName);

            if (user is null)
            {
                return null;
            }

            var passwordValid = await _userManager.CheckPasswordAsync(user, dto.Password);

            if (!passwordValid)
                return null;

            return GenerateToken(user);
        }

        private AuthResponseDto GenerateToken(ApplicationUser user)
        {
            var jwtSettings = _configuration.GetSection("Jwt");

            var key = jwtSettings["Key"]
                ?? throw new InvalidOperationException("JWT Key nincs konfigurálva.");

            var issuer = jwtSettings["Issuer"]
                ?? throw new InvalidOperationException("JWT Issuer nincs konfigurálva.");

            var audience = jwtSettings["Audience"]
                ?? throw new InvalidOperationException("JWT Audience nincs konfigurálva.");

            var expiresAt = DateTime.UtcNow.AddHours(24);

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, user.Id),
                new(JwtRegisteredClaimNames.UniqueName, user.UserName!),
                new(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty)
            };

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

            var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims:  claims,
                expires:  expiresAt,
                signingCredentials: credentials);

            return new AuthResponseDto
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                ExpiresAt = expiresAt,
            };
        }
    }
}
