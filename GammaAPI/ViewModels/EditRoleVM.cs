using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace GammaAPI.ViewModels
{
    public class EditRoleVM
    {
        public string RoleId { get; set; }

        [DisplayName("Denumirea rolului")]
        public string RoleName { get; set; }
        public List<string> Users { get; set; }
    }
}
