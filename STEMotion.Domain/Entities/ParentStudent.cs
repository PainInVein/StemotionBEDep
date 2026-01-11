using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEMotion.Domain.Entities
{
    public class ParentStudent
    {
        public Guid ParentId { get; set; }
        public Guid StudentId { get; set; }
        public virtual User Parent { get; set; }
        public virtual User Student { get; set; }
    }
}
