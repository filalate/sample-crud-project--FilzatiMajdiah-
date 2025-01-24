using MySql.Data.MySqlClient;
using EmployeeAttendanceSystem.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeAttendanceSystem.Controllers
{
    public class AttendanceController : Controller
    {
        private readonly string connectionString;

        public AttendanceController(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("MySqlConnection");
        }

        // Display Attendance Records
        public ActionResult Index()
        {
            var attendanceList = new List<AttendanceViewModel>();

            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = @"
                    SELECT 
                        a.AttendanceId, 
                        e.Name AS EmployeeName, 
                        a.AttendanceDate, 
                        a.Status, 
                        a.LeaveCategory
                    FROM Attendance a
                    JOIN Employees e ON a.EmployeeId = e.EmployeeId";

                using (var cmd = new MySqlCommand(query, connection))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            attendanceList.Add(new AttendanceViewModel
                            {
                                AttendanceId = int.Parse(reader["AttendanceId"].ToString()),
                                EmployeeName = reader["EmployeeName"].ToString(),
                                AttendanceDate = DateTime.TryParse(reader["AttendanceDate"]?.ToString(), out var date) ? date : DateTime.MinValue,
                                Status = reader["Status"].ToString(),
                                LeaveCategory = reader["LeaveCategory"].ToString()
                            });
                        }
                    }
                }
            }

            return View(attendanceList);
        }

        // Add Attendance
        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.Employees = GetEmployeeDropdown();
            return View();
        }

        [HttpPost]
        public ActionResult Create(Attendance attendance)
        {
            if (ModelState.IsValid)
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = @"
                        INSERT INTO Attendance 
                        (EmployeeId, AttendanceDate, Status, LeaveCategory) 
                        VALUES (@EmployeeId, @AttendanceDate, @Status, @LeaveCategory)";
                    using (var cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@EmployeeId", attendance.EmployeeId);
                        cmd.Parameters.AddWithValue("@AttendanceDate", attendance.AttendanceDate);
                        cmd.Parameters.AddWithValue("@Status", attendance.Status);
                        cmd.Parameters.AddWithValue("@LeaveCategory", attendance.LeaveCategory);
                        cmd.ExecuteNonQuery();
                    }
                }
                return RedirectToAction("Index");
            }
            ViewBag.Employees = GetEmployeeDropdown();
            return View(attendance);
        }

        // Edit Attendance
        [HttpGet]
        public ActionResult Edit(int id)
        {
            Attendance attendance = null;
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM Attendance WHERE AttendanceId = @AttendanceId";
                using (var cmd = new MySqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@AttendanceId", id);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            attendance = new Attendance
                            {
                                AttendanceId = int.Parse(reader["AttendanceId"].ToString()),
                                EmployeeId = int.Parse(reader["EmployeeId"].ToString()),
                                AttendanceDate = DateTime.Parse(reader["AttendanceDate"].ToString()),
                                Status = reader["Status"].ToString(),
                                LeaveCategory = reader["LeaveCategory"].ToString()
                            };
                        }
                    }
                }
            }
            ViewBag.Employees = GetEmployeeDropdown();
            return View(attendance);
        }

        [HttpPost]
        public ActionResult Edit(Attendance attendance)
        {
            if (ModelState.IsValid)
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = @"
                        UPDATE Attendance 
                        SET EmployeeId = @EmployeeId, 
                            AttendanceDate = @AttendanceDate, 
                            Status = @Status, 
                            LeaveCategory = @LeaveCategory 
                        WHERE AttendanceId = @AttendanceId";
                    using (var cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@EmployeeId", attendance.EmployeeId);
                        cmd.Parameters.AddWithValue("@AttendanceDate", attendance.AttendanceDate);
                        cmd.Parameters.AddWithValue("@Status", attendance.Status);
                        cmd.Parameters.AddWithValue("@LeaveCategory", attendance.LeaveCategory);
                        cmd.Parameters.AddWithValue("@AttendanceId", attendance.AttendanceId);
                        cmd.ExecuteNonQuery();
                    }
                }
                return RedirectToAction("Index");
            }
            ViewBag.Employees = GetEmployeeDropdown();
            return View(attendance);
        }

        // Delete Attendance
        [HttpGet]
        public ActionResult Delete(int id)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "DELETE FROM Attendance WHERE AttendanceId = @AttendanceId";
                using (var cmd = new MySqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@AttendanceId", id);
                    cmd.ExecuteNonQuery();
                }
            }
            return RedirectToAction("Index");
        }

        // Helper Method: Get Employee Dropdown
        private List<SelectListItem> GetEmployeeDropdown()
        {
            var employees = new List<SelectListItem>();
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT EmployeeId, Name FROM Employees";
                using (var cmd = new MySqlCommand(query, connection))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            employees.Add(new SelectListItem
                            {
                                Value = reader["EmployeeId"].ToString(),
                                Text = reader["Name"].ToString()
                            });
                        }
                    }
                }
            }
            return employees;
        }
    }
}
