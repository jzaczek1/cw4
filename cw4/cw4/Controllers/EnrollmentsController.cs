using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using cw4.DTOs.Requests;
using cw4.DTOs.Responses;
using cw4.Models;
using cw4.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace cw4.Controllers
{
    [Route("api/enrollments")]
    [ApiController] //-> implicit model validation
    public class EnrollmentsController : ControllerBase
    {
        private IStudentsDbService _service;

        public EnrollmentsController(IStudentsDbService service)
        {
            _service = service;
        }

        [HttpPost("enrollStudent")]
        public IActionResult EnrollStudent([FromBody] Student Student)
        {
            return StatusCode((int)HttpStatusCode.Created, _service.EnrollStudent(Student));
        }

        [HttpPost("promotions")]
        public IActionResult PromoteStudents([FromBody] StudSem StudiesSemester)
        {
            return StatusCode((int)HttpStatusCode.Created, _service.PromoteStudents(StudiesSemester));
        }

    }
}