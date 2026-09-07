using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using MunicipalServicesMVC.Models;

namespace MunicipalServicesMVC.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Service> Services { get; set; }
    }
}

//ربط الـ Models بقاعدة البيانات