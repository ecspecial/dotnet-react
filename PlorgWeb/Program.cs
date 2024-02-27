namespace PlorgWeb
{
  public class Program {
    public static void Main() {
      var builder = new AppBuilder();
      var app = builder.Build();
      app.Run();
    }
  }
}
