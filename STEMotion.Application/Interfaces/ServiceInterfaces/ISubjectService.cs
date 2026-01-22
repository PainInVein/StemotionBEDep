using STEMotion.Application.DTO.RequestDTOs;
using STEMotion.Application.DTO.ResponseDTOs;
using STEMotion.Application.Interfaces.RepositoryInterfaces;
using STEMotion.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEMotion.Application.Interfaces.ServiceInterfaces
{
    public interface ISubjectService 
    {
        Task<IEnumerable<ResponseDTO<SubjectResponseDTO>>> GetAllSubject();
        Task<ResponseDTO<SubjectResponseDTO>> GetSubjectById(Guid id);
        Task<ResponseDTO<SubjectResponseDTO>> CreateSubject(SubjectRequestDTO requestDTO);
        Task<ResponseDTO<SubjectResponseDTO>> UpdateSubject(Guid id, UpdateSubjectRequestDTO requestDTO);
        Task<ResponseDTO<bool>> DeleteSubject(Guid id);
    }
}
