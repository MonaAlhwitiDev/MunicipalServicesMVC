using Microsoft.EntityFrameworkCore;
using MunicipalServicesMVC.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// =========================
// Controllers
// =========================
builder.Services.AddControllers();

// =========================
// Database - SQL Server
// =========================
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultDatabase")
    )
);

// =========================
// Swagger
// =========================
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// =========================
// Swagger in Development
// =========================
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// =========================
// HTTP Pipeline
// =========================
app.UseHttpsRedirection();

app.UseAuthorization();

// =========================
// API Controllers
// =========================
app.MapControllers();

app.Run();