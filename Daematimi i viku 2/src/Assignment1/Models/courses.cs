using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Assignment1.models;

namespace Assignment1.models
{
    public class Course
    {
        [Required]
        public int ID { get; set; }
        [Required]
        public string TemplateID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public System.DateTime StartDate { get; set; }
        [Required]
        public System.DateTime EndDate { get; set; }
        [Required]
        public List<Student> students = new List<Student>();
    }
}



