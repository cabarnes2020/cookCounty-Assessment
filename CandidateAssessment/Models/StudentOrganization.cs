using System;
using System.Collections.Generic;

namespace CandidateAssessment.Models
{
    public partial class StudentOrganization
    {
        public StudentOrganization()
        {
            OrgAssignments = new HashSet<OrgAssignment>();
        }

        public int Id { get; set; }
        public string? OrgName { get; set; }
        public string? OrgDescription { get; set; }

        public virtual ICollection<OrgAssignment> OrgAssignments { get; set; }
    }
}
