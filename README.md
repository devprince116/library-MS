# Library Management System

## 📌 Overview
The **Library Management System** is a web application that helps manage library operations such as book borrowing, returning, and tracking users. It is built using **C# .NET, Entity Framework, and SQL Server**.

## 🚀 Features
- User authentication (Signup, Login, Role-based access)
- Book management (Add, Edit, Delete books)
- Borrowing and returning books
- Track available and total copies of books
- Entity relationships using **Entity Framework**
- Database integration with **SQL Server**

## 🛠️ Tech Stack
- **Backend**: C#, .NET Core, Entity Framework Core
- **Frontend**: Angular
- **Database**: SQL Server
- **ORM**: Entity Framework Core
- **Authentication**: JWT
- **Development Tools**: Visual Studio,Rider, Docker (optional)

## 📂 Project Structure
```
📦 library-MS
├── 📂 Authentication (Contains user authentication and security logic)
├── 📂 Models (Entity models for Users, Books, and BorrowRecords)
├── 📂 Services (Business logic layer)
├── 📂 Data (Database context and migrations)
├── 📂 Controllers (API endpoints for managing books and users)
└── appsettings.json (Configuration settings)
```

## ⚙️ Installation & Setup
1. **Clone the Repository**
   ```sh
   git clone https://github.com/devprince116/library-MS.git
   cd library-MS
   ```
2. **Set up the Database**
   - Ensure **SQL Server** is running.
   - Update **appsettings.json** with your database connection string.

3. **Run Migrations**
   ```sh
   dotnet ef database update
   ```

4. **Run the Application**
   ```sh
   dotnet run
   ```

5. **Run the Frontend**
   ```sh
   cd libraryMS_FE
   npm start
   ```

5. **Access the API**
   - API will be available at `http://localhost:5000`

## 📝 API Endpoints
| Endpoint | Method | Description |
|----------|--------|-------------|
| `/api/auth/register` | POST | Register a new user |
| `/api/auth/login` | POST | User login |
| `/api/books` | GET | Get all books |
| `/api/books/{id}` | GET | Get a book by ID |
| `/api/books` | POST | Add a new book |
| `/api/books/{id}` | PUT | Update a book |
| `/api/books/{id}` | DELETE | Delete a book |
| `/api/borrow` | POST | Borrow a book |
| `/api/return` | POST | Return a book |



---

🔗 **GitHub Repository**: [Library Management System](https://github.com/devprince116/library-MS)

