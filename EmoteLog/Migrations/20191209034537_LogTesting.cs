using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EmoteLog.Migrations
{
    public partial class LogTesting : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LogEntry_AspNetUsers_EmoteLogUserId",
                table: "LogEntry");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LogEntry",
                table: "LogEntry");

            migrationBuilder.DropIndex(
                name: "IX_LogEntry_EmoteLogUserId",
                table: "LogEntry");

            migrationBuilder.DropColumn(
                name: "EmoteLogUserId",
                table: "LogEntry");

            migrationBuilder.RenameTable(
                name: "LogEntry",
                newName: "UserLogs");

            migrationBuilder.AddColumn<DateTime>(
                name: "PublishDate",
                table: "UserLogs",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserLogs",
                table: "UserLogs",
                column: "LogId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserLogs",
                table: "UserLogs");

            migrationBuilder.DropColumn(
                name: "PublishDate",
                table: "UserLogs");

            migrationBuilder.RenameTable(
                name: "UserLogs",
                newName: "LogEntry");

            migrationBuilder.AddColumn<string>(
                name: "EmoteLogUserId",
                table: "LogEntry",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_LogEntry",
                table: "LogEntry",
                column: "LogId");

            migrationBuilder.CreateIndex(
                name: "IX_LogEntry_EmoteLogUserId",
                table: "LogEntry",
                column: "EmoteLogUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_LogEntry_AspNetUsers_EmoteLogUserId",
                table: "LogEntry",
                column: "EmoteLogUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
