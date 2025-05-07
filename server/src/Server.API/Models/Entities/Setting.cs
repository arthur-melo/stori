namespace Server.API.Models.Entities;

public partial class Setting
{
  public int Id { get; set; }

  public string? Name { get; set; }

  public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
