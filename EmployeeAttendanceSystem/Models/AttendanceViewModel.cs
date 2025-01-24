namespace EmployeeAttendanceSystem.Models
{
    public class AttendanceViewModel
    {
        public int AttendanceId { get; set; }
        public string? EmployeeName { get; set; }
        public DateTime AttendanceDate { get; set; }
        public string? Status { get; set; }
        public string? LeaveCategory { get; set; }
    }
}
