using CandidateAssessment.Models;
using Microsoft.EntityFrameworkCore;


/* 
    I originally created this service in order to retrieve all of the studentOrgs within the database. 

    Even though I feel this is the most maintainable way to retrieve 
    this data and any other studentOrg related data (as well as the solution that would be used in a real world application),
    I felt that the way the assessment was designed was to test one's querying abilities.
    
    Therefore, I opted to go that route since a studentOrg service was not provided
*/

namespace CandidateAssessment.Services
{
    public class StudentOrganizationService
    {
        private CandidateAssessmentContext _dbContext;

        public StudentOrganizationService(CandidateAssessmentContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<StudentOrganization> GetStudentOrgs()
        {
            return _dbContext.StudentOrganizations
                .Include(s => s.OrgAssignments);
        }
    }
}