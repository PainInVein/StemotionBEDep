using AutoMapper;
using Microsoft.EntityFrameworkCore;
using STEMotion.Application.DTO.RequestDTOs;
using STEMotion.Application.DTO.ResponseDTOs;
using STEMotion.Application.Interfaces.RepositoryInterfaces;
using STEMotion.Application.Interfaces.ServiceInterfaces;
using STEMotion.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEMotion.Application.Services
{
    public class GradeService : IGradeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GradeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ResponseDTO<GradeResponseDTO>> CreateGrade(GradeRequestDTO requestDTO)
        {
            try
            {
                var existingGrade = await _unitOfWork.GradeRepository.ExistsAsync(x => x.GradeLevel == requestDTO.GradeLevel);
                if (existingGrade)
                {
                    return new ResponseDTO<GradeResponseDTO>
                    {
                        IsSuccess = false,
                        Message = "Grade already exists.",
                        Result = null
                    };
                }
                var grade = _mapper.Map<Grade>(requestDTO);
                grade.Status = "Active";
                var request = await _unitOfWork.GradeRepository.CreateAsync(grade);
                await _unitOfWork.SaveChangesAsync();
                if (request == null)
                {
                    return new ResponseDTO<GradeResponseDTO>
                    {
                        IsSuccess = false,
                        Message = "Grade created fail",
                        Result = null
                    };
                }
                var response = _mapper.Map<GradeResponseDTO>(request);
                return new ResponseDTO<GradeResponseDTO>
                {
                    IsSuccess = true,
                    Message = "Grade created successfully",
                    Result = response
                };
            }
            catch (Exception ex) 
            {
                return new ResponseDTO<GradeResponseDTO>
                {
                    IsSuccess = false,
                    Message = $"Error creating grade: {ex.Message}",
                    Result = null
                };
            }
        }

        public async Task<ResponseDTO<bool>> DeleteGrade(Guid id)
        {
            try
            {
                var findingGrade = await _unitOfWork.GradeRepository.GetByIdAsync(id);
                if (findingGrade == null)
                {
                    return new ResponseDTO<bool>
                    {
                        IsSuccess = false,
                        Message = "Grade not found",
                        Result = false
                    };
                }
                _unitOfWork.GradeRepository.Delete(findingGrade);
                await _unitOfWork.SaveChangesAsync();
                return new ResponseDTO<bool>
                {
                    IsSuccess = true,
                    Message = "Grade deleted successfully",
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

        public async Task<IEnumerable<ResponseDTO<GradeResponseDTO>>> GetAllGrade()
        {
            var grade = _unitOfWork.GradeRepository.FindAll();
            var response = _mapper.Map<IEnumerable<GradeResponseDTO>>(grade);
            return response.Select(grade => new ResponseDTO<GradeResponseDTO>
            {
                IsSuccess = true,
                Message = "Get All Successfully",
                Result = grade
            });
        }

        public async Task<ResponseDTO<GradeResponseDTO>> GetGradeById(Guid id)
        {
            try
            {
                var result = await _unitOfWork.GradeRepository.FindByCondition(x => x.GradeId == id).FirstOrDefaultAsync();
                if (result == null)
                {
                    return new ResponseDTO<GradeResponseDTO>
                    {
                        IsSuccess = false,
                        Message = "Not Found",
                        Result = null
                    };
                }
                var response = _mapper.Map<GradeResponseDTO>(result);
                return new ResponseDTO<GradeResponseDTO>
                {
                    IsSuccess = true,
                    Message = "Get By Id Succesfully",
                    Result = response
                };
            }catch(Exception ex)
            {
                return new ResponseDTO<GradeResponseDTO>
                {
                    IsSuccess = false,
                    Message = $"Error: {ex.Message}"
                };
            }
        }

        public async Task<ResponseDTO<GradeResponseDTO>> UpdateGrade(Guid id, UpdateGradeRequest requestDTO)
        {
            try
            {
                var grade = await _unitOfWork.GradeRepository.GetByIdAsync(id);
                if (grade == null)
                {
                    return new ResponseDTO<GradeResponseDTO>
                    {
                        IsSuccess = false,
                        Message = "Grade not found"
                    };
                }
                _mapper.Map(requestDTO, grade);
                _unitOfWork.GradeRepository.Update(grade);
                await _unitOfWork.SaveChangesAsync();
                var response = _mapper.Map<GradeResponseDTO>(grade);
                return new ResponseDTO<GradeResponseDTO>
                {
                    IsSuccess = true,
                    Message = "Grade update successfully",
                    Result = response
                };
            }
            catch (Exception ex)
            {
                return new ResponseDTO<GradeResponseDTO>
                {
                    IsSuccess = false,
                    Message = $"Error: {ex.Message}"
                };
            }
        }
    }
}
