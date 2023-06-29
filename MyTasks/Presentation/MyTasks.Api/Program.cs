using Auth0.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.OpenApi.Models;
using MyTasks.Application;
using MyTasks.Infrastructure;
using MyTasks.Persistence;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddAuth0WebAppAuthentication(options =>
{
    options.Domain = builder.Configuration["Auth0:Domain"];
    options.ClientId = builder.Configuration["Auth0:ClientId"];
    options.ClientSecret = builder.Configuration["Auth0:ClientSecret"];
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    //options.Cookie.IsEssential = true;
   // options.Cookie.SecurePolicy = CookieSecurePolicy.None;
});

builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.MinimumSameSitePolicy = SameSiteMode.None;
    options.CheckConsentNeeded = context => true;
    //options.Secure = CookieSecurePolicy.Always; // Wymaga ustawienia zabezpieczonego po³¹czenia HTTPS
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowOrigin", builder =>
    {
        builder.WithOrigins(configuration["Origins:Dev"]) // Dodaj pochodzenie, z którego oczekujesz ¿¹dañ
                   .AllowAnyHeader()
                   .AllowAnyMethod()
                   .AllowCredentials();
    });
});


builder.Services.AddHttpContextAccessor();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddPersistence(builder.Configuration);

builder.Services.AddSwaggerGen(
    c =>
    {
        c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo()
        {
            Title = "WebApplication",
            Version = "v1",
            Description = "A simple web application, which user can manage tasks",
            TermsOfService = new Uri("https://example.com/terms"),
            Contact = new OpenApiContact
            {
                Name = "Karol",
                Email = String.Empty,
                Url = new Uri("https://example.com/me"),
            },
            License = new OpenApiLicense
            {
                Name = "Name license",
                Url = new Uri("https://example.com/license"),
            }
        });
        var filePath = Path.Combine(AppContext.BaseDirectory, "MyTasks.Api.xml");
        c.IncludeXmlComments(filePath);
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    /*app.UseCors(
        options => options
        .WithOrigins(builder.Configuration["Origins:Dev"])
        .AllowAnyMethod()
        .AllowAnyOrigin()
        .AllowAnyOrigin()
        .AllowAnyHeader()
        .SetIsOriginAllowed(origin => true)
    ); */
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSession();
app.UseHttpsRedirection();
app.UseCookiePolicy(new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.None,
   // HttpOnly = Microsoft.AspNetCore.CookiePolicy.HttpOnlyPolicy.Always,
    //Secure = CookieSecurePolicy.None
})
    ;
app.UseCors("AuthenticatedPolicy");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapGet("/home", () => "/swagger/index.html");

app.Run();