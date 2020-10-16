using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Models
{
   public interface IEmployeeRespository
    {
        Employee GetEmployee(int id);
        IEnumerable<Employee> GetAllEmplyees();
        Employee addEmployee(Employee employee);
        Employee Delete(int id);
        Employee Update(Employee employee);
    }
}
