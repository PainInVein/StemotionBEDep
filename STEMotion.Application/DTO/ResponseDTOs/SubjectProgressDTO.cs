using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEMotion.Application.DTO.ResponseDTOs
{
    public class SubjectProgressDTO
    {
        public Guid SubjectId { get; set; }
        public string SubjectName { get; set; }
        public string Description { get; set; }
        public double CompletionPercentage { get; set; } // % hoàn thành môn học
        public int TotalChapters { get; set; }
        public int CompletedChapters { get; set; }
        public List<ChapterProgressDTO> ChapterProgress { get; set; }
    }
}
