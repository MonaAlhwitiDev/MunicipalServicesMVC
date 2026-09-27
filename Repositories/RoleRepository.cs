using MunicipalServicesMVC.Data;
using MunicipalServicesMVC.Models;

namespace MunicipalServicesMVC.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly AppDbContext _db;

        public RoleRepository(AppDbContext db)
        {
            _db = db;
        }

        public IEnumerable<Role> GetAll()
        {
            return _db.Roles.ToList();
        }

        public Role? GetById(int id)
        {
            return _db.Roles.Find(id);
        }

        public void Add(Role role)
        {
            _db.Roles.Add(role);
        }

        public void Update(Role role)
        {
            _db.Roles.Update(role);
        }

        public void Delete(Role role)
        {
            _db.Roles.Remove(role);
        }

        public List<int> GetSelectedPermissionIds(int roleId)
        {
            return _db.RolePermissions
                .Where(rp => rp.RoleId == roleId)
                .Select(rp => rp.PermissionId)
                .ToList();
        }

        public IEnumerable<RolePermission> GetRolePermissions(int roleId)
        {
            return _db.RolePermissions
                .Where(rp => rp.RoleId == roleId)
                .ToList();
        }

        public void DeleteRolePermissions(
            IEnumerable<RolePermission> rolePermissions)
        {
            _db.RolePermissions.RemoveRange(rolePermissions);
        }

        public void AddRolePermission(
            RolePermission rolePermission)
        {
            _db.RolePermissions.Add(rolePermission);
        }
    }
}