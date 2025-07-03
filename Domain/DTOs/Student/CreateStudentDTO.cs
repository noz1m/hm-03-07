namespace Domain.DTOs.Student;

public class CreateStudentDTO
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public int Age { get; set; }
}
