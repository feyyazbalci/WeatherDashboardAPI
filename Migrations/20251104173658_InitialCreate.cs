using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WeatherDashboardAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Country = table.Column<string>(type: "TEXT", maxLength: 2, nullable: false),
                    Latitude = table.Column<double>(type: "REAL", nullable: false),
                    Longitude = table.Column<double>(type: "REAL", nullable: false),
                    StateCode = table.Column<string>(type: "TEXT", nullable: true),
                    OpenWeatherMapId = table.Column<int>(type: "INTEGER", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Username = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastLoginAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WeatherAlerts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CityId = table.Column<int>(type: "INTEGER", nullable: false),
                    AlertType = table.Column<int>(type: "INTEGER", nullable: false),
                    Message = table.Column<string>(type: "TEXT", nullable: false),
                    Severity = table.Column<int>(type: "INTEGER", nullable: false),
                    TriggerValue = table.Column<double>(type: "REAL", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeatherAlerts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WeatherAlerts_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WeatherRecords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CityId = table.Column<int>(type: "INTEGER", nullable: false),
                    Temperature = table.Column<double>(type: "REAL", nullable: false),
                    FeelsLike = table.Column<double>(type: "REAL", nullable: false),
                    TempMin = table.Column<double>(type: "REAL", nullable: false),
                    TempMax = table.Column<double>(type: "REAL", nullable: false),
                    Humidity = table.Column<int>(type: "INTEGER", nullable: false),
                    Pressure = table.Column<int>(type: "INTEGER", nullable: false),
                    WindSpeed = table.Column<double>(type: "REAL", nullable: false),
                    WindDegree = table.Column<int>(type: "INTEGER", nullable: true),
                    Cloudiness = table.Column<int>(type: "INTEGER", nullable: true),
                    Rainfall = table.Column<double>(type: "REAL", nullable: true),
                    Snowfall = table.Column<double>(type: "REAL", nullable: true),
                    WeatherMain = table.Column<string>(type: "TEXT", nullable: false),
                    WeatherDescription = table.Column<string>(type: "TEXT", nullable: false),
                    WeatherIcon = table.Column<string>(type: "TEXT", nullable: true),
                    RecordedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeatherRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WeatherRecords_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserFavoriteCities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    CityId = table.Column<int>(type: "INTEGER", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserFavoriteCities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserFavoriteCities_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserFavoriteCities_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Cities",
                columns: new[] { "Id", "Country", "CreatedAt", "Latitude", "Longitude", "Name", "OpenWeatherMapId", "StateCode" },
                values: new object[,]
                {
                    { 1, "TR", new DateTime(2025, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), 41.008200000000002, 28.978400000000001, "Istanbul", 745044, null },
                    { 2, "TR", new DateTime(2025, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), 39.933399999999999, 32.859699999999997, "Ankara", 323786, null },
                    { 3, "TR", new DateTime(2025, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), 38.423699999999997, 27.142800000000001, "Izmir", 311046, null },
                    { 4, "GB", new DateTime(2025, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), 51.507399999999997, -0.1278, "London", 2643743, null },
                    { 5, "US", new DateTime(2025, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), 40.712800000000001, -74.006, "New York", 5128581, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cities_Name_Country",
                table: "Cities",
                columns: new[] { "Name", "Country" });

            migrationBuilder.CreateIndex(
                name: "IX_UserFavoriteCities_CityId",
                table: "UserFavoriteCities",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_UserFavoriteCities_UserId_CityId",
                table: "UserFavoriteCities",
                columns: new[] { "UserId", "CityId" },
                unique: true);

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

            migrationBuilder.CreateIndex(
                name: "IX_WeatherAlerts_CityId",
                table: "WeatherAlerts",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_WeatherAlerts_CreatedAt",
                table: "WeatherAlerts",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_WeatherAlerts_IsActive",
                table: "WeatherAlerts",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_WeatherRecords_CityId_RecordedAt",
                table: "WeatherRecords",
                columns: new[] { "CityId", "RecordedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_WeatherRecords_RecordedAt",
                table: "WeatherRecords",
                column: "RecordedAt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserFavoriteCities");

            migrationBuilder.DropTable(
                name: "WeatherAlerts");

            migrationBuilder.DropTable(
                name: "WeatherRecords");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Cities");
        }
    }
}
