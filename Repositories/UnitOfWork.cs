using MunicipalServicesMVC.Data;

namespace MunicipalServicesMVC.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _db;

        public IDepartmentRepository Departments { get; }

        public UnitOfWork(AppDbContext db)
        {
            _db = db;

            Departments = new DepartmentRepository(_db);
        }

        // حفظ كل التغييرات 
        public void Save()
        {
            _db.SaveChanges();
        }
    }
}