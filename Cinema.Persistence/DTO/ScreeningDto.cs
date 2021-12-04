using System;
using System.Collections.Generic;
using System.Text;

namespace Cinema.Persistence.DTO
{
    public class ScreeningDto
    {
        public int Id { get; set; }

        public String ScreenTime { get; set; }

        public String Name { get; set; }

        public String PhoneNumber { get; set; }

        public virtual Hall ScreeningHall { get; set; }

        public Int32 TakenSeats { get; set; }
        public virtual List<Seat> Seats { get; set; }

        public int MovieId { get; set; }

        public static explicit operator Screening(ScreeningDto dto) => new Screening
        {

            Id = dto.Id,
            ScreenTime = dto.ScreenTime,
            Name = dto.Name,
            PhoneNumber = dto.PhoneNumber,
            ScreeningHall = dto.ScreeningHall,
            TakenSeats = dto.TakenSeats,
            Seats = dto.Seats,
            MovieId = dto.MovieId,
           
        };

        public static explicit operator ScreeningDto(Screening vm) => new ScreeningDto
        {
            Id = vm.Id,
            ScreenTime = vm.ScreenTime,
            Name = vm.Name,
            PhoneNumber = vm.PhoneNumber,
            ScreeningHall = vm.ScreeningHall,
            TakenSeats = vm.TakenSeats,
            Seats = vm.Seats,
            MovieId = vm.MovieId,
            
        };

    }
}
