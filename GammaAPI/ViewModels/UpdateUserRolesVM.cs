using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GammaAPI.ViewModels
{
    public class UpdateUserRolesVM
    {
        public string RoleId { get; set; }

        public List<AddUserRoleVM> UserRoles { get; set; }
    }
}
