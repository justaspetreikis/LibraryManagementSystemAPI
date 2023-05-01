# LibraryManagementSystemAPI
This is an API project for a library management system. The project contains several controllers that handle user-related operations, and is connected to a database using Entity Framework.

## Features
- JWT Authentication
- Limited access by Token
- Roles
- Sign-up
- Login
- CRUD methods
- API tests

## Endpoints
HTTP Method | Endpoint | Description
--- | --- | ---
POST | User/signup | Signs up a new user.
POST | User/login | Logs in an existing user.
POST | User/addInformation | Adds information to an existing user.
GET | User | Gets information of an existing user.
PUT | User/image | Updates the profile photo of an existing user.
PUT | User/firstName | Updates the first name of an existing user.
PUT | User/lastName | Updates the last name of an existing user.
PUT | User/phoneNumber | Updates the phone number of an existing user.
PUT | User/email | Updates the email address of an existing user.
PUT | User/personalCode | Updates the personal code of an existing user.
PUT | User/city | Updates the city of an existing user.
PUT | User/street | Updates the street of an existing user.
PUT | User/houseNumber | Updates the house number of an existing user.
PUT | User/flatNumber | Updates the flat number of an existing user.
DELETE | User/delete | Deletes an existing user.

## Getting Started
To get started, clone the repository and open the project in Visual Studio. From there, you can run the API and start testing the endpoints.

## Authors
Justas Petreikis - .Net student
