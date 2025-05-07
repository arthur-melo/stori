namespace Server.API.Models.Entities;

public partial class Book
{
  public int Id { get; set; }

  public int? PublisherId { get; set; }

  public string BookId { get; set; } = null!;

  public string Title { get; set; } = null!;

  public string? Series { get; set; }

  public string? Description { get; set; }

  public string? Language { get; set; }

  public string Isbn { get; set; } = null!;

  public string? BookFormat { get; set; }

  public string? Edition { get; set; }

  public int? Pages { get; set; }

  public DateOnly? PublishDate { get; set; }

  public string CoverImg { get; set; } = null!;

  public virtual Publisher? Publisher { get; set; }

  public virtual Rating? Rating { get; set; }

  public virtual ICollection<Readlist> Readlists { get; set; } = new List<Readlist>();

  public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

  public virtual ICollection<UserRating> UserRatings { get; set; } = new List<UserRating>();

  public virtual ICollection<Wishlist> Wishlists { get; set; } = new List<Wishlist>();

  public virtual ICollection<Award> Awards { get; set; } = new List<Award>();

  public virtual ICollection<Character> Characters { get; set; } = new List<Character>();

  public virtual ICollection<Genre> Genres { get; set; } = new List<Genre>();

  public virtual ICollection<Setting> Settings { get; set; } = new List<Setting>();
}
