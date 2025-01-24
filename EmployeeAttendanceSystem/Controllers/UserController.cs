using MySql.Data.MySqlClient;
using EmployeeAttendanceSystem.Models;
using Microsoft.AspNetCore.Http; // Add this for session handling
using System.Configuration;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeAttendanceSystem.Controllers
{
    public class UserController : Controller
    {
        private readonly string connectionString;

        public UserController(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("MySqlConnection");
        }
            // GET: Register
            [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        // POST: Register
        [HttpPost]
        public ActionResult Register(User user)
        {
            if (ModelState.IsValid)
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();

                        // Check for duplicate username
                        string checkQuery = "SELECT COUNT(*) FROM Users WHERE Username = @Username";
                        using (var checkCmd = new MySqlCommand(checkQuery, connection))
                        {
                            checkCmd.Parameters.AddWithValue("@Username", user.Username);
                            int count = Convert.ToInt32(checkCmd.ExecuteScalar());
                            if (count > 0)
                            {
                                TempData["Message"] = "Username already exists. Please choose a different one.";
                                TempData["MessageType"] = "error";
                                return RedirectToAction("Register");
                            }
                        }

                        // Insert user
                        string query = "INSERT INTO Users (Username, Password, Email, Role) VALUES (@Username, @Password, @Email, @Role)";
                        using (var cmd = new MySqlCommand(query, connection))
                        {
                            cmd.Parameters.AddWithValue("@Username", user.Username ?? string.Empty);
                            cmd.Parameters.AddWithValue("@Password", HashPassword(user.Password ?? string.Empty));
                            cmd.Parameters.AddWithValue("@Email", user.Email ?? string.Empty);
                            cmd.Parameters.AddWithValue("@Role", "User");
                            cmd.ExecuteNonQuery();
                        }

                        TempData["Message"] = "Successfully registered! You can now log in.";
                        TempData["MessageType"] = "success";
                        return RedirectToAction("Register");
                    }
                    catch (Exception ex)
                    {
                        TempData["Message"] = "An error occurred. Please try again.";
                        TempData["MessageType"] = "error";
                        return RedirectToAction("Register");
                    }
                }
            }

            TempData["Message"] = "Please fill out all required fields.";
            TempData["MessageType"] = "error";
            return RedirectToAction("Register");
        }


        [HttpGet]
        public ActionResult Welcome()
        {
            return View();
        }

        // GET: Login
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        // POST: Login
        [HttpPost]
        public ActionResult Login(User user)
        {
            if (user == null || string.IsNullOrEmpty(user.Username) || string.IsNullOrEmpty(user.Password))
            {
                ViewBag.Message = "Username and Password are required.";
                return View();
            }

            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM Users WHERE Username = @Username";
                using (var cmd = new MySqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@Username", user.Username);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read() && VerifyPassword(user.Password, reader["Password"]?.ToString()))
                        {
                            HttpContext.Session.SetString("UserId", reader["UserId"]?.ToString() ?? string.Empty);
                            HttpContext.Session.SetString("Username", reader["Username"]?.ToString() ?? string.Empty);
                            HttpContext.Session.SetString("Role", reader["Role"]?.ToString() ?? "User");
                            return RedirectToAction("Index", "Employee");
                        }
                    }
                }
            }
            ViewBag.Message = "Invalid Login Credentials.";
            return View();
        }

        // Logout
        public ActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        // Hash Password (for secure storage)
        private string HashPassword(string password)
        {
            if (string.IsNullOrEmpty(password)) throw new ArgumentNullException(nameof(password));

            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return System.Convert.ToBase64String(hashedBytes);
            }
        }

        // Verify Password (compare input with stored hash)
        private bool VerifyPassword(string inputPassword, string storedHashedPassword)
        {
            if (string.IsNullOrEmpty(inputPassword) || string.IsNullOrEmpty(storedHashedPassword))
                return false;

            return HashPassword(inputPassword) == storedHashedPassword;
        }
    }
}
