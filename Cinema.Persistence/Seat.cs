using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Cinema.Persistence
{
    public class Seat
    {
        [Key]
        public int Id { get; set; }

        public int RowID { get; set; }

        public int ColumnID { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public String Name { get; set; }

        [Required(ErrorMessage = "Mobile no. is required")]
        [Phone(ErrorMessage = "Mobile no. is not valid")]
        public String PhoneNumber { get; set; }

        public int SeatValue { get; set; }

        public int ScreeningId { get; set; }

        public int HallId { get; set; }

        public string HallName { get; set; }
    }
}
