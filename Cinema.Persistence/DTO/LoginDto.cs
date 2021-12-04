using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Cinema.Persistence.DTO
{
    public class LoginDto
    {
        [Required]
        public String UserName { get; set; }

        [Required]
        public String Password { get; set; }
    }

}
