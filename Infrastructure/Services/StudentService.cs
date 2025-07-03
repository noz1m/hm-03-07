using Domain.ApiResponse;
using System.Net;
using Domain.Entities;
using Domain.DTOs.Student;
using Domain.Filter;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class StudentService(DataContext context) : IStudentService
{
    public async Task<Response<List<GetStudentDTO>>> GetAllAsync(StudentFilter filter)
    {
        var validFilter = new ValidFilter(filter.PageNumber, filter.PageSize);
        var query = context.Students.AsQueryable();

        if (!string.IsNullOrEmpty(filter.FirstName))
            query = query.Where(x => x.FirstName.Contains(filter.FirstName));

        if (!string.IsNullOrEmpty(filter.LastName))
            query = query.Where(x => x.LastName.Contains(filter.LastName));

        var totalRecords = query.CountAsync();
        var data = await query.Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
            .Take(validFilter.PageSize)
            .Select(x => new GetStudentDTO
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName
            }).ToListAsync();

        return new PagedResponse<List<GetStudentDTO>>(data, await totalRecords, validFilter.PageNumber, validFilter.PageSize);
    }

    public async Task<Response<GetStudentDTO>> GetByIdAsync(int id)
    {
        var student = await context.Students.FirstOrDefaultAsync(x => x.Id == id);

        if (student == null)
            return new Response<GetStudentDTO>("Not found", HttpStatusCode.NotFound);

        var result = new GetStudentDTO
        {
            Id = student.Id,
            FirstName = student.FirstName,
            LastName = student.LastName
        };

        return new Response<GetStudentDTO>(result);        
    }
    public async Task<Response<string>> CreateAsync(CreateStudentDTO student)
    {
        if (string.IsNullOrWhiteSpace(student.FirstName))
            return new Response<string>("First name is required", HttpStatusCode.BadRequest);

        if (string.IsNullOrWhiteSpace(student.LastName))
            return new Response<string>("Last name is required", HttpStatusCode.BadRequest);

        var newStudent = new Student
        {
            FirstName = student.FirstName,
            LastName = student.LastName
        };

        await context.Students.AddAsync(newStudent);
        var result = await context.SaveChangesAsync();
        return result > 0
            ? new Response<string>("Successfully created")
            : new Response<string>("Error while creating", HttpStatusCode.InternalServerError);
    }
    public async Task<Response<string>> UpdateAsync(int id, UpdateStudentDTO student)
    {
        var exist = await context.Students.FindAsync(id);

        if (exist == null)
            return new Response<string>("Not found", HttpStatusCode.NotFound);
        
        exist.FirstName = student.FirstName;
        exist.LastName = student.LastName;
        var result = await context.SaveChangesAsync();
        return result > 0
            ? new Response<string>("Successfully updated")
            : new Response<string>("Error while updating", HttpStatusCode.InternalServerError);
    }
    public async Task<Response<string>> DeleteAsync(int id)
    {
        var student = await context.Students.FindAsync(id);

        if (student == null)
            return new Response<string>("Not found", HttpStatusCode.NotFound);

        context.Students.Remove(student);
        var result = await context.SaveChangesAsync();
        
        return result > 0
            ? new Response<string>("Successfully deleted")
            : new Response<string>("Error while deleting", HttpStatusCode.InternalServerError);
    }
}
