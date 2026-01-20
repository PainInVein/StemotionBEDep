using AutoMapper;
using STEMotion.Application.DTO.RequestDTOs;
using STEMotion.Application.DTO.ResponseDTOs;
using STEMotion.Application.Interfaces.RepositoryInterfaces;
using STEMotion.Application.Interfaces.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STEMotion.Domain.Entities;

namespace STEMotion.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPasswordService _passwordService;
        private readonly IJWTService _jwtService;
        public UserService(IUserRepository userRepository, IUnitOfWork unitOfWork, IMapper mapper, IPasswordService passwordService, IJWTService jWTService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _passwordService = passwordService;
            _jwtService = jWTService;
        }

        public Task<ResponseDTO<bool>> ChangePassword(Guid userId, ChangePasswordRequestDTO changePasswordRequest)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseDTO<UserResponseDTO>> CreateUser(CreateUserRequestDTO user)
        {
            try
            {
                var existingUser = await _unitOfWork.UserRepository
                .FirstOrDefaultAsync(u => u.Email == user.Email);
                if (existingUser != null)
                {
                    return new ResponseDTO<UserResponseDTO>
                    {
                        IsSuccess = false,
                        Message = "User with this email already exists.",
                        Result = null
                    };
                }
                var role = await _unitOfWork.RoleRepository.FirstOrDefaultAsync(r =>
                                        r.Name.ToLower() == user.RoleName.ToLower());
                if (role == null)
                {
                    return new ResponseDTO<UserResponseDTO>
                    {
                        IsSuccess = false,
                        Message = "Specified role does not exist.",
                        Result = null
                    };
                }
                var entites = _mapper.Map<User>(user);
                entites.RoleId = role.Id;
                entites.Status = "Active";
                entites.Password = _passwordService.HashPasswords(user.Password);
                var newUser = await _unitOfWork.UserRepository.CreateAsync(entites);
                await _unitOfWork.SaveChangesAsync();

                var response = _mapper.Map<UserResponseDTO>(newUser);
                response.RoleName = role.Name;


                return new ResponseDTO<UserResponseDTO>
                {
                    IsSuccess = true,
                    Message = "User created successfully",
                    Result = response
                };
            }
            catch (Exception ex)
            {
                return new ResponseDTO<UserResponseDTO>
                {
                    IsSuccess = false,
                    Message = $"Error creating user: {ex.Message}",
                    Result = null
                };
            }
        }

        public async Task<ResponseDTO<bool>> DeleteUser(Guid id)
        {
            try
            {
                var user = await _unitOfWork.UserRepository.GetByIdAsync(id);
                if (user == null)
                {
                    return new ResponseDTO<bool>
                    {
                        IsSuccess = false,
                        Message = "User not found.",
                        Result = false
                    };
                }
                _unitOfWork.UserRepository.Delete(user);
                await _unitOfWork.SaveChangesAsync();

                return new ResponseDTO<bool>
                {
                    IsSuccess = true,
                    Message = "User deleted successfully.",
                    Result = true
                };
            }
            catch (Exception ex)
            {
                return new ResponseDTO<bool>
                {
                    IsSuccess = false,
                    Message = $"Error: {ex.Message}",
                    Result = false
                };
            }
        }

        public async Task<IEnumerable<ResponseDTO<UserResponseDTO>>> GetAllUsers()
        {
            var items = await _unitOfWork.UserRepository.FindAllAsync(u => u.Role);
            var response = _mapper.Map<IEnumerable<UserResponseDTO>>(items);

            return response.Select(u => new ResponseDTO<UserResponseDTO>
            {
                IsSuccess = true,
                Message = "User retrieved successfully",
                Result = u
            });
        }

        public async Task<ResponseDTO<UserResponseDTO>> GetUserById(Guid id)
        {
            try
            {
                var user = await _unitOfWork.UserRepository.GetByIdAsync(id);
                if (user == null)
                {
                    return new ResponseDTO<UserResponseDTO>
                    {
                        IsSuccess = false,
                        Message = "User not found.",
                        Result = null
                    };
                }

                var response = _mapper.Map<UserResponseDTO>(user);
                response.RoleName = (await _unitOfWork.RoleRepository.GetByIdAsync(user.RoleId))?.Name;
                return new ResponseDTO<UserResponseDTO>
                {
                    IsSuccess = true,
                    Message = "User retrieved successfully",
                    Result = response
                };
            }
            catch (Exception ex)
            {
                return new ResponseDTO<UserResponseDTO>
                {
                    IsSuccess = false,
                    Message = $"Error: {ex.Message}"
                };
            }
        }

        public async Task<ResponseDTO<string>> LoginUser(LoginRequestDTO loginRequest)
        {
            try
            {
                var user = await _unitOfWork.UserRepository
                    .FirstOrDefaultAsync(u => u.Email == loginRequest.Email, u => u.Role);

                if (user == null)
                {
                    return new ResponseDTO<string>
                    {
                        IsSuccess = false,
                        Message = "Invalid email or password"
                    };
                }
                if (user.Status != "Active")
                {
                    return new ResponseDTO<string>
                    {
                        IsSuccess = false,
                        Message = "User account is inactive"
                    };
                }
                var checkpassword = _passwordService.VerifyPassword(loginRequest.Password, user.Password);

                if (!checkpassword)
                {
                    return new ResponseDTO<string>
                    {
                        IsSuccess = false,
                        Message = "Invalid email or password"
                    };
                }

                //var response = _mapper.Map<UserResponseDTO>(user);
                var response = _jwtService.GenerateToken(loginRequest.Email, user.Role.Name);

                return new ResponseDTO<string>
                {
                    IsSuccess = true,
                    Message = "Login successful",
                    Result = response
                };
            }
            catch (Exception ex)
            {
                return new ResponseDTO<string>
                {
                    IsSuccess = false,
                    Message = $"Error: {ex.Message}"
                };
            }
        }

        public async Task<ResponseDTO<UserResponseDTO>> UpdateUser(Guid id, UpdateUserRequestDTO userDTO)
        {
            try
            {
                var users = await _unitOfWork.UserRepository.GetByIdAsync(id);
                if (users == null)
                {
                    return new ResponseDTO<UserResponseDTO>
                    {
                        IsSuccess = false,
                        Message = "User not found"
                    };
                }

                _mapper.Map(userDTO, users);

                _unitOfWork.UserRepository.Update(users);
                await _unitOfWork.SaveChangesAsync();
                var response = _mapper.Map<UserResponseDTO>(users);
                response.RoleName = (await _unitOfWork.RoleRepository.GetByIdAsync(users.RoleId))?.Name;
                return new ResponseDTO<UserResponseDTO>
                {
                    IsSuccess = true,
                    Message = "User updated successfully",
                    Result = response
                };
            }
            catch (Exception ex)
            {
                return new ResponseDTO<UserResponseDTO>
                {
                    IsSuccess = false,
                    Message = $"Error: {ex.Message}"
                };
            }
        }
    }
}
