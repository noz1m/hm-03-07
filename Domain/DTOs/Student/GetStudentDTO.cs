namespace Domain.DTOs.Student;

public class GetStudentDTO
{
    public int Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public int Age { get; set; }
}
