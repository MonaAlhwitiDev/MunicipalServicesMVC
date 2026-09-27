using MunicipalServicesMVC.Data;
using MunicipalServicesMVC.Models;

namespace MunicipalServicesMVC.Repositories
{
    public class PermissionRepository : IPermissionRepository
    {
        private readonly AppDbContext _db;

        public PermissionRepository(AppDbContext db)
        {
            _db = db;
        }

        public IEnumerable<Permission> GetAll()
        {
            return _db.Permissions.ToList();
        }

        public Permission? GetById(int id)
        {
            return _db.Permissions.Find(id);
        }

        public void Add(Permission permission)
        {
            _db.Permissions.Add(permission);
        }

        public void Update(Permission permission)
        {
            _db.Permissions.Update(permission);
        }

        public void Delete(Permission permission)
        {
            _db.Permissions.Remove(permission);
        }
    }
}