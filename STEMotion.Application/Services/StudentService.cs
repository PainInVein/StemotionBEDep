using AutoMapper;
using STEMotion.Application.DTO.RequestDTOs.StudentReqDTOs;
using STEMotion.Application.DTO.ResponseDTOs.StudentResponseDTO;
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
    public class StudentService : IStudentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPasswordService _passwordService;

        public StudentService(IUnitOfWork unitOfWork, IMapper mapper, IPasswordService passwordService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _passwordService = passwordService;
        }

        public async Task<AddChildrenResponseDTO> AddStudentAsync(AddChildrenRequestDTO addChildrenRequestDTO)
        {
            var studentEntity = _mapper.Map<Student>(addChildrenRequestDTO);

            studentEntity.Status = "Active";
            studentEntity.CreatedAt = DateTime.UtcNow;
            studentEntity.Password = _passwordService.HashPasswords(addChildrenRequestDTO.Password!);

            await _unitOfWork.StudentRepository.CreateAsync(studentEntity);

            await _unitOfWork.SaveChangesAsync();

            var result = _mapper.Map<AddChildrenResponseDTO>(studentEntity);

            return result;
        }
    }
}
