# ASP.NET Core Microservice Web API Portfolio Project

This repository contains an ASP.NET Core Microservice Web API Portfolio Project.

## Tech Stack
- .NET 7
- gRPC
- RabbitMQ
- EFCore
- PostgreSQL
- Redis
- MediatR

## Features
- Permissions based JWT Authentication
- PostgreSQL support
- Redis support
- Generic Responses
- RabbitMQ Messaging
- Repository Pattern
- Scoped ApiEndpoints
- Microservices architecture with code-first gRPC
- Cached gRPC responses
- API Versionning

## Requirements
 - .NET 7 SDK
 - Docker
 - Docker Compose

## Installation
 1. Clone the repository.
 2. Run `docker-compose up -d` to start the required services (PostgreSQL, Redis, and RabbitMQ).
 2. Run `dotnet restore` & `dotnet run` to start the ASP.NET Core Web API.

## Configuration
Configuration can be found in the `appsettings.json` file.

## Contributing
We welcome contributions to this project! To get started, follow these steps:

1. Fork the repository.
2. Create a new branch for your feature or bug fix: `git checkout -b my-feature-branch`
3. Make your changes and commit them: `git commit -m "Add a new feature"`
4. Push your changes to your fork: `git push origin my-feature-branch`
5. Open a pull request on the original repository.

## License

This project is licensed under the MIT License. See the `LICENSE` file for details.
