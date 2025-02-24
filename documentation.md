# Project Documentation

This project is a C# backend application that uses Entity Framework Core to interact with a database. It provides functionality related to managing users, roles, configurations, questions, answers, tokens, settings, background images, and tasks.

## Purpose
The purpose of this project is to serve as the backend for an application that requires user management, configuration settings, question and answer functionalities, token generation, setting management, background image handling, and task management.

## Classes and Methods

### `DataContext`
- **Methods:**
  - `OnModelCreating(ModelBuilder modelBuilder)`: Overrides the default model creation and seeding logic for the database context.

### Data Models / Entities:
1. `ConfigUser`
2. `Role`
3. `Config`
4. `Question`
5. `QuestionCategory`
6. `Answer`
7. `ConfigUserToken`
8. `RefreshToken`
9. `Setting`
10. `BackgroundImage`
11. `Task`

### Entity Configuration Methods:
1. `CreateBackgroundImage(ModelBuilder modelBuilder)`
2. `CreateConfigUser(ModelBuilder modelBuilder)`
3. `CreateRole(ModelBuilder modelBuilder)`
4. `CreateConfig(ModelBuilder modelBuilder)`
5. `CreateQuestion(ModelBuilder modelBuilder)`
6. `CreateQuestionCategory(ModelBuilder modelBuilder)`
7. `CreateAnswer(ModelBuilder modelBuilder)`
8. `CreateConfigUserToken(ModelBuilder modelBuilder)`
9. `CreateRefreshToken(ModelBuilder modelBuilder)`
10. `CreateSetting(ModelBuilder modelBuilder)`
11. `CreateTasks(ModelBuilder modelBuilder)`

## Endpoints

The project does not explicitly define endpoints as it is focused on database context setup and entity configuration. However, based on the entities and relationships defined in the `DataContext` class, endpoints for CRUD operations related to users, roles, configurations, questions, answers, tokens, settings, background images, and tasks can be implemented in a corresponding API.

## Data Structures

1. `ConfigUser`
   - Id: int
   - FirstName: string
   - LastName: string
   - Email: string
   - Password: string (hashed)
   - RoleId: int
   - RefreshToken: string
   - TokenCreated: DateTime
   - TokenExpires: DateTime

2. `Role`
   - Id: int
   - Name: string

3. `Config`
   - Id: int
   - Title: string
   - Value: string

4. `Question`
   - Id: int
   - Text: string
   - Categories: ICollection<QuestionCategory>

5. `QuestionCategory`
   - Id: int
   - Name: string

6. `Answer`
   - Id: int
   - Text: string

7. `ConfigUserToken`
   - Id: int
   - Token: string
   - Email: string
   - CreatedAt: DateTime
   - RoleId: int

8. `RefreshToken`
   - Id: int

9. `Setting`
   - Id: int
   - Name: string
   - BackgroundImage: string
   - BackgroundImageId: int
   - Language: string
   - TalkingSpeed: double
   - GreetingMessage: string
   - State: boolean
   - AiInUse: string

10. `BackgroundImage`
   - Id: int
   - Base64Image: string

11. `Task`
   - Id: int
   - Text: string
   - Done: boolean

The defined data structures represent the entities stored in the database and their respective properties.

This project is instrumental in setting up the database context, configuring entities, and providing initial seed data, which can be extended and utilized to build a comprehensive backend system.# Project Documentation

## Description
This C# project is a backend application for a question and answer system. It provides APIs to manage answers to questions, including retrieving, updating, adding, and deleting answers.

## Purpose
The purpose of this project is to serve as the backend for a question and answer system, allowing users to interact with answers stored in a database. The project includes functionalities to handle CRUD (Create, Read, Update, Delete) operations on answers.

## Classes and Methods

### Class: Program
- **Main(string[] args)**: Entry point of the application, where the application is configured and started. It sets up the database connection, Swagger documentation, authentication, authorization, CORS policy, and starts the application.

### Class: AnswerController
- **GetAnswers()**: GET endpoint to retrieve all answers.
- **GetAnswer(int id)**: GET endpoint to retrieve a specific answer by ID.
- **ChangeAnswer(int id, UpdateAnswer answer)**: PUT endpoint to update an existing answer by ID.
- **AddAnswer(Answer answer)**: POST endpoint to add a new answer.
- **DeleteAnswer(int id)**: DELETE endpoint to delete an answer by ID.

## Endpoints
1. **GET /answer**: Retrieves all answers.
2. **GET /answer/{id}**: Retrieves a specific answer by ID.
3. **PUT /answer/{id}**: Updates an existing answer by ID.
4. **POST /answer**: Adds a new answer.
5. **DELETE /answer/{id}**: Deletes an answer by ID.

## Data Structures

### Request Body for Adding an Answer
```json
{
  "question": "Where is the gym?",
  "sessionId": "",
  "language": "en"
}
```

### Answer Entity
```csharp
public class Answer
{
    public int Id { get; set; }
    public string Text { get; set; }
}
```

### Updated Answer Model
```csharp
public class UpdateAnswer
{
    public string Text { get; set; }
}
```

This documentation provides an overview of the project, its purpose, classes and methods used, endpoints available, and data structures used for communication within the application.# Backend Project Documentation

The Backend project is a C# project that contains controllers responsible for handling different functionalities related to an assistant AI and user authentication.

## AssistantAiController

This controller manages interactions related to the assistant AI functionality. It contains the following classes:

### UserRequest
- `Question`: A string representing the user's question.
- `SessionId`: An optional string representing the session ID.
- `Language`: An optional string representing the language (default is "en-US").

### UserResponse
- `Answer`: A string representing the AI's response to the user question.
- `SessionId`: A string representing the session ID of the response.
- `TimeNeeded`: A string representing the time needed to process the response.

### Methods
1. `GetAiResponse`: Accepts a `UserRequest` object and returns an `ActionResult<UserResponse>` with the AI's response.
2. `GetAiResponseMistral`: Accepts a `MistralUserQuestion` object and returns an `ActionResult<UserResponse>` with the AI's response using Mistral.

## AuthenticationController

This controller manages user authentication functionalities. It contains methods for user login, token application, and token refresh.

### Methods
1. `Login`: Validates user credentials and generates a JWT token for authentication.
2. `ApplyToken`: Applies a token to create a new user.
3. `RefreshToken`: Refreshes the JWT token for a user based on the provided refresh token.

Both controllers interact with a `DataContext` that presumably handles database operations for the application.

## Endpoints

1. `/assistantai` (POST): Endpoint to interact with the AI and get a response to a user question.
2. `/assistantai/mistral` (POST): Endpoint to interact with the AI using Mistral for a response.
3. `/authentication` (POST): Endpoint for user login authentication.
4. `/authentication/applytoken` (POST): Endpoint to apply a token for creating a new user.
5. `/authentication/refresh-token` (POST): Endpoint to refresh the JWT token for a user.

## Data Structures

1. `UserRequest`: Represents user input data for the AI.
2. `UserResponse`: Represents the response data from the AI to the user.
3. `MistralUserQuestion`: Represents user input data specifically for Mistral AI.
4. `RefreshToken`: Represents a refresh token for token refreshing.

---

This documentation provides an overview of the controllers, methods, endpoints, and data structures used in the Backend project. It serves as a guide for developers to understand and work with the functionalities implemented in the project.# Project Documentation

This project is a backend application written in C# using ASP.NET Core. It provides endpoints to manage background images and configuration settings through RESTful APIs.

## Purpose
The purpose of this project is to allow users to perform CRUD (Create, Read, Update, Delete) operations on background images and configuration settings. It allows users to add, retrieve, update, and delete background images and configuration settings through HTTP requests.

## BackgroundImageController.cs
### Class Description
- **Namespace:** backend.Controllers
- **Description:** This class defines the controller for managing background images.

### Methods
1. **GetBackgroundImages()**
   - **Endpoint:** GET /BackgroundImage
   - **Description:** Retrieves all background images.
   
2. **GetBackgroundImage(int id)**
   - **Endpoint:** GET /BackgroundImage/{id}
   - **Description:** Retrieves a specific background image based on the provided ID.
   
3. **AddBackgroundImage(CreateBackgroundImage bi)**
   - **Endpoint:** POST /BackgroundImage
   - **Description:** Adds a new background image with the provided base64 image data.
   
4. **DeleteBackgroundImage(int id)**
   - **Endpoint:** DELETE /BackgroundImage/{id}
   - **Description:** Deletes the background image with the provided ID.

## ConfigController.cs
### Class Description
- **Namespace:** backend.Controllers
- **Description:** This class defines the controller for managing configuration settings.

### Methods
1. **GetConfigs()**
   - **Endpoint:** GET /Config
   - **Description:** Retrieves all configuration settings.
   
2. **GetConfig(int id)**
   - **Endpoint:** GET /Config/{id}
   - **Description:** Retrieves a specific configuration setting based on the provided ID.
   
3. **AddConfig(CreateConfig config)**
   - **Endpoint:** POST /Config
   - **Description:** Adds a new configuration setting with the provided title and value.
   
4. **ChangeConfig(int id, CreateConfig config)**
   - **Endpoint:** PUT /Config/{id}
   - **Description:** Updates the configuration setting with the provided ID with new title and value.
   
5. **DeleteConfig(int id)**
   - **Endpoint:** DELETE /Config/{id}
   - **Description:** Deletes the configuration setting with the provided ID.

## Data Structures
1. **BackgroundImage**
   - **Properties:** Id (int), Base64Image (string)
   
2. **Config**
   - **Properties:** Id (int), Title (string), Value (string)
   
3. **CreateBackgroundImage**
   - **Properties:** Base64Image (string)
   
4. **CreateConfig**
   - **Properties:** Title (string), Value (string)

These classes and methods provide a RESTful API interface for managing background images and configuration settings. They utilize Entity Framework Core for database operations and are protected by authorization.# Project Documentation

This C# project is a backend application that handles user configuration operations such as creating, reading, updating, and deleting user information. It provides endpoints for managing users and user invitations. The project includes classes and methods to interact with a database using Entity Framework Core for data storage.

## Purpose

The purpose of this project is to provide APIs for managing user configurations in an application. It allows creating users, getting user information, updating user details, changing user passwords, and handling user invitations. The project ensures secure user management by hashing passwords and sending email invitations with unique tokens.

## Classes and Methods

### `ConfigUserController` Class

- **`PostUser(CreateUserToken user)`**: Creates a new user with the provided details if the email is not already used. Sends an email invitation to the user.
- **`GetUsers()`**: Retrieves a list of users with limited information (email, first name, last name, id, role id).
- **`GetUser(int id)`**: Retrieves detailed information about a specific user based on the provided user id.
- **`GetInvitations()`**: Retrieves a list of user invitations.
- **`GetInvitation(int id)`**: Retrieves detailed information about a specific user invitation based on the provided invitation id.
- **`DeleteInvitation(int id)`**: Deletes a user invitation based on the provided id.
- **`ResendInvitation(int id)`**: Resends an invitation email with a new token to the user.
- **`DeleteUser(int id)`**: Deletes a user based on the provided id.
- **`PutUser(int id, ChangeConfigUser user)`**: Updates user details with the provided information.
- **`ChangePassword(int id, string newPassword)`**: Changes the password of a user identified by the provided id.

### Private Methods

- **`UserExists(int id)`**: Checks if a user exists based on the provided user id.
- **`EmailIsUsed(string email)`**: Checks if an email is already used by a user or a user invitation.
- **`CreateHtmlMailTemplate(Guid token)`**: Creates an HTML email template for sending user invitations with a unique token.

## Endpoints

- **`POST /ConfigUser`**: Create a new user.
- **`GET /ConfigUser`**: Get a list of users.
- **`GET /ConfigUser/{id}`**: Get details of a specific user.
- **`GET /ConfigUser/invitation`**: Get a list of user invitations.
- **`GET /ConfigUser/invitation/{id}`**: Get details of a specific user invitation.
- **`DELETE /ConfigUser/invitation/{id}`**: Delete a user invitation.
- **`POST /ConfigUser/invitation/{id}/resend`**: Resend an invitation email with a new token to a user.
- **`DELETE /ConfigUser/{id}`**: Delete a user.
- **`PUT /ConfigUser/{id}`**: Update user details.
- **`PUT /ConfigUser/{id}/change-password`**: Change the password of a user.
  
## Conclusion

The project provides a robust backend for managing user configurations efficiently. It uses secure practices for handling user data and interactions. Developers can utilize the defined endpoints to integrate user management functionalities within their application.# Project Documentation

This project contains controllers for managing questions and roles in a backend system. The controllers are responsible for handling CRUD operations related to questions and roles.

## QuestionController

### Purpose
The `QuestionController` provides endpoints to manage questions. Users can retrieve all questions, get a specific question by its ID, add a new question, update an existing question, and delete a question. Additionally, users can retrieve all question categories.

### Endpoints
1. `GET /Question`: Retrieve all questions with their associated answers and categories.
2. `GET /Question/{id}`: Get a specific question by ID.
3. `POST /Question`: Add a new question with specified categories and answers.
4. `PUT /Question/{id}`: Update an existing question by ID with specified categories and answers.
5. `DELETE /Question/{id}`: Delete a question by ID.
6. `GET /Question/categories`: Retrieve all question categories.

### Data Structures
- `Question`: Represents a question with text, categories, and answers.
- `CreateQuestion`: Input model for adding or updating a question with text, categories, and answers.
- `Answer`: Represents an answer to a question with text and user information.
- `QuestionCategory`: Represents a category associated with a question.

## RoleController

### Purpose
The `RoleController` manages roles in the system. Users can retrieve all roles, get a specific role by ID, create a new role, update an existing role, and delete a role.

### Endpoints
1. `GET /Role`: Retrieve all roles.
2. `GET /Role/{id}`: Get a specific role by ID.
3. `POST /Role`: Create a new role.
4. `PUT /Role/{id}`: Update an existing role by ID.
5. `DELETE /Role/{id}`: Delete a role by ID.

### Data Structures
- `Role`: Represents a role in the system.
- `CreateRole`: Input model for creating or updating a role with a name.

---
This documentation provides an overview of the controllers, endpoints, and data structures used in the project for managing questions and roles in a backend system.# C# Project Documentation

This project consists of multiple controllers in the `Backend\Controllers` namespace and several entities in the `Backend\Entities` namespace. The project utilizes the ASP.NET Core framework for creating RESTful APIs.

## Controllers

### 1. SettingsController

- **Purpose**: Manages settings data such as user preferences.
  
1. `GetSettings()` - **GET** endpoint
   - Returns a list of all settings.
  
2. `GetSetting(string id)` - **GET** endpoint
   - Returns a specific setting based on the provided ID.
  
3. `UpdateSetting(int id, CreateSetting newSetting)` - **PUT** endpoint
   - Updates an existing setting with new values.

4. `CreateSetting(CreateSetting setting)` - **POST** endpoint
   - Creates a new setting.

### 2. StreamingController

- **Purpose**: Handles image processing and facial recognition for streaming data.
  
1. `SendFrame(FrameObject frame)` - **POST** endpoint
   - Receives a frame image, processes it using Emgu CV, and returns any detected faces.

### 3. TaskController

- **Purpose**: Manages task data for users.
  
1. `GetTasks()` - **GET** endpoint
   - Retrieves a list of all tasks.
  
2. `GetTask(int id)` - **GET** endpoint
   - Retrieves a specific task by ID.
  
3. `UpdateTask(int id, UpdateTask updatedTask)` - **PUT** endpoint
   - Updates an existing task.
  
4. `CreateTask(CreateTask newTask)` - **POST** endpoint
   - Creates a new task.
  
5. `DeleteTask(int id)` - **DELETE** endpoint
   - Deletes a task based on the provided ID.

## Entities

1. **Answer**
   - Represents an answer object.
  
2. **BackgroundImage**
   - Contains information about background images.
  
3. **Config**
   - Represents configuration data with a title and value.
  
4. **ConfigUser**
   - Represents users with configuration and authentication data.

## Conclusion

The project offers a set of APIs to handle settings, streaming, and task-related operations, along with entity classes to define the data structures used in the application. The controllers facilitate interactions with the data store and provide endpoints for client applications to consume the services provided by the backend.# Project Documentation

This project is a backend application that consists of various entities representing different data structures and classes used for managing user tokens, roles, messages, questions, categories, refresh tokens, settings, tasks, and signalR connection handling.

## Classes and Methods

1. **ConfigUserToken**
   - Represents a configuration user token entity with properties like Id, Token, Email, CreatedAt, RoleId, and Role.
2. **CreateUserToken**
   - Represents the creation of a user token with properties Email and RoleId.
3. **Login**
   - Represents user login information with properties UserIdentificator and Password.
4. **MistralThread**
   - Represents a thread entity with an Id and a list of messages.
5. **Message**
   - Represents a message entity with properties role and content.
6. **MistralUserQuestion**
   - Represents a user question with properties Question and ThreadId.
7. **Question**
   - Represents a question entity with properties Id, Text, Categories, and Answers.
8. **CreateQuestion**
   - Represents the creation of a question with properties Text, Answers, and Categories.
9. **QuestionCategory**
   - Represents a question category entity with properties Id, Name, and Questions.
10. **CreateQuestionCategory**
    - Represents the creation of a question category with a Name property.
11. **RefreshToken**
    - Represents a refresh token entity with properties Id, Token, Created, and Expires.
12. **Role**
    - Represents a role entity with properties Id, and Name, and Users.
13. **CreateRole**
    - Represents the creation of a role with a Name property.
14. **Setting**
    - Represents a setting entity with various properties like Id, ConfigUserId, Name, BackgroundImage, Language, TalkingSpeed, GreetingMessage, State, and AiInUse.
15. **CreateSetting**
    - Represents the creation of a setting with properties Name, BackgroundImage, BackgroundImageId, Language, TalkingSpeed, GreetingMessage, State, and AiInUse.
16. **Task**
    - Represents a task entity with properties Id, Text, and Done.
17. **CreateTask**
    - Represents the creation of a task with properties Text and Done.
18. **UpdateTask**
    - Represents the update of a task with properties Text and Done.
19. **ConnectionStateHub**
    - Represents a SignalR hub for managing the connection state.

## Endpoints and Data Structures

- The entities represent different data structures used within the backend application for managing user-related information, messaging, questions, categories, roles, settings, and tasks.
- The classes consist of properties representing different attributes of the entities along with methods for operations like creation and updates.
- The SignalR hub `ConnectionStateHub` is used for managing the connection state with methods like `OnConnectedAsync()`.

This documentation provides an overview of the entities, classes, and methods used in the backend application, outlining their purpose and structure within the project.# Project Documentation

## Description
This C# project involves database migrations for a backend system. The provided migration file (`20250128065430_Initial.cs`) includes the initial setup for database tables using Entity Framework Core.

## Purpose
The purpose of these migrations is to create the necessary database tables and relationships for the backend system to function properly. The migration script sets up tables for storing configuration settings, user details, roles, tasks, questions, answers, refresh tokens, and more.

## Classes and Methods
- **Migration**: Initial migration class that inherits from `Microsoft.EntityFrameworkCore.Migrations.Migration`.
  - **Up**: Method to define the structure of the database tables.
  - **Down**: Method to revert the changes made in the `Up` method.

## Endpoints
This project does not directly define endpoints, as it focuses on database migrations. Any API endpoints or CRUD operations would be handled separately in the application that utilizes this database schema.

## Data Structures
The following tables are created in the database:
1. **backgroundimage**: Stores base64 images for background settings.
2. **config**: Contains key-value pairs for configuration settings.
3. **question**: Stores questions that can be asked.
4. **questioncategory**: Represents categories for questions.
5. **refreshtoken**: Stores tokens for user authentication.
6. **role**: Defines user roles.
7. **setting**: Contains settings like background image, language, etc.
8. **task**: Stores tasks with a text description.
9. **answer**: Stores answers to questions along with related user data.
10. **questionquestioncategory**: Represents a many-to-many relationship between questions and categories.
11. **configuser**: Stores user details including roles and access tokens.
12. **configusertoken**: Contains user tokens with associated roles.

---

This documentation provides an overview of the database structure created by the C# migration script. For detailed information on API endpoints or application functionality, refer to the relevant parts of the application codebase.# C# Backend Documentation

This C# project contains code related to the database migrations for a backend application. It includes the initial migration file with the generated code for creating database tables and schema.

## Purpose
The purpose of this project is to manage database migrations for the backend application. It uses Entity Framework Core to define the structure of the database, including tables, columns, relationships, and indexes.

## Classes and Methods

### Classes
1. **Initial**: This class represents the initial migration. It defines the database structure using Entity Framework Core's `ModelBuilder`.

### Data Models
1. **Answer**: Represents an answer entity with properties like Id, QuestionId, Text, and User.
2. **BackgroundImage**: Represents a background image entity with properties like Id and Base64Image.
3. **Config**: Represents a configuration entity with properties like Id, Title, and Value.
4. **ConfigUser**: Represents a user configuration entity with properties like Id, Email, FirstName, LastName, Password, RefreshToken, RoleId, TokenCreated, and TokenExpires.
5. **ConfigUserToken**: Represents a user token configuration entity with properties like Id, CreatedAt, Email, RoleId, and Token.
6. **Question**: Represents a question entity with properties like Id and Text.
7. **QuestionCategory**: Represents a question category entity with properties like Id and Name.
8. **RefreshToken**: Represents a refresh token entity with properties like Id, Created, Expires, and Token.
9. **Role**: Represents a role entity with properties like Id and Name.
10. **Setting**: Represents a setting entity with properties like Id, AiInUse, BackgroundImage, BackgroundImageId, ConfigUser, ConfigUserId, GreetingMessage, Language, Name, State, and TalkingSpeed.
11. **Task**: Represents a task entity with properties like Id, Done, and Text.

### Methods
1. **BuildTargetModel**: Method to define the database schema using `ModelBuilder`. It specifies the entities, properties, relationships, constraints, and indexes for the database tables.

## Endpoints
The project does not contain endpoint definitions as it focuses on database migrations and schema definition.

## Data Structures
The project defines various data structures representing entities in the database, such as Answer, BackgroundImage, Config, ConfigUser, ConfigUserToken, Question, QuestionCategory, RefreshToken, Role, Setting, and Task. These structures contain properties that map to database columns.

---
This documentation provides an overview of the C# project related to database migrations for a backend application. It includes information about classes, methods, data models, and their purposes.# Project Documentation

This project is a backend application developed in C# using Entity Framework Core for interacting with a PostgreSQL database. The purpose of this application is to provide functionality related to managing questions, answers, configurations, users, roles, and other related entities.

## Classes and Methods

### `DataContextModelSnapshot`
- This class is a snapshot of the data model configured in the `DataContext`.
- It defines the entities, properties, relationships, and constraints of the database model.
- The `BuildModel` method sets up the database schema by defining tables, columns, keys, indexes, and relationships.

### Entities:
1. `Answer`: Represents an answer given to a question.
2. `BackgroundImage`: Represents a background image entity.
3. `Config`: Represents a configuration entity.
4. `ConfigUser`: Represents a user configuration entity.
5. `ConfigUserToken`: Represents a token entity associated with a user.
6. `Question`: Represents a question entity.
7. `QuestionCategory`: Represents a category associated with a question.
8. `RefreshToken`: Represents a refresh token entity.
9. `Role`: Represents a user role entity.
10. `Setting`: Represents a setting entity.
11. `Task`: Represents a task entity.

### Endpoints and Data Structures
- The application defines various data structures for storing information related to users, questions, answers, configurations, tokens, roles, etc.
- It sets up relationships between entities to establish associations and constraints in the database schema.
- The `DataContextModelSnapshot` class utilizes Entity Framework Core methods for defining the database model, including table names, column types, constraints, and relationships.

Overall, this backend application is designed to handle CRUD operations for the entities mentioned above, manage data relationships, and maintain the integrity of the database schema using Entity Framework Core and PostgreSQL.# Project Documentation

This C# project is a backend application that serves HTTP requests using the Microsoft.AspNetCore framework. It includes various files and attributes generated by tools like MSBuild.

## Purpose
The purpose of this project is to provide backend services for a web application. It utilizes the .NETCoreApp framework and Microsoft.AspNetCore components to handle HTTP requests and responses.

## Classes and Methods
As the provided code snippets are autogenerated by tools like MSBuild, they do not contain explicit class and method definitions. However, the project most likely includes classes and methods used to configure routes, handle HTTP requests, and interact with services.

## Endpoints
The project likely exposes various endpoints for different functionalities such as retrieving data, processing requests, and serving static content. The exact endpoints and their functionalities would be defined in the project's source code.

## Data Structures
Considering the technologies used (Microsoft.AspNetCore), the project may utilize data structures like JSON to exchange data between the backend and clients. Additionally, it may incorporate models and entities to represent the data being processed by the application.

## Note
The provided documentation is based on the autogenerated files and does not contain specific implementation details. To understand the project's full functionality, it is recommended to explore the project's source code directly.

Overall, this project is a backend application designed to handle HTTP requests and provide services to a frontend application.# Project Documentation

This C# project includes a `AiUtil` class that provides utility methods for creating and managing AI assistants. The project interacts with the OpenAI API to create assistants, classify questions, and provide answers. 

## Purpose

The purpose of this project is to encapsulate the functionality related to interacting with the OpenAI API to create AI assistants, classify questions, and provide answers based on the trained models.

## `AiUtil` Class

The `AiUtil` class contains methods to interact with the OpenAI API for creating and managing AI assistants. Below are the methods available in the class:

### Constructor

- `AiUtil()`: Initializes the `AiUtil` class, sets up the HTTP client with necessary headers, and creates the default assistants.

### Public Methods

- `GetInstance()`: Returns an instance of the `AiUtil` class.
- `AskQuestion(DataContext ctx, string? threadId, string question, string language, bool isClassification)`: Asks a question to the AI assistant for answering or classification.
- `ClassifyQuestion(string question)`: Classifies a question into predefined categories.
- `CheckStatus(string threadId, string runId)`: Checks the status of a specific AI assistant run.
- `GetResultString(string threadId)`: Gets the result of the AI assistant as a string.
- `WaitForResult(string threadId, string runId)`: Waits for the AI assistant to complete the task.

### Private Methods

- `CreateAssistant()`: Creates an AI assistant for answering questions.
- `CreateClassifyAssistant()`: Creates an AI assistant for classifying questions.
- `GetThread()`: Gets the next available thread for an assistant.
- `GetClassifyThread()`: Gets the thread for the classification assistant.
- `UpdateThreads()`: Updates the list of active threads.
- `DeleteThread(string threadId)`: Deletes a specific thread.
- `CreateThread()`: Creates a new thread for the AI assistant.

## Endpoints

The project interacts with the OpenAI API using the following endpoints:

- `POST https://api.openai.com/v1/assistants`: Used to create AI assistants.
- `POST https://api.openai.com/v1/threads`: Used to create threads within an assistant.
- `POST https://api.openai.com/v1/threads/{threadId}/messages`: Used to send messages to an AI assistant.
- `POST https://api.openai.com/v1/threads/{threadId}/runs`: Used to start a run on a specific thread.
- `GET https://api.openai.com/v1/threads/{threadId}/runs/{runId}`: Used to check the status of a run.
- `GET https://api.openai.com/v1/threads/{threadId}/messages`: Used to retrieve messages from a thread.

## Data Structures

- `ThreadTime`: Represents a thread ID and the time it was last accessed.
- `assistantData`: Contains the data required to create an AI assistant.
- `classificationData`: Contains data for classifying questions.
- `messageData`: Contains data for sending messages to an AI assistant.

This documentation outlines the structure and purpose of the C# project for AI assistants interaction with the OpenAI API. The `AiUtil` class provides a central interface for managing AI assistants, asking questions, and retrieving answers.# Backend Util Documentation

This project contains utility classes for authentication and email functionality in a backend application. The `AuthenticationUtil.cs` file provides methods for generating JWT tokens, hashing and verifying passwords. The `MailUtil.cs` file contains methods for sending emails using SMTP.

## AuthenticationUtil

### Methods
1. `GenerateJwtToken(Login login, int userId, int roleId)`: Generates a JWT token with the specified user login information, user ID, and role ID.
2. `HashPassword(string password)`: Hashes a given password using BCrypt.
3. `VerifyPassword(string password, string hashedPassword)`: Verifies a password against a hashed password using BCrypt.

### Dependencies
- `System.IdentityModel.Tokens.Jwt`: Used for JWT token generation and decoding.
- `System.Security.Claims`: Provides access to claims-based identity information.
- `System.Text`: Used for text encoding.
- `backend.Entities`: Contains the `Login` entity used for user login information.
- `Microsoft.IdentityModel.Tokens`: Provides token validation and creation functionality.

## MailUtil

### Methods
1. `SendMail(string to, string subject, string body)`: Sends an email to the specified recipient with the given subject and body.

### Dependencies
- `System.Net`: Provides network-related functionality.
- `System.Net.Mail`: Used for sending emails via SMTP.
- `System.Text`: Used for text encoding.
- `SecretsProvider`: Accesses email server and authentication details for sending emails.

## Endpoint Usage
These utility classes can be used in the backend application to handle authentication and email functionality. For example, the `GenerateJwtToken` method can be called during user login to generate a JWT token for subsequent API requests. Similarly, the `SendMail` method can be used to send emails for various notification purposes within the application.

## Data Structures
- **Login**: Represents user login information.
- **JWT Token**: Contains user identification, role, and expiration time.
- **MailMessage**: Represents an email message with from, to, subject, and body fields.

---

By utilizing the `AuthenticationUtil` and `MailUtil` classes, the backend application can securely handle user authentication and send emails as part of its functionality.# Project Documentation

## Description
This C# project contains a class `MistralUtil` in the `MistralUtil.cs` file. The `MistralUtil` class provides methods to interact with the Mistral AI platform for asking questions and classifying questions based on predefined categories.

## Purpose
The purpose of the `MistralUtil` class is to facilitate communication with the Mistral AI platform to ask questions and classify questions based on categories. It uses the Mistral AI APIs to perform these actions.

## Classes and Methods
### MistralUtil Class
- **Fields**
  - `Threads`: A list of MistralThread objects.
  - `Key`: Mistral AI API key.
  - `HttpClient`: An instance of `HttpClient` for making HTTP requests.
  - `_categories`: A list of QuestionCategory objects.

- **Methods**
  - `AskQuestion(DataContext ctx, string? threadId, string question)`: 
    - Parameters: `ctx` - DataContext object, `threadId` - optional thread id, `question` - the question to ask.
    - Description: Asks a question to the Mistral AI platform and returns the response.
  
  - `ClassifyQuestion(string question)`: 
    - Parameters: `question` - the question to classify.
    - Description: Classifies a question based on predefined categories.

  - `CreateNewThread(out string threadId)`: 
    - Parameters: `threadId` - output parameter for the created thread Id.
    - Description: Creates a new MistralThread object and adds it to the Threads list.

  - `GetClassifyQuestions(DataContext ctx, string question)`: 
    - Parameters: `ctx` - DataContext object, `question` - the question to classify.
    - Description: Retrieves and classifies questions based on categories.

## Endpoints
- **AskQuestion**
  - Endpoint: POST `https://api.mistral.ai/v1/agents/completions`
  - Parameters: 
    - `ctx`: DataContext object
    - `threadId`: optional thread id
    - `question`: the question to ask
  - Returns: A tuple containing the response message and thread id.

- **ClassifyQuestion**
  - Endpoint: POST `https://api.mistral.ai/v1/agents/completions`
  - Parameters: 
    - `question`: the question to classify
  - Returns: A list of QuestionCategory objects based on the classification.

## Data Structures
- **Message**: Represents a message with role and content.
- **MistralThread**: Represents a thread with messages.
- **QuestionCategory**: Represents a category for questions.

---
This documentation provides an overview of the `MistralUtil` class and its functionalities for interacting with the Mistral AI platform.# Project Documentation

This project contains a C# class `SecretsProvider` under the namespace `backend.Util` that is responsible for providing access to various secret configurations such as Mail settings, JWT keys, and API keys. This class utilizes the `Microsoft.Extensions.Configuration` library to retrieve configurations from various sources like JSON files and environment variables.

## Purpose
The purpose of the `SecretsProvider` class is to centralize the handling of sensitive configuration data within the application. By using this class, developers can access these secrets securely and maintain a consistent approach to handling configurations.

## `SecretsProvider` Class
### Properties
- `MailHost`: Retrieves the mail server host configuration.
- `MailPort`: Retrieves the mail server port configuration.
- `MailAddress`: Retrieves the mail server address.
- `MailKey`: Retrieves the mail server key.
- `JwtKey`: Retrieves the JWT key.
- `JwtIssuer`: Retrieves the JWT issuer.
- `JwtAudience`: Retrieves the JWT audience.
- `OpenAiKey`: Retrieves the OpenAI API key.
- `MistralAiKey`: Retrieves the MistralAI API key.

### Methods
- `private SecretsProvider()`: Constructor method that loads configuration sources, such as `secrets.json` file and environment variables, sets up configurations, and displays the retrieved secrets in the console.

### Endpoints
This class does not expose any endpoints as it is used for internal configuration management within the application.

### Data Structures
The data structures used in this class are primarily string values representing various configuration keys and secrets. These values are retrieved from configuration files, environment variables, or Kubernetes secrets.

---
By documenting the `SecretsProvider` class, developers will have a clear understanding of how to access and manage sensitive configurations within the project. This documentation serves as a useful reference for maintaining the security and consistency of configuration data.