using UserService.Application.DTOs;
using UserService.Domain.Entities;

namespace UserService.Application
{
    public static class Mapper
    {
        public static UserResponseDTO ToDTO(User user)
        {
            return new UserResponseDTO
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName
            };
        }


        public static User ToEntity(UserRegisterDTO registerDTO, string hashedPassword)
        {
            return new User
            {
                Email = registerDTO.Email,
                FirstName = registerDTO.FirstName,
                LastName = registerDTO.LastName,
                PasswordHash = hashedPassword
            };
        }
    }
}
