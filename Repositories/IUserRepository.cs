using MunicipalServicesMVC.Models;

namespace MunicipalServicesMVC.Repositories
{
    public interface IUserRepository
    {
        // جميع المستخدمين مع الدور والموظف
        IEnumerable<User> GetAllWithRoleAndEmployee();

        // جلب مستخدم بالرقم
        User? GetById(int id);

        // جلب مستخدم مع الدور والموظف
        User? GetByIdWithRoleAndEmployee(int id);

        // جلب مستخدم بالإيميل مع الدور والموظف
        User? GetByEmailWithRoleAndEmployee(string email);

        // إضافة مستخدم
        void Add(User user);

        // تعديل مستخدم
        void Update(User user);

        // حذف مستخدم
        void Delete(User user);
    }
}