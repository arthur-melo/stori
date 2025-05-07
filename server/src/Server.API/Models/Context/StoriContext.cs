using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Server.API.Models.Entities;
using Server.API.Options;

namespace Server.API.Models.Context;

public partial class StoriContext : DbContext
{
  private readonly StoriDatabaseOptions _settings;

  public StoriContext(
    DbContextOptions<StoriContext> options,
    IOptions<StoriDatabaseOptions> settings
  )
    : base(options)
  {
    _settings = settings.Value;
  }

  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  {
    if (!optionsBuilder.IsConfigured)
    {
      optionsBuilder.UseSqlServer(
        _settings.ConnectionString,
        o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)
      );
    }
  }

  public virtual DbSet<Award> Awards { get; set; }

  public virtual DbSet<Book> Books { get; set; }

  public virtual DbSet<Character> Characters { get; set; }

  public virtual DbSet<Genre> Genres { get; set; }

  public virtual DbSet<Publisher> Publishers { get; set; }

  public virtual DbSet<Rating> Ratings { get; set; }

  public virtual DbSet<Readlist> Readlists { get; set; }

  public virtual DbSet<Review> Reviews { get; set; }

  public virtual DbSet<Setting> Settings { get; set; }

  public virtual DbSet<Token> Tokens { get; set; }

  public virtual DbSet<User> Users { get; set; }

  public virtual DbSet<UserRating> UserRatings { get; set; }

  public virtual DbSet<Wishlist> Wishlists { get; set; }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<Award>(entity =>
    {
      entity.HasKey(e => e.Id).HasName("PK__Award");

      entity.ToTable("Award");

      entity.Property(e => e.Id).HasColumnName("id");
      entity.Property(e => e.Name).HasMaxLength(512).HasColumnName("name");
    });

    modelBuilder.Entity<Book>(entity =>
    {
      entity.HasKey(e => e.Id).HasName("PK__Book");

      entity.ToTable("Book");

      entity.Property(e => e.Id).HasColumnName("id");
      entity.Property(e => e.BookFormat).HasMaxLength(64).HasColumnName("book_format");
      entity.Property(e => e.BookId).HasMaxLength(256).HasColumnName("book_id");
      entity.Property(e => e.CoverImg).HasMaxLength(128).HasColumnName("cover_img");
      entity.Property(e => e.Description).HasColumnName("description");
      entity.Property(e => e.Edition).HasMaxLength(512).HasColumnName("edition");
      entity.Property(e => e.Isbn).HasMaxLength(13).HasColumnName("isbn");
      entity.Property(e => e.Language).HasMaxLength(32).HasColumnName("language");
      entity.Property(e => e.Pages).HasColumnName("pages");
      entity.Property(e => e.PublishDate).HasColumnName("publish_date");
      entity.Property(e => e.PublisherId).HasColumnName("publisher_id");
      entity.Property(e => e.Series).HasMaxLength(256).HasColumnName("series");
      entity.Property(e => e.Title).HasMaxLength(256).HasColumnName("title");

      entity
        .HasOne(d => d.Publisher)
        .WithMany(p => p.Books)
        .HasForeignKey(d => d.PublisherId)
        .OnDelete(DeleteBehavior.Cascade)
        .HasConstraintName("FK_Book_Publisher");

      entity
        .HasMany(d => d.Awards)
        .WithMany(p => p.Books)
        .UsingEntity<Dictionary<string, object>>(
          "BookAward",
          r =>
            r.HasOne<Award>()
              .WithMany()
              .HasForeignKey("AwardId")
              .HasConstraintName("FK__Book_Award__Award"),
          l =>
            l.HasOne<Book>()
              .WithMany()
              .HasForeignKey("BookId")
              .HasConstraintName("FK__Book_Award__Book"),
          j =>
          {
            j.HasKey("BookId", "AwardId").HasName("PK__Book_Award");
            j.ToTable("Book_Award");
            j.IndexerProperty<int>("BookId").HasColumnName("book_id");
            j.IndexerProperty<int>("AwardId").HasColumnName("award_id");
          }
        );

      entity
        .HasMany(d => d.Characters)
        .WithMany(p => p.Books)
        .UsingEntity<Dictionary<string, object>>(
          "BookCharacter",
          r =>
            r.HasOne<Character>()
              .WithMany()
              .HasForeignKey("CharacterId")
              .HasConstraintName("FK__Book_Character__Character"),
          l =>
            l.HasOne<Book>()
              .WithMany()
              .HasForeignKey("BookId")
              .HasConstraintName("FK__Book_Character__Book"),
          j =>
          {
            j.HasKey("BookId", "CharacterId").HasName("PK__Book_Character");
            j.ToTable("Book_Character");
            j.IndexerProperty<int>("BookId").HasColumnName("book_id");
            j.IndexerProperty<int>("CharacterId").HasColumnName("character_id");
          }
        );

      entity
        .HasMany(d => d.Settings)
        .WithMany(p => p.Books)
        .UsingEntity<Dictionary<string, object>>(
          "BookSetting",
          r =>
            r.HasOne<Setting>()
              .WithMany()
              .HasForeignKey("SettingId")
              .HasConstraintName("FK__Book_Settings__Setting"),
          l =>
            l.HasOne<Book>()
              .WithMany()
              .HasForeignKey("BookId")
              .HasConstraintName("FK__Book_Setting__Book"),
          j =>
          {
            j.HasKey("BookId", "SettingId").HasName("PK__Book_Setting");
            j.ToTable("Book_Setting");
            j.IndexerProperty<int>("BookId").HasColumnName("book_id");
            j.IndexerProperty<int>("SettingId").HasColumnName("setting_id");
          }
        );
    });

    modelBuilder.Entity<Character>(entity =>
    {
      entity.HasKey(e => e.Id).HasName("PK__Character");

      entity.ToTable("Character");

      entity.Property(e => e.Id).HasColumnName("id");
      entity.Property(e => e.Name).HasMaxLength(128).HasColumnName("name");
    });

    modelBuilder.Entity<Genre>(entity =>
    {
      entity.HasKey(e => e.Id).HasName("PK__Genre");

      entity.ToTable("Genre");

      entity.Property(e => e.Id).HasColumnName("id");
      entity.Property(e => e.Name).HasMaxLength(64).HasColumnName("name");

      entity
        .HasMany(d => d.Books)
        .WithMany(p => p.Genres)
        .UsingEntity<Dictionary<string, object>>(
          "BookGenre",
          r =>
            r.HasOne<Book>()
              .WithMany()
              .HasForeignKey("BookId")
              .HasConstraintName("FK__Book_Genre__Book"),
          l =>
            l.HasOne<Genre>()
              .WithMany()
              .HasForeignKey("GenreId")
              .HasConstraintName("FK__Book_Genre__Genre"),
          j =>
          {
            j.HasKey("GenreId", "BookId").HasName("PK__Book_Genre");
            j.ToTable("Book_Genre");
            j.IndexerProperty<int>("GenreId").HasColumnName("genre_id");
            j.IndexerProperty<int>("BookId").HasColumnName("book_id");
          }
        );
    });

    modelBuilder.Entity<Publisher>(entity =>
    {
      entity.HasKey(e => e.Id).HasName("PK__Publisher");

      entity.ToTable("Publisher");

      entity.Property(e => e.Id).HasColumnName("id");
      entity.Property(e => e.Name).HasMaxLength(256).HasColumnName("name");
    });

    modelBuilder.Entity<Rating>(entity =>
    {
      entity.HasKey(e => e.BookId).HasName("PK__Rating");

      entity.ToTable("Rating");

      entity.Property(e => e.BookId).ValueGeneratedNever().HasColumnName("book_id");
      entity.Property(e => e.Star1).HasColumnName("star_1");
      entity.Property(e => e.Star2).HasColumnName("star_2");
      entity.Property(e => e.Star3).HasColumnName("star_3");
      entity.Property(e => e.Star4).HasColumnName("star_4");
      entity.Property(e => e.Star5).HasColumnName("star_5");
      entity.Property(e => e.StarsAverage).HasColumnName("stars_average");
      entity.Property(e => e.StarsTotal).HasColumnName("stars_total");

      entity
        .HasOne(d => d.Book)
        .WithOne(p => p.Rating)
        .HasForeignKey<Rating>(d => d.BookId)
        .HasConstraintName("FK__Rating__Book");
    });

    modelBuilder.Entity<Readlist>(entity =>
    {
      entity.HasKey(e => new { e.BookId, e.UserId }).HasName("PK__Readlist");

      entity.ToTable("Readlist");

      entity.Property(e => e.BookId).HasColumnName("book_id");
      entity.Property(e => e.UserId).HasColumnName("user_id");
      entity.Property(e => e.CreatedAt).HasColumnName("created_at");

      entity
        .HasOne(d => d.Book)
        .WithMany(p => p.Readlists)
        .HasForeignKey(d => d.BookId)
        .HasConstraintName("FK__Readlist__Book");

      entity
        .HasOne(d => d.User)
        .WithMany(p => p.Readlists)
        .HasForeignKey(d => d.UserId)
        .HasConstraintName("FK__Readlist__User");
    });

    modelBuilder.Entity<Review>(entity =>
    {
      entity.HasKey(e => e.Id).HasName("PK__Review");

      entity.ToTable("Review");

      entity.Property(e => e.Id).HasColumnName("id");
      entity.Property(e => e.BookId).HasColumnName("book_id");
      entity.Property(e => e.CreatedAt).HasColumnName("created_at");
      entity.Property(e => e.Text).HasMaxLength(1024).HasColumnName("text");
      entity.Property(e => e.UserId).HasColumnName("user_id");

      entity
        .HasOne(d => d.Book)
        .WithMany(p => p.Reviews)
        .HasForeignKey(d => d.BookId)
        .HasConstraintName("FK__Review__Book");

      entity
        .HasOne(d => d.User)
        .WithMany(p => p.Reviews)
        .HasForeignKey(d => d.UserId)
        .HasConstraintName("FK__Review__User");
    });

    modelBuilder.Entity<Setting>(entity =>
    {
      entity.HasKey(e => e.Id).HasName("PK__Setting");

      entity.ToTable("Setting");

      entity.Property(e => e.Id).HasColumnName("id");
      entity.Property(e => e.Name).HasMaxLength(128).HasColumnName("name");
    });

    modelBuilder.Entity<Token>(entity =>
    {
      entity.HasKey(e => e.Id).HasName("PK__Token");

      entity.ToTable("Token");

      entity.Property(e => e.Id).ValueGeneratedNever().HasColumnName("id");
      entity.Property(e => e.Expiration).HasColumnName("expiration");
      entity.Property(e => e.RefreshToken).HasMaxLength(60).HasColumnName("refresh_token");

      entity
        .HasOne(d => d.IdNavigation)
        .WithOne(p => p.Token)
        .HasForeignKey<Token>(d => d.Id)
        .HasConstraintName("FK__Token__User");
    });

    modelBuilder.Entity<User>(entity =>
    {
      entity.HasKey(e => e.Id).HasName("PK__User");

      entity.ToTable("User");

      entity.Property(e => e.Id).HasColumnName("id");
      entity.Property(e => e.CreatedAt).HasColumnName("created_at");
      entity.Property(e => e.Email).HasMaxLength(256).HasColumnName("email");
      entity.Property(e => e.Name).HasMaxLength(64).HasColumnName("name");
      entity.Property(e => e.Password).HasMaxLength(256).HasColumnName("password");
      entity.Property(e => e.ProfileImg).HasMaxLength(128).HasColumnName("profile_img");
      entity.Property(e => e.Username).HasMaxLength(32).HasColumnName("username");
    });

    modelBuilder.Entity<UserRating>(entity =>
    {
      entity.HasKey(e => new { e.UserId, e.BookId }).HasName("PK__UserRating");

      entity.ToTable("UserRating");

      entity.Property(e => e.UserId).HasColumnName("user_id");
      entity.Property(e => e.BookId).HasColumnName("book_id");
      entity.Property(e => e.CreatedAt).HasColumnName("created_at");
      entity.Property(e => e.Rating).HasColumnName("rating");

      entity
        .HasOne(d => d.Book)
        .WithMany(p => p.UserRatings)
        .HasForeignKey(d => d.BookId)
        .HasConstraintName("FK__UserRating__Book");

      entity
        .HasOne(d => d.User)
        .WithMany(p => p.UserRatings)
        .HasForeignKey(d => d.UserId)
        .HasConstraintName("FK__UserRating__User");
    });

    modelBuilder.Entity<Wishlist>(entity =>
    {
      entity.HasKey(e => new { e.UserId, e.BookId }).HasName("PK__Wishlist");

      entity.ToTable("Wishlist");

      entity.Property(e => e.UserId).HasColumnName("user_id");
      entity.Property(e => e.BookId).HasColumnName("book_id");
      entity.Property(e => e.CreatedAt).HasColumnName("created_at");

      entity
        .HasOne(d => d.Book)
        .WithMany(p => p.Wishlists)
        .HasForeignKey(d => d.BookId)
        .HasConstraintName("FK__Wishlist__Book");

      entity
        .HasOne(d => d.User)
        .WithMany(p => p.Wishlists)
        .HasForeignKey(d => d.UserId)
        .HasConstraintName("FK__Wishlist__User");
    });

    OnModelCreatingPartial(modelBuilder);
  }

  partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
