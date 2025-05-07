namespace Server.API.Models.Entities;

public partial class Genre
{
  public int Id { get; set; }

  public string? Name { get; set; }

  public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
