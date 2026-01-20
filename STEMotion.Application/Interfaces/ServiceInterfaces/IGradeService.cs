using STEMotion.Application.DTO.RequestDTOs;
using STEMotion.Application.DTO.ResponseDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEMotion.Application.Interfaces.ServiceInterfaces
{
    public interface IGradeService
    {
        Task<IEnumerable<ResponseDTO<GradeResponseDTO>>> GetAllGrade();
        Task<ResponseDTO<GradeResponseDTO>> GetGradeById(Guid id);
        Task<ResponseDTO<GradeResponseDTO>> CreateGrade(GradeRequestDTO requestDTO);
        Task<ResponseDTO<GradeResponseDTO>> UpdateGrade(Guid id, UpdateGradeRequest requestDTO);
        Task<ResponseDTO<bool>> DeleteGrade(Guid id);
    }
}
