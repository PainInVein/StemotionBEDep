
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
    public class SubjectService : ISubjectService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SubjectService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ResponseDTO<SubjectResponseDTO>> CreateSubject(SubjectRequestDTO requestDTO)
        {
            try
            {
                var grade = await _unitOfWork.GradeRepository
                                             .FindByCondition(g => g.GradeLevel == requestDTO.GradeLevel)
                                             .FirstOrDefaultAsync();
                if (grade == null)
                {
                    return new ResponseDTO<SubjectResponseDTO>
                    {
                        IsSuccess = false,
                        Message = $"Grade level {requestDTO.GradeLevel} not found.",
                        Result = null
                    };
                }

                var existing = await _unitOfWork.SubjectRepository.ExistsAsync(x =>
                                      x.GradeId == grade.GradeId
                                      && x.SubjectName.ToLower() == requestDTO.SubjectName.ToLower());
                if (existing)
                {
                    return new ResponseDTO<SubjectResponseDTO>
                    {
                        IsSuccess = false,
                        Message = $"Subject '{requestDTO.SubjectName}' already exists in Grade {requestDTO.GradeLevel}.",
                        Result = null
                    };
                }
                var subject = _mapper.Map<Subject>(requestDTO);
                subject.Status = "Active";
                subject.GradeId = grade.GradeId;
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
                response.GradeLevel = grade.GradeLevel;
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
            var subject = await _unitOfWork.SubjectRepository.FindAllAsync(x => x.Grade);
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
                var result =  await _unitOfWork.SubjectRepository.FindByCondition(x => x.SubjectId == id,false, x => x.Grade).FirstOrDefaultAsync();
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
                var subject = await _unitOfWork.SubjectRepository.FindByCondition(x => x.SubjectId == id, false, s => s.Grade).FirstOrDefaultAsync();
                if (subject == null)
                {
                    return new ResponseDTO<SubjectResponseDTO>
                    {
                        IsSuccess = false,
                        Message = "Subject not found"
                    };
                }
                if (subject.Grade.GradeLevel != requestDTO.GradeLevel)
                {
                    var newGrade = await _unitOfWork.GradeRepository
                        .FindByCondition(g => g.GradeLevel == requestDTO.GradeLevel)
                        .FirstOrDefaultAsync();

                    if (newGrade == null)
                    {
                        return new ResponseDTO<SubjectResponseDTO>
                        {
                            IsSuccess = false,
                            Message = $"Grade level {requestDTO.GradeLevel} not found.",
                            Result = null
                        };
                    }

                    subject.GradeId = newGrade.GradeId;
                    subject.Grade = newGrade;
                }
                if (subject.SubjectName != requestDTO.SubjectName || subject.Grade.GradeLevel != requestDTO.GradeLevel)
                {
                    var existing = await _unitOfWork.SubjectRepository.ExistsAsync(x =>
                        x.GradeId == subject.GradeId &&
                        x.SubjectName.ToLower() == requestDTO.SubjectName.ToLower() &&
                        x.SubjectId != id);

                    if (existing)
                    {
                        return new ResponseDTO<SubjectResponseDTO>
                        {
                            IsSuccess = false,
                            Message = $"Subject '{requestDTO.SubjectName}' already exists in Grade {requestDTO.GradeLevel}.",
                            Result = null
                        };
                    }
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
            catch (Exception ex)
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
