# Microservices Architecture - Clean Architecture Implementation

A comprehensive microservices solution built with .NET 9, following Clean Architecture principles and proper naming conventions.

## 🏗️ **Project Structure**

```
MicroservicesArchitecture/
├── src/
│   └── Services/
│       ├── DepartmentService/
│       │   ├── API/                 # Web API Layer
│       │   ├── Core/                # Domain & Application Layer
│       │   └── Infrastructure/      # Data Access & External Services
│       ├── EmployeeService/
│       │   ├── API/
│       │   ├── Core/
│       │   └── Infrastructure/
│       └── ProjectService/
│           ├── API/
│           ├── Core/
│           └── Infrastructure/
├── docs/                            # Documentation
├── tests/                           # Test Projects
├── scripts/                         # Build & Deployment Scripts
├── shared/                          # Shared Libraries
└── MicroservicesArchitecture.sln    # Main Solution File
```

## 📋 **Clean Architecture Layers**

### **API Layer** (Presentation)
- **Controllers** - HTTP endpoints and request/response handling
- **DTOs** - Data Transfer Objects for API contracts
- **Middleware** - Cross-cutting concerns (logging, authentication, etc.)

### **Core Layer** (Domain & Application)
- **Entities** - Domain models and business logic
- **Interfaces** - Repository and service contracts
- **Services** - Application business logic
- **Exceptions** - Custom domain exceptions
- **Extensions** - Utility extension methods

### **Infrastructure Layer** (Data & External)
- **DbContext** - Entity Framework configuration
- **Repositories** - Data access implementations
- **Migrations** - Database schema changes
- **External Services** - Third-party integrations

## 🚀 **Microservices Overview**

### **1. Department Service** (`http://localhost:5299`)
**Purpose**: Manages organizational departments and their metadata.

**Features**:
- ✅ Complete CRUD operations
- ✅ Soft delete functionality
- ✅ Data validation
- ✅ Repository pattern

**Endpoints**:
- `GET /api/departments` - Get all departments
- `GET /api/departments/{id}` - Get department by ID
- `POST /api/departments` - Create department
- `PUT /api/departments/{id}` - Update department
- `DELETE /api/departments/{id}` - Delete department

### **2. Employee Service** (`http://localhost:5148`)
**Purpose**: Manages employee data with inheritance and hierarchical relationships.

**Features**:
- ✅ Complete CRUD operations
- ✅ Inheritance (Manager class)
- ✅ Polymorphism
- ✅ Extension methods for name formatting
- ✅ LINQ queries and aggregations
- ✅ Self-referencing relationships (Manager → Team Members)
- ✅ Statistics endpoints (average age, salary, count)

**Endpoints**:
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

### **3. Project Service** (`http://localhost:5147`)
**Purpose**: Manages projects with many-to-many employee relationships.

**Features**:
- ✅ Complete CRUD operations
- ✅ Many-to-many relationships (Projects ↔ Employees)
- ✅ Employee assignment/removal
- ✅ Project statistics
- ✅ Advanced LINQ queries with joins

**Endpoints**:
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

1. **Open the solution**:
   ```bash
   # Open the main solution file
   MicroservicesArchitecture.sln
   ```

2. **Build all projects**:
   ```bash
   dotnet build
   ```

3. **Create databases and run migrations**:
   ```bash
   # Department Service
   cd src/Services/DepartmentService/API
   dotnet ef database update
   
   # Employee Service
   cd ../../EmployeeService/API
   dotnet ef database update
   
   # Project Service
   cd ../../ProjectService/API
   dotnet ef database update
   ```

4. **Run the microservices**:
   ```bash
   # Department Service (Port 5299)
   cd src/Services/DepartmentService/API
   dotnet run
   
   # Employee Service (Port 5148)
   cd ../../EmployeeService/API
   dotnet run
   
   # Project Service (Port 5147)
   cd ../../ProjectService/API
   dotnet run
   ```

### **Testing the APIs**

Use the provided Postman collection in the `docs/` folder:
- **Postman Collection**: `docs/Microservices_Postman_Collection.json`

Or use curl commands:
```bash
# Department Service
curl -X GET "http://localhost:5299/api/departments"

# Employee Service
curl -X GET "http://localhost:5148/api/employees"

# Project Service
curl -X GET "http://localhost:5147/api/projects"
```

## 📊 **Database Schema**

### **Department Service Database**
```sql
CREATE TABLE [Departments] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(100) NOT NULL,
    [Description] nvarchar(500) NOT NULL,
    [CreatedDate] datetime2 NOT NULL,
    [UpdatedDate] datetime2 NULL,
    [IsActive] bit NOT NULL
);
```

### **Employee Service Database**
```sql
CREATE TABLE [Employees] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(100) NOT NULL,
    [Age] int NOT NULL,
    [Department] nvarchar(50) NOT NULL,
    [Salary] float NOT NULL,
    [HireDate] datetime2 NOT NULL,
    [IsActive] bit NOT NULL,
    [ManagerId] int NULL,
    [Discriminator] nvarchar(8) NOT NULL,
    [TeamSize] int NULL,
    [ManagementLevel] nvarchar(50) NULL,
    CONSTRAINT [FK_Employees_Employees_ManagerId] FOREIGN KEY ([ManagerId]) 
        REFERENCES [Employees] ([Id])
);
```

### **Project Service Database**
```sql
CREATE TABLE [Projects] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(100) NOT NULL,
    [Description] nvarchar(500) NOT NULL,
    [StartDate] datetime2 NOT NULL,
    [EndDate] datetime2 NULL,
    [Status] nvarchar(20) NOT NULL,
    [Budget] float NOT NULL,
    [Department] nvarchar(50) NOT NULL,
    [IsActive] bit NOT NULL,
    [CreatedDate] datetime2 NOT NULL,
    [UpdatedDate] datetime2 NULL
);

CREATE TABLE [ProjectEmployees] (
    [ProjectId] int NOT NULL,
    [EmployeeId] int NOT NULL,
    [Role] nvarchar(50) NOT NULL,
    [AssignedDate] datetime2 NOT NULL,
    [UnassignedDate] datetime2 NULL,
    [IsActive] bit NOT NULL,
    CONSTRAINT [PK_ProjectEmployees] PRIMARY KEY ([ProjectId], [EmployeeId]),
    CONSTRAINT [FK_ProjectEmployees_Projects_ProjectId] FOREIGN KEY ([ProjectId]) 
        REFERENCES [Projects] ([Id]) ON DELETE CASCADE
);
```

## 🎯 **Advanced Features Implemented**

### **Object-Oriented Programming**
- ✅ **Inheritance** - Manager class inherits from Employee
- ✅ **Polymorphism** - Overridden methods and virtual properties
- ✅ **Encapsulation** - Private fields with public properties

### **Extension Methods**
- ✅ **Name Formatting** - `"john doe" → "John Doe"`
- ✅ **Department Filtering** - Filter employees by department
- ✅ **Statistics Calculation** - Average age, salary calculations

### **LINQ & Async Operations**
- ✅ **Async/Await** - All database operations are asynchronous
- ✅ **LINQ Queries** - Filtering, projections, aggregations
- ✅ **Complex Joins** - Self-referencing and many-to-many relationships

### **Clean Architecture**
- ✅ **Separation of Concerns** - Clear layer boundaries
- ✅ **Dependency Inversion** - Interfaces for loose coupling
- ✅ **Repository Pattern** - Data access abstraction
- ✅ **DTO Pattern** - Clean API contracts

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

This implementation provides a solid foundation for microservices architecture with clean code principles! 🎉 