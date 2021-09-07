using EldorAnnualLeave.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EldorAnnualLeave.Core.Repositories
{
    public interface IAppUserRepository : IRepository<AppUser>
    {
        /*Task AddUserAsync(AppUser appUser);
        Task<IEnumerable<AppUser>> GetAllUsersAsync();
        Task<AppUser> GetByUserIdAsync(int id);
        void RemoveUser(AppUser appUser);
        AppUser UpdateUser(AppUser appUser);*/
    }
}
