using cw4.DTOs.Requests;
using cw4.Encoding;
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

        public Enrollment EnrollStudent(EnrollStudentRequest Student)
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

        public Enrollment PromoteStudents(EnrollStudentRequest sem)
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
        public List<string> Login(Login request)
        {
            var list = new List<String>();
            using (var connection = new SqlConnection("Data Source=db-mssql; Initial Catalog=s18593;Integrated Security=True"))
            using (var command = new SqlCommand())
            {
                string salt = "";
                string password = "";

                string IndexNubmer = "";
                string FirstName = "";
                string Role = "";
                command.Connection = connection;
                command.CommandText = "Select Salt, Password from Student where Student.IndexNumber=@ID";
                command.Parameters.AddWithValue("ID", request.ID);

                connection.Open();
                var dr = command.ExecuteReader();
                if (!dr.Read())
                {
                    dr.Close();
                    return null;
                }
                salt = dr["Salt"].ToString();
                password = dr["Password"].ToString();

                dr.Close();
                if (!Encode.Validate(request.Haslo, salt, password))
                {
                    return null;
                }
                command.CommandText = "Select Student.IndexNumber AS IndexN, FirstName , Role.Role AS Role from Student, Role where Student.IndexNumber=@ID AND Student.Password=@Password AND Student.Role_ID = Role.IndexNumber";
                command.Parameters.AddWithValue("Password", password);

                dr = command.ExecuteReader();
                while (dr.Read())
                {
                    IndexNubmer = dr["IndexN"].ToString();
                    FirstName = dr["FirstName"].ToString();
                    Role = dr["Role"].ToString();
                }
                dr.Close();
                
                list.Add(IndexNubmer);
                list.Add(FirstName);
                list.Add(Role);
            }
            return list;
        }


        public List<String> TokenExists(string requestToken)
        {

            string IndexNubmer = "";
            string FirstName = "";
            string Role = "";
            var list = new List<String>();

            using (var connection = new SqlConnection("Data Source=db-mssql; Initial Catalog=s18593;Integrated Security=True"))
            using (var command = new SqlCommand())
            {
                command.Connection = connection;

                command.CommandText = "Select Student.IndexNumber AS IndexN, FirstName , Role.Role AS Role from Student, Role where Student.RefreshToken=@Token AND Student.Role_ID = Role.IndexNumber";
                command.Parameters.AddWithValue("Token", requestToken);

                connection.Open();

                var dr = command.ExecuteReader();
                while (dr.Read())
                {
                    IndexNubmer = dr["IndexN"].ToString();
                    FirstName = dr["FirstName"].ToString();
                    Role = dr["Role"].ToString();
                }
                dr.Close();
            }

            list.Add(IndexNubmer);
            list.Add(FirstName);
            list.Add(Role);
            return (list);
        }

        public void RefreshToken(string requestToken, string IndexNumber)
        {
            using (var connection = new SqlConnection("Data Source=db-mssql; Initial Catalog=s18593;Integrated Security=True"))
            using (var command = new SqlCommand())
            {
                command.Connection = connection;

                command.CommandText = "UPDATE Student SET RefreshToken =@Token WHERE IndexNumber=@IndexNumber";
                command.Parameters.AddWithValue("IndexNumber", IndexNumber);
                command.Parameters.AddWithValue("Token", requestToken);
                connection.Open();

                command.ExecuteNonQuery();
            }
        }

        public bool encodePasswords(string connString)
        {
            List<String> list = new List<String>();
            using (var con = new SqlConnection(connString))
            using (var com = new SqlCommand())

            {
                com.Connection = con;

                com.CommandText = "SELECT Password from Student";

                con.Open();

                var dr = com.ExecuteReader();
                if (!dr.Read())
                {
                    dr.Close();
                    return false;
                }
                else
                {
                    while (dr.Read())
                    {
                        var password = dr["Password"].ToString();
                        list.Add(password);
                    }
                    dr.Close();

                    com.Parameters.AddWithValue("Original", "");
                    com.Parameters.AddWithValue("Salt", "");
                    com.Parameters.AddWithValue("Encoded", "");
                    for (int i = 0; i < list.Count; i++)
                    {
                        com.CommandText = "UPDATE Student SET Password=@Encoded, Salt=@Salt WHERE Password=@Original";

                        string salt = Encode.CreateSalt();
                        com.Parameters["Original"].Value = list[i];
                        com.Parameters["Salt"].Value = salt;
                        com.Parameters["Encoded"].Value = Encode.Create(list[i], salt);

                        com.ExecuteNonQuery();
                    }
                    return true;
                }
            }
            return false;
        }
    }
}
