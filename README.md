# **Technical Architecture and Technology Stack Documentation**

## **Microservice Architecture based on Domain-Driven Design (DDD)**

The project is designed as a collection of independent **Microservices**, where each service covers a specific **business domain**.  
At the same time, **DDD (Domain-Driven Design)** principles are applied for domain modeling to ensure **scalability**, **loose coupling**, and **separation of concerns**.

---

## **Service Segmentation**

The services **Catalog**, **FileServer**, and **Identity** are implemented as independent **ASP.NET Core applications**.  
Each service has its own **dedicated configuration**, including port, **Dependency Injection (DI)** setup, and isolated deployment environment — allowing **independent deployment** and **scaling**.

---

## **Architectural and Design Patterns**

### **Layered Architecture (Domain / Application / Infrastructure)**

- The **Application** layer is registered using the `AddApplication()` method.  
- The **Infrastructure** layer is added via the `AddInfrastructure()` method.  
- Project references follow this layering to ensure **inversion of dependency direction** from the domain toward the infrastructure.

### **CQRS & Mediator Pattern**

- **Requests** and **Handlers** (via **MediatR**) — such as `GetCategoryListQuery` — separate the **read** and **write** operations.  
- **MediatR** is registered in the **Application layer Startup**, implementing a **message-driven** and **decoupled command/query flow**.

### **Specification & Repository Pattern**

- The generic repository `ApplicationDbRepository` is built on top of **Ardalis.Specification** to enable **composable queries** (e.g., `CategoryByNameSpec`).  
- Data mapping between **DTOs** and **Entities** is handled by **Mapster**.

### **Domain Events**

- The base class `AggregateRoot` contains a collection of **Domain Events**, implementing the **Domain Events pattern** within **Aggregates**.

---

## **Frameworks and Key Libraries**

### **ASP.NET Core (.NET 6 / .NET 7)**

- **Catalog** and **Identity** services are built on **.NET 6.0**, while **FileServer** runs on **.NET 7.0**.  
- Each service runs as an **independent ASP.NET Core web host**, allowing isolated runtime and deployment.

### **Dapr**

Each service initializes a **Dapr client** to handle **service invocation** and **inter-service communication** through the **sidecar** pattern.

### **Entity Framework Core**

Data access in **Catalog** and **Identity** services is implemented using **Entity Framework Core** with **DbContexts** such as `CatalogContext` and `IdentityDbContext`.

### **JWT Bearer Authentication**

Authentication across services is **token-based** using **JWT (JSON Web Token)**.  
In the **Identity** service, token key and expiration are configured, while the **Catalog** service activates the **JWT middleware**.

### **Mapster**

**Mapster** is used for mapping between **DTOs** and **Entities**.  
Configuration is defined in the **Infrastructure layer**.

### **Ardalis.Specification**

The **Specification Pattern** and **Generic Repository** are implemented using **Ardalis.Specification**, allowing flexible and reusable query definitions.

### **MediatR**

**MediatR** is registered in both **Application** and **Infrastructure** layers to implement the **CQRS pattern** and separate the **request/response** logic.

### **FluentValidation**

Request validation rules are defined using **FluentValidation**, for example, `CreateCategoryValidator` is used when creating a new category.

### **gRPC**

The **Identity** service implements a **gRPC service** named `Permission`, which is based on a shared **.proto** file.  
The corresponding `Service` class is defined and implemented within the same service.

### **StackExchange.Redis (Ready for Distributed Cache)**

The **Redis package** is integrated into the **Identity** service and configured for use as a **distributed caching layer**.

### **File Storage Infrastructure**

The **Catalog** service uses a `LocalFileStorageService` (based on **HttpClient**) to communicate with the **FileServer** through **Dapr**.  
The **FileServer** manages **file persistence** on the local disk.
