using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using MovieHubAPI.DTOs.Usuario;
using MovieHubAPI.Interfaces;

namespace MovieHubAPI.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly UserManager<UsuarioModel> _userManager;
        private readonly IConfiguration _configuration;

        public UsuarioService(UserManager<UsuarioModel> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        public Task<AuthResponseDto?> RegisterAsync(RegisterDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<AuthResponseDto?> LoginAsync(LoginDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<UserProfileDto?> GetProfileAsync(long userId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateProfileAsync(long userId, UpdateProfileDto dto)
        {
            throw new NotImplementedException();
        }

        private async Task<AuthResponseDto> GenerateAuthResponseAsync(UsuarioModel usuario)
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var key = jwtSettings["Key"]!;
            var issuer = jwtSettings["Issuer"]!;
            var audience = jwtSettings["Audience"]!;

            var tokenHandler = new JsonWebTokenHandler();
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                [
                    new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                    new Claim(ClaimTypes.Name, usuario.UserName!),
                    new Claim(ClaimTypes.Email, usuario.Email!)
                ]),
                Expires = DateTime.UtcNow.AddMinutes(4),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var expiration = tokenDescriptor.Expires!.Value;

            return new AuthResponseDto(
                Token: token,
                Expiration: expiration,
                UserId: usuario.Id,
                UserName: usuario.UserName!,
                Email: usuario.Email!
            );
        }
    }
}
