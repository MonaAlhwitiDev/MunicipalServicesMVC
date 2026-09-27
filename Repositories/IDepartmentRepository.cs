using MunicipalServicesMVC.Models;

namespace MunicipalServicesMVC.Repositories
{
    public interface IDepartmentRepository
    {
        // جلب جميع الإدارات
        IEnumerable<Department> GetAll();

        // جلب إدارة باستخدام UID
        Department? GetByUID(string uid);

        // إضافة إدارة
        void Add(Department department);

        // تعديل إدارة
        void Update(Department department);

        // حذف إدارة
        void Delete(Department department);

        // حفظ التغييرات
        void Save();
    }
}