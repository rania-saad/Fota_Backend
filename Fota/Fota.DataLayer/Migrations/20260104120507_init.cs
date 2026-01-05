using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Fota.DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Admins",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedById = table.Column<int>(type: "int", nullable: true),
                    UpdatedById = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admins", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Developers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdentityUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedById = table.Column<int>(type: "int", nullable: true),
                    UpdatedById = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Developers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Subscribers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedById = table.Column<int>(type: "int", nullable: true),
                    UpdatedById = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscribers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Topics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedById = table.Column<int>(type: "int", nullable: true),
                    UpdatedById = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Topics", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Teams",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    LeadId = table.Column<int>(type: "int", nullable: true),
                    CreatedByAdminId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedById = table.Column<int>(type: "int", nullable: true),
                    UpdatedById = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teams", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Teams_Admins_CreatedByAdminId",
                        column: x => x.CreatedByAdminId,
                        principalTable: "Admins",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Teams_Developers_LeadId",
                        column: x => x.LeadId,
                        principalTable: "Developers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "BaseMessages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MessageType = table.Column<int>(type: "int", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    HexFileContent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HexFileName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Version = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Status = table.Column<int>(type: "int", maxLength: 50, nullable: false),
                    ApprovedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PublishedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RejectedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RejectionReason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    TopicId = table.Column<int>(type: "int", nullable: false),
                    UploaderId = table.Column<int>(type: "int", nullable: false),
                    PublisherId = table.Column<int>(type: "int", nullable: true),
                    ApprovedById = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedById = table.Column<int>(type: "int", nullable: true),
                    UpdatedById = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaseMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BaseMessages_Developers_PublisherId",
                        column: x => x.PublisherId,
                        principalTable: "Developers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BaseMessages_Developers_UploaderId",
                        column: x => x.UploaderId,
                        principalTable: "Developers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BaseMessages_Topics_TopicId",
                        column: x => x.TopicId,
                        principalTable: "Topics",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Diagnostics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Priority = table.Column<int>(type: "int", maxLength: 50, nullable: false),
                    Status = table.Column<int>(type: "int", maxLength: 50, nullable: false),
                    ResolvedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ClosedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SubscriberId = table.Column<int>(type: "int", nullable: false),
                    TopicId = table.Column<int>(type: "int", nullable: true),
                    AssignedByAdminId = table.Column<int>(type: "int", nullable: true),
                    AssignedToDeveloperId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedById = table.Column<int>(type: "int", nullable: true),
                    UpdatedById = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Diagnostics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Diagnostics_Admins_AssignedByAdminId",
                        column: x => x.AssignedByAdminId,
                        principalTable: "Admins",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Diagnostics_Developers_AssignedToDeveloperId",
                        column: x => x.AssignedToDeveloperId,
                        principalTable: "Developers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Diagnostics_Subscribers_SubscriberId",
                        column: x => x.SubscriberId,
                        principalTable: "Subscribers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Diagnostics_Topics_TopicId",
                        column: x => x.TopicId,
                        principalTable: "Topics",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TopicSubscribers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TopicId = table.Column<int>(type: "int", nullable: false),
                    SubscriberId = table.Column<int>(type: "int", nullable: false),
                    SubscribedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UnsubscribedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedById = table.Column<int>(type: "int", nullable: true),
                    UpdatedById = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TopicSubscribers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TopicSubscribers_Subscribers_SubscriberId",
                        column: x => x.SubscriberId,
                        principalTable: "Subscribers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TopicSubscribers_Topics_TopicId",
                        column: x => x.TopicId,
                        principalTable: "Topics",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TeamDevelopers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TeamId = table.Column<int>(type: "int", nullable: false),
                    DeveloperId = table.Column<int>(type: "int", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    JoinedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LeftAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedById = table.Column<int>(type: "int", nullable: true),
                    UpdatedById = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamDevelopers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TeamDevelopers_Developers_DeveloperId",
                        column: x => x.DeveloperId,
                        principalTable: "Developers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TeamDevelopers_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TeamTopics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TeamId = table.Column<int>(type: "int", nullable: false),
                    TopicId = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedById = table.Column<int>(type: "int", nullable: true),
                    UpdatedById = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamTopics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TeamTopics_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TeamTopics_Topics_TopicId",
                        column: x => x.TopicId,
                        principalTable: "Topics",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MessageDeliveries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MessageId = table.Column<int>(type: "int", nullable: false),
                    SubscriberId = table.Column<int>(type: "int", nullable: false),
                    DeliveredAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReadAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDelivered = table.Column<bool>(type: "bit", nullable: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    IsFailed = table.Column<bool>(type: "bit", nullable: false),
                    FailureReason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedById = table.Column<int>(type: "int", nullable: true),
                    UpdatedById = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessageDeliveries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MessageDeliveries_BaseMessages_MessageId",
                        column: x => x.MessageId,
                        principalTable: "BaseMessages",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MessageDeliveries_Subscribers_SubscriberId",
                        column: x => x.SubscriberId,
                        principalTable: "Subscribers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DiagnosticSolutions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DiagnosticId = table.Column<int>(type: "int", nullable: false),
                    DeveloperId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    HexFileContent = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    HexFileName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Version = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ApprovedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RejectedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RejectionReason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedById = table.Column<int>(type: "int", nullable: true),
                    UpdatedById = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiagnosticSolutions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DiagnosticSolutions_Developers_DeveloperId",
                        column: x => x.DeveloperId,
                        principalTable: "Developers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DiagnosticSolutions_Diagnostics_DiagnosticId",
                        column: x => x.DiagnosticId,
                        principalTable: "Diagnostics",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "Admins",
                columns: new[] { "Id", "CreatedAt", "CreatedById", "Email", "IsActive", "Name", "PhoneNumber", "UpdatedAt", "UpdatedById" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 1, 15, 10, 0, 0, 0, DateTimeKind.Unspecified), null, "sarah.johnson@fotasystem.com", true, "Sarah Johnson", "+201234567890", null, null },
                    { 2, new DateTime(2024, 1, 15, 10, 0, 0, 0, DateTimeKind.Unspecified), null, "ahmed.hassan@fotasystem.com", true, "Ahmed Hassan", "+201234567891", null, null }
                });

            migrationBuilder.InsertData(
                table: "Developers",
                columns: new[] { "Id", "CreatedAt", "CreatedById", "Email", "IdentityUserId", "IsActive", "Name", "PhoneNumber", "UpdatedAt", "UpdatedById" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 2, 1, 9, 0, 0, 0, DateTimeKind.Unspecified), null, "mohamed.ali@fotasystem.com", null, true, "Mohamed Ali", "+201234567892", null, null },
                    { 2, new DateTime(2024, 2, 1, 9, 0, 0, 0, DateTimeKind.Unspecified), null, "fatima.nour@fotasystem.com", null, true, "Fatima Nour", "+201234567893", null, null },
                    { 3, new DateTime(2024, 2, 5, 9, 0, 0, 0, DateTimeKind.Unspecified), null, "omar.khaled@fotasystem.com", null, true, "Omar Khaled", "+201234567894", null, null }
                });

            migrationBuilder.InsertData(
                table: "Subscribers",
                columns: new[] { "Id", "CreatedAt", "CreatedById", "Email", "IsActive", "Name", "PhoneNumber", "UpdatedAt", "UpdatedById" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 4, 1, 8, 0, 0, 0, DateTimeKind.Unspecified), null, "fleet.cairo@toyota-eg.com", true, "Toyota Fleet - Cairo", "+201234567895", null, null },
                    { 2, new DateTime(2024, 4, 1, 8, 0, 0, 0, DateTimeKind.Unspecified), null, "test.center@bmw-eg.com", true, "BMW Test Center", "+201234567896", null, null }
                });

            migrationBuilder.InsertData(
                table: "Topics",
                columns: new[] { "Id", "CreatedAt", "CreatedById", "Description", "IsActive", "IsDeleted", "Name", "UpdatedAt", "UpdatedById" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 3, 1, 8, 0, 0, 0, DateTimeKind.Unspecified), null, "ECU firmware updates and diagnostics", true, false, "Engine Control Unit", null, null },
                    { 2, new DateTime(2024, 3, 1, 8, 0, 0, 0, DateTimeKind.Unspecified), null, "BMS firmware for electric vehicles", true, false, "Battery Management System", null, null },
                    { 3, new DateTime(2024, 3, 1, 8, 0, 0, 0, DateTimeKind.Unspecified), null, "Navigation and multimedia updates", true, false, "Infotainment System", null, null }
                });

            migrationBuilder.InsertData(
                table: "BaseMessages",
                columns: new[] { "Id", "ApprovedAt", "ApprovedById", "CreatedAt", "CreatedById", "Description", "HexFileContent", "HexFileName", "IsDeleted", "MessageType", "PublishedAt", "PublisherId", "RejectedAt", "RejectionReason", "Status", "TopicId", "UpdatedAt", "UpdatedById", "UploaderId", "Version" },
                values: new object[,]
                {
                    { 1, null, null, new DateTime(2024, 5, 10, 10, 0, 0, 0, DateTimeKind.Unspecified), null, "ECU Firmware Update v2.3.1 - Performance improvements", null, "ecu_v2.3.1.hex", false, 0, new DateTime(2024, 5, 10, 14, 30, 0, 0, DateTimeKind.Unspecified), 1, null, null, 3, 1, null, null, 1, "2.3.1" },
                    { 2, new DateTime(2024, 5, 15, 11, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(2024, 5, 15, 9, 0, 0, 0, DateTimeKind.Unspecified), null, "BMS Critical Security Patch v1.8.2", null, "bms_v1.8.2_security.hex", false, 0, null, null, null, null, 2, 2, null, null, 2, "1.8.2" }
                });

            migrationBuilder.InsertData(
                table: "Diagnostics",
                columns: new[] { "Id", "AssignedByAdminId", "AssignedToDeveloperId", "ClosedAt", "CreatedAt", "CreatedById", "Description", "Priority", "ResolvedAt", "Status", "SubscriberId", "Title", "TopicId", "UpdatedAt", "UpdatedById" },
                values: new object[] { 1, 1, 2, null, new DateTime(2024, 5, 20, 8, 30, 0, 0, DateTimeKind.Unspecified), null, "Battery pack temperature exceeding normal range in test vehicle VIN:ABC123", 0, null, 1, 2, "Battery Temperature Warning", 2, null, null });

            migrationBuilder.InsertData(
                table: "Teams",
                columns: new[] { "Id", "CreatedAt", "CreatedByAdminId", "CreatedById", "Description", "IsActive", "LeadId", "Name", "UpdatedAt", "UpdatedById" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 3, 10, 10, 0, 0, 0, DateTimeKind.Unspecified), 1, null, "Responsible for engine control updates", true, 1, "ECU Development Team", null, null },
                    { 2, new DateTime(2024, 3, 10, 10, 0, 0, 0, DateTimeKind.Unspecified), 1, null, "Handles BMS firmware development", true, 2, "Battery Systems Team", null, null }
                });

            migrationBuilder.InsertData(
                table: "TopicSubscribers",
                columns: new[] { "Id", "CreatedAt", "CreatedById", "IsActive", "SubscribedAt", "SubscriberId", "TopicId", "UnsubscribedAt", "UpdatedAt", "UpdatedById" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 4, 5, 9, 0, 0, 0, DateTimeKind.Unspecified), null, true, new DateTime(2024, 4, 5, 9, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, null, null, null },
                    { 2, new DateTime(2024, 4, 5, 9, 0, 0, 0, DateTimeKind.Unspecified), null, true, new DateTime(2024, 4, 5, 9, 0, 0, 0, DateTimeKind.Unspecified), 2, 2, null, null, null }
                });

            migrationBuilder.InsertData(
                table: "MessageDeliveries",
                columns: new[] { "Id", "CreatedAt", "CreatedById", "DeliveredAt", "FailureReason", "IsDelivered", "IsFailed", "IsRead", "MessageId", "ReadAt", "SubscriberId", "UpdatedAt", "UpdatedById" },
                values: new object[] { 1, new DateTime(2024, 5, 10, 14, 30, 0, 0, DateTimeKind.Unspecified), null, new DateTime(2024, 5, 10, 15, 0, 0, 0, DateTimeKind.Unspecified), null, true, false, true, 1, new DateTime(2024, 5, 10, 16, 30, 0, 0, DateTimeKind.Unspecified), 1, null, null });

            migrationBuilder.InsertData(
                table: "TeamDevelopers",
                columns: new[] { "Id", "CreatedAt", "CreatedById", "DeveloperId", "IsActive", "JoinedAt", "LeftAt", "Role", "TeamId", "UpdatedAt", "UpdatedById" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 3, 10, 10, 0, 0, 0, DateTimeKind.Unspecified), null, 1, true, new DateTime(2024, 3, 10, 10, 0, 0, 0, DateTimeKind.Unspecified), null, "Lead", 1, null, null },
                    { 2, new DateTime(2024, 3, 12, 9, 0, 0, 0, DateTimeKind.Unspecified), null, 3, true, new DateTime(2024, 3, 12, 9, 0, 0, 0, DateTimeKind.Unspecified), null, "Senior", 1, null, null },
                    { 3, new DateTime(2024, 3, 10, 10, 0, 0, 0, DateTimeKind.Unspecified), null, 2, true, new DateTime(2024, 3, 10, 10, 0, 0, 0, DateTimeKind.Unspecified), null, "Lead", 2, null, null }
                });

            migrationBuilder.InsertData(
                table: "TeamTopics",
                columns: new[] { "Id", "CreatedAt", "CreatedById", "IsActive", "TeamId", "TopicId", "UpdatedAt", "UpdatedById" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 3, 10, 10, 30, 0, 0, DateTimeKind.Unspecified), null, true, 1, 1, null, null },
                    { 2, new DateTime(2024, 3, 10, 10, 30, 0, 0, DateTimeKind.Unspecified), null, true, 2, 2, null, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Admins_Email",
                table: "Admins",
                column: "Email",
                unique: true,
                filter: "[Email] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_BaseMessages_PublisherId",
                table: "BaseMessages",
                column: "PublisherId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseMessages_TopicId",
                table: "BaseMessages",
                column: "TopicId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseMessages_UploaderId",
                table: "BaseMessages",
                column: "UploaderId");

            migrationBuilder.CreateIndex(
                name: "IX_Developers_Email",
                table: "Developers",
                column: "Email",
                unique: true,
                filter: "[Email] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Diagnostics_AssignedByAdminId",
                table: "Diagnostics",
                column: "AssignedByAdminId");

            migrationBuilder.CreateIndex(
                name: "IX_Diagnostics_AssignedToDeveloperId",
                table: "Diagnostics",
                column: "AssignedToDeveloperId");

            migrationBuilder.CreateIndex(
                name: "IX_Diagnostics_SubscriberId",
                table: "Diagnostics",
                column: "SubscriberId");

            migrationBuilder.CreateIndex(
                name: "IX_Diagnostics_TopicId",
                table: "Diagnostics",
                column: "TopicId");

            migrationBuilder.CreateIndex(
                name: "IX_DiagnosticSolutions_DeveloperId",
                table: "DiagnosticSolutions",
                column: "DeveloperId");

            migrationBuilder.CreateIndex(
                name: "IX_DiagnosticSolutions_DiagnosticId",
                table: "DiagnosticSolutions",
                column: "DiagnosticId");

            migrationBuilder.CreateIndex(
                name: "IX_MessageDeliveries_MessageId",
                table: "MessageDeliveries",
                column: "MessageId");

            migrationBuilder.CreateIndex(
                name: "IX_MessageDeliveries_SubscriberId",
                table: "MessageDeliveries",
                column: "SubscriberId");

            migrationBuilder.CreateIndex(
                name: "IX_Subscribers_Email",
                table: "Subscribers",
                column: "Email",
                unique: true,
                filter: "[Email] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_TeamDevelopers_DeveloperId",
                table: "TeamDevelopers",
                column: "DeveloperId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamDevelopers_TeamId",
                table: "TeamDevelopers",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Teams_CreatedByAdminId",
                table: "Teams",
                column: "CreatedByAdminId");

            migrationBuilder.CreateIndex(
                name: "IX_Teams_LeadId",
                table: "Teams",
                column: "LeadId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamTopics_TeamId",
                table: "TeamTopics",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamTopics_TopicId",
                table: "TeamTopics",
                column: "TopicId");

            migrationBuilder.CreateIndex(
                name: "IX_Topics_Name",
                table: "Topics",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TopicSubscribers_SubscriberId",
                table: "TopicSubscribers",
                column: "SubscriberId");

            migrationBuilder.CreateIndex(
                name: "IX_TopicSubscribers_TopicId",
                table: "TopicSubscribers",
                column: "TopicId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "DiagnosticSolutions");

            migrationBuilder.DropTable(
                name: "MessageDeliveries");

            migrationBuilder.DropTable(
                name: "TeamDevelopers");

            migrationBuilder.DropTable(
                name: "TeamTopics");

            migrationBuilder.DropTable(
                name: "TopicSubscribers");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Diagnostics");

            migrationBuilder.DropTable(
                name: "BaseMessages");

            migrationBuilder.DropTable(
                name: "Teams");

            migrationBuilder.DropTable(
                name: "Subscribers");

            migrationBuilder.DropTable(
                name: "Topics");

            migrationBuilder.DropTable(
                name: "Admins");

            migrationBuilder.DropTable(
                name: "Developers");
        }
    }
}
