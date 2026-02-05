namespace StudentManagementSystem.Domain.Entities
{
    public class Entrollment : BaseEntity
    {
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public DateTime EntrolledAt { get; set; }
    }
}
