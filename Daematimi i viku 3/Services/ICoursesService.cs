using System.Collections.Generic;
using Assign2.Models;
using System;

namespace Assign2.Services
{
    /// <summary>
    /// Connection to CoursesService
    /// </summary>
    public interface ICoursesService
    {
        /// <summary>
        /// Get courses by the semester
        /// </summary>
        /// <param name="semester">The semester which the courses from should be displayed</param>
        /// <returns>List of courses of that particular semester</returns>
        List<CourseLiteDTO> GetCoursesBySemester(string semester);
        /// <summary>
        /// Get course by an ID
        /// </summary>
        /// <param name="ID">The ID of the course</param>
        /// <returns>The course by that ID</returns>
        CourseLiteDTO GetCoursesByID(int ID);
        /// <summary>
        /// Deletes course by id from the database
        /// </summary>
        /// <param name="ID">The ID of the course which should be deleted from the database</param>
        void DeleteCoursesByID(int ID);
        /// <summary>
        /// Updates a course by ID
        /// </summary>
        /// <param name="ID">The ID of the course</param>
        /// <param name="startDate">The start date of the course</param>
        /// <param name="endDate">The end date of the course</param>
        void updateCourse(int ID, DateTime startDate, DateTime endDate);

    }
}