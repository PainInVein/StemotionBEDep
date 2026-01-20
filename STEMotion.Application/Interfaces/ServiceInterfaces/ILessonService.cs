using STEMotion.Application.DTO.RequestDTOs;
using STEMotion.Application.DTO.ResponseDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEMotion.Application.Interfaces.ServiceInterfaces
{
    public interface ILessonService
    {
        Task<IEnumerable<ResponseDTO<LessonResponseDTO>>> GetAllLesson();
        Task<ResponseDTO<LessonResponseDTO>> GetLessonById(Guid id);
        Task<ResponseDTO<LessonResponseDTO>> CreateLesson(LessonRequestDTO requestDTO);
        Task<ResponseDTO<LessonResponseDTO>> UpdateLesson(Guid id, UpdateLessonRequestDTO requestDTO);
        Task<ResponseDTO<bool>> DeleteLesson(Guid id);
    }
}
