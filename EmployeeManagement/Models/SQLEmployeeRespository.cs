using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Models
{
    public class SQLEmployeeRespository : IEmployeeRespository
    {
        private readonly AppDbContext context;

        public SQLEmployeeRespository(AppDbContext context )
        {
            this.context = context;
        }
        public Employee addEmployee(Employee employee)
        {
            context.Employees.Add(employee);
            context.SaveChanges();
            return employee;
        }

        public Employee Delete(int id)
        {
           Employee employee = context.Employees.Find(id);
            if (employee != null)
            {
                context.Employees.Remove(employee);
                context.SaveChanges();
            }
            return employee;
        }

        public IEnumerable<Employee> GetAllEmplyees()
        {
            return context.Employees;
        }

        public Employee GetEmployee(int id)
        {
           return context.Employees.Find(id);
          
        }

        public Employee Update(Employee employee)
        {
           var NewEmployee= context.Employees.Attach(employee);
            NewEmployee.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return employee;
                }
    }
}
