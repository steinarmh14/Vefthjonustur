using System;
using System.ComponentModel.DataAnnotations;

namespace Assignment1.models
{
    public class Student
    {
        [Required]
        public int SSN { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
