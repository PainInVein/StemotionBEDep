
using AutoMapper;
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
    public class SubjectService : ISubjectService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SubjectService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseDTO<SubjectResponseDTO>> CreateSubject(SubjectRequestDTO requestDTO)
        {
            try
            {
                var existing = await _unitOfWork.SubjectRepository.ExistsAsync(x => x.Grade.Name.Equals(requestDTO.GradeName, StringComparison.OrdinalIgnoreCase) && x.Name.Equals(requestDTO.SubjectName, StringComparison.OrdinalIgnoreCase));
                if (existing)
                {
                    return new ResponseDTO<SubjectResponseDTO>
                    {
                        IsSuccess = false,
                        Message = "Subject already exists.",
                        Result = null
                    };
                }
                var subject = _mapper.Map<Subject>(requestDTO);
                subject.Status = "Active";
                var request = await _unitOfWork.SubjectRepository.CreateAsync(subject);
                await _unitOfWork.SaveChangesAsync();
                if (request == null)
                {
                    return new ResponseDTO<SubjectResponseDTO>
                    {
                        IsSuccess = false,
                        Message = "Created Subject fail",
                        Result = null
                    };
                }
                var response = _mapper.Map<SubjectResponseDTO>(request);
                return new ResponseDTO<SubjectResponseDTO>
                {
                    IsSuccess = true,
                    Message = "Create Subject successfull",
                    Result = response
                };
            }
            catch (Exception ex)
            {
                return new ResponseDTO<SubjectResponseDTO>
                {
                    IsSuccess = false,
                    Message = $"Error creating subject: {ex.Message}",
                    Result = null
                };
            }
        }
        public async Task<ResponseDTO<bool>> DeleteSubject(Guid id)
        {
            try
            {
                var subject = await _unitOfWork.SubjectRepository.GetByIdAsync(id);
                if (subject == null)
                {
                    return new ResponseDTO<bool>
                    {
                        IsSuccess = false,
                        Message = "Not Found",
                        Result = false
                    };
                }
                _unitOfWork.SubjectRepository.Delete(subject);
                await _unitOfWork.SaveChangesAsync();
                return new ResponseDTO<bool>
                {
                    IsSuccess = true,
                    Message = "Subject deleted Successful",
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

        public async Task<IEnumerable<ResponseDTO<SubjectResponseDTO>>> GetAllSubject()
        {
            var subject = _unitOfWork.SubjectRepository.FindAll();
            var response = _mapper.Map<IEnumerable<SubjectResponseDTO>>(subject);
            return response.Select(subject => new ResponseDTO<SubjectResponseDTO>
            {
                IsSuccess = true,
                Message = "Subject created sucessfully",
                Result = subject
            });
        }

        public async Task<ResponseDTO<SubjectResponseDTO>> GetSubjectById(Guid id)
        {
            try
            {
                var result = _unitOfWork.SubjectRepository.FindByCondition(x => x.SubjectId == id);
                if (result == null)
                {
                    return new ResponseDTO<SubjectResponseDTO>
                    {
                        IsSuccess = false,
                        Message = "Not found",
                        Result = null
                    };
                }
                var response = _mapper.Map<SubjectResponseDTO>(result);
                return new ResponseDTO<SubjectResponseDTO>
                {
                    IsSuccess = true,
                    Message = "Created Subject successfull",
                    Result = response
                };
            }
            catch (Exception ex)
            {
                return new ResponseDTO<SubjectResponseDTO>
                {
                    IsSuccess = false,
                    Message = $"Error: {ex.Message}"
                };
            }
        }

        public async Task<ResponseDTO<SubjectResponseDTO>> UpdateSubject(Guid id, UpdateSubjectRequestDTO requestDTO)
        {
            try
            {
                var subject = await _unitOfWork.SubjectRepository.GetByIdAsync(id);
                if (subject == null)
                {
                    return new ResponseDTO<SubjectResponseDTO>
                    {
                        IsSuccess = false,
                        Message = "Grade not found"
                    };
                }
                _mapper.Map(requestDTO, subject);
                _unitOfWork.SubjectRepository.Update(subject);
                await _unitOfWork.SaveChangesAsync();
                var response = _mapper.Map<SubjectResponseDTO>(subject);
                return new ResponseDTO<SubjectResponseDTO>
                {
                    IsSuccess = true,
                    Message = "Update succesfull",
                    Result = response
                };
            }
            catch(Exception ex)
            {
                return new ResponseDTO<SubjectResponseDTO>
                {
                    IsSuccess = false,
                    Message = $"Error: {ex.Message}"
                };
            }
        }
    }
}
