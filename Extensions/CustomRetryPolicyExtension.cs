using System.Net;
using Polly;

namespace clientapi.Extensions;

public static class CustomRetryPolicyExtension
{
    public static IHttpClientBuilder AddCustomRetryPolicy(this IHttpClientBuilder builder, int retryCount)
    {
        return builder.AddPolicyHandler((request) =>
        {
            // Aplica la política de reintento solo si el método es GET
            if (request.Method == HttpMethod.Get)
            {
                return Policy
                    .Handle<HttpRequestException>()
                    .OrResult<HttpResponseMessage>(r => 
                                    r.StatusCode == HttpStatusCode.RequestTimeout || 
                                    r.StatusCode == HttpStatusCode.GatewayTimeout) // Maneja errores de timeout
                    .WaitAndRetryAsync(
                        retryCount, // Número de reintentos
                        retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                        onRetry: (outcome, timespan, retryAttempt, context) =>
                        {
                            // Muestra un mensaje en consola con el número de reintento
                            Console.WriteLine($"Reintento #{retryAttempt} de {retryCount} después de {timespan.Seconds} segundos debido a: {outcome.Exception?.Message ?? outcome.Result.ReasonPhrase}");
                        }
                    );
            }
            
            // Si no es GET, devuelve una política de no-op (sin reintento)
            return Policy.NoOpAsync<HttpResponseMessage>();
        });

    }
}