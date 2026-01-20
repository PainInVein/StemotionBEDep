using STEMotion.Application.DTO.RequestDTOs;
using STEMotion.Application.DTO.ResponseDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEMotion.Application.Interfaces.ServiceInterfaces
{
    public interface IChapterService
    {
        Task<IEnumerable<ResponseDTO<ChapterResponseDTO>>> GetAllChapter();
        Task<ResponseDTO<ChapterResponseDTO>> GetChapterById(Guid id);
        Task<ResponseDTO<ChapterResponseDTO>> CreateChapter(ChapterRequestDTO requestDTO);
        Task<ResponseDTO<ChapterResponseDTO>> UpdateChapter(Guid id, UpdateChapterRequestDTO requestDTO);
        Task<ResponseDTO<bool>> DeleteChapter(Guid id);
    }
}
