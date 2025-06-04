namespace APBD_task_11.DTOs.Employee;

public class EmployeeDetailDto
{
    public string PassportNumber { get; set; }
    public string FirstName { get; set; }
    public string? MiddleName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    
    public decimal Salary { get; set; }
    public DateTime HireDate { get; set; }

    public PositionDto Position { get; set; }

    public class PositionDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }   
}