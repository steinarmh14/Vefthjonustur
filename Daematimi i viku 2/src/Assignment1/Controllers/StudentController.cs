using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Assignment1.models;


[Route("/students")]
public class StudentController : Controller
{

    // Note: the variable is static such that the data will persist during
    // the execution of the web service. Data will be lost when the service
    // is restarted (and that is OK for now).
    private static List<Student> _student;

    public StudentController()
    {
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
};
