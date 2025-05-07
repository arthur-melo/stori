namespace Server.API.Models.Entities;

public partial class Review
{
  public int Id { get; set; }

  public int UserId { get; set; }

  public int BookId { get; set; }

  public string Text { get; set; } = null!;

  public DateTime CreatedAt { get; set; }

  public virtual Book Book { get; set; } = null!;

  public virtual User User { get; set; } = null!;
}
