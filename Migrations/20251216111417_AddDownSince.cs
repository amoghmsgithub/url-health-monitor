using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UrlHealthMonitor.Migrations
{
    /// <inheritdoc />
    public partial class AddDownSince : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DownSince",
                table: "MonitoredUrls",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DownSince",
                table: "MonitoredUrls");
        }
    }
}
