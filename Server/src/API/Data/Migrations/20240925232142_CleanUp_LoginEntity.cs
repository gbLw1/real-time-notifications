using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RTN.API.Migrations
{
    /// <inheritdoc />
    public partial class CleanUp_LoginEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AuthToken",
                table: "LoginEntity");

            migrationBuilder.DropColumn(
                name: "AuthTokenExpiryTime",
                table: "LoginEntity");

            migrationBuilder.RenameColumn(
                name: "RefreshTokenExpiryTime",
                table: "LoginEntity",
                newName: "RefreshTokenExpirationTime");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RefreshTokenExpirationTime",
                table: "LoginEntity",
                newName: "RefreshTokenExpiryTime");

            migrationBuilder.AddColumn<Guid>(
                name: "AuthToken",
                table: "LoginEntity",
                type: "uniqueidentifier",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "AuthTokenExpiryTime",
                table: "LoginEntity",
                type: "datetime2",
                nullable: true);
        }
    }
}
