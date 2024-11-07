using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LinkyAppBackend.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddedProfilePhotoIdOnDeleteNull : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_File_ProfilePhotoId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Link_AspNetUsers_CreatorId",
                table: "Link");

            migrationBuilder.DropForeignKey(
                name: "FK_Link_LinkGroup_LinkGroupId",
                table: "Link");

            migrationBuilder.DropForeignKey(
                name: "FK_LinkGroupUser_AspNetUsers_UserId",
                table: "LinkGroupUser");

            migrationBuilder.DropForeignKey(
                name: "FK_LinkGroupUser_LinkGroup_GroupId",
                table: "LinkGroupUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LinkGroupUser",
                table: "LinkGroupUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LinkGroup",
                table: "LinkGroup");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Link",
                table: "Link");

            migrationBuilder.DropIndex(
                name: "IX_Link_LinkGroupId",
                table: "Link");

            migrationBuilder.DropPrimaryKey(
                name: "PK_File",
                table: "File");

            migrationBuilder.DropColumn(
                name: "LinkGroupId",
                table: "Link");

            migrationBuilder.RenameTable(
                name: "LinkGroupUser",
                newName: "LinkGroupUsers");

            migrationBuilder.RenameTable(
                name: "LinkGroup",
                newName: "LinkGroups");

            migrationBuilder.RenameTable(
                name: "Link",
                newName: "Links");

            migrationBuilder.RenameTable(
                name: "File",
                newName: "Files");

            migrationBuilder.RenameIndex(
                name: "IX_LinkGroupUser_GroupId",
                table: "LinkGroupUsers",
                newName: "IX_LinkGroupUsers_GroupId");

            migrationBuilder.RenameIndex(
                name: "IX_Link_CreatorId",
                table: "Links",
                newName: "IX_Links_CreatorId");

            migrationBuilder.AlterColumn<string>(
                name: "CreatorId",
                table: "Links",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "GroupId",
                table: "Links",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LinkGroupUsers",
                table: "LinkGroupUsers",
                columns: new[] { "UserId", "GroupId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_LinkGroups",
                table: "LinkGroups",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Links",
                table: "Links",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Files",
                table: "Files",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Links_GroupId",
                table: "Links",
                column: "GroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Files_ProfilePhotoId",
                table: "AspNetUsers",
                column: "ProfilePhotoId",
                principalTable: "Files",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_LinkGroupUsers_AspNetUsers_UserId",
                table: "LinkGroupUsers",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LinkGroupUsers_LinkGroups_GroupId",
                table: "LinkGroupUsers",
                column: "GroupId",
                principalTable: "LinkGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Links_AspNetUsers_CreatorId",
                table: "Links",
                column: "CreatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Links_LinkGroups_GroupId",
                table: "Links",
                column: "GroupId",
                principalTable: "LinkGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Files_ProfilePhotoId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_LinkGroupUsers_AspNetUsers_UserId",
                table: "LinkGroupUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_LinkGroupUsers_LinkGroups_GroupId",
                table: "LinkGroupUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Links_AspNetUsers_CreatorId",
                table: "Links");

            migrationBuilder.DropForeignKey(
                name: "FK_Links_LinkGroups_GroupId",
                table: "Links");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Links",
                table: "Links");

            migrationBuilder.DropIndex(
                name: "IX_Links_GroupId",
                table: "Links");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LinkGroupUsers",
                table: "LinkGroupUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LinkGroups",
                table: "LinkGroups");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Files",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "Links");

            migrationBuilder.RenameTable(
                name: "Links",
                newName: "Link");

            migrationBuilder.RenameTable(
                name: "LinkGroupUsers",
                newName: "LinkGroupUser");

            migrationBuilder.RenameTable(
                name: "LinkGroups",
                newName: "LinkGroup");

            migrationBuilder.RenameTable(
                name: "Files",
                newName: "File");

            migrationBuilder.RenameIndex(
                name: "IX_Links_CreatorId",
                table: "Link",
                newName: "IX_Link_CreatorId");

            migrationBuilder.RenameIndex(
                name: "IX_LinkGroupUsers_GroupId",
                table: "LinkGroupUser",
                newName: "IX_LinkGroupUser_GroupId");

            migrationBuilder.AlterColumn<string>(
                name: "CreatorId",
                table: "Link",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255);

            migrationBuilder.AddColumn<string>(
                name: "LinkGroupId",
                table: "Link",
                type: "text",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Link",
                table: "Link",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LinkGroupUser",
                table: "LinkGroupUser",
                columns: new[] { "UserId", "GroupId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_LinkGroup",
                table: "LinkGroup",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_File",
                table: "File",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Link_LinkGroupId",
                table: "Link",
                column: "LinkGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_File_ProfilePhotoId",
                table: "AspNetUsers",
                column: "ProfilePhotoId",
                principalTable: "File",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Link_AspNetUsers_CreatorId",
                table: "Link",
                column: "CreatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Link_LinkGroup_LinkGroupId",
                table: "Link",
                column: "LinkGroupId",
                principalTable: "LinkGroup",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LinkGroupUser_AspNetUsers_UserId",
                table: "LinkGroupUser",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LinkGroupUser_LinkGroup_GroupId",
                table: "LinkGroupUser",
                column: "GroupId",
                principalTable: "LinkGroup",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
