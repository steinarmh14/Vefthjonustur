using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Assignment1.models;


[Route("/api/courses")]
public class CoursesController : Controller
{

    // Note: the variable is static such that the data will persist during
    // the execution of the web service. Data will be lost when the service
    // is restarted (and that is OK for now).
    private static List<Course> _courses;
    private static List<Student> _student;


    public CoursesController()
    {
        if (_courses == null)
        {
            _courses = new List<Course>
             {
                 new Course
                 {
                     ID         = 1,
                     Name       = "Web services",
                     TemplateID = "T-514-VEFT",
                     StartDate  = DateTime.Now,
                     EndDate    = DateTime.Now.AddMonths(3),
                     students   = _student; 
                 },
                 new Course
                 {
                     ID = 2,
                     Name = "Tölvusamskipti",
                     TemplateID = "TSAM",
                     StartDate = DateTime.Now,
                     EndDate = DateTime.Now.AddMonths(3)
                     students = _student;
    }
            };
        }
        if (_student == null)
        {
            _student = new List<Student>
             {
                 new Student
                 {
                     SSN         = 260593,
                     Name       = "Sigmar",
                 },
                 new Student
                 {
                     SSN = 101096,
                     Name = "Birta",
                 }

            };
        }
    }
    
    // GET api/courses
    [HttpGet]
    public IEnumerable<Course> GetCourses()
    {
        return _courses;
    }
    // POST api/courses
    [HttpPost]
    public IActionResult CreateCourse(int id, string name, string templateID, DateTime startDate, DateTime endDate)
    {
        Course newCourse = new Course { ID = id, Name = name, TemplateID = templateID, StartDate = startDate, EndDate = endDate };
        _courses.Add(newCourse);
        var location = Url.Link("GetCourse", new { id = newCourse.ID });
        return Created(location,newCourse);
    }

    [HttpGet]
    [Route("{id:int}", Name = "GetCourse")]
    public IActionResult GetCourseByID(int id)
    {
        Course c = _courses.Find(item => item.ID == id);
        if (c == null)
        {
            return StatusCode(404);
        }
        return Ok(c);
    }

    [HttpPut]
    [Route("{id:int}", Name = "GetCourse")]
    public IActionResult UpdateCourseByID(int id, string name, string templateID, DateTime startDate, DateTime endDate)
    {
        Course c = _courses.Find(item => item.ID == id);
        if (c == null)
        {
            return StatusCode(404);
        }
        c.ID = id;
        c.Name = name;
        c.TemplateID = templateID;
        c.StartDate = startDate;
        c.EndDate = endDate;
        return Ok(c);
    }

    [HttpDelete]
    [Route("{id:int}", Name = "GetCourse")]
    public IActionResult DeleteCourseByID(int id)
    {
        Course c = _courses.Find(item => item.ID == id);
        if (c == null)
        {
            return StatusCode(404);
        }
        _courses.Remove(c);
        return StatusCode(204);
    }
    
    [HttpGet]
    [Route("{id:int}/students", Name = "GetStudents")]
    public IEnumerable<Student> GetStudentsInCourseByID(int id)
    {
        Course c = _courses.Find(item => item.ID == id);
        if (c == null)
        {
            return null;
        }
        
        return c.students;
    }


    [HttpPost]
    [Route("{id:int}/students", Name = "GetStudents")]
    public IActionResult AddStudentToCourse(int ssn, string name, int courseID)
    {
        Student newStudent = new Student { SSN = ssn, Name = name};
        Course c = _courses.Find(item => item.ID == courseID);
        if (c == null)
        {
            return StatusCode(400);
        }
        c.students.Add(newStudent);
        var location = Url.Link("GetStudents", new { ssn = newStudent.SSN });
        return Created(location, newStudent);
    }
    





}