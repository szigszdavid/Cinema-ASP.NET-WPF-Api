using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Cinema.Web.Models
{
    public class LoginViewModel
    {
        //Külön bejelentkezés és regisztrációs nézetmodell
        [DisplayName("Név")]
        [Required]
        public string UserName { get; set; }

        [DisplayName("Jelszó")]
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }

    public class RegisterViewModel
    {
        //Külön bejelentkezés és regisztrációs nézetmodell
        [DisplayName("Név")]
        [Required]
        public string UserName { get; set; }

        [DisplayName("Jelszó")]
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DisplayName("Jelszó megerősítése")]
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string PasswordRepeat { get; set; }
    }
}
