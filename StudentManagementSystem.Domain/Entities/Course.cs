namespace StudentManagementSystem.Domain.Entities
{
    public class Course : BaseEntity
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public int? Credits { get; set; }
    }
}
