var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/api/estimation", () =>
{
	int[] fibonacciValues = { 2, 3, 5, 8, 13 };
	int randomValue = fibonacciValues[Random.Shared.Next(fibonacciValues.Length)];
	return Results.Ok(randomValue);
});

app.Run();
