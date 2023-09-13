using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using QuestionnaireService.DbContexts;

var mysqlHost = "mysql";

var mysqlUser = "root";

var mysqlPassword = "password";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", builder =>
    {
        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    })
    .ConfigureApiBehaviorOptions(options =>
    {
        // Disables ProblemDetails response
        options.SuppressMapClientErrors = true;
    });
builder.Services.AddDbContext<QuestionnaireDbContext>(options => 
    options
        .UseMySQL($"server={mysqlHost};database=questionnaire;user={mysqlUser};password={mysqlPassword}")
        .LogTo(Console.WriteLine));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors("AllowAllOrigins");

    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<QuestionnaireDbContext>();
    DbInitializer.Initialize(context);
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();