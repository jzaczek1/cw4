using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace cw4.Models
{
    public class StudSem
    {
        [Required]
        public string Studies { get; set; }
        [Required]
        public int Semester { get; set; }

    }
}
