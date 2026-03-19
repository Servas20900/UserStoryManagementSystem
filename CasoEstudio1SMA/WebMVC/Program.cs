var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient("StoryApi", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiSettings:StoryApiBaseUrl"] ?? "http://localhost:5003/");
});
builder.Services.AddHttpClient("EstimationApi", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiSettings:EstimationApiBaseUrl"] ?? "http://localhost:5216/");
});
builder.Services.AddHttpClient("PokeApi", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiSettings:PokeApiBaseUrl"] ?? "https://pokeapi.co/");
});
builder.Services.AddScoped<WebMVC.Services.IUserStoryOrchestrationService, WebMVC.Services.UserStoryOrchestrationService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
