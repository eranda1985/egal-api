using LinearServices;
using Microsoft.AspNetCore.Mvc;
using Serilog;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    ApplicationName = typeof(Program).Assembly.FullName,
    ContentRootPath = Directory.GetCurrentDirectory()
});

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddWebEncoders();
builder.Services.AddLogging((loggerFactory) =>
{
    loggerFactory.ClearProviders();
    loggerFactory.AddSerilog(new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration).CreateLogger());
});

builder.Services.AddTransient<ILinearService, LinearService>();

// Register any services here for DI. 

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.UseForwardedHeaders(); // Forward headers in case we are behind a reverse proxy. 

// Specify end-point routing. Route handlers are specifed as inline functions. 
app.MapGet("/linear", () => { return "Welcome to Linear Regression API."; });

app.MapPost("/linear/regression", async ([FromBody] RegressionRequest req) =>
 {
     try
     {
         var service = app.Services.GetRequiredService<ILinearService>();
         return Results.Ok(await service.GetLinearRegression(req.url));
     }
     catch (Exception ex)
     {
         app.Logger.LogError($"{ex.Message} ---- {ex.StackTrace}");
         return Results.Problem($"Error processing the csv file. Details: {ex.Message}");
     }
 })
 .Produces<LinearRegressionResult>(200)
 .Produces(StatusCodes.Status404NotFound);

app.MapPost("/linear/plot", async ([FromBody] RegressionRequest req) =>
{
    try
    {
        var service = app.Services.GetRequiredService<ILinearService>();
        var base64Str = await service.GetGraph(req.url);
        var bytes = Convert.FromBase64String(base64Str);
        return Results.File(bytes, "image/png");
    }
    catch (Exception ex)
    {
        app.Logger.LogError($"{ex.Message} ---- {ex.StackTrace}");
        return Results.Problem($"Error generating the graph. Details: {ex.Message}");
    }
});

app.Run();
