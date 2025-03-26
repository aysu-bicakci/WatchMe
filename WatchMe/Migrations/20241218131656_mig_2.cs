using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WatchMe.Migrations
{
    /// <inheritdoc />
    public partial class mig_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Movie Yorum Ekleme
        migrationBuilder.Sql(@"
            CREATE OR REPLACE PROCEDURE sp_insert_movie_comment(
                p_movie_id INT,
                p_user_id INT,
                p_comment TEXT
            ) AS $$
            BEGIN
                INSERT INTO ""MovieComments"" (""MovieId"", ""UserId"", ""Comment"")
                VALUES (p_movie_id, p_user_id, p_comment);
            END;
            $$ LANGUAGE plpgsql;
        ");

        // TV Show Yorum Ekleme
        migrationBuilder.Sql(@"
            CREATE OR REPLACE PROCEDURE sp_insert_tvshow_comment(
                p_tvshow_id INT,
                p_user_id INT,
                p_comment TEXT
            ) AS $$
            BEGIN
                INSERT INTO ""TVShowComments"" (""TVShowId"", ""UserId"", ""Comment"")
                VALUES (p_tvshow_id, p_user_id, p_comment);
            END;
            $$ LANGUAGE plpgsql;
        ");

        // Movie Yorum Silme
        migrationBuilder.Sql(@"
            CREATE OR REPLACE PROCEDURE sp_delete_movie_comment(
                p_movie_comment_id INT
            ) AS $$
            BEGIN
                DELETE FROM ""MovieComments""
                WHERE ""MovieCommentId"" = p_movie_comment_id;
            END;
            $$ LANGUAGE plpgsql;
        ");

        // TV Show Yorum Silme
        migrationBuilder.Sql(@"
            CREATE OR REPLACE PROCEDURE sp_delete_tvshow_comment(
                p_tvshow_comment_id INT
            ) AS $$
            BEGIN
                DELETE FROM ""TVShowComments""
                WHERE ""TVShowCommentId"" = p_tvshow_comment_id;
            END;
            $$ LANGUAGE plpgsql;
        ");

        // Movie Yorum Güncelleme
        migrationBuilder.Sql(@"
            CREATE OR REPLACE PROCEDURE sp_update_movie_comment(
                p_movie_comment_id INT,
                p_comment TEXT
            ) AS $$
            BEGIN
                UPDATE ""MovieComments""
                SET ""Comment"" = p_comment
                WHERE ""MovieCommentId"" = p_movie_comment_id;
            END;
            $$ LANGUAGE plpgsql;
        ");

        // TV Show Yorum Güncelleme
        migrationBuilder.Sql(@"
            CREATE OR REPLACE PROCEDURE sp_update_tvshow_comment(
                p_tvshow_comment_id INT,
                p_comment TEXT
            ) AS $$
            BEGIN
                UPDATE ""TVShowComments""
                SET ""Comment"" = p_comment
                WHERE ""TVShowCommentId"" = p_tvshow_comment_id;
            END;
            $$ LANGUAGE plpgsql;
        ");

        // Movie Yorumları Görüntüleme
        migrationBuilder.Sql(@"
            CREATE OR REPLACE PROCEDURE sp_get_movie_comments(
                p_movie_id INT
            ) AS $$
            BEGIN
                RAISE NOTICE 'Movie Comments:';
                PERFORM * FROM ""MovieComments"" WHERE ""MovieId"" = p_movie_id;
            END;
            $$ LANGUAGE plpgsql;
        ");

        // TV Show Yorumları Görüntüleme
        migrationBuilder.Sql(@"
            CREATE OR REPLACE PROCEDURE sp_get_tvshow_comments(
                p_tvshow_id INT
            ) AS $$
            BEGIN
                RAISE NOTICE 'TV Show Comments:';
                PERFORM * FROM ""TVShowComments"" WHERE ""TVShowId"" = p_tvshow_id;
            END;
            $$ LANGUAGE plpgsql;
        ");

        // Movie için Toplam Yorum Sayısı
        migrationBuilder.Sql(@"
            CREATE OR REPLACE FUNCTION sp_get_total_movie_comments(
                p_movie_id INT
            ) RETURNS INT AS $$
            DECLARE
                total_comments INT;
            BEGIN
                SELECT COUNT(*) INTO total_comments
                FROM ""MovieComments""
                WHERE ""MovieId"" = p_movie_id;
                
                RETURN total_comments;
            END;
            $$ LANGUAGE plpgsql;
        ");

        // TV Show için Toplam Yorum Sayısı
        migrationBuilder.Sql(@"
            CREATE OR REPLACE FUNCTION sp_get_total_tvshow_comments(
                p_tvshow_id INT
            ) RETURNS INT AS $$
            DECLARE
                total_comments INT;
            BEGIN
                SELECT COUNT(*) INTO total_comments
                FROM ""TVShowComments""
                WHERE ""TVShowId"" = p_tvshow_id;
                
                RETURN total_comments;
            END;
            $$ LANGUAGE plpgsql;
        ");


        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

            // Stored Procedure'leri silme
        migrationBuilder.Sql("DROP PROCEDURE IF EXISTS sp_insert_movie_comment");
        migrationBuilder.Sql("DROP PROCEDURE IF EXISTS sp_insert_tvshow_comment");
        migrationBuilder.Sql("DROP PROCEDURE IF EXISTS sp_delete_movie_comment");
        migrationBuilder.Sql("DROP PROCEDURE IF EXISTS sp_delete_tvshow_comment");
        migrationBuilder.Sql("DROP PROCEDURE IF EXISTS sp_update_movie_comment");
        migrationBuilder.Sql("DROP PROCEDURE IF EXISTS sp_update_tvshow_comment");
        migrationBuilder.Sql("DROP PROCEDURE IF EXISTS sp_get_movie_comments");
        migrationBuilder.Sql("DROP PROCEDURE IF EXISTS sp_get_tvshow_comments");

        // Fonksiyonları Silme
        migrationBuilder.Sql("DROP FUNCTION IF EXISTS sp_get_total_movie_comments");
        migrationBuilder.Sql("DROP FUNCTION IF EXISTS sp_get_total_tvshow_comments");

        }
    }
}
