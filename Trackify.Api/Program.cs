using Microsoft.EntityFrameworkCore;
using Trackify.Data;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Trackify API",
        Version = "v1",
        Description = "API cho hệ thống tự động theo dõi giá sản phẩm từ các trang thương mại điện tử tại Việt Nam.",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Trackify Dev Team",
            Email = ""
        }
    });
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(options =>
    {
        options.RouteTemplate = "openapi/{documentName}.json";
    });
    app.MapScalarApiReference(options =>
    {
        options.WithTitle("Trackify API")
               .WithTheme(ScalarTheme.Default);
    });
}

// Test database connection at startup
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        if (context.Database.CanConnect())
        {
            app.Logger.LogInformation("================================================");
            app.Logger.LogInformation("DATABASE CONNECTION VERIFIED SUCCESSFULLY!");
            app.Logger.LogInformation("================================================");
        }
        else
        {
            app.Logger.LogError("DATABASE CONNECTION FAILED!");
        }
    }
    catch (Exception ex)
    {
        app.Logger.LogError(ex, "An error occurred while testing database connection.");
    }
}

app.Run();


