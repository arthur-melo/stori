namespace Server.API.Models;

public class Envelope<T>
{
  public List<T> Data { get; }

  public Envelope(List<T> data)
  {
    Data = data;
  }
}
