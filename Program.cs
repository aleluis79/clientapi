using clientapi.Extensions;
using TodoApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddScoped<ITodoService, TodoService>();

// Add HttpClient with custom retry policy

builder.Services.AddHttpClient("jsonplaceholder",
    client => {
        client.BaseAddress = new Uri("http://localhost:3000");
    }
).AddCustomRetryPolicy(5);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
