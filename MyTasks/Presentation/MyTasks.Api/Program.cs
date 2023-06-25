using Auth0.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.OpenApi.Models;
using MyTasks.Application;
using MyTasks.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//builder.Services.ConfigureSameSiteNoneCookies();
builder.Services.AddAuth0WebAppAuthentication(options =>
{
    options.Domain = builder.Configuration["Auth0:Domain"];
    options.ClientId = builder.Configuration["Auth0:ClientId"];
    options.ClientSecret = builder.Configuration["Auth0:ClientSecret"];
    /*
    options.CookieAuthenticationScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.ClientSecret = builder.Configuration["Auth0:ClientSecret"];
    options.ResponseType = OpenIdConnectResponseType.Token;
    options.CallbackPath = new PathString("/callback");
    options.CallbackPath = "/callback";
    options.OpenIdConnectEvents = new OpenIdConnectEvents
    {
        OnRedirectToIdentityProviderForSignOut = (context) =>
        {
            var loqoutUri = $"https://{builder.Configuration["Auth0:Domain"]}/v2/loqout?client_id={builder.Configuration["Auth0:ClientId"]}";

            var postLoqoutUri = context.Properties.RedirectUri;
            if (!string.IsNullOrEmpty(postLoqoutUri))
            {
                if (postLoqoutUri.StartsWith("/"))
                {
                    var request = context.Request;
                    postLoqoutUri = request.Scheme + "://" + request.Host + request.PathBase + postLoqoutUri;
                }
                loqoutUri += $"&returnTo={Uri.EscapeDataString(postLoqoutUri)}";
            }

            context.Response.Redirect(loqoutUri);
            context.HandleResponse();

            return Task.CompletedTask;
        }
    };*/
});

//builder.Services.AddAuth0ManagementClient().AddManagementAccessToken();


/*
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;

}).AddCookie().AddOpenIdConnect("Auth0", options =>
{
    options.Authority = $"https://{builder.Configuration["Auth0:Domain"]}";
    options.ClientId = builder.Configuration["Auth0:ClientId"];
    options.ClientSecret = builder.Configuration["Auth0:ClientSecret"];
    options.ResponseType = OpenIdConnectResponseType.Code;
    options.Scope.Clear();
    options.Scope.Add("openid");
    options.CallbackPath = new PathString("/callback");
    options.ClaimsIssuer = "Auth0";
    options.Events = new OpenIdConnectEvents
    {
        OnRedirectToIdentityProviderForSignOut = (context) =>
        {
            var loqoutUri = $"https://{builder.Configuration["Auth0:Domain"]}/v2/loqout?client_id={builder.Configuration["Auth0:ClientId"]}";

            var postLoqoutUri = context.Properties.RedirectUri;
            if (!string.IsNullOrEmpty(postLoqoutUri))
            {
                if (postLoqoutUri.StartsWith("/"))
                {
                    var request = context.Request;
                    postLoqoutUri = request.Scheme + "://" + request.Host + request.PathBase + postLoqoutUri;
                }
                loqoutUri += $"&returnTo={Uri.EscapeDataString(postLoqoutUri)}";
            }

            context.Response.Redirect(loqoutUri);
            context.HandleResponse();

            return Task.CompletedTask;
        }
    };
});
*/
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
app.UseCookiePolicy(new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.None,
    Secure = CookieSecurePolicy.Always
});
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapGet("/home", () => "/swagger/index.html");

app.Run();