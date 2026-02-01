using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEMotion.Application.DTO.ResponseDTOs
{
    public class ParentDashboardDTO
    {
        public Guid ParentId { get; set; }
        public List<ChildProgressSummaryDTO> Children { get; set; }
    }
}
