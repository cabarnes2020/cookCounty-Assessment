using CandidateAssessment.Models;
using CandidateAssessment.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace CandidateAssessment.Controllers
{
    public class RecordsController : Controller
    {
        private StudentService _studentService;
        private SchoolService _schoolService;
        public RecordsController(StudentService studentService, SchoolService schoolService)
        {
            _studentService = studentService;
            _schoolService = schoolService;
        }

        public IActionResult Students()
        {
            ViewBag.AgeList = CreateAgeList();
            ViewBag.SchoolList = CreateSchoolDropdownList();
            ViewBag.OrgList = CreateStudentOrgDropdown();

            var model = _studentService.GetStudents().OrderBy(s => s.LastName);
            return View(model);
        }

        public IActionResult Schools()
        {
            var model = _schoolService.GetSchools().OrderBy(s => s.Name);
            return View(model);
        }

        [HttpGet]
        public IActionResult SchoolsStudents(string id)
        {
            //Convert query string parameter value to an int
            int schoolId = Int32.Parse(id);

            //Retrieved a singular school from database using the query parameter value
            var school = _schoolService.GetASchool(schoolId);
            return View(school);
        }

        [HttpPost]
        public IActionResult SaveStudent(Student model)
        {
           //Initialize sql connection for saving new student model to db
            using(SqlConnection connection = new SqlConnection("Server=tcp:applicanttest.database.windows.net,1433;Initial Catalog=ApplicantTest;Persist Security Info=False;User ID=user2023;Password=C00kC0unty2023"))
            {
                //Created insert query for new student model
                string insertStudentQuery = "INSERT INTO dbo.Students (SchoolId, FirstName, LastName, Email, Age) VALUES (@SchoolId, @FirstName, @LastName, @Email, @Age)";
                using(SqlCommand command = new SqlCommand(insertStudentQuery, connection))
                {
                    connection.Open();
                    command.Parameters.AddWithValue("@SchoolId", model.SchoolId);
                    command.Parameters.AddWithValue("@FirstName", model.FirstName);
                    command.Parameters.AddWithValue("@LastName", model.LastName);
                    command.Parameters.AddWithValue("@Email", model.Email);
                    command.Parameters.AddWithValue("@Age", model.Age);

                   //Executed query command to save new student model to db
                    int result = command.ExecuteNonQuery();
                    
                    if(result < 0)
                        Console.WriteLine("Error inserting data into Database!");

                   
                }  
            }


            var students = _studentService.GetStudents().OrderBy(s => s.LastName);
            //Query expression that grabs id of the newly saved student from the above insert query
            var newStudent = 
                from student in students 
                where student.FirstName == model.FirstName && student.LastName == model.LastName
                select student.StudentId;
            
            //Initalize sql connection for saving possible org assignments of the new student to the db
            using(SqlConnection orgAssignmentConnection = new SqlConnection("Server=tcp:applicanttest.database.windows.net,1433;Initial Catalog=ApplicantTest;Persist Security Info=False;User ID=user2023;Password=C00kC0unty2023"))
            {
                //Insert query for org assignments
                string insertOrgQuery = "INSERT INTO dbo.OrgAssignments ([StudentOrgId], [StudentId]) VALUES (@StudentOrgId, @StudentId)";
                using(SqlCommand orgCommand = new SqlCommand(insertOrgQuery, orgAssignmentConnection))
                {
                    orgAssignmentConnection.Open();
                    orgCommand.Parameters.Add(new SqlParameter("@StudentOrgId", System.Data.SqlDbType.Int));
                    orgCommand.Parameters.Add(new SqlParameter("@StudentId", System.Data.SqlDbType.Int));

                    //Loop through each studentOrg that the new student selected, 
                    //and append the studentId and studentOrgId to their respective parameters
                    for(int i = 0; i < model.SelectedOrgs.Count(); i++){
                        orgCommand.Parameters[0].Value = model.SelectedOrgs[i];
                        orgCommand.Parameters[1].Value = newStudent.First();

                        //Execute insert query for each org that was selected
                        int orgResult = orgCommand.ExecuteNonQuery();
                        if(orgResult < 0)
                        Console.WriteLine("Error inserting org assignments into Database!");
                    }

                    orgAssignmentConnection.Close();
                }          
            }
            return RedirectToAction("Students");
        }

        private List<SelectListItem> CreateAgeList()
        {
            var ageList = new List<SelectListItem>();
            for (int i = 18; i < 100; i++)
            {
                ageList.Add(new SelectListItem
                {
                    Text = i.ToString(),
                    Value = i.ToString()
                });
            }
            return ageList;
        }

        private List<SelectListItem> CreateSchoolDropdownList()
        {
            //Variable for the collection of schools in the db
            var schools = _schoolService.GetSchools().OrderBy(s => s.Name);

            //Variable for the return value of this method 
            var schoolList = new List<SelectListItem>();

            //Loop used to convert each school in the collection, to a list item in the dropdown menu
            foreach(var s in schools){
                schoolList.Add(new SelectListItem {
                    Text = s.Name,
                    Value = s.SchoolId.ToString(),
                });
            }
            return schoolList;
        }

        private MultiSelectList CreateStudentOrgDropdown()
        {
            //Initialize List of select items to form multi-select list
            var options = new List<SelectListItem>();
           
           //list of students to perform query on
            var students = _studentService.GetStudents().OrderBy(s => s.FirstName);

            //query used to grab each orgAssignment on each student
            var studentOrgs = 
                from student in students
                select student.OrgAssignments;

            //A HashSet variable used to hold each distinct studentOrg within the database
            var distinctOrgs = new HashSet<StudentOrganization>();

            //Used a double for loop to extract nested studentOrgs into the hashset, 
            foreach(var org in studentOrgs){
                foreach(var o in org){
                    distinctOrgs.Add(o.StudentOrg);
                }
            }

            //Looped over hashset to create a list item for each distinct studentOrg within the set 
            foreach(var org in distinctOrgs){
                options.Add(new SelectListItem {
                    Text = org.OrgName,
                    Value = org.Id.ToString()
                });
            }

            return new MultiSelectList(options, "Value", "Text");
        }
    }
}