using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Zymora.Migrations
{
    /// <inheritdoc />
    public partial class UpdateModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_user_roles_RoleId",
                table: "user_roles",
                column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_roles_claims_roles_RoleId",
                table: "roles_claims",
                column: "RoleId",
                principalTable: "roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_user_claims_user_UserId",
                table: "user_claims",
                column: "UserId",
                principalTable: "user",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_user_logins_user_UserId",
                table: "user_logins",
                column: "UserId",
                principalTable: "user",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_user_roles_roles_RoleId",
                table: "user_roles",
                column: "RoleId",
                principalTable: "roles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_user_roles_user_UserId",
                table: "user_roles",
                column: "UserId",
                principalTable: "user",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_user_tokens_user_UserId",
                table: "user_tokens",
                column: "UserId",
                principalTable: "user",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_roles_claims_roles_RoleId",
                table: "roles_claims");

            migrationBuilder.DropForeignKey(
                name: "FK_user_claims_user_UserId",
                table: "user_claims");

            migrationBuilder.DropForeignKey(
                name: "FK_user_logins_user_UserId",
                table: "user_logins");

            migrationBuilder.DropForeignKey(
                name: "FK_user_roles_roles_RoleId",
                table: "user_roles");

            migrationBuilder.DropForeignKey(
                name: "FK_user_roles_user_UserId",
                table: "user_roles");

            migrationBuilder.DropForeignKey(
                name: "FK_user_tokens_user_UserId",
                table: "user_tokens");

            migrationBuilder.DropIndex(
                name: "IX_user_roles_RoleId",
                table: "user_roles");
        }
    }
}
