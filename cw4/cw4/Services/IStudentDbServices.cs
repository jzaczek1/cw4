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
        public Enrollment EnrollStudent(EnrollStudentRequest request);
        public Enrollment PromoteStudents(EnrollStudentRequest request);
        public bool CheckIndex(String IndexNumber);
        List<String> Login(Login request);
        List<String> TokenExists(string requestToken);
        void RefreshToken(string requestToken, string IndexNumber);
        bool encodePasswords(string connString);

    }

}
