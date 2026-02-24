namespace StudentManagementSystem.Domain.Entities
{
    public class Education : BaseEntity
    {
        public int? Id { get; set; }
        public int? StudentId { get; set; }
        public string? Institute { get; set; }
        public string? Degree { get; set; }
        public string? Major { get; set; }
        public DateTime? StartingDate { get; set; }
        public DateTime? EndingDate { get; set; }
        public bool? IsStudying { get; set; }
        public string? Description { get; set; }
    }
}
