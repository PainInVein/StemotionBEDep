using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEMotion.Application.DTO.ResponseDTOs
{
    public class StudentOverallProgressResponseDTO
    {
        public Guid StudentId { get; set; }
        public string StudentName { get; set; }
        public int GradeLevel { get; set; }
        public List<SubjectProgressDTO> SubjectProgress { get; set; }
        public double OverallProgress { get; set; } // Tiến trình tổng thể
    }
}
