using System;

namespace Assign2.Models
{
    /// <summary>
    /// This class hold information of a course
    /// </summary>
    public class CourseLiteDTO
    {
        /// <summary>
        /// Database-generated ID of the course
        /// Example: 1
        /// </summary>
        /// <returns></returns>
        public int ID { get; set; }

        /// <summary>
        /// The name of the course.
        /// Example: "Web Service"
        /// </summary>
        /// <returns></returns>
        public string Name { get; set; }
        
        /// <summary>
        /// Number of the semester.
        /// Example: "20163"
        /// </summary>
        /// <returns></returns>
        public string Semester { get; set; }
        
        /// <summary>
        /// Start date of the course.
        /// </summary>
        /// <returns></returns>
        public System.DateTime startDate {get; set;}
        /// <summary>
        /// The end date of the course.
        /// </summary>
        /// <returns></returns>
        public System.DateTime endDate {get; set;}
        
    }
}
