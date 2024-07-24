namespace AdminHRM.Server.Entities;

public class Employee : BaseEntities
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string JobTitle { get; set; }
    public string Status { get; set; }
    public Guid? EmployeeId { get; set; }
    public Guid SubUnitId { get; set; }
    public Employee SupperEmployee { get; set; }
    public IList<Employee> Employees { get; set; }
    public SubUnit SubUnits { get; set; }
}