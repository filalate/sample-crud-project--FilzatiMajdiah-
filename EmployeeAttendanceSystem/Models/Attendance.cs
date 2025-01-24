using System;

namespace EmployeeAttendanceSystem.Models
{
    public class Attendance
    {
        public int AttendanceId { get; set; }

        public int EmployeeId { get; set; }

        public DateTime AttendanceDate { get; set; }

        public string? Status { get; set; }

        public string? LeaveCategory { get; set; }
    }
}
