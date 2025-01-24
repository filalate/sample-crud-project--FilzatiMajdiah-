using System.ComponentModel.DataAnnotations;

namespace EmployeeAttendanceSystem.Models
{
    public class Employee
    {
        public int EmployeeId { get; set; }

        public string? Name { get; set; }

        public string? Position { get; set; }

        public string? Department { get; set; }

        public string? Email { get; set; }

        public string? Phone { get; set; }
    }
}
