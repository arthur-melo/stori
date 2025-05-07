namespace Server.UnitTests.Helpers;

public static class QueryableUtils
{
  public static IQueryable<T> MapToIQueryable<T>(T obj)
  {
    return new List<T> { obj }.AsQueryable();
  }
}
