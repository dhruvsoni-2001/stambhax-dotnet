using Microsoft.EntityFrameworkCore;
using StambhaX.Api.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddSwaggerGen();

// Configure Entity Framework Core with PostgreSQL as default
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection"));
    
    // To switch to SQL Server, uncomment the line below and comment out UseNpgsql:
    // options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServerConnection"));
    
    // To switch to SQLite, uncomment the line below and comment out UseNpgsql:
    // options.UseSqlite(builder.Configuration.GetConnectionString("SqliteConnection"));
});

var app = builder.Build();

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
