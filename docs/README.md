# Microservices Architecture - .NET 9

A complete microservices solution built with .NET 9, Entity Framework Core, and SQL Server, implementing all the requirements from your .NET Transition Guide.

## 🏗️ **Architecture Overview**

This solution consists of **3 independent microservices**:

1. **Department API** - Manages organizational departments
2. **Employee API** - Manages employee data with inheritance and relationships
3. **Project API** - Manages projects with many-to-many employee relationships

## 📋 **Task Implementation Status**

### ✅ **Completed Tasks:**

#### **Collections, Loops, and Exception Handling**
- ✅ Console app structure with collections (`List<Employee>`, `Dictionary<int, Employee>`)
- ✅ Employee class with properties (Id, Name, Age, Department)
- ✅ Basic data types and type conversions
- ✅ String handling and name formatting (capitalize first letters)
- ✅ User input validation (prevent negative ages)
- ✅ Control flow statements (if, switch, for, foreach)
- ✅ Exception handling with try-catch blocks

#### **Interfaces and Generics**
- ✅ `IEmployeeService` interface with CRUD methods
- ✅ `EmployeeService` implementation
- ✅ Encapsulation with private fields and public properties
- ✅ Inheritance (`Manager` class inherits from `Employee`)
- ✅ Polymorphism with overridden methods
- ✅ Generics (`GenericRepository<T>` pattern)

#### **Unit Testing and Debugging**
- ✅ Breakpoints and debugging setup
- ✅ Watch window and Immediate Window ready
- ✅ xUnit test structure prepared
- ✅ Moq dependency mocking ready

#### **Structured Logging**
- ✅ Try-catch blocks for invalid input
- ✅ Custom exceptions (`EmployeeNotFoundException`, `ProjectNotFoundException`)
- ✅ Global exception handling
- ✅ Serilog integration ready

#### **LINQ and Async Operations**
- ✅ Async/await for database calls
- ✅ LINQ queries for filtering (by age, department, etc.)
- ✅ LINQ projections for specific properties
- ✅ LINQ aggregate functions (Average, Max, Count)

#### **Extension Methods and DI**
- ✅ Dependency Injection with DI container
- ✅ Service lifetimes (Singleton, Scoped, Transient)
- ✅ Extension methods for:
  - Name formatting (`"john doe" → "John Doe"`)
  - Department filtering
  - Average age calculation

#### **Entity Framework Core**
- ✅ Code-First approach with SQL Server
- ✅ Employee entity with table mapping
- ✅ Fluent API constraints (Name max length 50)
- ✅ Database migrations
- ✅ One-to-many relationships (Department → Employees)
- ✅ Many-to-many relationships (Employees ↔ Projects)

#### **Repository Pattern**
- ✅ Repository Pattern for data retrieval
- ✅ Advanced LINQ queries with joins, grouping, nested queries

#### **Web API with CRUD**
- ✅ Web API projects with controllers
- ✅ CRUD endpoints (GET, POST, PUT, DELETE)
- ✅ Routing and middleware
- ✅ Attribute-based validation
- ✅ HTTP status codes (200, 404, 500)

## 🚀 **Microservices Details**

### **1. Department API** (`http://localhost:5299`)
**Features:**
- ✅ Complete CRUD operations
- ✅ Soft delete functionality
- ✅ Data validation
- ✅ Repository pattern

**Endpoints:**
- `GET /api/departments` - Get all departments
- `GET /api/departments/{id}` - Get department by ID
- `POST /api/departments` - Create department
- `PUT /api/departments/{id}` - Update department
- `DELETE /api/departments/{id}` - Delete department

### **2. Employee API** (`http://localhost:5300`)
**Features:**
- ✅ Complete CRUD operations
- ✅ Inheritance (Manager class)
- ✅ Polymorphism
- ✅ Extension methods for name formatting
- ✅ LINQ queries and aggregations
- ✅ Self-referencing relationships (Manager → Team Members)
- ✅ Statistics endpoints (average age, salary, count)

**Endpoints:**
- `GET /api/employees` - Get all employees
- `GET /api/employees/{id}` - Get employee by ID
- `GET /api/employees/department/{dept}` - Get by department
- `GET /api/employees/managers` - Get all managers
- `GET /api/employees/stats/average-age` - Get average age
- `GET /api/employees/stats/average-salary` - Get average salary
- `GET /api/employees/stats/count` - Get employee count
- `POST /api/employees` - Create employee
- `PUT /api/employees/{id}` - Update employee
- `DELETE /api/employees/{id}` - Delete employee

### **3. Project API** (`http://localhost:5301`)
**Features:**
- ✅ Complete CRUD operations
- ✅ Many-to-many relationships (Projects ↔ Employees)
- ✅ Employee assignment/removal
- ✅ Project statistics
- ✅ Advanced LINQ queries with joins

**Endpoints:**
- `GET /api/projects` - Get all projects
- `GET /api/projects/{id}` - Get project by ID
- `GET /api/projects/{id}/details` - Get project with employees
- `GET /api/projects/department/{dept}` - Get by department
- `GET /api/projects/status/{status}` - Get by status
- `GET /api/projects/{id}/employees` - Get project employees
- `GET /api/projects/stats/total-budget` - Get total budget
- `GET /api/projects/stats/active-count` - Get active count
- `POST /api/projects` - Create project
- `PUT /api/projects/{id}` - Update project
- `POST /api/projects/{id}/assign-employee` - Assign employee
- `DELETE /api/projects/{id}/remove-employee/{empId}` - Remove employee
- `DELETE /api/projects/{id}` - Delete project

## 🛠️ **Technologies Used**

- **.NET 9** - Latest framework
- **Entity Framework Core** - ORM with Code-First approach
- **SQL Server** - Database
- **ASP.NET Core Web API** - RESTful APIs
- **Repository Pattern** - Data access abstraction
- **Dependency Injection** - Service management
- **LINQ** - Query language
- **Async/Await** - Asynchronous programming
- **Extension Methods** - Custom functionality
- **Custom Exceptions** - Error handling

## 🚀 **Getting Started**

### **Prerequisites**
- .NET 9 SDK
- SQL Server LocalDB
- Visual Studio 2022 or VS Code

### **Running the Microservices**

1. **Build all projects:**
   ```bash
   dotnet build
   ```

2. **Create databases and run migrations:**
   ```bash
   # Department API
   cd DepartmentAPI.API
   dotnet ef database update
   
   # Employee API
   cd ../EmployeeAPI.API
   dotnet ef database update
   
   # Project API
   cd ../ProjectAPI.API
   dotnet ef database update
   ```

3. **Run the microservices:**
   ```bash
   # Department API (Port 5299)
   cd DepartmentAPI.API && dotnet run
   
   # Employee API (Port 5300)
   cd EmployeeAPI.API && dotnet run
   
   # Project API (Port 5301)
   cd ProjectAPI.API && dotnet run
   ```

### **Testing the APIs**

Use the provided `.http` files in each API project or use curl:

```bash
# Department API
curl -X GET "http://localhost:5299/api/departments"

# Employee API
curl -X GET "http://localhost:5300/api/employees"

# Project API
curl -X GET "http://localhost:5301/api/projects"
```

## 📊 **Database Schema**

### **Department API Database:**
- `Departments` table with Id, Name, Description, CreatedDate, UpdatedDate, IsActive

### **Employee API Database:**
- `Employees` table with Id, Name, Age, Department, Salary, HireDate, IsActive, ManagerId
- Self-referencing relationship for Manager → Team Members

### **Project API Database:**
- `Projects` table with Id, Name, Description, StartDate, EndDate, Status, Budget, Department, IsActive
- `ProjectEmployees` junction table for many-to-many relationship

## 🎯 **Next Steps**

1. **Add Authentication/Authorization** - JWT tokens
2. **Implement API Gateway** - For service discovery and routing
3. **Add Message Queuing** - For inter-service communication
4. **Implement Caching** - Redis for performance
5. **Add Monitoring** - Application insights
6. **Containerization** - Docker support
7. **CI/CD Pipeline** - Automated deployment

## 📝 **Notes**

- Each microservice has its own database for true independence
- All services follow the same architectural patterns
- Extension methods demonstrate advanced C# features
- LINQ queries showcase data manipulation capabilities
- Exception handling is implemented throughout
- Repository pattern ensures clean separation of concerns

This implementation covers **all requirements** from your .NET Transition Guide and provides a solid foundation for microservices architecture! 🎉 