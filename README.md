
# 🏗️ Task API

[![.NET](https://img.shields.io/badge/.NET-8-blue?style=for-the-badge&logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/) [![C#](https://img.shields.io/badge/C%23-9.0-blue?style=for-the-badge&logo=c-sharp&logoColor=white)](https://learn.microsoft.com/dotnet/csharp/) [![Entity Framework](https://img.shields.io/badge/Entity_Framework-Core-2C3E50?style=for-the-badge&logo=entity-framework&logoColor=white)](https://learn.microsoft.com/ef/) [![SQL Server](https://img.shields.io/badge/SQL_Server-CC2927?style=for-the-badge&logo=microsoft-sql-server&logoColor=white)](https://www.microsoft.com/en-us/sql-server) [![Swagger](https://img.shields.io/badge/Swagger-85EA2D?style=for-the-badge&logo=swagger&logoColor=white)](https://swagger.io/) [![JWT](https://img.shields.io/badge/JWT-000000?style=for-the-badge&logo=json-web-tokens&logoColor=white)](https://jwt.io/) [![Dependency Injection](https://img.shields.io/badge/Dependency_Injection-FF69B4?style=for-the-badge)](https://learn.microsoft.com/dotnet/core/extensions/dependency-injection) [![SOLID](https://img.shields.io/badge/SOLID-Principles-FFA500?style=for-the-badge)](https://en.wikipedia.org/wiki/SOLID) [![DDD](https://img.shields.io/badge/DDD-Domain_Driven_Design-4B0082?style=for-the-badge)](https://en.wikipedia.org/wiki/Domain-driven_design) [![xUnit](https://img.shields.io/badge/xUnit-8A2BE2?style=for-the-badge&logo=xunit&logoColor=white)](https://xunit.net/) [![Dapper](https://img.shields.io/badge/Dapper-FF4500?style=for-the-badge)](https://github.com/DapperLib/Dapper) [![FluentValidation](https://img.shields.io/badge/Fluent_Validation-20B2AA?style=for-the-badge)](https://fluentvalidation.net/) [![BCrypt](https://img.shields.io/badge/Bcrypt-008000?style=for-the-badge)](https://github.com/BcryptNet/bcrypt.net) [![FluentMigrator](https://img.shields.io/badge/Fluent_Migrator-4682B4?style=for-the-badge)](https://fluentmigrator.github.io/)

## ✨ Sobre

**Task API** é sua assistente flexível para gerenciar tarefas de forma rápida e eficiente. Ela entende sua rotina, permitindo criar, atualizar, consultar e remover tarefas sem complicação. Ideal para automatizar fluxos e nunca perder um prazo!  

## 🛠️ Funcionalidades

- CRUD completo de tarefas  
- Filtros e busca rápida  
- Status e prioridades personalizáveis  
- Estrutura pronta para testes automatizados  
- Documentação via Swagger/OpenAPI  
- Fácil de rodar localmente ou em Docker  

## 🚀 Endpoints principais

- `GET /tasks` – Lista todas as tarefas  
- `GET /tasks/{id}` – Consulta uma tarefa específica  
- `POST /tasks` – Cria uma nova tarefa  
- `PUT /tasks/{id}` – Atualiza uma tarefa existente  
- `DELETE /tasks/{id}` – Remove uma tarefa  

## ⚡ Como rodar

### Usando .NET CLI
```bash
git clone https://github.com/asafeCode/Api-Tarefas.git
cd src/Backend/Template.API
dotnet run


Abra no navegador: `http://localhost:5000/swagger`
```
### Usando Docker

```bash
docker build -t task-api .
docker run -d -p 5000:8080 --name task-api task-api
```
### 📄 Licença
- Projeto open source — use, adapte e aproveite à vontade!
