using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    FirstName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Username = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "TEXT", maxLength: 254, nullable: false),
                    Password = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false),
                    Role = table.Column<string>(type: "TEXT", maxLength: 5, nullable: false),
                    Active = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Articles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Content = table.Column<string>(type: "TEXT", maxLength: 10000, nullable: false),
                    AuthorId = table.Column<Guid>(type: "TEXT", nullable: false),
                    PublicationDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Articles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Articles_Users_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    ArticleId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Content = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: false),
                    AuthorId = table.Column<Guid>(type: "TEXT", nullable: false),
                    PublicationDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comments_Articles_ArticleId",
                        column: x => x.ArticleId,
                        principalTable: "Articles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Comments_Users_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Articles_AuthorId",
                table: "Articles",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ArticleId",
                table: "Comments",
                column: "ArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_AuthorId",
                table: "Comments",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);

            migrationBuilder.Sql(@"
                INSERT INTO Users (
                    Id,
                    FirstName,
                    LastName,
                    Username,
                    Email,
                    Password,
                    Role,
                    Active
                ) VALUES (
                    '369CF847-C815-43A7-83E5-48E0FD7E1CF6',
                    'Admin',
                    'User',
                    'administrator',
                    'admin@localhost.com',
                    'PBKDF2$100000$BjopTEv6EhkX/WGNek4ijQ==$KAxtSB5aKZ+IqVJ+SnfwjoV3TTwbodR84hCFAqf1Nzk=',
                    'Admin',
                    1
                );
            ");

            migrationBuilder.Sql(@"
                INSERT INTO Users (
                    Id,
                    FirstName,
                    LastName,
                    Username,
                    Email,
                    Password,
                    Role,
                    Active
                ) VALUES (
                    '0D5C2B6E-E8C0-4296-92F6-584993773850',
                    'Wilbert',
                    'Marcia',
                    'wmarcia',
                    'test@gmail.com',
                    'PBKDF2$100000$38vC5QqTmVbFhVmH7MtBjw==$d/+DFxrjriIXPWApnRiTbc9PeH204SlCndelzDiko4g=',
                    'User',
                    1
                );
            ");

            migrationBuilder.Sql(@"
                INSERT INTO Articles (
                    Id,
                    Title,
                    Content,
                    AuthorId,
                    PublicationDate
                ) VALUES (
                    '47AC7793-E471-4329-AE8C-C9745F0B8BF0',
                    '.Net 8',
                    '.NET 8 es una versión del framework multiplataforma de Microsoft, diseñado para desarrollar aplicaciones web, de escritorio, móviles y basadas en la nube. Ofrece mejoras significativas en rendimiento, seguridad y desarrollo de aplicaciones, incluyendo soporte para C# 12 y compilación nativa AOT. También proporciona herramientas para la creación de aplicaciones nativas de la nube con .NET Aspire y mejoras en la integración con contenedores y Kubernetes. 

            Características principales de .NET 8:
            - Rendimiento mejorado:
            .NET 8 introduce optimizaciones en el compilador Just-In-Time, la recolección de basura y la asignación de memoria, lo que resulta en tiempos de arranque más rápidos y un mejor rendimiento general. 

            - Desarrollo de aplicaciones:
            Se enfoca en mejorar la autenticación y autorización en ASP.NET Core, con nuevas funcionalidades en Blazor y mejoras en SignalR y APIs RESTful. 

            - Desarrollo en la nube:
            .NET 8 incluye .NET Aspire para facilitar la creación de aplicaciones nativas en la nube, con énfasis en escalabilidad, resiliencia y observabilidad. 

            - Compilación nativa AOT:
            Permite una compilación anticipada del código, lo que mejora el rendimiento y reduce el uso de memoria, logrando un inicio de aplicación casi instantáneo. 

            - Soporte para contenedores y Kubernetes:
            Facilita la creación y gestión de aplicaciones .NET en contenedores e integra mejoras para el despliegue y gestión en Kubernetes. 

            - C# 12:
            .NET 8 incorpora la última versión del lenguaje C#, con mejoras en tipos de referencia, patrones y sintaxis. 

            - Soporte a largo plazo (LTS):
            .NET 8 es una versión LTS, lo que significa que tendrá soporte durante tres años a partir de su lanzamiento, brindando estabilidad a los proyectos.',
                    '369CF847-C815-43A7-83E5-48E0FD7E1CF6',
                    '2025-07-27 22:00:00'
                );
            ");

            migrationBuilder.Sql(@"
                INSERT INTO Articles (
                    Id,
                    Title,
                    Content,
                    AuthorId,
                    PublicationDate
                ) VALUES (
                    'FF4D8C83-5FEB-481B-B7F9-EF0B56A788C2',
                    '.Net 9',
                    '.NET 9 es una versión del framework multiplataforma de código abierto de Microsoft, diseñado para desarrollar aplicaciones web, móviles, de escritorio, IoT y en la nube. Se enfoca en mejorar el rendimiento, la seguridad y la facilidad de desarrollo, ofreciendo nuevas funcionalidades y actualizaciones en bibliotecas y herramientas.',
                    '0D5C2B6E-E8C0-4296-92F6-584993773850',
                    '2025-07-28 06:11:54.3463231'
                );
            ");

            migrationBuilder.Sql(@"
                INSERT INTO Comments (
                    Id,
                    ArticleId,
                    Content,
                    AuthorId,
                    PublicationDate
                ) VALUES (
                    '5353E5B5-8E97-4603-A9B4-035112D0E985',
                    '47AC7793-E471-4329-AE8C-C9745F0B8BF0',
                    'Este articulo es interesante.
            muestra de una forma resumida que es .net 8',
                    '369CF847-C815-43A7-83E5-48E0FD7E1CF6',
                    '2025-07-28 05:53:40.6450677'
                );
            ");

            migrationBuilder.Sql(@"
                INSERT INTO Comments (
                    Id,
                    ArticleId,
                    Content,
                    AuthorId,
                    PublicationDate
                ) VALUES (
                    '97EF6D6F-24D9-4617-A0C3-3D6CF08692BC',
                    '47AC7793-E471-4329-AE8C-C9745F0B8BF0',
                    'Articulo excelente.',
                    '0D5C2B6E-E8C0-4296-92F6-584993773850',
                    '2025-07-28 10:00:00'
                );
            ");

            migrationBuilder.Sql(@"
                INSERT INTO Comments (
                    Id,
                    ArticleId,
                    Content,
                    AuthorId,
                    PublicationDate
                ) VALUES (
                    '4186CF56-ABA3-41D2-A685-FD7561AE3FCE',
                    'FF4D8C83-5FEB-481B-B7F9-EF0B56A788C2',
                    'Este articulo todavía esta incompleto',
                    '0D5C2B6E-E8C0-4296-92F6-584993773850',
                    '2025-07-28 06:13:34.2925966'
                );
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "Articles");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
