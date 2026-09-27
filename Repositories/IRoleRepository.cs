using MunicipalServicesMVC.Models;

namespace MunicipalServicesMVC.Repositories
{
    public interface IRoleRepository
    {
        IEnumerable<Role> GetAll();

        Role? GetById(int id);

        void Add(Role role);

        void Update(Role role);

        void Delete(Role role);

        List<int> GetSelectedPermissionIds(int roleId);

        IEnumerable<RolePermission> GetRolePermissions(int roleId);

        void DeleteRolePermissions(IEnumerable<RolePermission> rolePermissions);

        void AddRolePermission(RolePermission rolePermission);
    }
}