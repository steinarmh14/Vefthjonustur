using System.Linq;
using System.Collections.Generic;
using Assign2.Models;
using System;

namespace Assign2.Services
{
         /// <summary>
        /// CourseService inherits from ICoursesService
        /// </summary>
    public class CoursesService : ICoursesService
    {
        /// <summary>
        /// Connection to the database with AppDataContext variable
        /// </summary>
        private readonly AppDataContext _db;

        /// <summary>
        /// Constructor for CoursesServices
        /// </summary>
        public CoursesService(AppDataContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Get course by id
        /// </summary>
        /// <param name="ID">ID is the id of the course user is searching for </param>
        /// <returns>The course with that ID</returns>
        public CourseLiteDTO GetCoursesByID(int ID)
        {
            if(ID != null)
            {
                var result = _db.Courses
                .Where(x => x.ID == ID)
                .Select(x => new CourseLiteDTO{
                        ID = x.ID,
                        Name = x.Name,
                        Semester = x.Semester
                }).SingleOrDefault();

                return result;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Get a list of courses of a certain semester
        /// </summary>
        /// <param name="semester">The semester which the courses from should be displayed</param>
        /// <returns>List of the courses of that semester</returns>
        public List<CourseLiteDTO> GetCoursesBySemester(string semester)
        {
            if (semester != null)
            {
                var result = _db.Courses
                .Where(x => x.Semester == semester)
                .OrderBy(x => x.Name)
                .Select(x => new CourseLiteDTO{
                    ID = x.ID,
                 Name = x.Name,
                 Semester = x.Semester
                }).ToList();
            return result;

            }
            else
            {
                var result = _db.Courses
                .Where(x => x.Semester == "20163")
                .OrderBy(x => x.Name)
                .Select(x => new CourseLiteDTO{
                    ID = x.ID,
                 Name = x.Name,
                 Semester = x.Semester
                }).ToList();
            return result;
            }
            return null;
        }
        /// <summary>
        /// Deletes a course by ID from the database
        /// </summary>
        /// <param name="ID">The ID of the course which should be deleted</param>
        public void DeleteCoursesByID(int ID)
        {
            if(ID != null)
            {
                var result = _db.Courses.SingleOrDefault(x => x.ID == ID);
                _db.Courses.Remove(result);
                _db.SaveChanges();
            }            
        }

        /// <summary>
        /// Updates a course by ID
        /// </summary>
        /// <param name="ID">The ID of the course</param>
        /// <param name="startDate">The new start date of the course</param>
        /// <param name="endDate">The new end date of the course</param>
        public void updateCourse(int ID, DateTime startDate, DateTime endDate)
        {
            var result = _db.Courses.SingleOrDefault(x => x.ID == ID);

            if(result != null)
            {
                result.startDate = startDate;
                result.endDate = endDate;

                 _db.SaveChanges();
            }
        }
    }
}