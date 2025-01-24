# Employee Management System

This repository contains a simple web-based **Employee Management System** built with **ASP.NET Core MVC** and **MySQL**. The system demonstrates basic **CRUD (Create, Read, Update, Delete)** operations and showcases skills in backend development, frontend design and database integration.

---

## Technologies Used

- **Framework:** ASP.NET Core MVC (.NET 6.0 or higher)
- **Frontend:** HTML, CSS, Bootstrap 5
- **Backend Language:** C#
- **Database:** MySQL (phpMyAdmin for database management)
- **SDK:** .NET 6.0 SDK
- **IDE:** Visual Studio 2022 
- **Package Manager:** NuGet

---

## Prerequisites

Before running the application, ensure the following tools are installed:

- **MySQL** (with phpMyAdmin recommended)
- **.NET 6.0 SDK**
- **Visual Studio 2022**
- **Git** (for cloning the repository)

---

## Steps to Set Up and Run the Application

### 1. Clone the Repository

Open your terminal and run the following commands:

```bash
git clone <your-repository-link>
cd sample-crud-project--FilzatiMajdiah
```

### 2. Open the Project

- Open the cloned project folder in **Visual Studio 2022**.

### 3. Configure the Database

- Open **phpMyAdmin** or your preferred database management tool.
- Create a new database named `employeeattendance`.
- Import the provided SQL dump file (`employeeattendance.sql`) into the database:
  1. Go to the **Import** tab in phpMyAdmin.
  2. Select the file `employeeattendance.sql` located in the `/db` folder.
  3. Click **Go** to complete the import.

### 4. Update `appsettings.json`

- Open the `appsettings.json` file in the root of the project.
- Update the connection string with your MySQL credentials:

```json
"ConnectionStrings": {
  "MySqlConnection": "Server=localhost;Database=employeeattendance;User Id=root;Password=yourpassword;"
}
```

Replace `root` and `yourpassword` with your MySQL username and password.

### 5. Run the Application

- In **Visual Studio 2022**, press `F5` or `Ctrl + F5` to start the project.

### 6. Access the Application

- Open your browser and navigate to: `http://localhost:<port>`

## Application Features

### **User Authentication**
- **Register:**
  - New users can create an account by providing their username, email, and password.
  - Passwords are securely hashed using SHA-256 before being stored in the database.
  - After successful registration, the user is redirected to the login page with a success message.
- **Login:**
  - Registered users can log in to access the system.
  - The system validates the entered username and password against the database.
  - Session management is used to track the logged-in user.
  - If login credentials are incorrect, an error message is displayed.

### **Employee Management**
- **Add New Employees:**
  - Admins can add employees by providing details such as name, position, department, email and phone number.
  - The added employee is stored in the `Employees` table of the database.
- **Edit Employee Details:**
  - Admins can edit the details of existing employees.
- **View Employees:**
  - All employees are listed in a tabular format.
  - The table displays employee ID, name, position, department, email and phone number.
- **Delete Employees:**
  - Admins can delete employees from the system.
  - The system checks for any associated attendance records before allowing deletion to maintain data integrity.

### **Attendance Management**
- **Record Employee Attendance:**
  - Admins can add attendance records by selecting an employee, date, status (present/absent), and leave category (e.g., annual or emergency leave).
  - Attendance data is stored in the `Attendance` table of the database.
- **Edit Attendance Records:**
  - Admins can update attendance records if corrections are needed.
  - Changes are reflected immediately in the database.
- **Delete Attendance Records:**
  - Attendance records can be deleted if they are no longer needed.
  - The system ensures that records are removed cleanly without affecting other data.
- **View Attendance History:**
  - Attendance records are displayed in a table with options to filter or search by employee.

### Responsive UI
- Built using **Bootstrap 5** for a modern and mobile-friendly design.

---

This README contains all necessary instructions for setting up, running and evaluating the system. Let me know if further details are needed!
