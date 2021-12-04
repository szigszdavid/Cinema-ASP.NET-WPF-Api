using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Cinema.Persistence
{
    public class Screening
    {
        [Key]
        public int Id { get; set; }

        public String ScreenTime { get; set; }

        [Required(ErrorMessage = "Name is required")]
        
        public String Name { get; set; }

        [Required(ErrorMessage = "Mobile no. is required")]
        [Phone(ErrorMessage = "Mobile no. is not valid")]
        public String PhoneNumber { get; set; }

        public virtual Hall ScreeningHall { get; set; }

        public Int32 TakenSeats { get; set; }
        public virtual List<Seat> Seats { get; set; }

        public int MovieId { get; set; }
    }
}
