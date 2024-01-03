using System;
using System.Collections.Generic;
using System.Linq;

namespace CandidateAssessment.Models
{
    public partial class School
    {
        public School()
        {
            Students = new HashSet<Student>();
        }

        public int SchoolId { get; set; }
        public string? Name { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }

        public virtual ICollection<Student> Students { get; set; }
    }
}
