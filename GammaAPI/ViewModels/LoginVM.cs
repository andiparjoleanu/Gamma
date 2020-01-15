using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace GammaAPI.ViewModels
{
    public class LoginVM
    {
        [DisplayName("Nume de utilizator")]
        public string UserName { get; set; }

        [DisplayName("Parolă")]
        public string Password { get; set; }
    }
}
