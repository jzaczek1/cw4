using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace cw4.Models
{
    public class Student
    {
        [Required(ErrorMessage = "Musisz podac index")]
        public string IndexNumber { get; set; }
        [Required(ErrorMessage = "Muisz podac imie")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Musisz podac nazwisko")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Musisz podac date urodzin")]
        public DateTime BirthDate { get; set; }
        [Required(ErrorMessage = "Musisz podac studia")]
        public string Studies { get; set; }


        public Student() { }

    }
}
