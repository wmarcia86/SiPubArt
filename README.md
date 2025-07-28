# SiPubArt ![Project Status](https://img.shields.io/badge/status-in%20development-yellow)

Sistema de Publicación de Artículos

## 📋 Descripción
**SiPubArt** es una plataforma web integral para la publicación y gestión de artículos, diseñada con una arquitectura limpia y moderna. Permite a los usuarios registrarse, autenticarse, gestionar roles, crear, editar, eliminar y comentar artículos. El sistema implementa seguridad robusta con JWT, manejo avanzado de errores, pruebas unitarias y una interfaz web responsiva y amigable.

## ✨ Características Principales
- Registro y autenticación de usuarios con JWT.
- Gestión de roles (Admin y User) y autorización basada en roles.
- CRUD completo de artículos con paginación y filtrado.
- Comentarios en artículos, con validaciones y control de acceso.
- Protección de rutas y acciones según el rol del usuario.
- Consumo de APIs RESTful vía AJAX (jQuery).
- Interfaz responsiva y moderna con Bootstrap 5.
- Documentación interactiva de la API con Swagger/OpenAPI.
- Manejo global de excepciones y logs personalizados.
- Pruebas unitarias con xUnit y Moq.
- Arquitectura limpia (Clean Architecture) y separación de capas.
- Uso de Entity Framework Core con migraciones automáticas.
- Soporte para OData en endpoints de consulta avanzada.

## 🛠 Tecnologías y Herramientas Utilizadas

**Frontend:**
- Razor Pages (.NET 8)
- Bootstrap 5
- jQuery (AJAX & DOM Manipulation)

**Backend:**
- ASP.NET Core (.NET 8)
- Entity Framework Core (SQLite)
- AutoMapper
- MediatR (CQRS & Mediator Pattern)
- JWT Authentication (Bearer Token)
- OData (consultas avanzadas)
- ErrorOr (manejo funcional de errores)
- FluentValidation (validación de datos)
- Middleware personalizado para logs y manejo global de errores
- Swagger / Swashbuckle para documentación de la API

**Testing:**
- xUnit
- Moq

**DevOps y Utilidades:**
- Git & GitHub
- Visual Studio 2022 / .NET CLI
- Migraciones automáticas de base de datos

## ⚙️ Requerimientos

- .NET 8 SDK
- Visual Studio 2022 (o superior) o .NET CLI
- SQLite
- Navegador web moderno
- Git

## 📂 Estructura del Proyecto

- **Application**: Lógica de negocio, CQRS, validaciones y casos de uso.
- **Domain**: Entidades, Value Objects y lógica de dominio.
- **Infrastructure**: Persistencia, servicios externos y configuración de base de datos.
- **UnitTest**: Pruebas unitarias.
- **Web**: Proyecto web principal basado en Razor Pages (.NET 8).
- **Web.API**: Proyecto principal de la API.

## 🚀 Instalación y Ejecución

1. **Clona el repositorio:**
    ```bash
    git clone https://github.com/wmarcia86/SiPubArt.git
    ```
2. **Accede a la carpeta del proyecto:**
    ```bash
    cd SiPubArt
    ```
3. **Abre la solución en Visual Studio:**
    - Inicia **Visual Studio 2022** o superior.
    - Haz clic en **Archivo > Abrir > Solución o proyecto...**
    - Selecciona el archivo `SiPubArt.sln` en la raíz del repositorio.
4. **Configura los proyectos de inicio múltiple:**
    - Haz clic derecho sobre la solución en el **Explorador de soluciones** y selecciona **Propiedades de la solución**.
    - Ve a la sección __Propiedades comunes > Configurar Proyectos de inicio__.
    - Selecciona la opción **Varios proyectos de inicio** (__Multiple startup projects__).
    - Verifica y selecciona el perfil __Startup profile__.
    - Haz clic en **Aceptar** para guardar los cambios.
5. **Restaura las dependencias:**
    - Visual Studio restaurará automáticamente los paquetes al abrir la solución.
    - O bien, puedes ejecutar:
    ```bash
    dotnet restore
    ```
6. **Aplica las migraciones e inicializa la base de datos:**
    - **Opción automática (recomendada):** Al ejecutar la aplicación, las migraciones pendientes se aplican automáticamente y la base de datos se inicializa sin intervención manual.
    - **Opción manual:** Si prefieres hacerlo manualmente antes de ejecutar la aplicación, puedes usar:
    ```bash
    dotnet ef database update
    ```
7. **Ejecuta la aplicación:**
    - Presiona **F5** o haz clic en **Iniciar depuración** en Visual Studio.
    - O bien, desde la terminal:
    ```bash
    dotnet run
    ```
8. **Accede desde tu navegador:**
    ```
    https://localhost:7028
    ```
9. **Explora la documentación de la API:**
    ```
    https://localhost:7241/swagger
    ```

### 🧑‍💻 Usuarios de prueba

- **Administrador**
  - Usuario: `administrator`
  - Contraseña: `adm.2025W`
- **Usuario estándar**
  - Usuario: `wmarcia`
  - Contraseña: `adm.2025A`

## 🧪 Pruebas de OData (Consultas Avanzadas)

Algunos endpoints de la API soportan consultas avanzadas mediante OData. Puedes probar las siguientes opciones agregando los parámetros OData a las URLs de los endpoints que lo soportan (por ejemplo, `/api/articles`):

- **$select**: Selecciona campos específicos.
    ```
    https://localhost:7241/api/articles/odata?$select=title,author
    ```
- **$filter**: Filtra resultados por una condición.
    ```
    https://localhost:7241/api/articles/odata?$filter=author eq 'Wilbert Marcia'
    ```
- **$orderby**: Ordena los resultados.
    ```
    https://localhost:7241/api/articles/odata?$orderby=publicationDate desc
    ```
- **$expand**: Incluye entidades relacionadas (por ejemplo, comentarios).
    ```
    https://localhost:7241/api/articles/odata?$expand=comments
    ```
- **$count**: Devuelve el total de elementos.
    ```
    https://localhost:7241/api/articles/odata?$count=true
    ```
- **$top**: Limita la cantidad máxima de resultados (hasta 100).
    ```
    https://localhost:7241/api/articles/odata?$top=100
    ```

> Puedes combinar varias opciones en una misma consulta, por ejemplo:
> ```
> https://localhost:7241/api/articles/odata?$select=title&$filter=author eq 'Wilbert Marcia'&$orderby=publicationDate desc&$top=10&$count=true
> ```

**Importante:**  
Para consumir estos endpoints, debes incluir un **token JWT** válido en la cabecera de la petición HTTP.

## 👤 Autor
Ing. Wilbert Antonio Marcia Lanzas | Nicaragua | 2025

## ⚠️ Licencia
Este código es privado y no está autorizado para uso, copia, modificación ni distribución por terceros.