# Task Management Application

A simple and efficient Todo application built with ASP.NET Core. This application allows users to create, edit, and delete tasks, helping them manage their daily activities effectively.

## Features

- Create new tasks
- Edit existing tasks
- Delete tasks with confirmation
- Add users to tasks
- Task due date management/filter
- User-friendly interface

## Technologies Used

- HTML
- CSS
- JavaScript
- ASP.NET Core
- Entity Framework
- Bootstrap
- SQL Server

## Installation

1. Clone the repository:
   ```
   git clone https://github.com/katlicia/Task-Management-Application.git
   ```
2. Navigate to the project directory:
   ```
   cd Task-Management-Application
   ```
3. Install the necessary dependencies:
   ```
   dotnet restore
   ```
4. Add the first migration (for initial setup):
   ```
   dotnet ef migrations add InitialCreate
   ```
5. Update the database:
   ```
   dotnet ef database update
   ```
6. Build the project:
   ```
   dotnet build
   ```
7. Run the application:
   ```
   dotnet run
   ```
   
## Usage
1. Start the application:
  ```
  dotnet run
  ```
2. Open your web browser and navigate to http://localhost:3000 (or the port your application runs on).
3. You can now create, edit, and delete tasks.

## Project Structure:
- Controllers/: Contains MVC controllers to handle user requests and return views.
- Models/: Defines the ViewModels used for data transfer between the UI and controllers, including data validation.
- Entities/: Defines the data entities that map to the database tables, representing the core data structure of the application.
- Views/: Contains the Razor views that render the UI.
- wwwroot/: Holds static files like CSS, JavaScript, and images.

## Contributing
Contributions are welcome! If you have suggestions for improvements or find a bug, please create an issue or submit a pull request.
