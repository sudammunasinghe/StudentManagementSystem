namespace StudentManagementSystem.Application.Interfaces.IServices
{
    public interface IStudentService
    {
        Task EnrollToCourseAsync(int courseId);
    }
}
