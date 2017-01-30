using System;
namespace Assign2.Services.Entities
{
         /// <summary>
        /// Class for course
        /// Course has id, name, semester, start and end date
        /// </summary>
        /// <returns></returns>
    public class Course
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Semester { get; set; } 
        public System.DateTime startDate {get; set;}
        public System.DateTime endDate {get; set;}
    }
}