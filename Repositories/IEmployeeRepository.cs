using MunicipalServicesMVC.Models;

namespace MunicipalServicesMVC.Repositories
{
    public interface IEmployeeRepository
    {
        IEnumerable<Employee> GetAllWithDepartment();

        Employee? GetById(int id);

        Employee? GetByIdWithDepartment(int id);

        void Add(Employee employee);

        void Update(Employee employee);

        void Delete(Employee employee);

        Dictionary<int, string> GetLatestImages();

        IEnumerable<EmployeeFile> GetFiles(int employeeId);

        EmployeeFile? GetFileById(int id);

        void AddFile(EmployeeFile employeeFile);

        void DeleteFile(EmployeeFile employeeFile);
    }
}