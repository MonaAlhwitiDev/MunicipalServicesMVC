namespace MunicipalServicesMVC.Models
{
    public class RolePermissionsViewModel
    {
        public int RoleId { get; set; }

        public string RoleName { get; set; } = string.Empty;

        public List<Permission> Permissions { get; set; } = new List<Permission>();

        public List<int> SelectedPermissionIds { get; set; } = new List<int>();
    }
}