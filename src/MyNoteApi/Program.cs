using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyNoteApi.Data;
using MyNoteApi.Data.Initial;
using MyNoteApi.Models.Entities.User;
using MyNoteApi.Repositories.Interfaces.User;
using MyNoteApi.Repositories.Services;
using MyNoteApi.Repositories.Services.User;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.Configure<ApiBehaviorOptions>(options => // Show Errors in Result Format
{
    options.InvalidModelStateResponseFactory = (actionContext) =>
    {
        var errors = actionContext.ModelState.Values.SelectMany(m => m.Errors)
                                  .Select(e => e.ErrorMessage)
                                  .ToList();
        return new JsonResult(Result.Failure(string.Join('\n', errors)).ToResult());
    };
});
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(option => option.UseSqlServer(connectionString)); // Use Sql For Store Data
builder.Services.AddIdentity<AppUser, AppRole>().AddEntityFrameworkStores<AppDbContext>(); // Use Microsoft Identity For Authentication
builder.Services.AddScoped<IUserService, UserService>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IDatabaseInitializer, DatabaseInitializer>();
var app = builder.Build();
using (var serviceScope = app.Services.CreateScope()) // Create/Seed Database (if necessary)
{
    var services = serviceScope.ServiceProvider;
    var dbInitializer = services.GetRequiredService<IDatabaseInitializer>();
    dbInitializer.Initial();
}
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
