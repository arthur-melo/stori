namespace Server.API.Models.Entities;

public partial class Rating
{
  public int BookId { get; set; }

  public int? Star1 { get; set; }

  public int? Star2 { get; set; }

  public int? Star3 { get; set; }

  public int? Star4 { get; set; }

  public int? Star5 { get; set; }

  public double? StarsAverage { get; set; }

  public int? StarsTotal { get; set; }

  public virtual Book Book { get; set; } = null!;
}
