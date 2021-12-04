using Cinema.Persistence;
using Cinema.Persistence.DTO;
using System.Collections.Generic;

namespace Cinema.Desktop.ViewModel
{
    public class ScreeningViewModel : ViewModelBase
    {
        private int id;

        public int Id
        {
            get { return id; }
            set { id = value; OnPropertyChanged(); }
        }

        private string screeningTime;

        public string ScreenTime
        {
            get { return screeningTime; }
            set { screeningTime = value; OnPropertyChanged(); }
        }

        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; OnPropertyChanged(); }
        }

        private string phoneNumber;

        public string PhoneNumber
        {
            get { return phoneNumber; }
            set { phoneNumber = value; OnPropertyChanged(); }
        }

        private Hall screeningHall;

        public Hall ScreeningHall
        {
            get { return screeningHall; }
            set { screeningHall = value; OnPropertyChanged(); }
        }

        private int takenSeats;

        public int TakenSeats
        {
            get { return takenSeats; }
            set { takenSeats = value; OnPropertyChanged(); }
        }

        private int movieId;

        public int MovieId
        {
            get { return movieId; }
            set { movieId = value; OnPropertyChanged(); }
        }

        public virtual List<Seat> Seats { get; set; }

        public ScreeningViewModel ShallowClone()
        {
            return (ScreeningViewModel)this.MemberwiseClone();
        }

        public void CopyFrom(ScreeningViewModel rhs)
        {
            Id = rhs.Id;
            ScreenTime = rhs.ScreenTime;
            Name = rhs.Name;
            PhoneNumber = rhs.PhoneNumber;
            ScreeningHall = rhs.ScreeningHall;
            TakenSeats = rhs.TakenSeats;
            Seats = rhs.Seats;
            MovieId = rhs.MovieId;
        }

        public static explicit operator ScreeningViewModel(ScreeningDto dto) => new ScreeningViewModel
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

        public static explicit operator ScreeningDto(ScreeningViewModel vm) => new ScreeningDto
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