using JobBoardApp.Application.Common.Interfaces;
using JobBoardApp.Domain.Entities;
using JobBoardApp.Infrastructure.Data;

namespace JobBoardApp.Infrastructure.Repositories
{
    public class UserProfileRepository(AppDbContext db) : Repository<UserProfile>(db), 
        IUserProfileRepository
    {
        private readonly AppDbContext _db = db;
    }
}
