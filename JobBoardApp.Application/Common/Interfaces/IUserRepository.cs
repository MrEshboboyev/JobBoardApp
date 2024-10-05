using JobBoardApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoardApp.Application.Common.Interfaces
{
    public interface IUserRepository : IRepository<AppUser>
    {
    }
}
