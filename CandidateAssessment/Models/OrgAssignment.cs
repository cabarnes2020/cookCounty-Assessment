using System;
using System.Collections.Generic;

namespace CandidateAssessment.Models
{
    public partial class OrgAssignment
    {
        public int Id { get; set; }
        public int StudentOrgId { get; set; }
        public int StudentId { get; set; }

        public virtual Student Student { get; set; } = null!;
        public virtual StudentOrganization StudentOrg { get; set; } = null!;
    }
}
