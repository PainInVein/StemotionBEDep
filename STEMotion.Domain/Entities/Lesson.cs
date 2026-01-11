using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEMotion.Domain.Entities
{
    public class Lesson
    {
        public Guid LessonId { get; set; }
        public Guid ChapterId { get; set; }
        public string Title { get; set; }
        public int? EstimatedTime { get; set; } 
        public virtual Chapter Chapter { get; set; }
    }
}
