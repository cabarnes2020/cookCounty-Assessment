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
        private StudentOrganizationService _studentOrgService;
        public RecordsController(StudentService studentService, SchoolService schoolService, StudentOrganizationService studentOrgService)
        {
            _studentService = studentService;
            _schoolService = schoolService;
            _studentOrgService = studentOrgService;
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
            //Sent model to student service to handle "saving to db" logic
            _studentService.CreateStudent(model);
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
           
           //collection of all studentOrgs within the database
            var studentOrgs = _studentOrgService.GetStudentOrgs().OrderBy(so => so.OrgName);

            //Loop used to convert each studentOrg in the collection, to a list item in the dropdown menu, 
            foreach(var org in studentOrgs){
                options.Add(new SelectListItem {
                    Text = org.OrgName,
                    Value = org.Id.ToString(),
                });
            }
            return new MultiSelectList(options, "Value", "Text");
        }
    }
}