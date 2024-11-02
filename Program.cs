using Polly;
using TodoApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddScoped<ITodoService, TodoService>();

builder.Services.AddHttpClient(
    "jsonplaceholder",
    client => {
        client.BaseAddress = new Uri("http://localhost:3000");
    }
)    
.AddPolicyHandler((request) =>
{
    // Aplica la política de reintento solo si el método es GET
    if (request.Method == HttpMethod.Get)
    {
        return Policy
            .Handle<HttpRequestException>()
            .OrResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
            .WaitAndRetryAsync(
                3, // Número de reintentos
                retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                onRetry: (outcome, timespan, retryAttempt, context) =>
                {
                    // Muestra un mensaje en consola con el número de reintento
                    Console.WriteLine($"Reintento #{retryAttempt} después de {timespan.Seconds} segundos debido a: {outcome.Exception?.Message ?? outcome.Result.ReasonPhrase}");
                }
            );
    }
    
    // Si no es GET, devuelve una política de no-op (sin reintento)
    return Policy.NoOpAsync<HttpResponseMessage>();
});
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
