//var builder = WebApplication.CreateBuilder(args);
//var app = builder.Build();
//
//app.MapGet("/", () => "Hello World!");
//
//app.Run();
//

using System.Reflection;

var basePath = AppContext.BaseDirectory;
Console.WriteLine($"EXHL SERVICE: {basePath}");
var startup = new EXHL.ExStartup(basePath);