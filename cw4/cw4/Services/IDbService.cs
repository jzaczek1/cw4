using cw4.Models;
using System.Collections.Generic;

namespace cw4.Controllers
{
    public interface IDbService
    {
        public IEnumerable<Student> GetStudents();
    }
}