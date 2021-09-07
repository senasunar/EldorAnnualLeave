using EldorAnnualLeave.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EldorAnnualLeave.Core.Services
{
    public interface IAppRoleService : IService<AppRole>
    {
        Task<IEnumerable<AppRole>> GetAllRolesAsync();
    }
}
