using MunicipalServicesMVC.Models;

namespace MunicipalServicesMVC.Repositories
{
    public interface IServiceRepository
    {
        // جميع الخدمات مع الإدارة
        IEnumerable<Service> GetAllWithDepartment();

        // جلب خدمة بالرقم
        Service? GetById(int id);

        // جلب خدمة مع الإدارة
        Service? GetByIdWithDepartment(int id);

        // إضافة خدمة
        void Add(Service service);

        // تعديل خدمة
        void Update(Service service);

        // حذف خدمة
        void Delete(Service service);
    }
}