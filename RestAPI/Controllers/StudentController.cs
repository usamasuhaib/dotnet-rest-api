using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestAPI.Data;
using RestAPI.Models;
using RestAPI.Responses;
using RestAPI.Services.StudentService;

namespace RestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        private readonly IStudentService _studentService;

        public StudentController(AppDbContext dbContext, IStudentService studentService)
        {
            _dbContext = dbContext;
            _studentService = studentService;
        }


        [HttpGet]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult GetStudents()
        {
            var students = _studentService.GetStudents();

            if (students == null)
            {
                return Ok(new StudentResponse
                {
                    Result = "No Records Found"
                });

            }
            return Ok(students);

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> getStdById(int id)
        {
            try
            {
                var data = await _studentService.GetStudentById(id);
              
                   return Ok(data);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new StudentResponse
                {
                    Result = ex.Message
                });
            }

            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }




        [HttpGet("Count")]
        public async Task<IActionResult> stdCount()
        {
            var count = await _studentService.stdCount();

            if (count > 0)
            {
                return Ok(count);
            }

            return Ok(new StudentResponse { Result = "No Student Records found" });

        }



        [HttpPost("Create")]
        public async Task<IActionResult> addStudent([FromBody] Student std)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    await _studentService.AddStudent(std);
                    return Ok(new StudentResponse
                    {
                        Result="Student Record added Successfully"
                    });
                }
                catch (InvalidOperationException ex)
                {
                    return BadRequest(new StudentResponse
                    {
                        Result = ex.Message
                    });
                }
            }
            else
            {
                return BadRequest("invalid payload");
            }
           
        }




        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStudent(int id, Student std)
        {
            try
            {
                await _studentService.UpdateStudent(id, std);

                return Ok(new StudentResponse
                {
                    Result = "Record Updated Sussessfully"
                });

            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new StudentResponse
                {
                    Result = ex.Message
                });
            }

        }




        //delete student 

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            try
            {
               await _studentService.DeleteStudent(id);
                return Ok(new StudentResponse
                {
                    Result = "Student Deleted Successfully",
                });
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new StudentResponse
                {
                    Result = ex.Message
                });
            }
        }


    }
}
