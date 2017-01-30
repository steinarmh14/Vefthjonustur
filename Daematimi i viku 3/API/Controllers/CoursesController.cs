using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Assign2.Models;
using Assign2.Services;

namespace Assign2.API.Controllers
{
    [Route("api/courses")]
    public class CoursesController : Controller
    {
        private readonly ICoursesService _service;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="service"> the connection to the services</param>
        public CoursesController(ICoursesService service)
        {
            _service = service;
        }

        /// <summary>
        /// Get courses by semester
        /// Example : "20163" should return all courses on semester 20163
        /// </summary>
        /// <returns> <summary>
        /// return all courses on semester 20163
        /// </summary>
        /// <param name="semester"> The semester for the courses</param>
        /// <returns>Courses of that certain semester</returns>
        [HttpGet]
        public List<CourseLiteDTO> GetCoursesBySemester(string semester = null)
        {
            return _service.GetCoursesBySemester(semester);
        }
         
         /// <summary>
         /// Get courses by ID
         /// Example: 1 should return the course with the ID = 1
         /// </summary>
         /// <param name="ID"> The ID of the course</param>
         /// <returns>The course with that ID</returns>
         [HttpGet]
        [Route("{ID:int}")]
        public IActionResult GetCoursesByID(int ID)
        {
            var course = _service.GetCoursesByID(ID);
            if (course != null)
            {
                return Ok(course);
            }
            return StatusCode(404);
        }

        /// <summary>
        /// Deletes course from the database with this ID if the ID doesnt exist it returns status code 404
        /// Example: 1 should delete the course with the ID = 1 from the database
        /// </summary>
        /// <param name="ID">The ID of the course</param>
        /// <returns>Status code 204 if delete worked. Status code 404 if delete didnt work</returns>
        [HttpDelete]
        [Route("{ID:int}")]
        public IActionResult DeleteCoursesByID(int ID)
        {
            var course = _service.GetCoursesByID(ID);
            if (course != null)
            {
                return StatusCode(204);
            }
            return StatusCode(404);
        }
        
        /// <summary>
        /// Updates the course by its ID, if the ID doesnt exist it returns status code 404
        /// </summary>
        /// <param name="ID"> The ID of the course</param>
        /// <param name="startDate"> The start date of the course</param>
        /// <param name="endDate"> The end date of the course</param>
        /// <returns>Status code 204 if update worked. Status code 404 if update didnt work</returns>
        [HttpPut]
        public IActionResult updateCourse(int ID, DateTime startDate, DateTime endDate)
        {
            var course = _service.GetCoursesByID(ID);
            if (course != null)
            {
                return StatusCode(204);
            }
            return StatusCode(404);
        }
        
    }
}
