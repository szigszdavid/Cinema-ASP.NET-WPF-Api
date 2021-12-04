using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Cinema.Persistence
{
    public class List
    {
        [Key]
        public Int32 Id { get; set; }

        [Required] //Ha meg akarjuk mondani, hogy ez a  prop kötelező, vagyis nem lehet null -> Stringnél mindig kell
        [MaxLength(30)] //Max 30 karakter
        public String Name { get; set; }
        public virtual ICollection<Movie> Movies { get; set; }
    }
}
