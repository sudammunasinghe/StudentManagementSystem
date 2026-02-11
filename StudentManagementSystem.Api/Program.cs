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

builder.Services.AddScoped<IInstructorService, InstructorService>();
builder.Services.AddScoped<IInstructorRepository, InstructorRepository>();

builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<ICourseRepository, CourseRepository>();

builder.Services.AddScoped<IEnrollService, EnrollService>();
builder.Services.AddScoped<IEnrollRepository, EnrollRepository>();

builder.Services.AddScoped<IEnrollmentApprovalService, EnrollmentApprovalService>();
builder.Services.AddScoped<IEnrollmentApprovalRepository, EnrollmentApprovalRepository>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

//Register global exception
app.UseMiddleware<StudentManagementSystem.Api.Middleware.GlobalExceptionMiddleware>();

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
