using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace cw4.DTOs.Responses
{
    public class EnrollStudentResponse
    {
        [RegularExpression("^(?i)s[0-9]+$")]
        public string IndexNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Semester { get; set; }
        public DateTime startDate { get; set; }
        public string Studies { get; set; }
    }

}
