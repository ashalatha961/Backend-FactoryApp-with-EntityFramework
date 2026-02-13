using PopsicleFactoryCo.Validators;
using FluentValidation;
using PopsicleFactoryCo.Data;
using PopsicleFactoryCo.CustomMiddleware;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;

// This code sets up a basic ASP.NET Core application with Swagger for API documentation,
// HTTPS redirection, and dependency injection for the Popsicle inventory.

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(); // Adds support for controllers in the application

// Register the ProductDbContext with dependency injection
builder.Services.AddDbContext<PopsicleDBContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("PopsicleFactoryCoDBConnection")));

builder.Services.AddEndpointsApiExplorer(); // Enables API Explorer for Minimal APIs
builder.Services.AddSwaggerGen();

//builder.Services.AddTransient<ExceptionHandlerMiddleware>();
//builder.Services.AddTransient<RequestTimingMiddleware>();
//builder.Services.AddLogging(); // Adds logging services to the application
//AreaRegistration: This line is not needed in ASP.NET Core 6 and later, as the routing is handled automatically by the framework.

// Configure dependency injection
builder.Services.AddValidatorsFromAssemblyContaining<PopsicleValidator>(); // Registers all validators from assembly containing PopsicleValidator
builder.Services.AddScoped<IPopsicleInventory, PopsicleInventory>(); // can also use AddTransient/AddSingleton based on desired lifetime of service
//builder.Services.AddHealthChecks(); // Adds health check services to the application

// configure the HTTP request pipeline using the app instance returned by builder.Build().
var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger(); // Enable Swagger middleware to generate API documentation
app.UseSwaggerUI(); // Enable Swagger UI to visualize and interact with the API endpoints
app.UseHttpsRedirection(); // Enable HTTPS redirection middleware to redirect HTTP requests to HTTPS
app.UseExceptionHandler("/error"); // Global error handling middleware to catch and handle exceptions gracefully
app.UseAuthorization(); // Enable authorization middleware to handle authentication and authorization for the API endpoints

//app.UseMiddleware<RequestTimingMiddleware>(); // Use custom middleware to log request processing time
//app.UseMiddleware<ExceptionHandlerMiddleware>(); // Use custom middleware to handle exceptions globally

app.MapControllers(); // It internally adds Routing and Endpoints middlewares to route HTTP requests to controller.For .NET 6 and lower versions  we need to use UseRouting() and UseEndpoints() explicitly
//app.MapHealthChecks("/health"); // Map health check endpoint to /health
app.Run();



// Can configure middlewares here if needed Ex: app.UseRouting(); app.useAuthentication(); app.UseAuthorization(); app.UseexceptionHandler();
// ex: app.UseMiddleware<CustomMiddleware>(); app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
// or app.UseEndpoints(endpoints => { endpoints.MapGet("/", async context => { await context.Response.WriteAsync("Hello World!"); }); });


/*the application validates JWT tokens issued by an external identity provider before allowing access to protected resources.
builder.Services.AddAuthentication(jwtBearerDefaults =>
{
    jwtBearerDefaults.Authority = "https://your-auth-server.com";
    jwtBearerDefaults.Audience = "your-api-audience";
});


policy-based authorization which allows you to define custom authorization policies
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdministratorRole", policy => policy.RequireRole("Administrator"));
    options.AddPolicy("RequireUserRole", policy => policy.RequireRole("User"));
});
app.useAuthentication(); // Ensure this is placed before app.UseAuthorization()
app.UseAuthorization(); // Ensure this is placed after app.UseAuthentication()
 */

/*
// This should be called after UseRouting and before UseEndpoints to ensure the policy is applied before endpoint execution.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder => builder.WithOrigins("https://example.com")
                          .AllowAnyHeader()
                          .AllowAnyMethod());
});
app.UseCors("AllowSpecificOrigin");*/