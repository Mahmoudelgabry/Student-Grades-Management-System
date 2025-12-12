# Student Grades Management System

## 📌 Project Overview
This project is a **Student Grades Management System** developed using **F#** and **Avalonia UI** following **functional programming principles**.  
The system allows managing student records, calculating grades, generating statistics, and handling role-based access (Admin / Viewer).

---

## 🎯 Objectives
- Manage student data using functional design
- Apply pure functions for calculations and statistics
- Separate business logic from UI
- Practice teamwork using Git & GitHub
- Implement unit testing for core logic

---

## 🛠️ Technologies Used
- **F#**
- **.NET**
- **Avalonia UI**
- **xUnit** (Unit Testing)
- **JSON** (Persistence)
- **Git & GitHub** (Version Control)

---

## ✅ System Features
- Add, update, and delete student records
- Calculate student averages and pass/fail status
- Generate class statistics (highest, lowest, pass rate)
- Role-based access (Admin / Viewer)
- Save and load data using JSON files
- Unit tests for core functional logic



## 🧪 Unit Testing
Unit tests are implemented using **xUnit** and are isolated in a separate test project.

### Tested Modules:
- Grade calculations
- Statistics logic
- CRUD operations

### Not Tested:
- UI components
- File-based persistence (JSON)
  
> This follows best practices by testing **pure functional logic only**.

---

## ▶️ How to Run the Project

### 1️⃣ Run the application
```bash
dotnet run

### 2️⃣ Run unit tests 

dotnet test
