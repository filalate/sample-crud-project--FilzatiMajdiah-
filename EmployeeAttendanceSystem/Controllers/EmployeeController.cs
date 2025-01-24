using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Configuration;
using EmployeeAttendanceSystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeAttendanceSystem.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly string connectionString;

        public EmployeeController(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("MySqlConnection");
        }

        // Display Employees
        public ActionResult Index()
        {
            var employees = new List<Employee>();
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM Employees";
                using (var cmd = new MySqlCommand(query, connection))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            employees.Add(new Employee
                            {
                                EmployeeId = int.Parse(reader["EmployeeId"].ToString()),
                                Name = reader["Name"].ToString(),
                                Position = reader["Position"].ToString(),
                                Department = reader["Department"].ToString(),
                                Email = reader["Email"].ToString(),
                                Phone = reader["Phone"].ToString()
                            });
                        }
                    }
                }
            }
            return View(employees);
        }

        // Add Employee
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Employee employee)
        {
            if (ModelState.IsValid)
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO Employees (Name, Position, Department, Email, Phone) VALUES (@Name, @Position, @Department, @Email, @Phone)";
                    using (var cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@Name", employee.Name);
                        cmd.Parameters.AddWithValue("@Position", employee.Position);
                        cmd.Parameters.AddWithValue("@Department", employee.Department);
                        cmd.Parameters.AddWithValue("@Email", employee.Email);
                        cmd.Parameters.AddWithValue("@Phone", employee.Phone);
                        cmd.ExecuteNonQuery();
                    }
                }
                return RedirectToAction("Index");
            }
            return View(employee);
        }

        // Edit Employee
        [HttpGet]
        public ActionResult Edit(int id)
        {
            Employee employee = null;
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM Employees WHERE EmployeeId = @EmployeeId";
                using (var cmd = new MySqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@EmployeeId", id);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            employee = new Employee
                            {
                                EmployeeId = int.Parse(reader["EmployeeId"].ToString()),
                                Name = reader["Name"].ToString(),
                                Position = reader["Position"].ToString(),
                                Department = reader["Department"].ToString(),
                                Email = reader["Email"].ToString(),
                                Phone = reader["Phone"].ToString()
                            };
                        }
                    }
                }
            }
            return View(employee);
        }

        [HttpPost]
        public ActionResult Edit(Employee employee)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "UPDATE Employees SET Name = @Name, Position = @Position, Department = @Department, Email = @Email, Phone = @Phone WHERE EmployeeId = @EmployeeId";
                using (var cmd = new MySqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@Name", employee.Name);
                    cmd.Parameters.AddWithValue("@Position", employee.Position);
                    cmd.Parameters.AddWithValue("@Department", employee.Department);
                    cmd.Parameters.AddWithValue("@Email", employee.Email);
                    cmd.Parameters.AddWithValue("@Phone", employee.Phone);
                    cmd.Parameters.AddWithValue("@EmployeeId", employee.EmployeeId);
                    cmd.ExecuteNonQuery();
                }
            }
            return RedirectToAction("Index");
        }

        // Delete Employee
        [HttpGet]
        public ActionResult Delete(int id)
        {
            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // Delete associated attendance records first
                    string deleteAttendanceQuery = "DELETE FROM Attendance WHERE EmployeeId = @EmployeeId";
                    using (var cmd = new MySqlCommand(deleteAttendanceQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@EmployeeId", id);
                        cmd.ExecuteNonQuery();
                    }

                    // Delete the employee record
                    string deleteEmployeeQuery = "DELETE FROM Employees WHERE EmployeeId = @EmployeeId";
                    using (var cmd = new MySqlCommand(deleteEmployeeQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@EmployeeId", id);
                        cmd.ExecuteNonQuery();
                    }
                }

                return RedirectToAction("Index");
            }
            catch (MySqlException ex)
            {
                // Handle any MySQL errors
                TempData["Error"] = "An error occurred while deleting the employee: " + ex.Message;
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Handle any other exceptions
                TempData["Error"] = "An unexpected error occurred: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

    }
}
