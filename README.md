# E-Commerce Backend.

## Start the app
* Run in the root directory: <br>
  ```docker compose up```
* Note: if you are on Windows, it is recommended to run this in WSL.
## Tech stack
* ASP.NET Core(.NET 9).
* Database: PostgreSQL.
* Docker, Docker Compose.

## Features
* Create, delete, and update store locations.
* Create, delete, and update products.
* User authentication and role-based authorization.
* User cart + making orders.
* Minimalistic payment support using Stripe + modify product stock after a purchase.

## Design
* Followed Clean Architecture principles.
* Implemented auth using Identity API.
* Containerized the backend using Docker + set up a development environment using Docker Compose.
* Followed a database-first approach by writing SQL to initialize the database.
* Implemented repository pattern to separate database logic from application logic.
* Set up testing using xUnit and TestContainers.

