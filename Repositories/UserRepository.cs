using Microsoft.EntityFrameworkCore;
using MunicipalServicesMVC.Data;
using MunicipalServicesMVC.Models;

namespace MunicipalServicesMVC.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _db;

        public UserRepository(AppDbContext db)
        {
            _db = db;
        }

        public IEnumerable<User> GetAllWithRoleAndEmployee()
        {
            return _db.Users
                .Include(u => u.Role)
                .Include(u => u.Employee)
                .ToList();
        }

        public User? GetById(int id)
        {
            return _db.Users.Find(id);
        }

        public User? GetByIdWithRoleAndEmployee(int id)
        {
            return _db.Users
                .Include(u => u.Role)
                .Include(u => u.Employee)
                .FirstOrDefault(u => u.Id == id);
        }

        public User? GetByEmailWithRoleAndEmployee(string email)
        {
            return _db.Users
                .Include(u => u.Role)
                .Include(u => u.Employee)
                .FirstOrDefault(u => u.Email == email);
        }

        public void Add(User user)
        {
            _db.Users.Add(user);
        }

        public void Update(User user)
        {
            _db.Users.Update(user);
        }

        public void Delete(User user)
        {
            _db.Users.Remove(user);
        }
    }
}