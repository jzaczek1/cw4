using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cw4.DTOs.Responses
{
    public class EnrollStudentResponse
    {
        public string IndexNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Semester { get; set; }
        public DateTime startDate { get; set; }
        public string Studies { get; set; }
    }

}
