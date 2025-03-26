using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace WatchMe.Migrations
{
    /// <inheritdoc />
    public partial class mig_1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Genres",
                columns: table => new
                {
                    GenreId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genres", x => x.GenreId);
                });

            migrationBuilder.CreateTable(
                name: "Movies",
                columns: table => new
                {
                    MovieId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Popularity = table.Column<double>(type: "double precision", nullable: true),
                    Overview = table.Column<string>(type: "text", nullable: true),
                    ReleaseDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    PosterPath = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movies", x => x.MovieId);
                });

            migrationBuilder.CreateTable(
                name: "TVShows",
                columns: table => new
                {
                    TVShowId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Popularity = table.Column<double>(type: "double precision", nullable: true),
                    Overview = table.Column<string>(type: "text", nullable: true),
                    ReleaseDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    PosterPath = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TVShows", x => x.TVShowId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nickname = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "MovieGenres",
                columns: table => new
                {
                    MovieId = table.Column<int>(type: "integer", nullable: false),
                    GenreId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovieGenres", x => new { x.MovieId, x.GenreId });
                    table.ForeignKey(
                        name: "FK_MovieGenres_Genres_GenreId",
                        column: x => x.GenreId,
                        principalTable: "Genres",
                        principalColumn: "GenreId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MovieGenres_Movies_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movies",
                        principalColumn: "MovieId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TVShowGenres",
                columns: table => new
                {
                    TVShowId = table.Column<int>(type: "integer", nullable: false),
                    GenreId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TVShowGenres", x => new { x.TVShowId, x.GenreId });
                    table.ForeignKey(
                        name: "FK_TVShowGenres_Genres_GenreId",
                        column: x => x.GenreId,
                        principalTable: "Genres",
                        principalColumn: "GenreId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TVShowGenres_TVShows_TVShowId",
                        column: x => x.TVShowId,
                        principalTable: "TVShows",
                        principalColumn: "TVShowId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MovieComments",
                columns: table => new
                {
                    MovieCommentId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MovieId = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    Comment = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovieComments", x => x.MovieCommentId);
                    table.ForeignKey(
                        name: "FK_MovieComments_Movies_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movies",
                        principalColumn: "MovieId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MovieComments_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MovieDislikes",
                columns: table => new
                {
                    MovieId = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    MovieDislikeId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovieDislikes", x => new { x.MovieId, x.UserId });
                    table.ForeignKey(
                        name: "FK_MovieDislikes_Movies_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movies",
                        principalColumn: "MovieId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MovieDislikes_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MovieLikes",
                columns: table => new
                {
                    MovieId = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    MovieLikeId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovieLikes", x => new { x.MovieId, x.UserId });
                    table.ForeignKey(
                        name: "FK_MovieLikes_Movies_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movies",
                        principalColumn: "MovieId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MovieLikes_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MovieWatchLists",
                columns: table => new
                {
                    MovieId = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    MovieWatchListId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovieWatchLists", x => new { x.MovieId, x.UserId });
                    table.ForeignKey(
                        name: "FK_MovieWatchLists_Movies_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movies",
                        principalColumn: "MovieId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MovieWatchLists_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TVShowComments",
                columns: table => new
                {
                    TVShowCommentId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TVShowId = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    Comment = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TVShowComments", x => x.TVShowCommentId);
                    table.ForeignKey(
                        name: "FK_TVShowComments_TVShows_TVShowId",
                        column: x => x.TVShowId,
                        principalTable: "TVShows",
                        principalColumn: "TVShowId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TVShowComments_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TVShowDislikes",
                columns: table => new
                {
                    TVShowId = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    TVShowDislikeId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TVShowDislikes", x => new { x.TVShowId, x.UserId });
                    table.ForeignKey(
                        name: "FK_TVShowDislikes_TVShows_TVShowId",
                        column: x => x.TVShowId,
                        principalTable: "TVShows",
                        principalColumn: "TVShowId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TVShowDislikes_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TVShowLikes",
                columns: table => new
                {
                    TVShowId = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    TVShowLikeId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TVShowLikes", x => new { x.TVShowId, x.UserId });
                    table.ForeignKey(
                        name: "FK_TVShowLikes_TVShows_TVShowId",
                        column: x => x.TVShowId,
                        principalTable: "TVShows",
                        principalColumn: "TVShowId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TVShowLikes_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TVShowWatchLists",
                columns: table => new
                {
                    TVShowId = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    TVShowWatchListId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TVShowWatchLists", x => new { x.TVShowId, x.UserId });
                    table.ForeignKey(
                        name: "FK_TVShowWatchLists_TVShows_TVShowId",
                        column: x => x.TVShowId,
                        principalTable: "TVShows",
                        principalColumn: "TVShowId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TVShowWatchLists_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Genres_Name",
                table: "Genres",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MovieComments_MovieId",
                table: "MovieComments",
                column: "MovieId");

            migrationBuilder.CreateIndex(
                name: "IX_MovieComments_UserId",
                table: "MovieComments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_MovieDislikes_UserId",
                table: "MovieDislikes",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_MovieGenres_GenreId",
                table: "MovieGenres",
                column: "GenreId");

            migrationBuilder.CreateIndex(
                name: "IX_MovieLikes_UserId",
                table: "MovieLikes",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Movies_Title",
                table: "Movies",
                column: "Title",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MovieWatchLists_UserId",
                table: "MovieWatchLists",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TVShowComments_TVShowId",
                table: "TVShowComments",
                column: "TVShowId");

            migrationBuilder.CreateIndex(
                name: "IX_TVShowComments_UserId",
                table: "TVShowComments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TVShowDislikes_UserId",
                table: "TVShowDislikes",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TVShowGenres_GenreId",
                table: "TVShowGenres",
                column: "GenreId");

            migrationBuilder.CreateIndex(
                name: "IX_TVShowLikes_UserId",
                table: "TVShowLikes",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TVShows_Title",
                table: "TVShows",
                column: "Title",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TVShowWatchLists_UserId",
                table: "TVShowWatchLists",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);


            migrationBuilder.Sql(@"
                CREATE OR REPLACE VIEW ""Romance_Movies"" AS
                SELECT m.""PosterPath"", m.""Title""
                FROM ""Movies"" m
                INNER JOIN ""MovieGenres"" mg ON m.""MovieId"" = mg.""MovieId""
                WHERE mg.""GenreId"" = 10749;
            ");

            migrationBuilder.Sql(@"
                CREATE OR REPLACE VIEW ""Horror_Movies"" AS
                SELECT m.""PosterPath"", m.""Title""
                FROM ""Movies"" m
                INNER JOIN ""MovieGenres"" mg ON m.""MovieId"" = mg.""MovieId""
                WHERE mg.""GenreId"" = 27;
            ");

            migrationBuilder.Sql(@"
                CREATE OR REPLACE VIEW ""Comedy_Movies"" AS
                SELECT m.""PosterPath"", m.""Title""
                FROM ""Movies"" m
                INNER JOIN ""MovieGenres"" mg ON m.""MovieId"" = mg.""MovieId""
                WHERE mg.""GenreId"" = 35;
            ");

             migrationBuilder.Sql(@"
                CREATE OR REPLACE VIEW ""Action_Movies"" AS
                SELECT m.""PosterPath"", m.""Title""
                FROM ""Movies"" m
                INNER JOIN ""MovieGenres"" mg ON m.""MovieId"" = mg.""MovieId""
                WHERE mg.""GenreId"" = 28;
            ");


             migrationBuilder.Sql(@"
                CREATE OR REPLACE VIEW ""ScienceFiction_Movies"" AS
                SELECT m.""PosterPath"", m.""Title""
                FROM ""Movies"" m
                INNER JOIN ""MovieGenres"" mg ON m.""MovieId"" = mg.""MovieId""
                WHERE mg.""GenreId"" = 878;
            ");

            migrationBuilder.Sql(@"
                CREATE OR REPLACE VIEW ""Animation_Movies"" AS
                SELECT m.""PosterPath"", m.""Title""
                FROM ""Movies"" m
                INNER JOIN ""MovieGenres"" mg ON m.""MovieId"" = mg.""MovieId""
                WHERE mg.""GenreId"" = 16;
            ");


            migrationBuilder.Sql(@"
            CREATE OR REPLACE VIEW ""ActionAdventure_TVShows"" AS
            SELECT t.""PosterPath"", t.""Title"" 
            FROM ""TVShows"" t
            JOIN ""TVShowGenres"" tg ON t.""TVShowId"" = tg.""TVShowId""
            WHERE tg.""GenreId"" = 10759;

            CREATE OR REPLACE VIEW ""Drama_TVShows"" AS
            SELECT t.""PosterPath"", t.""Title"" 
            FROM ""TVShows"" t
            JOIN ""TVShowGenres"" tg ON t.""TVShowId"" = tg.""TVShowId""
            WHERE tg.""GenreId"" = 18;

            CREATE OR REPLACE VIEW ""Comedy_TVShows"" AS
            SELECT t.""PosterPath"", t.""Title"" 
            FROM ""TVShows"" t
            JOIN ""TVShowGenres"" tg ON t.""TVShowId"" = tg.""TVShowId""
            WHERE tg.""GenreId"" = 35;

            CREATE OR REPLACE VIEW ""Family_TVShows"" AS
            SELECT t.""PosterPath"", t.""Title"" 
            FROM ""TVShows"" t
            JOIN ""TVShowGenres"" tg ON t.""TVShowId"" = tg.""TVShowId""
            WHERE tg.""GenreId"" = 10751;

            CREATE OR REPLACE VIEW ""Mystery_TVShows"" AS
            SELECT t.""PosterPath"", t.""Title"" 
            FROM ""TVShows"" t
            JOIN ""TVShowGenres"" tg ON t.""TVShowId"" = tg.""TVShowId""
            WHERE tg.""GenreId"" = 9648;

            CREATE OR REPLACE VIEW ""Crime_TVShows"" AS
            SELECT t.""PosterPath"", t.""Title"" 
            FROM ""TVShows"" t
            JOIN ""TVShowGenres"" tg ON t.""TVShowId"" = tg.""TVShowId""
            WHERE tg.""GenreId"" = 80;
        ");






        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MovieComments");

            migrationBuilder.DropTable(
                name: "MovieDislikes");

            migrationBuilder.DropTable(
                name: "MovieGenres");

            migrationBuilder.DropTable(
                name: "MovieLikes");

            migrationBuilder.DropTable(
                name: "MovieWatchLists");

            migrationBuilder.DropTable(
                name: "TVShowComments");

            migrationBuilder.DropTable(
                name: "TVShowDislikes");

            migrationBuilder.DropTable(
                name: "TVShowGenres");

            migrationBuilder.DropTable(
                name: "TVShowLikes");

            migrationBuilder.DropTable(
                name: "TVShowWatchLists");

            migrationBuilder.DropTable(
                name: "Movies");

            migrationBuilder.DropTable(
                name: "Genres");

            migrationBuilder.DropTable(
                name: "TVShows");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.Sql(@"
                DROP VIEW IF EXISTS ""Romance_Movies"";
                DROP VIEW IF EXISTS ""Horror_Movies"";
                DROP VIEW IF EXISTS ""Comedy_Movies"";
                DROP VIEW IF EXISTS ""Action_Movies"";
                DROP VIEW IF EXISTS ""ScienceFiction_Movies"";
                DROP VIEW IF EXISTS ""Animation_Movies"";
            ");


            migrationBuilder.Sql(@"
                DROP VIEW IF EXISTS ""ActionAdventure_TVShows"";
                DROP VIEW IF EXISTS ""Drama_TVShows"";
                DROP VIEW IF EXISTS ""Comedy_TVShows"";
                DROP VIEW IF EXISTS ""Family_TVShows"";
                DROP VIEW IF EXISTS ""Mystery_TVShows"";
                DROP VIEW IF EXISTS ""Crime_TVShows"";
            ");

        }
    }
}
