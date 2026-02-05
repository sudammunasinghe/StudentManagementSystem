namespace StudentManagementSystem.Domain.Entities
{
    public class BaseEntity
    {
        public bool IsActive { get; set; }
        public int CreatedDateTime { get; set; }
        public int LastModifiedDateTime { get; set; }
    }
}
