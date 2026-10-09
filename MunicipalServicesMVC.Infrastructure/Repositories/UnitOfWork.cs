using MunicipalServicesMVC.Infrastructure.Data;

namespace MunicipalServicesMVC.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _db;

        public IDepartmentRepository Departments { get; }

        public IEmployeeRepository Employees { get; }

        public IServiceRepository Services { get; }

        public IRoleRepository Roles { get; }

        public IPermissionRepository Permissions { get; }

        public IUserRepository Users { get; }

        public UnitOfWork(AppDbContext db)
        {
            _db = db;

            Departments = new DepartmentRepository(_db);
            Employees = new EmployeeRepository(_db);
            Services = new ServiceRepository(_db);
            Roles = new RoleRepository(_db);
            Permissions = new PermissionRepository(_db);
            Users = new UserRepository(_db);
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}