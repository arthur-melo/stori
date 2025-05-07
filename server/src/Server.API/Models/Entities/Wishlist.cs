namespace Server.API.Models.Entities;

public partial class Wishlist
{
  public int UserId { get; set; }

  public int BookId { get; set; }

  public DateTime CreatedAt { get; set; }

  public virtual Book Book { get; set; } = null!;

  public virtual User User { get; set; } = null!;
}
