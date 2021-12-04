using Cinema.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cinema.Desktop.ViewModel
{
    public class SeatViewModel : ViewModelBase
    {
        private int _id;

        public int Id
        {
            get { return _id; }
            set { _id = value; OnPropertyChanged(); }
        }

        private int _seatValue;

        public int SeatValue
        {
            get { return _seatValue; }
            set { _seatValue = value; OnPropertyChanged(); }
        }

        private Seat _seat;

        public Seat SeatData
        {
            get { return _seat; }
            set { _seat = value; OnPropertyChanged(); }
        }

        private int _number;

        public int Number
        {
            get { return _number; }
            set { _number = value; OnPropertyChanged(); }
        }

        public DelegateCommand StepCommand { get; set; }
        public String Text { get; internal set; }
    }
}
