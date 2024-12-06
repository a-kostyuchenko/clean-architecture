# Clean Architecture API

A comprehensive .NET 9 Web API project showcasing Clean Architecture principles and best practices.

## 🌟 Project Overview

This project demonstrates a robust implementation of Clean Architecture, providing a scalable and maintainable solution for building modern web APIs. It leverages cutting-edge technologies and design patterns to create a modular, testable, and extensible application.

## 🛠 Technologies Used

- **Language & Framework**: C#, .NET 9
- **ORM**: Entity Framework Core
- **Architectural Patterns**:
    - Clean Architecture
    - CQRS
- **Utilities**:
    - MediatR
    - Mapster
    - Serilog
- **Databases**:
    - PostgreSQL (primary database)
    - Redis (caching)
- **Logging & Monitoring**: Seq
- **Containerization**: Docker

## 📂 Project Structure

The solution is organized into clear, separated layers:

- `src/Domain`: Core domain entities and business logic
- `src/Application`: Application services, commands, and queries
- `src/Infrastructure`: Data access, external service integrations
- `src/Web.API`: RESTful API endpoints
- `src/SharedKernel`: Cross-cutting concerns and shared utilities

## 🚀 Getting Started

### Prerequisites

Ensure you have the following installed:
- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [Docker](https://www.docker.com/get-started)

### Installation & Setup

1. **Clone the Repository**
    ```bash
    git clone https://github.com/a-kostyuchenko/clean-architecture.git
    cd clean-architecture
    ```

2. **Build and Run with Docker**
    ```bash
    docker-compose up --build
    ```

3. **API Access**
    - HTTP: `http://localhost:7070`
    - HTTPS: `https://localhost:7071`

## 🤝 Contributing

Contributions are welcome! Feel free to submit a pull request or open an issue.

## 📄 License

Distributed under the MIT License. See `LICENSE` for more information.

## 🌐 Contact

Email - [kosttchka@gmail.com](mailto:kosttchka@gmail.com)