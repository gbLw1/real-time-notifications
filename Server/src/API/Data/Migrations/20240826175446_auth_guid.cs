using System;

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RTN.API.Migrations {
    /// <inheritdoc />
    public partial class auth_guid : Migration {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder) {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Notifications",
                table: "Notifications");

            migrationBuilder.RenameTable(
                name: "Notifications",
                newName: "NotificationEntity");

            migrationBuilder.AlterColumn<Guid>(
                name: "RefreshToken",
                table: "LoginEntity",
                type: "uniqueidentifier",
                maxLength: 250,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldMaxLength: 250,
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "AuthToken",
                table: "LoginEntity",
                type: "uniqueidentifier",
                maxLength: 250,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldMaxLength: 250,
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_NotificationEntity",
                table: "NotificationEntity",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder) {
            migrationBuilder.DropPrimaryKey(
                name: "PK_NotificationEntity",
                table: "NotificationEntity");

            migrationBuilder.RenameTable(
                name: "NotificationEntity",
                newName: "Notifications");

            migrationBuilder.AlterColumn<string>(
                name: "RefreshToken",
                table: "LoginEntity",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldMaxLength: 250,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AuthToken",
                table: "LoginEntity",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldMaxLength: 250,
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Notifications",
                table: "Notifications",
                column: "Id");
        }
    }
}
