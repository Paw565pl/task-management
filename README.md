# Task Management

This is a robust and flexible RESTful API designed for comprehensive project and task management. Built with C# and ASP.NET Core, it offers a solid foundation for organizing and tracking work efficiently. This project was primarily created for educational purposes, serving as a practical demonstration of modern web API development principles, including secure authentication, data persistence, and containerization.

## Technologies Used

- **C#**: The versatile and powerful programming language at the core of the API.
- **ASP.NET**: A high-performance, open-source framework for building modern, cloud-enabled, Internet-connected applications.
- **Keycloak**: An open-source Identity and Access Management solution, providing robust authentication and authorization services via OAuth2.
- **Mapperly**: A compile-time source generator for fast and type-safe object mapping in C#.
- **PostgreSQL**: A powerful, open-source object-relational database system known for its reliability, feature robustness, and performance.
- **Docker**: A platform for developing, shipping, and running applications in containers, ensuring environmental consistency.
- **Scalar**: A modern and intuitive tool for generating interactive API documentation.

## How to run it locally?

It is fairly simple thanks to docker. Simply run this command after cloning the repository.

```sh
docker compose -f docker-compose.dev.yml up --build --watch
```

That's all! Now simply hit [localhost:5000](http://localhost:5000) and explore available endpoints. API is documented on this URL [localhost:5000/scalar](http://localhost:5000/scalar).
