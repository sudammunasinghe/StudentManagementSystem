namespace StudentManagementSystem.Infrastructure.Persistence.Sql.Helpers
{
    public interface ISqlQueryLoader
    {
        string Load(string folder, string fileName);
    }
}
