using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using cw4.Models;
using Microsoft.AspNetCore.Mvc;

namespace cw4.Controllers
{
    [ApiController]
    [Route("api/students")]
    public class StudentsController : ControllerBase
    {
        private const string name = "Data Source=db-mssql;Initial Catalog=s18593;Integrated Security=True";
        [HttpGet("{id}")]
        public IActionResult GetStudents(string id)
        {
            var list = new List<Student>();

            using (var client = new SqlConnection(name))
            using(var com = new SqlCommand())
            {
                string zmienna = "111";
                com.Connection = client;
                //com.CommandText = $"select Student.IndexNumber, Student.FirstName, Student.LastName, Studies.Name, Enrollment.Semester from Student inner join Enrollment on Student.IdEnrollment = Enrollment.IdEnrollment join Studies on Enrollment.IdStudy = Studies.IdStudy where Student.IndexNumber ={id}";
                com.CommandText = "select * from Student where IndexNumber=@zmienna";
                com.Parameters.AddWithValue("zmienna", zmienna);

                client.Open();
                var dr = com.ExecuteReader();
                while (dr.Read())
                {
                    var st = new Student();
                    st.IndexNumber = dr["IndexNumber"].ToString();
                    st.FirstName = dr["FirstName"].ToString();
                    st.LastName = dr["LastName"].ToString();
                    st.BirthDate = DateTime.Parse(dr["BirthDate"].ToString());
                    st.IdEnrollment = int.Parse(dr["IdEnrollment"].ToString());
                    list.Add(st);
                }
            }
            return Ok(list);
        }

        [HttpGet]
        public IActionResult GetStudents()
        {
            return Ok("Done!");
        }
    }
}