using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CyberQuiz.DAL.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSchemaAndSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserResults_Questions_QuestionId",
                table: "UserResults");

            migrationBuilder.DropIndex(
                name: "IX_SubCategories_CategoryId",
                table: "SubCategories");

            migrationBuilder.DropIndex(
                name: "IX_AnswerOptions_QuestionId",
                table: "AnswerOptions");

            migrationBuilder.RenameIndex(
                name: "IX_UserResults_UserId",
                table: "UserResults",
                newName: "IX_UserResult_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserResults_QuestionId",
                table: "UserResults",
                newName: "IX_UserResult_QuestionId");

            migrationBuilder.AddColumn<DateTime>(
                name: "AnsweredAt",
                table: "UserResults",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "SubCategories",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "SortOrder",
                table: "SubCategories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Text",
                table: "Questions",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "Points",
                table: "Questions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Categories",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ProfilePictureUrl",
                table: "AspNetUsers",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "AspNetUsers",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastLogIn",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<string>(
                name: "Text",
                table: "AnswerOptions",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "DisplayOrder",
                table: "AnswerOptions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_UserResult_AnsweredAt",
                table: "UserResults",
                column: "AnsweredAt");

            migrationBuilder.CreateIndex(
                name: "IX_UserResult_UserId_AnsweredAt",
                table: "UserResults",
                columns: new[] { "UserId", "AnsweredAt" });

            migrationBuilder.CreateIndex(
                name: "IX_UserResults_UserId_AnsweredAt",
                table: "UserResults",
                columns: new[] { "UserId", "AnsweredAt" });

            migrationBuilder.CreateIndex(
                name: "IX_SubCategories_CategoryId_SortOrder",
                table: "SubCategories",
                columns: new[] { "CategoryId", "SortOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_AnswerOptions_QuestionId_DisplayOrder",
                table: "AnswerOptions",
                columns: new[] { "QuestionId", "DisplayOrder" });

            migrationBuilder.AddForeignKey(
                name: "FK_UserResults_Questions_QuestionId",
                table: "UserResults",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserResults_Questions_QuestionId",
                table: "UserResults");

            migrationBuilder.DropIndex(
                name: "IX_UserResult_AnsweredAt",
                table: "UserResults");

            migrationBuilder.DropIndex(
                name: "IX_UserResult_UserId_AnsweredAt",
                table: "UserResults");

            migrationBuilder.DropIndex(
                name: "IX_UserResults_UserId_AnsweredAt",
                table: "UserResults");

            migrationBuilder.DropIndex(
                name: "IX_SubCategories_CategoryId_SortOrder",
                table: "SubCategories");

            migrationBuilder.DropIndex(
                name: "IX_AnswerOptions_QuestionId_DisplayOrder",
                table: "AnswerOptions");

            migrationBuilder.DropColumn(
                name: "AnsweredAt",
                table: "UserResults");

            migrationBuilder.DropColumn(
                name: "SortOrder",
                table: "SubCategories");

            migrationBuilder.DropColumn(
                name: "Points",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "FullName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastLogIn",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "DisplayOrder",
                table: "AnswerOptions");

            migrationBuilder.RenameIndex(
                name: "IX_UserResult_UserId",
                table: "UserResults",
                newName: "IX_UserResults_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserResult_QuestionId",
                table: "UserResults",
                newName: "IX_UserResults_QuestionId");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "SubCategories",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "Text",
                table: "Questions",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "ProfilePictureUrl",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Text",
                table: "AnswerOptions",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.CreateIndex(
                name: "IX_SubCategories_CategoryId",
                table: "SubCategories",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_AnswerOptions_QuestionId",
                table: "AnswerOptions",
                column: "QuestionId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserResults_Questions_QuestionId",
                table: "UserResults",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
