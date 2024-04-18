using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestAPI.Data;
using RestAPI.Models;
using RestAPI.Responses;

namespace RestAPI.Services.StudentService
{
    public class StudentService : IStudentService
    {
        private readonly AppDbContext _dbContext;

        public StudentService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<Student> GetStudents()
        {
            return _dbContext.Students.ToList();
        }

 
        public async Task<int> stdCount()
        {
            return await _dbContext.Students.CountAsync();

        }

        public async Task DeleteStudent(int id)
        {
            var std = _dbContext.Students.Find(id);
            if (std != null)
            {
                _dbContext.Students.Remove(std);
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                throw new InvalidOperationException($"Student with ID {id} not found.");
            }
        }

        public async  Task UpdateStudent(int id, Student std)
        {
            var student = await _dbContext.Students.FindAsync(id);

            if (student == null)
            {

                throw new InvalidOperationException($"Student with ID {id} not found.");
            }
            student.Name = std.Name;
            student.Email = std.Email;
            student.Phone = std.Phone;

            await _dbContext.SaveChangesAsync();

        }

        public async Task<Student> GetStudentById(int id)
        {
            var data = await _dbContext.Students.FindAsync(id);
            if (data != null)
            {
                return data;
            }
      
           throw new InvalidOperationException($"Student with ID {id} not found.");
        }

        public async Task  AddStudent(Student std)
        {
            var stdExist = await _dbContext.Students.FirstOrDefaultAsync(s => s.Email == std.Email);
            if (stdExist == null)
            {
                await _dbContext.Students.AddAsync(std);
                await _dbContext.SaveChangesAsync();
            }

            else
            {
                throw new InvalidOperationException($" {std.Email}  is already in use, please try with another email");

            }
        }


   
    }
}
