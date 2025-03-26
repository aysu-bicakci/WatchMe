using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace WatchMe.Models
{
public class User
{
    public int UserId { get; set; }
    public required string Nickname { get; set; }
    public required string Email { get; set; }

    // Password, hash'lenmiş haliyle veritabanında saklanacak
    public required string Password { get; set; }
    public string? ResetToken { get; set; } // Token nullable olabilir
    public DateTime? ResetTokenExpiry { get; set; } // Token geçerlilik süresi

    // Şifreyi doğrulamak için kullanılan metot
    public bool VerifyPassword(string enteredPassword)
    {
        // Şifre doğrulama sırasında hash'lenmiş şifre ile karşılaştırma yapılır
        var enteredPasswordHash = HashPassword(enteredPassword);
        return Password == enteredPasswordHash;
    }

    // Hashleme metodu (şifre doğrulama için kullanılacak)
    private string HashPassword(string password)
    {
        using (var sha256 = SHA256.Create())
        {
            var passwordBytes = Encoding.UTF8.GetBytes(password);
            var hashedBytes = sha256.ComputeHash(passwordBytes);
            return Convert.ToBase64String(hashedBytes);
        }
    }



    public ICollection<MovieLike>? MovieLikes { get; set; } = new List<MovieLike>();
    public ICollection<TVShowLike>? TVShowLikes { get; set; } = new List<TVShowLike>();
    public ICollection<MovieDislike>? MovieDislikes { get; set; } = new List<MovieDislike>();
    public ICollection<TVShowDislike>? TVShowDislikes { get; set; } = new List<TVShowDislike>();
    public ICollection<MovieComment>? MovieComments { get; set; } = new List<MovieComment>();
    public ICollection<TVShowComment>? TVShowComments { get; set; } = new List<TVShowComment>();
    public ICollection<MovieWatchList>? MovieWatchLists { get; set; } = new List<MovieWatchList>();
    public ICollection<TVShowWatchList>? TVShowWatchLists { get; set; } = new List<TVShowWatchList>();
}

public class Movie
{
    public int MovieId { get; set; }
    public string Title { get; set; } = null!;
    public double? Popularity { get; set; }
    public string? Overview { get; set; }
    public DateTime? ReleaseDate { get; set; }
    public string? PosterPath { get; set; }

    public ICollection<MovieGenre>? MovieGenres { get; set; } = new List<MovieGenre>();
    public ICollection<MovieLike>? MovieLikes { get; set; } = new List<MovieLike>();
    public ICollection<MovieDislike>? MovieDislikes { get; set; } = new List<MovieDislike>();
    public ICollection<MovieComment>? MovieComments { get; set; } = new List<MovieComment>();
    public ICollection<MovieWatchList>? MovieWatchLists { get; set; } = new List<MovieWatchList>();
}

public class TVShow
{
    public int TVShowId { get; set; }
    public string? Title { get; set; }
    public double? Popularity { get; set; }
    public string? Overview { get; set; }
    public DateTime? ReleaseDate { get; set; }
    public string? PosterPath { get; set; }

    public ICollection<TVShowGenre>? TVShowGenres { get; set; } = new List<TVShowGenre>();
    public ICollection<TVShowLike>? TVShowLikes { get; set; } = new List<TVShowLike>();
    public ICollection<TVShowDislike>? TVShowDislikes { get; set; } = new List<TVShowDislike>();
    public ICollection<TVShowComment>? TVShowComments { get; set; } = new List<TVShowComment>();
    public ICollection<TVShowWatchList>? TVShowWatchLists { get; set; } = new List<TVShowWatchList>();
}

public class Genre
{
  
    public int GenreId { get; set; }
    public string? Name { get; set; } // Eklenen 'required' ile çözüm


    public ICollection<MovieGenre>? MovieGenres { get; set; } = new List<MovieGenre>();
    public ICollection<TVShowGenre>? TVShowGenres { get; set; } = new List<TVShowGenre>();
}

// Relationship Entities
public class MovieGenre
{
    public int MovieId { get; set; }
    public Movie Movie { get; set; } = null!;

    public int GenreId { get; set; }
    public Genre Genre { get; set; } = null!;
}

public class TVShowGenre
{
    public int TVShowId { get; set; }
    public TVShow TVShow { get; set; } = null!;

    public int GenreId { get; set; }
    public Genre Genre { get; set; } = null!;
}

public class MovieLike
{
    public int MovieLikeId { get; set; }
    public int MovieId { get; set; }
    public Movie Movie { get; set; } = null!;

    public int UserId { get; set; }
    public User User { get; set; } = null!;
}

public class TVShowLike
{
    public int TVShowLikeId { get; set; }
    public int TVShowId { get; set; }
    public TVShow TVShow { get; set; } = null!;

    public int UserId { get; set; }
    public User User { get; set; } = null!;
}

public class MovieDislike
{
    public int MovieDislikeId { get; set; }
    public int MovieId { get; set; }
    public Movie Movie { get; set; } = null!;

    public int UserId { get; set; }
    public User User { get; set; } = null!;
}

public class TVShowDislike
{
    public int TVShowDislikeId { get; set; }
    public int TVShowId { get; set; }
    public TVShow TVShow { get; set; } = null!;

    public int UserId { get; set; }
    public User User { get; set; } = null!;
}

public class MovieComment
{
    public int MovieCommentId { get; set; }
    public int MovieId { get; set; }
    public Movie Movie { get; set; } = null!;

    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public string Comment { get; set; } = string.Empty;
}

public class TVShowComment
{
    public int TVShowCommentId { get; set; }
    public int TVShowId { get; set; }
    public TVShow TVShow { get; set; } = null!;

    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public string Comment { get; set; } = string.Empty;
}

public class MovieWatchList
{
    public int MovieWatchListId { get; set; }
    public int MovieId { get; set; }
    public Movie Movie { get; set; } = null!;

    public int UserId { get; set; }
    public User User { get; set; } = null!;
}

public class TVShowWatchList
{
    public int TVShowWatchListId { get; set; }
    public int TVShowId { get; set; }
    public TVShow TVShow { get; set; } = null!;

    public int UserId { get; set; }
    public User User { get; set; } = null!;
}
}