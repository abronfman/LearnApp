using LearnApp.Api.Services;
using LearnApp.Api.Settings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddSingleton<IContactService, InMemoryContactService>();

builder.Services.AddTransient<ITransientProbe, Probe>();   // new instance EVERY time it's requested
builder.Services.AddScoped<IScopedProbe, Probe>();         // one instance PER HTTP REQUEST
builder.Services.AddSingleton<ISingletonProbe, Probe>();   // one instance for the APP'S LIFETIME
builder.Services.AddScoped<ProbeReporter>();

builder.Services.AddOptions<ContactSettings>()
    .BindConfiguration(ContactSettings.SectionName)
    .ValidateDataAnnotations()
    .ValidateOnStart();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.MapControllers();

app.Run();
