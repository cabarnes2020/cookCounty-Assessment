using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CandidateAssessment.Models
{
    public partial class Student
    {
        public Student()
        {
            OrgAssignments = new HashSet<OrgAssignment>();
            SelectedOrgs = new List<int>();
        }

        public int StudentId { get; set; }
        public int SchoolId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public int? Age { get; set; }

        public virtual School School { get; set; } = null!;
        public virtual ICollection<OrgAssignment> OrgAssignments { get; set; }

        [NotMapped]
        public List<int> SelectedOrgs { get; set; }
    }
}
