using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using cw4.DAL;
using cw4.Models;
using Microsoft.AspNetCore.Mvc;

namespace cw4.Controllers
{
    //[ApiController]
    //[Route("api/students")]
    //public class StudentsController : ControllerBase
    //{
    //    private IDbService _dbService;

    //    public StudentsController(IDbService dbService)
    //    {
    //        _dbService = dbService;
    //    }

    //    [HttpGet]
    //    public IActionResult GetStudents([FromServices]IDbService dbService)
    //    {
    //        var list = new List<Student>();
    //        using (SqlConnection con = new SqlConnection("Data Source=db-mssql; Initial Catalog=s18593;Integrated Security=True"))
    //        using (SqlCommand com = new SqlCommand())
    //        {

    //            com.Connection = con;
    //            com.CommandText = "select IndexNumber, firstname, lastname, birthdate, name, semester from Student inner join Enrollment on Enrollment.IdEnrollment = Student.IdEnrollment inner join Studies on Enrollment.IdStudy = Studies.IdStudy";

    //            con.Open();
    //            var dr = com.ExecuteReader();
    //            while (dr.Read())
    //            {
    //                var st = new Student();
    //                st.IndexNumber = dr["IndexNumber"].ToString();
    //                st.FirstName = dr["FirstName"].ToString();
    //                st.LastName = dr["LastName"].ToString();
    //                st.BirthDate = DateTime.Parse(dr["BirthDate"].ToString());
    //                st.IdEnrollment = int.Parse(dr["IdEnrollment"].ToString());
    //                list.Add(st);

    //            }
    //            con.Close();
    //        }

    //        return Ok(list);
    //    }

    //    [HttpGet("{IndexNumber}")]
    //    public IActionResult GetStudent(string indexNumber)
    //    {
    //        int index = int.Parse(indexNumber);
    //        using (SqlConnection con = new SqlConnection("Data Source=db-mssql; Initial Catalog=s18593;Integrated Security=True"))
    //        using (SqlCommand com = new SqlCommand())
    //        {
    //            com.Connection = con;
    //            com.CommandText = "Select * from student where IndexNumber =@index";
    //            com.Parameters.AddWithValue("index", index);

    //            con.Open();
    //            var dr = com.ExecuteReader();
    //            if (dr.Read())
    //            {
    //                var st = new Student();
    //                st.IndexNumber = dr["IndexNumber"].ToString();
    //                st.FirstName = dr["FirstName"].ToString();
    //                st.LastName = dr["LastName"].ToString();
    //                st.BirthDate = DateTime.Parse(dr["BirthDate"].ToString());
    //                st.IdEnrollment = int.Parse(dr["IdEnrollment"].ToString());
    //                return Ok(st);

    //            }
    //            con.Close();
    //            return NotFound();

    //        }
    //    }
    }

    //[HttpGet]
    //public IActionResult GetStudents()
    //{
    //    return Ok("Done!");
    //}