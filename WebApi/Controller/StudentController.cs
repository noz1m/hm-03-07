using Domain.ApiResponse;
using Domain.DTOs.Student;
using Domain.Filter;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentController(IStudentService studentService) : ControllerBase
{
    [HttpGet]
    [Authorize]
    public async Task<Response<List<GetStudentDTO>>> GetAllAsync([FromQuery] StudentFilter filter)
    {
        return await studentService.GetAllAsync(filter);
    }

    [HttpGet("{id:int}")]
    public async Task<Response<GetStudentDTO>> GetByIdAsync(int id)
    {
        return await studentService.GetByIdAsync(id);
    }

    [HttpPost]
    public async Task<Response<string>> CreateAsync([FromBody] CreateStudentDTO student)
    {
        return await studentService.CreateAsync(student);
    }

    [HttpPut("{id:int}")]
    public async Task<Response<string>> UpdateAsync(int id, [FromBody] UpdateStudentDTO student)
    {
        return await studentService.UpdateAsync(id, student);
    }

    [HttpDelete("{id:int}")]
    public async Task<Response<string>> DeleteAsync(int id)
    {
        return await studentService.DeleteAsync(id);
    }
}