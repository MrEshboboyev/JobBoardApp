using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using JobBoardApp.Domain.Entities;

namespace JobBoardApp.Infrastructure.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : 
        IdentityDbContext<AppUser>(options)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
