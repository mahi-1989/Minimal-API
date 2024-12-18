using System.Net;
using System.Text.Json.Serialization;
using Carter;
using Carter.ModelBinding;
using Carter.Response;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureCors();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.ConfigureIISIntegration();

builder.Services.AddCarter();
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IPhoneBookService, PhoneBookService>();
builder.Services.AddScoped<IFrameworkService, FrameworkService>();
builder.Services.AddScoped<IConfigService, ConfigService>();
builder.Services.AddScoped<IOrganizationFlowService, OrganizationFlowService>();
builder.Services.AddSingleton<IApiService, ApiService>();

builder.Services.ConfigureIntranetContext(builder.Configuration);

builder.Services.AddHttpClient();


builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();
app.UseCors("CorsPolicy");
app.UseMiddleware<ApiLoginMiddleware>();


app.MapCarter();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseSession();



app.MapGet("/", () => "Api is running...");

app.Run();
