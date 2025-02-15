using weather1.Services;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using weather1;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;
using Microsoft.Extensions.Configuration;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddMudServices();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<IWeatherFoodService, WeatherFoodService>();
// builder.Services.AddScoped<IGeolocationService, GeolocationService>();


builder.Services.AddSingleton(_ => new Supabase.Client(
    "https://rmpibggfdpvtdinotvgc.supabase.co",
    "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6InJtcGliZ2dmZHB2dGRpbm90dmdjIiwicm9sZSI6ImFub24iLCJpYXQiOjE3Mzg4MTcxNjUsImV4cCI6MjA1NDM5MzE2NX0.tUuLhRAVXcU213FnQELyvV1VYQ64wAwuc8E6Dm4efzY"
));

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddMudServices();
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddHttpClient<WindyService>();
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

                
await builder.Build().RunAsync();
