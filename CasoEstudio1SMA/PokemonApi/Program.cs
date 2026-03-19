var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/api/pokemon-id", () =>
{
	var randomValue = Random.Shared.Next(1, 152);
	return Results.Ok(randomValue);
});

app.Run();
