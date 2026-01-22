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
    public class LessonService : ILessonService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public LessonService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ResponseDTO<LessonResponseDTO>> CreateLesson(LessonRequestDTO requestDTO)
        {
            try
            {
                var chapter = await _unitOfWork.ChapterRepository
                .FindByCondition(c => c.ChapterName.ToLower() == requestDTO.ChapterName.ToLower())
                .FirstOrDefaultAsync();

                if (chapter == null)
                {
                    return new ResponseDTO<LessonResponseDTO>
                    {
                        IsSuccess = false,
                        Message = "Chapter not found"
                    };
                }
                var isDuplicate = await _unitOfWork.LessonRepository
                .ExistsAsync(x => x.LessonName.ToLower() == requestDTO.LessonName.ToLower() && x.ChapterId == chapter.ChapterId);

                if (isDuplicate)
                    return new ResponseDTO<LessonResponseDTO>
                    {
                        IsSuccess = false,
                        Message = "Chapter already exists."
                    };
                var lesson = _mapper.Map<Lesson>(requestDTO);
                lesson.Status = "Active";
                lesson.ChapterId = chapter.ChapterId;
                var request = await _unitOfWork.LessonRepository.CreateAsync(lesson);
                await _unitOfWork.SaveChangesAsync();
                if (request == null)
                {
                    return new ResponseDTO<LessonResponseDTO>
                    {
                        IsSuccess = false,
                        Message = "Lesson created fail",
                        Result = null
                    };
                }
                var response = _mapper.Map<LessonResponseDTO>(request);
                response.ChapterName = chapter.ChapterName;
                return new ResponseDTO<LessonResponseDTO>
                {
                    IsSuccess = true,
                    Message = "Lesson created successfully",
                    Result = response
                };
            }
            catch (Exception ex)
            {
                return new ResponseDTO<LessonResponseDTO>
                {
                    IsSuccess = false,
                    Message = $"Error creating lesson: {ex.Message}",
                    Result = null
                };
            }
        }

        public async Task<ResponseDTO<bool>> DeleteLesson(Guid id)
        {
            try
            {
                var findingLesson = await _unitOfWork.LessonRepository.GetByIdAsync(id);
                if (findingLesson == null)
                {
                    return new ResponseDTO<bool>
                    {
                        IsSuccess = false,
                        Message = "Lesson not found",
                        Result = false
                    };
                }
                _unitOfWork.LessonRepository.Delete(findingLesson);
                await _unitOfWork.SaveChangesAsync();
                return new ResponseDTO<bool>
                {
                    IsSuccess = true,
                    Message = "Lesson deleted successfully",
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

        public async Task<IEnumerable<ResponseDTO<LessonResponseDTO>>> GetAllLesson()
        {
            var lesson = await _unitOfWork.LessonRepository.FindAllAsync(x => x.Chapter);
            var response = _mapper.Map<IEnumerable<LessonResponseDTO>>(lesson);
            return response.Select(lessons => new ResponseDTO<LessonResponseDTO>
            {
                IsSuccess = true,
                Message = "Get All Successfully",
                Result = lessons
            });
        }

        public async Task<ResponseDTO<LessonResponseDTO>> GetLessonById(Guid id)
        {
            try
            {
                var result = await _unitOfWork.LessonRepository.FindByCondition(x => x.LessonId == id, false, x => x.Chapter).FirstOrDefaultAsync();
                if (result == null)
                {
                    return new ResponseDTO<LessonResponseDTO>
                    {
                        IsSuccess = false,
                        Message = "Not Found",
                        Result = null
                    };
                }
                var response = _mapper.Map<LessonResponseDTO>(result);
                return new ResponseDTO<LessonResponseDTO>
                {
                    IsSuccess = true,
                    Message = "Get By Id Succesfully",
                    Result = response
                };
            }
            catch (Exception ex)
            {
                return new ResponseDTO<LessonResponseDTO>
                {
                    IsSuccess = false,
                    Message = $"Error: {ex.Message}"
                };
            }
        }

        public async Task<ResponseDTO<LessonResponseDTO>> UpdateLesson(Guid id, UpdateLessonRequestDTO requestDTO)
        {
            try
            {
                var lesson = await _unitOfWork.LessonRepository.GetByIdAsync(id);
                if (lesson == null)
                {
                    return new ResponseDTO<LessonResponseDTO>
                    {
                        IsSuccess = false,
                        Message = "Lesson not found"
                    };
                }
                _mapper.Map(requestDTO, lesson);
                _unitOfWork.LessonRepository.Update(lesson);
                await _unitOfWork.SaveChangesAsync();
                var response = _mapper.Map<LessonResponseDTO>(lesson);
                return new ResponseDTO<LessonResponseDTO>
                {
                    IsSuccess = true,
                    Message = "Lesson update successfully",
                    Result = response
                };
            }
            catch (Exception ex)
            {
                return new ResponseDTO<LessonResponseDTO>
                {
                    IsSuccess = false,
                    Message = $"Error: {ex.Message}"
                };
            }
        }
    }
}
