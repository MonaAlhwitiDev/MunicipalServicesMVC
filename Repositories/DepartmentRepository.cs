using Microsoft.EntityFrameworkCore;
using MunicipalServicesMVC.Data;
using MunicipalServicesMVC.Models;

namespace MunicipalServicesMVC.Repositories
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly AppDbContext _db;

        public DepartmentRepository(AppDbContext db)
        {
            _db = db;
        }

        // جلب جميع الإدارات
        public IEnumerable<Department> GetAll()
        {
            return _db.Departments
                .Include(d => d.Employees)
                .Include(d => d.Services)
                .ToList();
        }

        // جلب إدارة باستخدام UID
        public Department? GetByUID(string uid)
        {
            return _db.Departments
                .Include(d => d.Employees)
                .Include(d => d.Services)
                .FirstOrDefault(d => d.UID == uid);
        }

        // إضافة إدارة
        public void Add(Department department)
        {
            _db.Departments.Add(department);
        }

        // تعديل إدارة
        public void Update(Department department)
        {
            _db.Departments.Update(department);
        }

        // حذف إدارة
        public void Delete(Department department)
        {
            _db.Departments.Remove(department);
        }

        // حفظ التغييرات
        public void Save()
        {
            _db.SaveChanges();
        }
    }
}