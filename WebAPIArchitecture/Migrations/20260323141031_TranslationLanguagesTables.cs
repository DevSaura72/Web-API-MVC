using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAPIArchitecture.Migrations
{
    /// <inheritdoc />
    public partial class TranslationLanguagesTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Languages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Language = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    CultureCode = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    AddedById = table.Column<int>(type: "int", nullable: false),
                    AddedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedById = table.Column<int>(type: "int", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedById = table.Column<int>(type: "int", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Languages", x => x.Id);
                    table.UniqueConstraint("AK_Languages_CultureCode", x => x.CultureCode);
                    table.ForeignKey(
                        name: "FK_Languages_AspNetUsers_AddedById",
                        column: x => x.AddedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Languages_AspNetUsers_DeletedById",
                        column: x => x.DeletedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Languages_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Translations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CultureCode = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    TranslationKey = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    TranslationValue = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    AddedById = table.Column<int>(type: "int", nullable: false),
                    AddedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedById = table.Column<int>(type: "int", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedById = table.Column<int>(type: "int", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Translations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Translations_AspNetUsers_AddedById",
                        column: x => x.AddedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Translations_AspNetUsers_DeletedById",
                        column: x => x.DeletedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Translations_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Translations_Languages_CultureCode",
                        column: x => x.CultureCode,
                        principalTable: "Languages",
                        principalColumn: "CultureCode",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Languages_AddedById",
                table: "Languages",
                column: "AddedById");

            migrationBuilder.CreateIndex(
                name: "IX_Languages_DeletedById",
                table: "Languages",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_Languages_UpdatedById",
                table: "Languages",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Translations_AddedById",
                table: "Translations",
                column: "AddedById");

            migrationBuilder.CreateIndex(
                name: "IX_Translations_CultureCode",
                table: "Translations",
                column: "CultureCode");

            migrationBuilder.CreateIndex(
                name: "IX_Translations_DeletedById",
                table: "Translations",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_Translations_UpdatedById",
                table: "Translations",
                column: "UpdatedById");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Translations");

            migrationBuilder.DropTable(
                name: "Languages");
        }
    }
}
