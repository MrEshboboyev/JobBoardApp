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
    public class JobListingRepository(AppDbContext db) : Repository<JobListing>(db),
        IJobListingRepository
    {
        private readonly AppDbContext _db = db;
    }
}
