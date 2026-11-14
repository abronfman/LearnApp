using LearnApp.Api.Data;
using LearnApp.Api.Infrastructure;
using LearnApp.Api.Services;
using LearnApp.Api.Settings;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddDbContext<AppDbContext>(options => 
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));

    if (builder.Environment.IsDevelopment())
    {
        options.EnableSensitiveDataLogging();
    }
});

builder.Services.AddScoped<IContactService, EfContactService>();
builder.Services.AddOptions<ContactSettings>()
    .BindConfiguration(ContactSettings.SectionName)
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services.AddTransient<ITransientProbe, Probe>();   // new instance EVERY time it's requested
builder.Services.AddScoped<IScopedProbe, Probe>();         // one instance PER HTTP REQUEST
builder.Services.AddSingleton<ISingletonProbe, Probe>();   // one instance for the APP'S LIFETIME
builder.Services.AddScoped<ProbeReporter>();

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

const string AngularPolicy = "Angular";
builder.Services.AddCors(options => options.AddPolicy(AngularPolicy, policy => policy
                        .WithOrigins(builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? [])
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .WithExposedHeaders("Ex-Correlation-Id")));



var app = builder.Build();

app.UseMiddleware<RequestTimingMiddleware>();
app.UseExceptionHandler();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseCors(AngularPolicy);

app.UseAuthorization();
app.MapControllers();

app.Run();
