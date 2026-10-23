using Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSingleton<ITodoService, TodoService>();

var app = builder.Build();

app.MapControllers();

app.Run();
