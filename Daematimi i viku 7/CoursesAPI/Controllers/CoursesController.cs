using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace CoursesAPI.Controllers
{
    [Route("api/courses")]
    [Authorize]
    public class CoursesController : Controller
    {
        // GET api/courses
        [HttpGet]
        [AllowAnonymous]
        public IActionResult getCourses()
        {
            return Ok("Courses successfully retrieved");
        }

        // POST api/courses as teacher
        [HttpPost]
        [Authorize(Policy = "Teacher")]
        public IActionResult addCourse()
        {
            return Created("Created!","Course successfully created");
        }
        
        // GET api/values/id
        [HttpGet("{id}")]
        public IActionResult getSingleCourse(int id)
        {
            return Ok("Course successfully retrieved");
        }
    }
}
