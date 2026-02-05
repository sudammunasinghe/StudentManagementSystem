using StudentManagementSystem.Application.Interfaces.IRepositories;
using StudentManagementSystem.Application.Interfaces.IServices;
using StudentManagementSystem.Application.Services;
using StudentManagementSystem.Domain.Persistence;
using StudentManagementSystem.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Register Services
builder.Services.AddScoped<IDbConnectionFactory, DbConnectionFactory>();

builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<IStudentRepository, StudentRepository>();

builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<ICourseRepository, CourseRepository>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
