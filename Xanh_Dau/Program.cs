using System.Text;
using DataAccess;
using DataAccess.DAOs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Repository;
using Repository.Interface;
using Xanh_Dau.Services;

var builder = WebApplication.CreateBuilder(args);

// Add configuration for Azure Application Insights
builder.Logging.AddConsole();
builder.Logging.AddDebug();


// Get the secret key from configuration
var secretKey = builder.Configuration["JwtSettings:SecretKey"];
if (string.IsNullOrEmpty(secretKey)) throw new Exception("JWT Secret Key is missing in configuration!");

// Add database context
builder.Services.AddDbContext<BanHangContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container
builder.Services.AddControllersWithViews();

// Configure Azure Blob Storage
var azureBlobConnectionString = builder.Configuration["AzureBlobStorage:ConnectionString"];
var azureBlobContainerName = builder.Configuration["AzureBlobStorage:ContainerName"];
if (string.IsNullOrEmpty(azureBlobConnectionString) || string.IsNullOrEmpty(azureBlobContainerName))
{
    throw new Exception("Azure Blob Storage configuration is missing!");
}

// DI
builder.Services.AddScoped<BanHangContext>();
builder.Services.AddScoped<TokenService>();
builder.Services.AddScoped<FileService>();

// DataAccess
builder.Services.AddScoped<CustomerDAO>();
builder.Services.AddScoped<ProductDAO>();
builder.Services.AddScoped<ProductImageDAO>();
builder.Services.AddScoped<OrderDAO>();
builder.Services.AddScoped<AdminDAO>();
builder.Services.AddScoped<CategoryDAO>();
builder.Services.AddScoped<CartDAO>();
builder.Services.AddScoped<FeedbackDAO>();
builder.Services.AddScoped<AddressDAO>();
builder.Services.AddScoped<CustomProductDAO>();

// Repository
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IAdminRepository, AdminRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductImageRepository, ProductImageRepository>();
builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IFeedbackRepository, FeedbackRepository>();
builder.Services.AddScoped<IAddressRepository, AddressRepository>();
builder.Services.AddScoped<ICustomProductRepository, CustomProductRepository>();

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder => builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

// Configure authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = "MyAuthService",
        ValidAudience = "MyApp",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler(errorApp =>
    {
        errorApp.Run(async context =>
        {
            var logger = errorApp.ApplicationServices.GetRequiredService<ILogger<Program>>();
            var exceptionHandlerPathFeature = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerPathFeature>();
            var exception = exceptionHandlerPathFeature?.Error;

            logger.LogError(exception, "An unhandled exception occurred.");

            context.Response.StatusCode = 500;
            context.Response.ContentType = "text/html";
            await context.Response.WriteAsync("<html><body>An error occurred. Please try again later.</body></html>");
        });
    });
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// Use CORS
app.UseCors("AllowAll");

// Configure routing
app.UseRouting();

// Custom middlewares - after routing but before auth
app.UseMiddleware<JwtCookieMiddleware>();
app.UseMiddleware<UnauthorizedRedirectMiddleware>();

// Authentication & Authorization middleware
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}");

// Add health check endpoint
app.MapGet("/health", () => "Healthy");

app.Run();