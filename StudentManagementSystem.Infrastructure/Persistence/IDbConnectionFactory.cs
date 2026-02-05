using System.Data;

namespace StudentManagementSystem.Domain.Persistence
{
    public interface IDbConnectionFactory
    {
        IDbConnection CreateConnection();
    }
}
