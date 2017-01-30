using System.Collections.Generic;
using System.Linq;
using CoursesAPI.Models;
using CoursesAPI.Services.DataAccess;
using CoursesAPI.Services.Exceptions;
using CoursesAPI.Services.Models.Entities;
using System;

namespace CoursesAPI.Services.Services
{
	public class CoursesServiceProvider
	{
		private readonly IUnitOfWork _uow;

		private readonly IRepository<CourseInstance> _courseInstances;
		private readonly IRepository<TeacherRegistration> _teacherRegistrations;
		private readonly IRepository<CourseTemplate> _courseTemplates; 
		private readonly IRepository<Person> _persons;

		public CoursesServiceProvider(IUnitOfWork uow)
		{
			_uow = uow;

			_courseInstances      = _uow.GetRepository<CourseInstance>();
			_courseTemplates      = _uow.GetRepository<CourseTemplate>();
			_teacherRegistrations = _uow.GetRepository<TeacherRegistration>();
			_persons              = _uow.GetRepository<Person>();
		}

		/// <summary>
		/// You should implement this function, such that all tests will pass.
		/// </summary>
		/// <param name="courseInstanceID">The ID of the course instance which the teacher will be registered to.</param>
		/// <param name="model">The data which indicates which person should be added as a teacher, and in what role.</param>
		/// <returns>Should return basic information about the person.</returns>
		public PersonDTO AddTeacherToCourse(int courseInstanceID, AddTeacherViewModel model)
		{
			// TODO: implement this logic!
			return null;
		}

		/// <summary>
		/// You should write tests for this function. You will also need to
		/// modify it, such that it will correctly return the name of the main
		/// teacher of each course.
		/// </summary>
		/// <param name="semester">What semester you are looking for courses at</param>
		/// <param name="lang">What language to display on</param>
		/// <param name="page">What page of courses do you want</param>
		/// <returns>A list of courses</returns>
		public Envelope GetCourseInstancesBySemester(string lang, int page, string semester = null)
		{
			if (string.IsNullOrEmpty(semester))
			{
				semester = "20153";
			}

			var courses = (from c in _courseInstances.All()
				join ct in _courseTemplates.All() on c.CourseID equals ct.CourseID
				where c.SemesterID == semester
				select new CourseInstanceDTO
				{
					Name               = (lang == "en") ? ct.NameEN : ct.Name, //If language is english then write out english version.
					TemplateID         = ct.CourseID,
					CourseInstanceID   = c.ID,
					MainTeacher        = "" // Hint: it should not always return an empty string!
				});
			
			//Envelope with courses on a specified page, how many course on a page, how many pages there are, how many courses are on a page
			var env = new Envelope{
				
				coursesList = courses.Skip(page-1*10).Take(10).ToList(),
				
				pageInfo =  new PageInfo{
					pageSize = 10,
					pageNumber = page,
					totalNumberOfItems = courses.Count(),
					pageCount = (int) Math.Ceiling(Convert.ToDouble(courses.Count())/Convert.ToDouble(10))
				}
			};


			return env;
		}
	}
}
