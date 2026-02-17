using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using TaskFlow.API.Middleware;
using TaskFlow.Application;
using TaskFlow.Infrastructure;
using TaskFlow.Infrastructure.Persistence;


var builder = WebApplication.CreateBuilder(args);

// __ Layers______________________________________________
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();

// __ API Services________________________________________
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(
    options =>
    {
        options.SwaggerDoc("v1", new()
        {
            Title = "TaskFlow API",
            Version = "v1",
            Description = "A task managment REST API"
        });
    });

// __ Exception Handling__________________________________
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

// __ Auto-Migrate on Startup_____________________________
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<TaskFlowDbContext>();
    await dbContext.Database.MigrateAsync();
}

// __ Middleware Pipeline_________________________________
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "TaskFlow API v1");
        options.RoutePrefix = string.Empty; // Swagger at root URL
    });
}

app.UseExceptionHandler();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();

