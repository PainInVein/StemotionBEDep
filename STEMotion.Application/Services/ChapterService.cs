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
            var chapters = await _unitOfWork.ChapterRepository.FindAllAsync(x => x.Subject);
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
                var grade = await _unitOfWork.GradeRepository
                                .FindByCondition(g => g.GradeLevel == requestDTO.GradeLevel).FirstOrDefaultAsync();
                if (grade == null)
                {
                    return new ResponseDTO<ChapterResponseDTO>
                    {
                        IsSuccess = false,
                        Message = $"Không tìm thấy Khối lớp {requestDTO.GradeLevel}.",
                        Result = null
                    };
                }
                var subject = await _unitOfWork.SubjectRepository
                    .FindByCondition(s => s.Name.ToLower() == requestDTO.SubjectName.ToLower()
                                         && s.GradeId == grade.GradeId).FirstOrDefaultAsync();

                if (subject == null)
                {
                    return new ResponseDTO<ChapterResponseDTO>
                    {
                        IsSuccess = false,
                        Message = $"Môn học '{requestDTO.SubjectName}' không tồn tại trong Khối lớp {requestDTO.GradeLevel}.",
                        Result = null
                    };
                }
                var existingChapter = await _unitOfWork.ChapterRepository.ExistsAsync(x => x.Title.ToLower().Equals(requestDTO.Title.ToLower()));
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
                chapter.SubjectId = subject.SubjectId;
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
                response.SubjectName = subject.Name;
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
                var chapter = await _unitOfWork.ChapterRepository.FindByCondition(x => x.ChapterId == id, false, x => x.Subject).FirstOrDefaultAsync();
                if (chapter == null)
                {
                    return new ResponseDTO<ChapterResponseDTO> { IsSuccess = false, Message = "Not found" };
                }
                var response = _mapper.Map<ChapterResponseDTO>(chapter);
                response.SubjectName = chapter.Subject.Name;
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
                var chapter = await _unitOfWork.ChapterRepository.FindByCondition(x => x.ChapterId == id, false, x => x.Subject).FirstOrDefaultAsync();
                if (chapter == null)
                {
                    return new ResponseDTO<ChapterResponseDTO>
                    {
                        IsSuccess = false,
                        Message = "Chapter not found"
                    };
                }
                var grade = await _unitOfWork.GradeRepository
                  .FindByCondition(g => g.GradeLevel == requestDTO.GradeLevel).FirstOrDefaultAsync();

                if (grade == null)
                    return new ResponseDTO<ChapterResponseDTO> { IsSuccess = false, Message = "Grade level not found" };

                var subject = await _unitOfWork.SubjectRepository
                   .FindByCondition(s => s.Name.ToLower() == requestDTO.SubjectName.ToLower()
                                    && s.GradeId == grade.GradeId).FirstOrDefaultAsync();

                if (subject == null)
                    return new ResponseDTO<ChapterResponseDTO> { IsSuccess = false, Message = "Subject not found in this Grade" };

                var isDuplicate = await _unitOfWork.ChapterRepository
                  .ExistsAsync(x => x.Title.ToLower() == requestDTO.Title.ToLower() && x.ChapterId != id);

                if (isDuplicate)
                    return new ResponseDTO<ChapterResponseDTO> { IsSuccess = false, Message = "Chapter title already exists" };

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
