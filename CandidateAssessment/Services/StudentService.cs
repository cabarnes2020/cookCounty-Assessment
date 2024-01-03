using CandidateAssessment.Models;
using Microsoft.EntityFrameworkCore;

namespace CandidateAssessment.Services
{
    public class StudentService
    {
        private CandidateAssessmentContext _dbContext;

        public StudentService (CandidateAssessmentContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<Student> GetStudents()
        {
            return _dbContext.Students
                .Include(s => s.School)
                .Include(s => s.OrgAssignments)
                    .ThenInclude(oa => oa.StudentOrg);
        }

        //Function to save new student to db
        public Student CreateStudent(Student newStudent)
        {
            //Adds Student to db
            _dbContext.Students.Add(newStudent);
            _dbContext.SaveChanges();

            //Loop through newStudent's selected orgs, 
            //create a new orgAssigment obj for each org, then save each obj to db
            foreach(var org in newStudent.SelectedOrgs)
            {
                var orgAssignment = new OrgAssignment
                {
                    StudentOrgId = org,
                    StudentId = newStudent.StudentId
                };
                _dbContext.OrgAssignments.Add(orgAssignment);
                _dbContext.SaveChanges();
            }

            Student createdStudent = _dbContext.Students.Include(s => s.OrgAssignments).Single(s => s.FirstName == newStudent.FirstName && s.LastName == newStudent.LastName);
            return createdStudent;
        }
    }
}
