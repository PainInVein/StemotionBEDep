
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
    public class SubjectService : ISubjectService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SubjectService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<SubjectResponseDTO> CreateSubject(SubjectRequestDTO requestDTO)
        {
            var grade = await _unitOfWork.GradeRepository
                                         .FindByCondition(g => g.GradeLevel == requestDTO.GradeLevel)
                                         .FirstOrDefaultAsync();
            if (grade == null)
            {
                throw new NotFoundException("Môn học", requestDTO.SubjectName);
            }

            var existing = await _unitOfWork.SubjectRepository.ExistsAsync(x =>
                                  x.GradeId == grade.GradeId
                                  && x.SubjectName.ToLower() == requestDTO.SubjectName.ToLower());
            if (existing)
            {
                throw new AlreadyExistsException($"{requestDTO.SubjectName}", $"{requestDTO.GradeLevel}");
            }
            var subject = _mapper.Map<Subject>(requestDTO);
            subject.Status = "Active";
            subject.GradeId = grade.GradeId;
            var request = await _unitOfWork.SubjectRepository.CreateAsync(subject);
            await _unitOfWork.SaveChangesAsync();
            if (request == null)
            {
                throw new InternalServerException("Không thể tạo môn học");
            }
            var response = _mapper.Map<SubjectResponseDTO>(request);
            response.GradeLevel = grade.GradeLevel;
            return response;
        }
        public async Task<bool> DeleteSubject(Guid id)
        {
            var subject = await _unitOfWork.SubjectRepository.GetByIdAsync(id);
            if (subject == null)
            {
                throw new NotFoundException("Môn học này không tồn tại");
            }
            _unitOfWork.SubjectRepository.Delete(subject);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<SubjectResponseDTO>> GetAllSubject()
        {
            var subject = await _unitOfWork.SubjectRepository.FindAllAsync(x => x.Grade);
            var response = _mapper.Map<IEnumerable<SubjectResponseDTO>>(subject);
            return response;
        }

        public async Task<SubjectResponseDTO> GetSubjectById(Guid id)
        {
            var result = await _unitOfWork.SubjectRepository.FindByCondition(x => x.SubjectId == id, false, x => x.Grade).FirstOrDefaultAsync();
            if (result == null)
            {
                throw new NotFoundException("Môn học này không tồn tại");
            }
            var response = _mapper.Map<SubjectResponseDTO>(result);
            return response;
        }

        public async Task<SubjectResponseDTO> UpdateSubject(Guid id, UpdateSubjectRequestDTO requestDTO)
        {
                var subject = await _unitOfWork.SubjectRepository.FindByCondition(x => x.SubjectId == id, false, s => s.Grade).FirstOrDefaultAsync();
                if (subject == null)
                {
                    throw new NotFoundException("Môn học này không tồn tại");
                }
                if (subject.Grade.GradeLevel != requestDTO.GradeLevel)
                {
                    var newGrade = await _unitOfWork.GradeRepository
                        .FindByCondition(g => g.GradeLevel == requestDTO.GradeLevel)
                        .FirstOrDefaultAsync();

                    if (newGrade == null)
                    {
                       throw new NotFoundException("Lớp", requestDTO.GradeLevel);
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
                        throw new AlreadyExistsException($"{requestDTO.SubjectName}", $"{requestDTO.GradeLevel}");
                    }
                }
                _mapper.Map(requestDTO, subject);
                _unitOfWork.SubjectRepository.Update(subject);
                await _unitOfWork.SaveChangesAsync();
                var response = _mapper.Map<SubjectResponseDTO>(subject);
                return response;
        }
    }
}
