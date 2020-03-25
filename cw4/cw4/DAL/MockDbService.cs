using cw4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cw4.DAL
{
    public class MockDbService : IDbService
    {

        public IEnumerable<Student> GetStudents()
        {
            return null;
        }
        //private static IEnumerable<Student> _students;

        //static MockDbService()
        //{
        //    _students = new List<Student>
        //    {
        //        new Student{FirstName="Jan",LastName="Kowalski"},
        //        new Student{FirstName="Anna",LastName="Pała"},
        //        new Student{FirstName="Basia",LastName="Komar"}
        //    };
        //}
        //public IEnumerable<Student> GetStudents()
        //{
        //    return _students;
        //}

        //IEnumerable<Student> IDbService.GetStudents()
        //{
        //    throw new NotImplementedException();
        //}
    }
}