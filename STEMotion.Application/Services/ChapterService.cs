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
    public class ChapterService : IChapterService
    {
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper;

        public ChapterService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IEnumerable<ResponseDTO<ChapterResponseDTO>>> GetAllChapter()
        {
            var chapters = _unitOfWork.ChapterRepository.FindAll();
            var response = _mapper.Map<IEnumerable<ChapterResponseDTO>>(chapters);
            return response.Select(chapter => new ResponseDTO<ChapterResponseDTO>
            {
                IsSuccess = true,
                Message = "Get All Successfully",
                Result = chapter
            });
        }

        public async Task<ResponseDTO<ChapterResponseDTO>> CreateChapter(ChapterRequestDTO requestDTO)
        {
            try
            {
                var existingChapter = await _unitOfWork.ChapterRepository.ExistsAsync(x => x.Subject.Name == requestDTO.SubjectName && x.Subject.Grade.GradeLevel.Equals(requestDTO.GradeName) && x.Title.Equals(requestDTO.Title));
                if (existingChapter)
                {
                    return new ResponseDTO<ChapterResponseDTO>
                    {
                        IsSuccess = false,
                        Message = "Chapter already exists.",
                        Result = null
                    };
                }
                var chapter = _mapper.Map<Chapter>(requestDTO);
                chapter.Status = "Active";
                var request = await _unitOfWork.ChapterRepository.CreateAsync(chapter);
                await _unitOfWork.SaveChangesAsync();
                if (request == null)
                {
                    return new ResponseDTO<ChapterResponseDTO>
                    {
                        IsSuccess = false,
                        Message = "Chapter created fail",
                        Result = null
                    };
                }
                var response = _mapper.Map<ChapterResponseDTO>(request);
                return new ResponseDTO<ChapterResponseDTO>
                {
                    IsSuccess = true,
                    Message = "Chapter created successfully",
                    Result = response
                };
            }
            catch (Exception ex)
            {
                return new ResponseDTO<ChapterResponseDTO>
                {
                    IsSuccess = false,
                    Message = $"Error creating chapter: {ex.Message}",
                    Result = null
                };
            }
        }
        public async Task<ResponseDTO<ChapterResponseDTO>> GetChapterById(Guid id)
        {
            try
            {
                var chapter = await _unitOfWork.ChapterRepository.GetByIdAsync(id);
                if (chapter == null)
                {
                    return new ResponseDTO<ChapterResponseDTO> { IsSuccess = false, Message = "Not found" };
                }
                var response = _mapper.Map<ChapterResponseDTO>(chapter);
                return new ResponseDTO<ChapterResponseDTO>
                {
                    IsSuccess = true,
                    Message = "Get By Id Succesfully",
                    Result = response
                };
            }
            catch (Exception ex)
            {
                return new ResponseDTO<ChapterResponseDTO>
                {
                    IsSuccess = false,
                    Message = $"Error: {ex.Message}"
                };
            }
        }
        public async Task<ResponseDTO<ChapterResponseDTO>> UpdateChapter(Guid id, UpdateChapterRequestDTO requestDTO)
        {
            try
            {
                var chapter = await _unitOfWork.ChapterRepository.GetByIdAsync(id);
                if (chapter == null)
                {
                    return new ResponseDTO<ChapterResponseDTO>
                    {
                        IsSuccess = false,
                        Message = "Chapter not found"
                    };
                }
                _mapper.Map(requestDTO, chapter);
                _unitOfWork.ChapterRepository.Update(chapter);
                await _unitOfWork.SaveChangesAsync();
                var response = _mapper.Map<ChapterResponseDTO>(chapter);
                return new ResponseDTO<ChapterResponseDTO>
                {
                    IsSuccess = true,
                    Message = "Grade update successfully",
                    Result = response
                };
            }
            catch (Exception ex)
            {
                return new ResponseDTO<ChapterResponseDTO>
                {
                    IsSuccess = false,
                    Message = $"Error: {ex.Message}"
                };
            }
        }
        public async Task<ResponseDTO<bool>> DeleteChapter(Guid id)
        {
            try
            {
                var findingChapter = await _unitOfWork.ChapterRepository.GetByIdAsync(id);
                if (findingChapter == null)
                {
                    return new ResponseDTO<bool>
                    {
                        IsSuccess = false,
                        Message = "Chapter not found",
                        Result = false
                    };
                }
                _unitOfWork.ChapterRepository.Delete(findingChapter);
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
    }
}
