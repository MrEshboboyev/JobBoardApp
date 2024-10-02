using JobBoardApp.Application.Common.Interfaces;
using JobBoardApp.Domain.Entities;
using JobBoardApp.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoardApp.Infrastructure.Repositories
{
    public class NotificationRepository(AppDbContext db) : Repository<Notification>(db),
        INotificationRepository
    {
        private readonly AppDbContext _db = db;
    }
}
