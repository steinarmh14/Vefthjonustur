using Microsoft.AspNetCore.Mvc;

using CoursesAPI.Models;
using CoursesAPI.Services.DataAccess;
using CoursesAPI.Services.Services;

namespace CoursesAPI.Controllers
{
	[Route("api/courses")]
	public class CoursesController : Controller
	{
		private readonly CoursesServiceProvider _service;

		public CoursesController(IUnitOfWork uow)
		{
			_service = new CoursesServiceProvider(uow);
		}

		[HttpGet]
		public IActionResult GetCoursesBySemester(int page, string semester = null)
		{
			//passing language with : Request.Headers["accept-language"].ToString()
			return Ok(_service.GetCourseInstancesBySemester(Request.Headers["accept-language"].ToString(), page, semester));
		}

		/// <summary>
		/// </summary>
		/// <param name="id"></param>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost]
		[Route("{id}/teachers")]
		public IActionResult AddTeacher(int id, AddTeacherViewModel model)
		{
			var result = _service.AddTeacherToCourse(id, model);
			return Created("TODO", result);
		}
	}
}
