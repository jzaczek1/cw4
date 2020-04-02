using cw4.DTOs.Requests;
using cw4.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cw4.Services
{

    public interface IStudentsDbService
    {
        public Enrollment EnrollStudent([FromBody] Student Student);
        public Enrollment PromoteStudents([FromBody] StudSem StudeSem);
    }

}
