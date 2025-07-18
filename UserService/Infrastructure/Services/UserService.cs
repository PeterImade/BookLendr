using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using UserService.Application;
using UserService.Application.DTOs;
using UserService.Domain.Entities;
using UserService.Exceptions;
using UserService.Infrastructure.Repositories;

namespace UserService.Infrastructure.Services
{
    public class UserBLService
    {
        private readonly UserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private User? _user;

        public UserBLService(UserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        public async Task<UserResponseDTO> RegisterUserAsync(UserRegisterDTO registerDTO, CancellationToken cancellationToken)
        {
            var userExists = await _userRepository.CheckUserExists(registerDTO.Email, cancellationToken);

            if (userExists)
                throw new BadRequestException("Email is already taken");

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(registerDTO.Password);


            var user = Mapper.ToEntity(registerDTO, hashedPassword);

            var createdUser = await _userRepository.CreateUser(user, cancellationToken);

            var userResponseDTO = Mapper.ToDTO(createdUser);

            return userResponseDTO;
        }


        public async Task<TokenDTO> LoginUserAsync(UserLoginDTO userLoginDTO, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByEmail(userLoginDTO.Email, cancellationToken);

            if (user is null)
                throw new BadRequestException("Email or password is incorrect");

            _user = user;

            var isPasswordVerified = BCrypt.Net.BCrypt.Verify(userLoginDTO.Password, user.PasswordHash);

            if (!isPasswordVerified)
            {
                throw new BadRequestException("Email or password is incorrect");
            }

            var tokens = await CreateToken(cancellationToken);

            return tokens;
        }

        public async Task<TokenDTO> CreateToken(CancellationToken cancellationToken)
        {
            var signingCredentials = GetSigningCredentials();
            var claims = GetClaims();
            var tokenOptions = GenerateTokenOptions(signingCredentials, claims);
            var refreshToken = GenerateRefreshToken();
            await _userRepository.SaveRefreshToken(_user, refreshToken, DateTime.Now.AddDays(7), cancellationToken);
            var accessToken = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

            return new TokenDTO { AccessToken = accessToken, RefreshToken = refreshToken };
        }

        private SigningCredentials GetSigningCredentials()
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["secretKey"];
            var key = Encoding.UTF8.GetBytes(secretKey);
            var secret = new SymmetricSecurityKey(key);
            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }

        private List<Claim> GetClaims()
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, _user.Email),
                new Claim(ClaimTypes.NameIdentifier, _user.Id.ToString()),
                new Claim(ClaimTypes.Role, _user.Role.ToString())
            };

            return claims;
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
            }
            return Convert.ToBase64String(randomNumber);
        }

        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var tokenOptions = new JwtSecurityToken
            (
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings["expires"])),
                signingCredentials: signingCredentials
            );

            return tokenOptions;
        }
        public async Task<UserResponseDTO> GetUserByEmailAsync(string email, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByEmail(email, cancellationToken);

            if (user is null)
                throw new NotFoundException($"User with email:{email} not found");

            var userResponseDTO = new UserResponseDTO
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName
            };

            return userResponseDTO;
        }

        public async Task<UserResponseDTO> GetUserByIdAsync(int id, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserById(id, cancellationToken);

            if (user is null)
                throw new NotFoundException($"User with id:{id} not found");

            var userResponseDTO = new UserResponseDTO
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName
            };

            return userResponseDTO;
        }
    }
}
