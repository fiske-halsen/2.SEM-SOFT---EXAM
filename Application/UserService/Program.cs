using Microsoft.EntityFrameworkCore;
using UserService.Context;
using UserService.ErrorHandling;
using UserService.IdentityConfig;
using UserService.Repository;
using UserService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var configuration = builder.Configuration;


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers().AddNewtonsoftJson(options =>
                   options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

builder.Services.AddHttpContextAccessor();

builder.Services.AddIdentityServer()
    .AddDeveloperSigningCredential()
    .AddInMemoryApiResources(IdentityConfiguration.ApiResources(configuration))
    .AddInMemoryApiScopes(IdentityConfiguration.ApiScopes)
    .AddInMemoryClients(IdentityConfiguration.Clients(configuration))
    .AddProfileService<ProfileService>()
    .AddResourceOwnerValidator<ResourceOwnerPasswordValidator>();

builder.Services.AddDbContext<UserDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UsersService>();
builder.Services.AddScoped<IUserProducer, UserProducer>();
//builder.Services.AddHostedService<UserKafkaConsumer>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<UserDbContext>();
    if (db.Database.IsRelational())
    {
        db.Database.Migrate();
        db.Database.EnsureCreated();
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(x => x
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .SetIsOriginAllowed(origin => true) // allow any origin
                                                        //.WithOrigins("https://localhost:44351")); // Allow only this origin can also have multiple origins separated with comma
                    .AllowCredentials());


app.ConfigureExceptionHandler();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseIdentityServer();
app.MapControllers();
app.Run();

// For integration testing purposes; Woops! Needed because program is behind the scenes a internal class, we need a public way to get it
public partial class Program {}
