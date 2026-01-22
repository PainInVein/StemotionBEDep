using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEMotion.Application.DTO.ResponseDTOs
{
    public class ChapterResponseDTO
    {
        public Guid ChapterId { get; set; }
        public string SubjectName { get; set; }

        public string ChapterName { get; set; }

        //public string Title { get; set; }
        public string Status { get; set; }
    }
}
