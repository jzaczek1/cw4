using cw4.DTOs.Requests;
using cw4.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace cw4.Services
{
    public class SqlServerStudentDbService : ControllerBase, IStudentsDbService
    {

        public Enrollment EnrollStudent([FromBody] Student Student)
        {
            using (SqlConnection connection = new SqlConnection("Data Source=db-mssql;Initial Catalog=s18593;Integrated Security=true"))
            using (SqlCommand command = new SqlCommand())
            {
                command.Parameters.AddWithValue("@indexNumber", Student.IndexNumber);
                command.Parameters.AddWithValue("@firstName", Student.FirstName);
                command.Parameters.AddWithValue("@lastName", Student.LastName);
                command.Parameters.AddWithValue("@birthDate", DateTime.Parse(Student.BirthDate.ToString()));
                command.Parameters.AddWithValue("@studies", Student.Studies);

                command.Connection = connection;
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();
                command.Transaction = transaction;

                command.CommandText = "SELECT * FROM Studies WHERE Name = @studies";

                SqlDataReader dr = command.ExecuteReader();

                if (!dr.HasRows)
                {
                    transaction.Rollback();
                }
                  // to wszystko ^ wykład
                command.CommandText = "SELECT IdStudy FROM Studies WHERE Name = @studies";
                dr.Read();
                string idStudy = dr["IdStudy"].ToString();
                dr.Close();

                command.Parameters.AddWithValue("@idStudy", idStudy);

                command.CommandText = "SELECT IdEnrollment FROM Enrollment WHERE IdStudy = @idStudy AND semester = 1";
                string nowy = command.ExecuteScalar().ToString();

                command.CommandText = "SELECT * FROM Enrollment WHERE IdStudy = @idStudy AND semester = 1";
                var res = command.ExecuteScalar();

                if (res == null)
                {
                    string SqlFormattedDate = (DateTime.Now).ToString("yyyy-MM-dd");
                    command.Parameters.AddWithValue("@date", SqlFormattedDate);
                    command.CommandText = "INSERT INTO Enrollment VALUES (15, 1, @idStudy, @date)";
                    nowy = "15";
                    command.ExecuteNonQuery();
                }

                command.Parameters.AddWithValue("@newEnrol", nowy);
                command.CommandText = "SELECT IndexNumber FROM Student WHERE IndexNumber = @indexNumber";
                var res2 = command.ExecuteScalar();
                if (res2 == null)
                {
                    transaction.Rollback();
                }

                command.CommandText = "INSERT INTO Student VALUES (@IndexNumber, @FirstName, @LastName, @BirthDate, @NewEnrol);";
                command.ExecuteNonQuery();
                transaction.Commit();

                Enrollment e = new Enrollment
                {
                    Semester = 1,
                    Studies = Student.Studies
                };

                return e;

            }
        }

        public Enrollment PromoteStudents([FromBody] StudSem sem)
        {
            using (SqlConnection connection = new SqlConnection("Data Source=db-mssql;Initial Catalog=s18593;Integrated Security=true"))
            using (SqlCommand command = new SqlCommand())
            {
                connection.Open();

                command.Parameters.AddWithValue("@studies", sem.Studies);
                command.Parameters.AddWithValue("@semester", sem.Semester);

                command.CommandType = CommandType.StoredProcedure;
                command.ExecuteNonQuery();

                Enrollment e = new Enrollment
                {
                    Semester = sem.Semester + 1,
                    Studies = sem.Studies
                };

                return e;
            }
        }

        public bool CheckIndex(String IndexNumber)
        {
            bool res = false;
            using (var connection = new SqlConnection("Data Source=db-mssql; Initial Catalog=s18593;Integrated Security=True"))
            using (var command = new SqlCommand())
            {
                command.Connection = connection;
                connection.Open();

                command.CommandText = "select 1 from student where IndexNumber = @IndexNumber";
                command.Parameters.AddWithValue("IndexNumber", IndexNumber);

                var dr = command.ExecuteReader();
                if (dr.Read())
                {
                    res = true;
                }
                else
                    res = false;
            }
            return res;
        }
    }
}
