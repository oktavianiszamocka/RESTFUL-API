# RESTFUL-API
This RestFul API project is written in C# and ASP.NET Core 2.1 using SQL Server and Entity Framework Code First

a.	What is required for running the project
- Open from Visual Studio
- Change the database Server in appsetting.json to be your server

b.	Steps how to run scripts that will setup database for the project
To able to run the database you need to run these scripts in Package Manager Console. The scripts are :
- first => 'add-migration nameOfMigration'
- second => 'update-database'

then database scheme has been written in your database

c.	Steps how to build and run the project
- Build the project using Visual Studio or on the command line with 'dotnet build'
- Run the project. API will start up on http://localhost:5000/ , or http://localhost:5000/ with 'dotnet run' if u use command line
- Use HTTP client like Postman or Swagger. The API has been configured wit Swagger. It can be checked on https://localhost:5001/swagger/index.html


d.	Example usages (ie. like example curl commands)
- GET All Note => http://localhost:5000/api/v1/Note
- GET Specific Note => http://localhost:5000/api/v1/Note/{id}
- POST Note => http://localhost:5000/api/v1/Note/ Body:{"title": "string","content":"string"}
- PUT Note (Update) => http://localhost:5000/api/v1/Note/{id} Body:{"title": "string","content":"string"}
- DELETE Note => http://localhost:5000/api/v1/Note/{id}
- GET history of specific Note =>  http://localhost:5000/api/v1/Note/history/{id}

