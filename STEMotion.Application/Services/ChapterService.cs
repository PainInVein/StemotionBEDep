using AutoMapper;
using Microsoft.EntityFrameworkCore;
using STEMotion.Application.DTO.RequestDTOs;
using STEMotion.Application.DTO.ResponseDTOs;
using STEMotion.Application.Exceptions;
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
        #region cto
        public ChapterService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        #endregion cto
        #region CRUD
        public async Task<IEnumerable<ChapterResponseDTO>> GetAllChapter()
        {
            var chapters = await _unitOfWork.ChapterRepository.GetAllChapterAsync();
            var response = _mapper.Map<IEnumerable<ChapterResponseDTO>>(chapters);
            return response;
        }

        public async Task<ChapterResponseDTO> CreateChapter(ChapterRequestDTO requestDTO)
        {
            var grade = await _unitOfWork.GradeRepository.GetGradeByLevelAsync(requestDTO.GradeLevel);
            if (grade == null)
            {
                throw new NotFoundException("Lớp", requestDTO.GradeLevel);
            }
            var subject = await _unitOfWork.SubjectRepository.GetSubjectByNameAndGradeAsync(requestDTO.SubjectName, grade.GradeId);

            if (subject == null)
            {
                throw new NotFoundException($"{requestDTO.SubjectName}", requestDTO.GradeLevel);
            }
            var existingChapter = await _unitOfWork.ChapterRepository.ExistsAsync(x => x.ChapterName.ToLower().Equals(requestDTO.ChapterName.ToLower()));
            if (existingChapter)
            {
                throw new AlreadyExistsException("Chương", $"{requestDTO.ChapterName}");
            }
            var chapter = _mapper.Map<Chapter>(requestDTO);
            chapter.Status = "Active";
            chapter.SubjectId = subject.SubjectId;
            var request = await _unitOfWork.ChapterRepository.CreateAsync(chapter);
            await _unitOfWork.SaveChangesAsync();
            if (request == null)
            {
                throw new InternalServerException("Không thể tạo chương");
            }
            var response = _mapper.Map<ChapterResponseDTO>(request);
            return response;

        }
        public async Task<ChapterResponseDTO> GetChapterById(Guid id)
        {
            var chapter = await _unitOfWork.ChapterRepository.GetByIdAsync(id);
            if (chapter == null)
            {
                throw new NotFoundException("Chương không tồn tại");
            }
            var response = _mapper.Map<ChapterResponseDTO>(chapter);
            return response;
        }
        public async Task<ChapterResponseDTO> UpdateChapter(Guid id, UpdateChapterRequestDTO requestDTO)
        {
            var chapter = await _unitOfWork.ChapterRepository.GetByIdAsync(id);
            if (chapter == null)
            {
                throw new NotFoundException($"Chương {requestDTO.ChapterName} không tồn tại");
            }
            var grade = await _unitOfWork.GradeRepository.GetGradeByLevelAsync(requestDTO.GradeLevel);

            if (grade == null)
                throw new NotFoundException("Lớp", requestDTO.GradeLevel);
            var subject = await _unitOfWork.SubjectRepository.GetSubjectByNameAndGradeAsync(requestDTO.SubjectName, grade.GradeId);
            if (subject == null)
                throw new NotFoundException($"Môn học {requestDTO.SubjectName} không tồn tại trong lớp {grade.GradeLevel}");

            var isDuplicate = await _unitOfWork.ChapterRepository
              .ExistsAsync(x => x.ChapterName.ToLower() == requestDTO.ChapterName.ToLower() && x.ChapterId != id);

            if (isDuplicate)
                throw new AlreadyExistsException("Chương", $"{requestDTO.ChapterName}");

            _mapper.Map(requestDTO, chapter);
            _unitOfWork.ChapterRepository.Update(chapter);
            await _unitOfWork.SaveChangesAsync();
            var response = _mapper.Map<ChapterResponseDTO>(chapter);
            return response;
        }
        public async Task<bool> DeleteChapter(Guid id)
        {
            var findingChapter = await _unitOfWork.ChapterRepository.GetByIdAsync(id);
            if (findingChapter == null)
            {
                throw new NotFoundException($"Chương này không tồn tại");
            }
            _unitOfWork.ChapterRepository.Delete(findingChapter);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
        #endregion CRUD
    }
}
