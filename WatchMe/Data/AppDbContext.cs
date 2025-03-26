using Microsoft.EntityFrameworkCore;
using WatchMe.Models;

namespace WatchMe.Data
{

    
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<TVShow> TVShows { get; set; }
        public DbSet<Genre> Genres { get; set; }

        public DbSet<MovieGenre> MovieGenres { get; set; }
        public DbSet<TVShowGenre> TVShowGenres { get; set; }

        public DbSet<MovieLike> MovieLikes { get; set; }
        public DbSet<TVShowLike> TVShowLikes { get; set; }

        public DbSet<MovieDislike> MovieDislikes { get; set; }
        public DbSet<TVShowDislike> TVShowDislikes { get; set; }

        public DbSet<MovieComment> MovieComments { get; set; }
        public DbSet<TVShowComment> TVShowComments { get; set; }

        public DbSet<MovieWatchList> MovieWatchLists { get; set; }
        public DbSet<TVShowWatchList> TVShowWatchLists { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User Email için Unique Constraint
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<Movie>()
                .HasIndex(m => m.Title)
                .IsUnique();

            // TVShow Title için Unique kısıtlaması
            modelBuilder.Entity<TVShow>()
                .HasIndex(t => t.Title)
                .IsUnique();
                
            modelBuilder.Entity<Genre>()
                .HasIndex(g => g.Name)
                .IsUnique();

            // Movie ve Genre arasındaki Many-to-Many ilişki
            modelBuilder.Entity<MovieGenre>()
                .HasKey(mg => new { mg.MovieId, mg.GenreId });

            modelBuilder.Entity<MovieGenre>()
                .HasOne(mg => mg.Movie)
                .WithMany(m => m.MovieGenres)
                .HasForeignKey(mg => mg.MovieId);

            modelBuilder.Entity<MovieGenre>()
                .HasOne(mg => mg.Genre)
                .WithMany(g => g.MovieGenres)
                .HasForeignKey(mg => mg.GenreId);

            // TVShow ve Genre arasındaki Many-to-Many ilişki
            modelBuilder.Entity<TVShowGenre>()
                .HasKey(tg => new { tg.TVShowId, tg.GenreId });

            modelBuilder.Entity<TVShowGenre>()
                .HasOne(tg => tg.TVShow)
                .WithMany(t => t.TVShowGenres)
                .HasForeignKey(tg => tg.TVShowId);

            modelBuilder.Entity<TVShowGenre>()
                .HasOne(tg => tg.Genre)
                .WithMany(g => g.TVShowGenres)
                .HasForeignKey(tg => tg.GenreId);

            // User ve Movie arasındaki Many-to-Many ilişkileri
            modelBuilder.Entity<MovieLike>()
                .HasKey(ml => new { ml.MovieId, ml.UserId });

            modelBuilder.Entity<MovieLike>()
                .HasOne(ml => ml.Movie)
                .WithMany(m => m.MovieLikes)
                .HasForeignKey(ml => ml.MovieId);

            modelBuilder.Entity<MovieLike>()
                .HasOne(ml => ml.User)
                .WithMany(u => u.MovieLikes)
                .HasForeignKey(ml => ml.UserId);

            modelBuilder.Entity<MovieDislike>()
                .HasKey(md => new { md.MovieId, md.UserId });

            modelBuilder.Entity<MovieDislike>()
                .HasOne(md => md.Movie)
                .WithMany(m => m.MovieDislikes)
                .HasForeignKey(md => md.MovieId);

            modelBuilder.Entity<MovieDislike>()
                .HasOne(md => md.User)
                .WithMany(u => u.MovieDislikes)
                .HasForeignKey(md => md.UserId);

            modelBuilder.Entity<MovieComment>()
                .HasKey(mc => mc.MovieCommentId);

            modelBuilder.Entity<MovieComment>()
                .HasOne(mc => mc.Movie)
                .WithMany(m => m.MovieComments)
                .HasForeignKey(mc => mc.MovieId);

            modelBuilder.Entity<MovieComment>()
                .HasOne(mc => mc.User)
                .WithMany(u => u.MovieComments)
                .HasForeignKey(mc => mc.UserId);

            modelBuilder.Entity<MovieWatchList>()
                .HasKey(mw => new { mw.MovieId, mw.UserId });

            modelBuilder.Entity<MovieWatchList>()
                .HasOne(mw => mw.Movie)
                .WithMany(m => m.MovieWatchLists)
                .HasForeignKey(mw => mw.MovieId);

            modelBuilder.Entity<MovieWatchList>()
                .HasOne(mw => mw.User)
                .WithMany(u => u.MovieWatchLists)
                .HasForeignKey(mw => mw.UserId);

            // User ve TVShow arasındaki Many-to-Many ilişkileri
            modelBuilder.Entity<TVShowLike>()
                .HasKey(tsl => new { tsl.TVShowId, tsl.UserId });

            modelBuilder.Entity<TVShowLike>()
                .HasOne(tsl => tsl.TVShow)
                .WithMany(t => t.TVShowLikes)
                .HasForeignKey(tsl => tsl.TVShowId);

            modelBuilder.Entity<TVShowLike>()
                .HasOne(tsl => tsl.User)
                .WithMany(u => u.TVShowLikes)
                .HasForeignKey(tsl => tsl.UserId);

            modelBuilder.Entity<TVShowDislike>()
                .HasKey(tsd => new { tsd.TVShowId, tsd.UserId });

            modelBuilder.Entity<TVShowDislike>()
                .HasOne(tsd => tsd.TVShow)
                .WithMany(t => t.TVShowDislikes)
                .HasForeignKey(tsd => tsd.TVShowId);

            modelBuilder.Entity<TVShowDislike>()
                .HasOne(tsd => tsd.User)
                .WithMany(u => u.TVShowDislikes)
                .HasForeignKey(tsd => tsd.UserId);

            modelBuilder.Entity<TVShowComment>()
                .HasKey(tsc => tsc.TVShowCommentId);

            modelBuilder.Entity<TVShowComment>()
                .HasOne(tsc => tsc.TVShow)
                .WithMany(t => t.TVShowComments)
                .HasForeignKey(tsc => tsc.TVShowId);

            modelBuilder.Entity<TVShowComment>()
                .HasOne(tsc => tsc.User)
                .WithMany(u => u.TVShowComments)
                .HasForeignKey(tsc => tsc.UserId);

            modelBuilder.Entity<TVShowWatchList>()
                .HasKey(tsw => new { tsw.TVShowId, tsw.UserId });

            modelBuilder.Entity<TVShowWatchList>()
                .HasOne(tsw => tsw.TVShow)
                .WithMany(t => t.TVShowWatchLists)
                .HasForeignKey(tsw => tsw.TVShowId);

            modelBuilder.Entity<TVShowWatchList>()
                .HasOne(tsw => tsw.User)
                .WithMany(u => u.TVShowWatchLists)
                .HasForeignKey(tsw => tsw.UserId);
        }

      
      
    }
}