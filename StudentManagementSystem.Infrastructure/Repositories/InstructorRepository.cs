using Dapper;
using StudentManagementSystem.Application.Interfaces.IRepositories;
using StudentManagementSystem.Domain.Entities;
using StudentManagementSystem.Domain.Persistence;
using StudentManagementSystem.Infrastructure.Persistence.Sql.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagementSystem.Infrastructure.Repositories
{
    public class InstructorRepository : IInstructorRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ISqlQueryLoader _queryLoader;

        private readonly string _Select_InstructorDetailsByUserId;
        private readonly string _Insert_NewCourse;
        private readonly string _Select_CourseDetailsByCourseId;
        private readonly string _Insert_CourseContent;
        private readonly string _Select_CourseDetailsAndContents;
        public InstructorRepository(IDbConnectionFactory connectionFactory, ISqlQueryLoader queryLoader)
        {
            _connectionFactory = connectionFactory;
            _queryLoader = queryLoader;
            _Select_InstructorDetailsByUserId = _queryLoader.Load("Instructor", "Select_InstructorDetailsByUserId.sql");
            _Insert_NewCourse = _queryLoader.Load("Instructor", "Insert_NewCourse.sql");
            _Select_CourseDetailsByCourseId = _queryLoader.Load("Instructor", "Select_CourseDetailsByCourseId.sql");
            _Insert_CourseContent = _queryLoader.Load("Instructor", "Insert_CourseContent.sql");
            _Select_CourseDetailsAndContents = _queryLoader.Load("Instructor", "Select_CourseDetailsAndContents.sql");
        }

        public async Task<(List<Course>? courses, List<CourseContent>? contents)> GetCoursesByInstructorAsync(int instructorId)
        {
            using var db = _connectionFactory.CreateConnection();
            var multi = await db.QueryMultipleAsync(_Select_CourseDetailsAndContents, new { InstructorId = instructorId });
            var courses = (await multi.ReadAsync<Course>()).ToList();
            var courseContents = (await multi.ReadAsync<CourseContent>()).ToList();
            return (courses, courseContents);
        }

        public async Task<Instructor?> GetInstructorDetailsByUserIdAsync(int userId)
        {
            using var db = _connectionFactory.CreateConnection();
            return await db.QueryFirstOrDefaultAsync<Instructor?>(_Select_InstructorDetailsByUserId, new { UserId = userId });
        }

        public async Task CreateNewCourseAsync(Course newCourse)
        {
            using var db = _connectionFactory.CreateConnection();
            await db.ExecuteAsync(_Insert_NewCourse, newCourse );
        }

        public async Task<Course?> GetCourseDetailsByCourseIdAsync(int courseId)
        {
            using var db = _connectionFactory.CreateConnection();
            return await db.QueryFirstOrDefaultAsync<Course?>(_Select_CourseDetailsByCourseId, new { CourseId = courseId });
        }

        public async Task UploadCourseContentAsync(CourseContent newContent)
        {
            using var db = _connectionFactory.CreateConnection();
            await db.ExecuteAsync(_Insert_CourseContent, newContent);
        }
    }
}
