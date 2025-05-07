namespace Server.API.Models.Entities;

public partial class Token
{
  public int Id { get; set; }

  public string RefreshToken { get; set; } = null!;

  public DateTime Expiration { get; set; }

  public virtual User IdNavigation { get; set; } = null!;
}
