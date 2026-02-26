using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagementSystem.Infrastructure.Persistence.Sql.Helpers
{
    public interface ISqlQueryLoader
    {
        string Load(string folder, string fileName);
    }
}
