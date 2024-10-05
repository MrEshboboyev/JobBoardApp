using JobBoardApp.Application.Common.Interfaces;
using JobBoardApp.Domain.Entities;
using JobBoardApp.Infrastructure.Data;

namespace JobBoardApp.Infrastructure.Repositories
{
    public class UserRepository(AppDbContext db) : Repository<AppUser>(db),
        IUserRepository
    {
        private readonly AppDbContext _db = db;
    }
}
