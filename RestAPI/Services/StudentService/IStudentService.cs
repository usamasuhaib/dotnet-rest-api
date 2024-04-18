using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestAPI.Models;
using RestAPI.Models.DTOs;

namespace RestAPI.Services.StudentService
{
    public interface IStudentService
    {
        Task<int> stdCount();

        IEnumerable<Student> GetStudents();

        public Task AddStudent(Student std);

        public Task  UpdateStudent(int id, Student std);


        public Task DeleteStudent(int id);

        public Task<Student> GetStudentById(int id);



    }
}
