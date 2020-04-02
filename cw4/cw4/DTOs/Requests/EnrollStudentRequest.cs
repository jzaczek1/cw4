﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace cw4.DTOs.Requests
{
    public class EnrollStudentRequest : Attribute
    {
        [Required(ErrorMessage = "Brakuje indeksu")]
        public object IndexNumber { get; set; } 

        [Required(ErrorMessage = "Brakuje imienia")]
        public string FirstName { get; set; }
        public string Name { get; set; }

        [Required(ErrorMessage = "Brakuje nazwiska")]
        [MaxLength(30)]
        public string LastName { get; set; }

        [Required]
        public DateTime startDate { get; set; }

        [Required(ErrorMessage = "Brakuje nazwy studiów")]
        public string Studies { get; set; }
        public string IdStudy { get; set; }

        public int IdEnrollment { get; set; }

        public int Semester { get; set; }
    }

}
