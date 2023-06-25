using Microsoft.OpenApi.Models;
using MyTasks.Application;
using MyTasks.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();

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
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();