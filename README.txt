Objective: Develop a web application using the MVC (Model-View-Controller) architecture to manage PDF file uploads. Users can subscribe to other researchers, allowing them to view all the files uploaded by the researchers they follow. Subscribers can only view these files; downloading is not permitted.
Database Schema:
1.	User (Researcher):
o	Unique ID: A unique identifier for each researcher
o	Name: The name of the researcher
o	Join Date: The date when the researcher joined the platform
o	Total Uploads: The total number of research papers uploaded by the researcher
2.	Research (Journal):
o	Unique ID: A unique identifier for each research paper
o	Researcher ID (Foreign Key): The ID of the researcher who uploaded the paper
o	File Name: The name of the PDF file
o	File Path: The path where the file is stored
o	Description: A description of the research paper
o	Upload Date: The date when the research paper was uploaded
3.	Subscriptions:
o	Unique ID: A unique identifier for each subscription
o	Subscriber ID: The ID of the researcher who is subscribing
o	Subscribed To ID: The ID of the researcher being followed
Models:
•	Researcher
•	Journal
•	Subscription
Views:
•	Login: A page for user authentication
•	Register: A page for new user registration
•	Main (All Researchers): A dashboard displaying all researchers
•	Researcher Journals: A page showing all journals uploaded by a specific researcher
Controllers:
•	MainDbContext: Context class for database interactions
•	ResearcherController: Controller managing researcher-related actions
•	JournalController: Controller handling journal-related actions
•	SubscriptionController: Controller managing subscription actions
Frontend Implementation: The frontend will be developed using React. React Router will be utilized to manage navigation between different routes within the application. Additionally, @react-pdf-viewer/core and pdfjs-dist will be used to enable PDF viewing capabilities.
Backend Implementation: The backend will be created using ASP.NET Core MVC, with Entity Framework for data management. The application will interact with a SQL Server Express database for storing and retrieving data.
Steps to Create the React Application:
1.	Initialize the React App: npx create-react-app dotnetfront
2.	Install React Router: npm install react-router-dom
Steps to Create the Backend with ASP.NET Core MVC:
1.	Install Required NuGet Packages:
o	Entity Framework Core
o	Entity Framework Core for SQL Server
o	iTextSharp for PDF processing
Use the NuGet Package Manager Console to install these packages
"Note: It is important to emphasize that for creating the database, you need to use the NuGet Package Manager to create the initial database migration based on the models and the DbContext using the following commands:
1.	Add-Migration InitialCreate
2.	Update-Database"

