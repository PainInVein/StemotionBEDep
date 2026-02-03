using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEMotion.Application.DTO.ResponseDTOs
{
    public class SubjectSummaryDTO
    {
        public Guid SubjectId { get; set; }
        public string SubjectName { get; set; }
        public double CompletionPercentage { get; set; }
    }

}
