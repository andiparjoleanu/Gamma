using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Gamma.ViewModels
{
    public class RegisterVM
    { 
        [DisplayName("Prenume")]
        public string FirstName { get; set; }

        [DisplayName("Nume")]
        public string LastName { get; set; }

        [DisplayName("Nume de Utilizator")]
        public string UserName { get; set; }
        
        [DataType(DataType.EmailAddress, 
            ErrorMessage = "Adresă de email incorectă")]
        public string Email { get; set; }

        [DisplayName("Parola")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DisplayName("Reintrodu Parola")]
        [Compare("Password", 
            ErrorMessage = "Parola introdusă nu se potrivește cu parola aleasă")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        [DisplayName("Alege un tip de cont")]
        public string SelectedRole { get; set; }
    }
}
