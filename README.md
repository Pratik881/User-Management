# User Management System

## Overview

This project is a **User Management System** built with **ASP.NET Core MVC** to practice **Identity** features. It includes authentication, authorization, and user role management.

## Features

* **User Registration & Login**
* **Role-Based Authorization**
* **Email Confirmation for Registration**
* **User Profile Management**

## Technologies Used

* **ASP.NET Core MVC**
* **Identity in ASP.NET Core**
* **Entity Framework Core** (for database operations)
* **SQL Server** (Database)


## Setup Instructions

1. **Clone the repository:**
```bash
git clone https://github.com/Pratik881/User-Management.git
cd UserManagementSystem
```
## Setting Up `appsettings.json`

After cloning the repository, create a new `appsettings.json` file in the root directory. You can use `appsettings.example.json` as a template.



2. **Install Dependencies:**
```bash
dotnet restore
```

3. **Update Database:**
```bash
dotnet ef database update
```

4. **Run the Application:**
```bash
dotnet run
```



