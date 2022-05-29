using System.Security.Claims;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

using AutoMapper;
using DinkToPdf.Contracts;
using DinkToPdf;

using inventory_control_of_dep_api.Infrastructure.JwtTokenAuth;
using inventory_control_of_dep_api.Infrastructure.Services.AuthorizationService;
using inventory_control_of_dep_api.Infrastructure.Services.JwtTokenServices;
using inventory_control_of_dep_api.Infrastructure.Swagger;
using inventory_control_of_dep_dal.Extensions;
using inventory_control_of_dep_api.Infrastructure.Services.Mapper;
using inventory_control_of_dep_api.Infrastructure.Services.AuthorizationServices;
using inventory_control_of_dep_api.Infrastructure.Services.Validators.MaterialValueValidators;
using inventory_control_of_dep_api.Infrastructure.Services.Validators.InventoryBookValidators;
using inventory_control_of_dep_api.Infrastructure.Services.Validators.UserValidators;
using inventory_control_of_dep_api.Infrastructure.Utility;
using inventory_control_of_dep_api.Infrastructure.Services.PDFService;

WebApplicationBuilder? builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var tokenSettings = builder.Configuration.GetSection("JwtTokenSettings");
builder.Services.AddAuthentication(JwtAutheticationConstants.SchemeName)
    .AddScheme<JwtAuthenticationOptions, JwtAuthenticationHandler>(JwtAutheticationConstants.SchemeName, null);

builder.Services.AddAuthorization(config =>
{
    config.AddPolicy("ShouldContainMaterialPersonRole",
        options => options.RequireRole("MaterialPerson"));
});

builder.Services.AddAuthorization(config =>
{
    config.AddPolicy("ShouldContainAdminRole",
        options => options.RequireClaim(ClaimTypes.Role, "Admin"));
});

builder.Services.AddAuthorization(config =>
{
    config.AddPolicy("ShouldContainDepHeadRole",
        options => options.RequireClaim(ClaimTypes.Role, "DepHead"));
});

builder.Services.AddAuthorization(config =>
{
    config.AddPolicy("ShouldContainPurchaseDepartmentRole",
        options => options.RequireClaim(ClaimTypes.Role, "PurchaseDepartment"));
});

var allRoles = new string[] { "MaterialPerson", "Admin", "DepHead", "PurchaseDepartment" };

builder.Services.AddAuthorization(config =>
{
    config.AddPolicy("ShouldContainAnyRole",
        options => options.RequireClaim(ClaimTypes.Role, allRoles));
});

builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

builder.Services.AddApiVersioning(setup =>
{
    setup.DefaultApiVersion = new ApiVersion(1, 0);
    setup.AssumeDefaultVersionWhenUnspecified = true;
    setup.ReportApiVersions = true;
});

builder.Services.AddVersionedApiExplorer(setup =>
{
    setup.GroupNameFormat = "'v'VVV";
    setup.SubstituteApiVersionInUrl = true;
});

builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();

builder.Services.Configure<JwtTokenSettings>(tokenSettings);
builder.Services.AddScoped(typeof(IJwtTokenService), typeof(JwtTokenService));

builder.Services
.ConfigureDALServices(builder.Configuration.GetConnectionString("DefaultConnection"));

builder.Services.AddScoped<IAuthorizationService, AuthorizationService>();

builder.Services.AddScoped<IMaterialValueValidator, MaterialValueValidator>();
builder.Services.AddScoped<IInventoryBookValidator, InventoryBookValidator>();
builder.Services.AddScoped<IUserValidator, UserValidator>();

builder.Services.AddScoped<IPdfCreatorService, PdfCreatorService>();

var context = new CustomAssemblyLoadContext();
context.LoadUnmanagedLibrary("../" + Path.Combine(Directory.GetCurrentDirectory(), "libwkhtmltox.dll"));

builder.Services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));

builder.Services.AddSingleton(provider => new MapperConfiguration(cfg =>
{
    cfg.AddProfile(new CategoryMapperProfile());
    cfg.AddProfile(new DepartmentMapperProfile());
    cfg.AddProfile(new FacultyMapperProfile());
    cfg.AddProfile(new OperationsTypeMapperProfile());
    cfg.AddProfile(new PositionMapperProfile());
    cfg.AddProfile(new RoomMapperProfile());
    cfg.AddProfile(new InventoryBookMapperProfile());
    cfg.AddProfile(new MaterialValueMapperProfile());
    cfg.AddProfile(new UserMapperProfile());
    cfg.AddProfile(new AuthorizationMapperProfile());
}).CreateMapper());

   WebApplication? app = builder.Build();

string? port = Environment.GetEnvironmentVariable("PORT");

if (!string.IsNullOrWhiteSpace(port)) { app.Urls.Add("http://*:" + port); }

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

//    app.UseSwagger();
//    app.UseSwaggerUI(options =>
//    {
//        foreach (var description in provider.ApiVersionDescriptions)
//        {
//            options.SwaggerEndpoint(
//                $"/swagger/{description.GroupName}/swagger.json",
//                description.GroupName.ToUpperInvariant());
//        }
//    });
//}

var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    foreach (var description in provider.ApiVersionDescriptions)
    {
        options.SwaggerEndpoint(
            $"/swagger/{description.GroupName}/swagger.json",
            description.GroupName.ToUpperInvariant());
    }
});

app.UseCors(policy => policy
.AllowAnyOrigin()
.AllowAnyMethod()
.AllowAnyHeader());

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
