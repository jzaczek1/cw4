using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using cw4.DTOs.Requests;
using cw4.Models;
using cw4.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace cw4.Controllers
{
    [Route("api/enrollments")]
    [ApiController] //-> implicit model validation
    public class EnrollmentsController : ControllerBase
    {
        private string con = "Data Source=db-mssql;Initial Catalog=s18593;Integrated Security=True";
        private IStudentsDbService _service;

        public EnrollmentsController(IStudentsDbService service)
        {
            _service = service;
        }

        public IConfiguration _configuration { get; set; }
        public EnrollmentsController(IConfiguration configuration, IStudentsDbService service)
        {
            _configuration = configuration;
            _service = service;
        }

        [HttpPost]
        public IActionResult Login(Login request)
        {
            var list = _service.Login(request);

            if (list == null)
            {
                return Unauthorized("401 Unathorized Error!");
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, list[0]),
                new Claim(ClaimTypes.Name, list[1]),
                new Claim(ClaimTypes.Role, list[2]),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken
            (
                issuer: "Gakko",
                audience: "Students",
                claims: claims,
                expires: DateTime.Now.AddMinutes(10),
                signingCredentials: creds
            );

            var refreshToken = Guid.NewGuid();
            _service.RefreshToken(refreshToken.ToString(), list[0]);

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                refreshToken = Guid.NewGuid()
            });
        }

        [HttpPost("refresh-token/{requestToken}")]
        public IActionResult RefreshToken(string requestToken)
        {
            var list = _service.TokenExists(requestToken);
            if (list == null)
            {
                return Unauthorized("401 Unathorized Error!");
            }
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, list[0]),
                new Claim(ClaimTypes.Name, list[1]),
                new Claim(ClaimTypes.Role, list[2]),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken
            (
                issuer: "Gakko",
                audience: "Students",
                claims: claims,
                expires: DateTime.Now.AddMinutes(10),
                signingCredentials: creds
            );

            var refreshToken = Guid.NewGuid();
            _service.RefreshToken(refreshToken.ToString(), list[0]);

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                refreshToken = Guid.NewGuid()
            });
        }

        [HttpPost("encryption")]
        public IActionResult EncryptMe()
        {
            bool result = _service.encodePasswords(con);
            if (result == false)
            {
                return BadRequest("Encryption has failed");
            }
            return Ok("Encryption has successfuly finished");
        }

        [HttpPost("enroll")]
        [Authorize(Roles = "employee")]
        public IActionResult EnrollStudent(EnrollStudentRequest request)
        {
            var enrollment = _service.EnrollStudent(request);
            if (enrollment == null)
            {
                return BadRequest("400 Bad Request Error!");
            }
            return CreatedAtAction(nameof(EnrollStudent), enrollment);
        }


        [HttpPost("promotion")]
        [Authorize(Roles = "employee")]
        public IActionResult PromoteStudents(EnrollStudentRequest request)
        {
            var enrollment = _service.PromoteStudents(request);
            if (enrollment == null)
            {
                return BadRequest("400 Bad Request Error!");
            }
            return CreatedAtAction(nameof(EnrollStudent), enrollment);
        }

        //[HttpGet("{MoodPill}")]
        //public IActionResult MoodPill(int MoodPill)
        //{
        //    if (MoodPill == 888)
        //    {
        //        return Ok("Don't accidentally choke: https://www.youtube.com/watch?v=_tSWSpvNT1Y");
        //    }
        //    return BadRequest("Ok, but try again next time, if you like :)");
        //}
    }


    //lepiej napisać metody od początku a te sobie zostawić w razie ew. powtórki do kolokwium
    //
    //[HttpPost("enrollStudent")]
    //public IActionResult EnrollStudent([FromBody] Student Student)
    //{
    //    return StatusCode((int)HttpStatusCode.Created, _service.EnrollStudent(Student));
    //}

    //[HttpPost("promotions")]
    //public IActionResult PromoteStudents([FromBody] StudSem StudiesSemester)
    //{
    //    return StatusCode((int)HttpStatusCode.Created, _service.PromoteStudents(StudiesSemester));
    //}

}