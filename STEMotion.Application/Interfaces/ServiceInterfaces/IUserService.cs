using STEMotion.Application.DTO.RequestDTOs;
using STEMotion.Application.DTO.ResponseDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEMotion.Application.Interfaces.ServiceInterfaces
{
    public interface IUserService
    {
        Task<IEnumerable<ResponseDTO<UserResponseDTO>>> GetAllUsers();
        Task<ResponseDTO<UserResponseDTO>> GetUserById(Guid id);
        Task<ResponseDTO<UserResponseDTO>> CreateUser(CreateUserRequestDTO user);
        Task<ResponseDTO<UserResponseDTO>> UpdateUser(Guid id, UpdateUserRequestDTO user);
        Task<ResponseDTO<bool>> DeleteUser(Guid id);
        Task<ResponseDTO<string>> LoginUser(LoginRequestDTO loginRequest);
        Task<ResponseDTO<bool>> ChangePassword(Guid userId, ChangePasswordRequestDTO changePasswordRequest);

        Task<ResponseDTO<string>> AuthenticateGoogleUserAsync(GoogleRequestDTO googleRequestDTO);

        Task<ResponseDTO<string>> RequestRegistrationOtpAsync(CreateUserRequestDTO createUserRequest);

        Task<ResponseDTO<string>> RequestPasswordResetOtpAsync(string email);
        Task<ResponseDTO<UserResponseDTO>> VerifyOtpAndRegisterAsync(string email, string otpCode);
    }
}
