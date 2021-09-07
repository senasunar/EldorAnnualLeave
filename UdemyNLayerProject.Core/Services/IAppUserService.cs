using EldorAnnualLeave.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EldorAnnualLeave.Core.Services
{
    public interface IAppUserService : IService<AppUser>
    {
        Task AddUserAsync(AppUser appUser);
        Task<IEnumerable<AppUser>> GetAllUsersAsync();
        Task<AppUser> GetByUserIdAsync(string id);
        AppUser UpdateUser(AppUser appUser);
        void RemoveUser(AppUser appUser);
        public Task<List<AppUser>> CreateEmployeeTable();
        public Task<List<AppUser>> CreateEmployeeTableMember(string email);
    }
}
