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
        Task<IEnumerable<LessonResponseDTO>> GetAllLesson();
        Task<LessonResponseDTO> GetLessonById(Guid id);
        Task<LessonResponseDTO> CreateLesson(LessonRequestDTO requestDTO);
        Task<LessonResponseDTO> UpdateLesson(Guid id, UpdateLessonRequestDTO requestDTO);
        Task<bool> DeleteLesson(Guid id);
    }
}
