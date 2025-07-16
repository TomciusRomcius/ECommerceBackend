# E-Commerce Backend.

## Start the app
* Run in the root directory: <br>
  ```docker compose up```
* Note: if you are on Windows, it is recommended to run this in WSL.
## K8 local development
* Run build-images-for-k8.sh(if dotnet installation fails you will have to add dns records to Docker: https://github.com/dotnet/core/issues/8048)

## Tech stack
* Backend: ASP.NET Core(.NET 9).
* Database: PostgreSQL.
* Containerization & Orchestration: Docker, Docker Compose.
* Event streaming: Kafka.

## Features
* Create, delete, and update store locations.
* Create, delete, and update products, categories, and manufacturers.
* User authentication and role-based authorization.
* User cart + making orders.
* Payment support using Stripe.

## Design
* Followed Clean Architecture and CQRS design patterns.
* Implemented auth using the Identity API.
* Containerized the backend using Docker + set up a development environment using Docker Compose.
* Followed a database-first approach to database design.
* Implemented repository pattern to separate database logic from application logic.
* Set up testing using xUnit and TestContainers.
* Built a dedicated payment microservice to improve payment system resiliency, integrating Stripe for transaction processing.
* Architected the payment system to support adding additional payment providers in the future.
* Used MediatR for in-process messaging and separation of concerns, and Kafka for inter-service communication.

