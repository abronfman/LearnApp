using System.Text;
using LearnApp.Api.Auth;
using LearnApp.Api.Data;
using LearnApp.Api.Infrastructure;
using LearnApp.Api.Services;
using LearnApp.Api.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddOpenApi();


// EF Core
builder.Services.AddDbContext<AppDbContext>(options => 
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));

    if (builder.Environment.IsDevelopment())
    {
        options.EnableSensitiveDataLogging();
    }
});

builder.Services.AddScoped<IContactService, EfContactService>();

// Typed Settings
builder.Services.AddOptions<ContactSettings>()
    .BindConfiguration(ContactSettings.SectionName)
    .ValidateDataAnnotations()
    .ValidateOnStart();

// Demo
builder.Services.AddTransient<ITransientProbe, Probe>();   // new instance EVERY time it's requested
builder.Services.AddScoped<IScopedProbe, Probe>();         // one instance PER HTTP REQUEST
builder.Services.AddSingleton<ISingletonProbe, Probe>();   // one instance for the APP'S LIFETIME
builder.Services.AddScoped<ProbeReporter>();

// Errors and Cors
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

const string AngularPolicy = "Angular";
builder.Services.AddCors(options => options.AddPolicy(AngularPolicy, policy => policy
                        .WithOrigins(builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? [])
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .WithExposedHeaders("Ex-Correlation-Id")));

// Autnentication and Authorization
builder.Services.AddOptions<JwtSettings>()
    .BindConfiguration(JwtSettings.SectionName)
    .ValidateDataAnnotations()
        .ValidateOnStart();

// The JWT handler needs the settings now, while services are still being registered.
var jwt = builder.Configuration.GetSection(JwtSettings.SectionName).Get<JwtSettings>()
          ?? throw new InvalidOperationException("Missing Jwt settings.");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        // Keep claim names exactly as they are in the token ("role", "name", "sub").
        // Without this, the handler renames some of them to long Microsoft URIs (see Break 2).
        options.MapInboundClaims = false;

        // The checks run on EVERY incoming token. Any failure means 401.
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwt.Issuer,       // made by us?
            ValidateAudience = true,
            ValidAudience = jwt.Audience,   // meant for us?
            ValidateIssuerSigningKey = true,                                 // signature intact?
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.SigningKey)),
            ValidateLifetime = true,                                         // not expired?
            ClockSkew = TimeSpan.FromSeconds(30),   // the default tolerates 5 minutes of clock drift
            NameClaimType = "name",                  // what User.Identity.Name reads
            RoleClaimType = "role"                   // what [Authorize(Roles = ...)] and RequireRole check
        };
    });

builder.Services.AddAuthorization(options =>
{
    // A named policy: ONE place that says who may edit contacts.
    options.AddPolicy(Policies.CanEditContacts, p => p.RequireRole("Admin", "CaseManager"));

    // Secure by default: any endpoint WITHOUT its own rule still requires a signed-in user.
    // New endpoints are protected automatically; you opt out explicitly with [AllowAnonymous].
    options.FallbackPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
});

builder.Services.AddSingleton<IDemoUserStore, DemoUserStore>();   // fixed data, safe to share
builder.Services.AddSingleton<ITokenService, TokenService>();     // stateless; IOptions is a singleton too

var app = builder.Build();

// =============== PIPELINE: every request runs through these IN ORDER ===============

app.UseMiddleware<RequestTimingMiddleware>();
app.UseExceptionHandler();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseCors(AngularPolicy);

app.UseAuthentication();   // Step 4: reads and validates the token, then sets HttpContext.User
app.UseAuthorization();    // checks the endpoint's rule against that user (MUST come after authentication)
app.MapControllers();

app.Run();
