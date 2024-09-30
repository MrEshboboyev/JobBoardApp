using JobBoardApp.Application.Common.Interfaces;
using JobBoardApp.Infrastructure.Data;

namespace JobBoardApp.Infrastructure.Repositories
{
    public class UnitOfWork(AppDbContext db) : IUnitOfWork
    {
        private readonly AppDbContext _db = db;

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
