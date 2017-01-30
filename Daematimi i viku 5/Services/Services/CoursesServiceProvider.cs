using System.Collections.Generic;
using System.Linq;
using CoursesAPI.Models;
using CoursesAPI.Services.DataAccess;
using CoursesAPI.Services.Exceptions;
using CoursesAPI.Services.Models.Entities;

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
			//Gets the course
			var course = _courseInstances.All().SingleOrDefault(x => x.ID == courseInstanceID);			
			//If there was no course with that id then throw AppObjectNotFoundException
			if(course == null) 
			{
				throw new AppObjectNotFoundException();
			}

			//Get the person
			var person = _persons.All().SingleOrDefault(x => x.SSN == model.SSN);
			//If the person doesnt excist then throw AppObjectNotFoundException
			if(person == null)
			{
				throw new AppObjectNotFoundException();
			}
			
			//Check if we are adding main teacher or assistant
			if(model.Type == TeacherType.MainTeacher)
			{
				//Check if there already is an main teacher
				var teacher = _teacherRegistrations.All().SingleOrDefault(x => x.CourseInstanceID == courseInstanceID && x.Type == TeacherType.MainTeacher);
				//Throw AppValidationException if ther was already a main teacher
				if(teacher != null)
				{
					throw new AppValidationException("COURSE_ALREADY_HAS_A_MAIN_TEACHER");
				}
			}
			
			//check if person was already registered a main teacher
			var personRegisterdAsTeacher = _teacherRegistrations.All().SingleOrDefault(x => x.CourseInstanceID == courseInstanceID && x.SSN == model.SSN);
			//Throw AppValidationException if the person already was registerd teacher in the course
			if(personRegisterdAsTeacher != null)
			{
				throw new AppValidationException("PERSON_ALREADY_REGISTERED_TEACHER_IN_COURSE");
			}

			//Add the new teacher
			TeacherRegistration newTeacher = new TeacherRegistration{SSN = model.SSN, CourseInstanceID = courseInstanceID, Type = model.Type};
			_teacherRegistrations.Add(newTeacher);
			_uow.Save();
			
			//The person to return
			PersonDTO thePerson = new PersonDTO{Name = person.Name, SSN = person.SSN};

			return thePerson;
		}

		/// <summary>
		/// You should write tests for this function. You will also need to
		/// modify it, such that it will correctly return the name of the main
		/// teacher of each course.
		/// </summary>
		/// <param name="semester">The semester which we are looking for courses on</param>
		/// <returns>Should return List of courses which are on that particular semester</returns>
		public List<CourseInstanceDTO> GetCourseInstancesBySemester(string semester = null)
		{
			//puts semester as 20153 if it wasnt any value put in semester
			if (string.IsNullOrEmpty(semester))
			{
				semester = "20153";
			}

			//Get list of courses with that semester
			var courses = (from c in _courseInstances.All()
				join ct in _courseTemplates.All() on c.CourseID equals ct.CourseID
				where c.SemesterID == semester
				select new CourseInstanceDTO
				{
					Name               = ct.Name,
					TemplateID         = ct.CourseID,
					CourseInstanceID   = c.ID,
					MainTeacher        = (from teacher in _teacherRegistrations.All() //Getting the teacher name.
										  where c.ID == teacher.CourseInstanceID
										  join person in _persons.All() on teacher.SSN equals person.SSN
										  select person.Name).SingleOrDefault() ?? ""
				}).ToList();

			return courses;
		}
	}
}
