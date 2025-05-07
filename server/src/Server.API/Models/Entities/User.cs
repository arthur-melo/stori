namespace Server.API.Models.Entities;

public partial class User
{
  public int Id { get; set; }

  public string Email { get; set; } = null!;

  public string Password { get; set; } = null!;

  public string Username { get; set; } = null!;

  public string Name { get; set; } = null!;

  public string? ProfileImg { get; set; }

  public DateTime CreatedAt { get; set; }

  public virtual ICollection<Readlist> Readlists { get; set; } = new List<Readlist>();

  public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

  public virtual Token? Token { get; set; }

  public virtual ICollection<UserRating> UserRatings { get; set; } = new List<UserRating>();

  public virtual ICollection<Wishlist> Wishlists { get; set; } = new List<Wishlist>();
}
