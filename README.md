*** Backend dotnet application built using C#, .NETcore 8 and connected to database using a entity framework.
*** It is a popsicle factory where we can use swagger and hit end point and apply CRUD operation on factory data to persist data across application.

Programming language & Framworks: 

		C#, .NET core 8, Entity Framework, XUNit framework, Fluent Validation

Design pattrens:

		I have used design patterns like repository pattern and dependency injection making code more modular and testable.

Database base:

		- To Generating Migration and Syncing with Database, open the NuGet Package Manager Console in Visual Studio by selecting Tools => NuGet Package Manager => Package Manager Console. Then, execute the command.
		- "Add-Migration PoscicleCoDB"
		- Once build is succeded. Which means after creating the migration file, we need to update the database using the "Update-Database" command. you can use–verbose option to view the generated SQL statements executed in the target database.
		- "Update-Database -Verbose"
		- Once the above command is executed successfully, it will generate and execute the required SQL Statements in the defined database.
		- Now sign in to you SSMS and can find the ECommerceDB under your DB and can see all the tables created matching with Entity classes / database tables that we specified as the DbSet property. 

Testing framework: 
		
		Used Xunit framework with NSubstitute and Moq. NSubstitute and Moq are popular, feature-rich .NET mocking frameworks used to simulate dependencies (interfaces/classes) in unit tests, with NSubstitute often praised for its cleaner, more concise syntax, while Moq is widely used with extensive options for verifying behavior. 

Testable both in swagger and postman
