using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RTN.API.Migrations
{
    /// <inheritdoc />
    public partial class IndividualNoties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "NotificationEntity",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_NotificationEntity_UserId",
                table: "NotificationEntity",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_NotificationEntity_UserEntity_UserId",
                table: "NotificationEntity",
                column: "UserId",
                principalTable: "UserEntity",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NotificationEntity_UserEntity_UserId",
                table: "NotificationEntity");

            migrationBuilder.DropIndex(
                name: "IX_NotificationEntity_UserId",
                table: "NotificationEntity");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "NotificationEntity");
        }
    }
}
