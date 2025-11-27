WPF Admin Tool – Bookstore

This project is a WPF application built with Entity Framework (Database First) for administering a bookstore database.
The application supports full CRUD operations (Create, Read, Update, Delete) for books, authors, stores and inventory.

The project is structured using a basic MVVM pattern and interacts with a SQL Server database.

How to run the application
1. Restore the database
   
A backup of the database is included in the solution: /DatabaseBackup/BookstoreDB.bak

2. Update the connection string

Open the file: /Models/AppDbContext.cs
Inside the OnConfiguring() method, replace the connection string with your own SQL Server instance, for example:

optionsBuilder.UseSqlServer(
    "Data Source=YOUR_SERVER_NAME;Initial Catalog=BookstoreDB;Integrated Security=True;TrustServerCertificate=True");

3. Run the application
