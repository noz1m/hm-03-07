using Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class DataContext(DbContextOptions options) : IdentityDbContext(options)
{
    public DbSet<Student> Students { get; set; }
}
