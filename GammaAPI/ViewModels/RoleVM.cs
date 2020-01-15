using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GammaAPI.ViewModels
{
    public class RoleVM
    {
        [DisplayName("Denumirea rolului")]
        [Required(ErrorMessage = "Numele funcției trebuie să fie un șir valid")]
        public string RoleName { get; set; }
    }
}
