namespace Domain.Filter;

public class StudentFilter : ValidFilter
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
}
