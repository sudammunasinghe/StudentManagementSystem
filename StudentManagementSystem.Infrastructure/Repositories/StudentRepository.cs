using Dapper;
using StudentManagementSystem.Application.Interfaces.IRepositories;
using StudentManagementSystem.Domain.Entities;
using StudentManagementSystem.Domain.Persistence;
using StudentManagementSystem.Infrastructure.Persistence.Sql.Helpers;

namespace StudentManagementSystem.Infrastructure.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ISqlQueryLoader _queryLoader;

        private readonly string _Select_StudentDetailsByUserId;
        private readonly string _Insert_NewEnrollment;
        private readonly string _Select_EnrollmentDetails;
        public StudentRepository(IDbConnectionFactory connectionFactory, ISqlQueryLoader queryLoader)
        {
            _connectionFactory = connectionFactory;
            _queryLoader = queryLoader;
            _Select_StudentDetailsByUserId = _queryLoader.Load("Student", "Select_StudentDetailsByUserId.sql");
            _Insert_NewEnrollment = _queryLoader.Load("Student", "Insert_NewEnrollment.sql");
            _Select_EnrollmentDetails = _queryLoader.Load("Student", "Select_EnrollmentDetails.sql");
        }

        public async Task<Student?> GetStudentByUserIdAsync(int userId)
        {
            using var db = _connectionFactory.CreateConnection();
            return await db.QueryFirstOrDefaultAsync<Student>(_Select_StudentDetailsByUserId, new { UserId = userId });
        }

        public async Task EnrollToCourseAsync(int studentId, int courseId)
        {
            using var db = _connectionFactory.CreateConnection();
            await db.ExecuteAsync(_Insert_NewEnrollment, new { StudentId = studentId, CourseId = courseId });
        }

        public async Task<Enrollment?> GetEnrollmentDetailsAsync(int studentId, int courseId)
        {
            using var db = _connectionFactory.CreateConnection();
            return await db.QueryFirstOrDefaultAsync<Enrollment>(_Select_EnrollmentDetails, new { StudentId = studentId, CourseId = courseId });
        }
    }
}
