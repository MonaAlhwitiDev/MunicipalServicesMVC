using Microsoft.EntityFrameworkCore;
using MunicipalServicesMVC.Data;
using MunicipalServicesMVC.Models;

namespace MunicipalServicesMVC.Repositories
{
    public class ServiceRepository : IServiceRepository
    {
        private readonly AppDbContext _db;

        public ServiceRepository(AppDbContext db)
        {
            _db = db;
        }

        public IEnumerable<Service> GetAllWithDepartment()
        {
            return _db.Services
                .Include(s => s.Department)
                .ToList();
        }

        public Service? GetById(int id)
        {
            return _db.Services
                .FirstOrDefault(s => s.Id == id);
        }

        public Service? GetByIdWithDepartment(int id)
        {
            return _db.Services
                .Include(s => s.Department)
                .FirstOrDefault(s => s.Id == id);
        }

        public void Add(Service service)
        {
            _db.Services.Add(service);
        }

        public void Update(Service service)
        {
            _db.Services.Update(service);
        }

        public void Delete(Service service)
        {
            _db.Services.Remove(service);
        }
    }
}