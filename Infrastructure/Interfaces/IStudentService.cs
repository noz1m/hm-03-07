using Domain.ApiResponse;
using Domain.DTOs.Student;
using Domain.Entities;
using Domain.Filter;

namespace Infrastructure.Interfaces;

public interface IStudentService
{
    Task<Response<List<GetStudentDTO>>> GetAllAsync(StudentFilter filter);
    Task<Response<GetStudentDTO>> GetByIdAsync(int id);
    Task<Response<string>> CreateAsync(CreateStudentDTO student);
    Task<Response<string>> UpdateAsync(int id,UpdateStudentDTO student);
    Task<Response<string>> DeleteAsync(int id);
}
