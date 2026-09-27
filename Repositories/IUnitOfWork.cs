namespace MunicipalServicesMVC.Repositories
{
    public interface IUnitOfWork
    {
        // Repository الخاص بالإدارات
        IDepartmentRepository Departments { get; }

        // حفظ جميع التغييرات مرة واحدة
        void Save();
    }
}