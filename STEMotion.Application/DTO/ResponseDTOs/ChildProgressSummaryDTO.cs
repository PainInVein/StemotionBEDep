using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEMotion.Application.DTO.ResponseDTOs
{
    public class ChildProgressSummaryDTO
    {
        public Guid StudentId { get; set; }
        public string StudentName { get; set; }
        public int GradeLevel { get; set; }
        public double OverallProgress { get; set; }
        public int TotalLessons { get; set; }
        public int CompletedLessons { get; set; }
        public DateTime? LastActivity { get; set; }
        public List<SubjectSummaryDTO> TopSubjects { get; set; } // Top 3 môn học
    }
}
