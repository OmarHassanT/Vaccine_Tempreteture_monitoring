using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Models
{
    public class MockEmployeeRespository : IEmployeeRespository
    {
        private List<Employee> _employeeList;
        public MockEmployeeRespository()
        {
            _employeeList = new List<Employee>()
        {
            new Employee() { Id = 1, Name = "Mary", Department = dept.HR, Email = "mary@pragimtech.com" },
            new Employee() { Id = 2, Name = "John", Department = dept.IT, Email = "john@pragimtech.com" },
            new Employee() { Id = 3, Name = "Sam", Department = dept.Payroll, Email = "sam@pragimtech.com" },
        };
        }

        public Employee addEmployee(Employee employee)
        {
            employee.Id= _employeeList.Max(e => e.Id) + 1;
            _employeeList.Add(employee);
            return employee;
        }

        public Employee Delete(int id)
        {
            Employee employee = _employeeList.FirstOrDefault(e => e.Id == id);
            if (employee != null)
            {
                _employeeList.Remove(employee);
            }
            return employee;
        }

        public IEnumerable<Employee> GetAllEmplyees()
        {
            return _employeeList;
        }

        public Employee GetEmployee(int id)
        {
            return _employeeList.FirstOrDefault(e => e.Id == id);
        }

        public Employee Update(Employee ChangEmployee)
        {
           Employee employee= _employeeList.FirstOrDefault(e => e.Id == ChangEmployee.Id);
            if (employee != null)
            {
                employee.Name = ChangEmployee.Name;
                employee.Email = ChangEmployee.Email;
                employee.Department = ChangEmployee.Department;
            }
            return employee;


        }
    }
}
