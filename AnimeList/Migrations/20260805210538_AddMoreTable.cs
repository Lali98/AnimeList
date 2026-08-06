using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AnimeList.Migrations
{
    /// <inheritdoc />
    public partial class AddMoreTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Animes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MalId = table.Column<int>(type: "integer", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false),
                    TrailerUrls = table.Column<string[]>(type: "text[]", nullable: true),
                    ImageUrl = table.Column<string>(type: "text", nullable: true),
                    Year = table.Column<int>(type: "integer", nullable: true),
                    Season = table.Column<string>(type: "text", nullable: true),
                    Episodes = table.Column<int>(type: "integer", nullable: true),
                    Duration = table.Column<int>(type: "integer", nullable: true),
                    MalScore = table.Column<float>(type: "real", nullable: true),
                    FanSubId = table.Column<int>(type: "integer", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Descriptions = table.Column<string>(type: "jsonb", nullable: false),
                    Titles = table.Column<string>(type: "jsonb", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Animes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Genres",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genres", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Studios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Studios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AnimeFanSub",
                columns: table => new
                {
                    AnimesId = table.Column<int>(type: "integer", nullable: false),
                    FanSubsId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnimeFanSub", x => new { x.AnimesId, x.FanSubsId });
                    table.ForeignKey(
                        name: "FK_AnimeFanSub_Animes_AnimesId",
                        column: x => x.AnimesId,
                        principalTable: "Animes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AnimeFanSub_FanSubs_FanSubsId",
                        column: x => x.FanSubsId,
                        principalTable: "FanSubs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AnimeGenre",
                columns: table => new
                {
                    AnimesId = table.Column<int>(type: "integer", nullable: false),
                    GenresId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnimeGenre", x => new { x.AnimesId, x.GenresId });
                    table.ForeignKey(
                        name: "FK_AnimeGenre_Animes_AnimesId",
                        column: x => x.AnimesId,
                        principalTable: "Animes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AnimeGenre_Genres_GenresId",
                        column: x => x.GenresId,
                        principalTable: "Genres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AnimeStudio",
                columns: table => new
                {
                    AnimesId = table.Column<int>(type: "integer", nullable: false),
                    StudiosId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnimeStudio", x => new { x.AnimesId, x.StudiosId });
                    table.ForeignKey(
                        name: "FK_AnimeStudio_Animes_AnimesId",
                        column: x => x.AnimesId,
                        principalTable: "Animes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AnimeStudio_Studios_StudiosId",
                        column: x => x.StudiosId,
                        principalTable: "Studios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnimeFanSub_FanSubsId",
                table: "AnimeFanSub",
                column: "FanSubsId");

            migrationBuilder.CreateIndex(
                name: "IX_AnimeGenre_GenresId",
                table: "AnimeGenre",
                column: "GenresId");

            migrationBuilder.CreateIndex(
                name: "IX_Animes_MalId",
                table: "Animes",
                column: "MalId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AnimeStudio_StudiosId",
                table: "AnimeStudio",
                column: "StudiosId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnimeFanSub");

            migrationBuilder.DropTable(
                name: "AnimeGenre");

            migrationBuilder.DropTable(
                name: "AnimeStudio");

            migrationBuilder.DropTable(
                name: "Genres");

            migrationBuilder.DropTable(
                name: "Animes");

            migrationBuilder.DropTable(
                name: "Studios");
        }
    }
}
