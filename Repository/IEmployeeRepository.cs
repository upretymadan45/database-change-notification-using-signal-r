using System.Collections.Generic;
using dbChange.Models;

namespace dbChange.Repository
{
    public interface IEmployeeRepository
    {
        List<Employee> GetAllEmployees();
    }
}