using Cinema.Persistence.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cinema.Desktop.ViewModel
{
    public class MovieViewModel : ViewModelBase
    {
        private Int32 _id;

        public Int32 Id
        {
            get { return _id; }
            set { _id = value; }
        }

        private String _title;

        public String Title
        {
            get { return _title; }
            set { _title = value; OnPropertyChanged(); }
        }

        private String _director;

        public String Director
        {
            get { return _director; }
            set { _director = value; OnPropertyChanged(); }
        }

        private String _szinopszis;

        public String Szinopszis
        {
            get { return _szinopszis; }
            set { _szinopszis = value; OnPropertyChanged(); }
        }

        private Int32 _length;

        public Int32 Length
        {
            get { return _length; }
            set { _length = value; OnPropertyChanged(); }
        }

        private DateTime _releaseData;

        public DateTime ReleaseDate
        {
            get { return _releaseData; }
            set { _releaseData = value; OnPropertyChanged(); }
        }

        private Int32 _listId;

        public Int32 ListId
        {
            get { return _listId; }
            set { _listId = value; OnPropertyChanged(); }
        }

        private String _screeningTimes;

        public String ScreeningTimes
        {
            get { return _screeningTimes; }
            set { _screeningTimes = value; OnPropertyChanged(); }
        }

        
        private byte[] _image;

        public byte[] Image
        {
            get { return _image; }
            set { _image = value; OnPropertyChanged(); }
        }
        

        public MovieViewModel ShallowClone()
        {
            return (MovieViewModel)this.MemberwiseClone();
        }

        public void CopyFrom(MovieViewModel rhs)
        {
            Id = rhs.Id;
            Title = rhs.Title;
            Director = rhs.Director;
            Szinopszis = rhs.Szinopszis;
            Length = rhs.Length;
            Image = rhs.Image;
            ListId = rhs.ListId;
            ReleaseDate = rhs.ReleaseDate;
            ScreeningTimes = rhs.ScreeningTimes;
        }

        public static explicit operator MovieViewModel(MovieDto dto) => new MovieViewModel
        {
            Id = dto.Id,
            Title = dto.Title,
            Director = dto.Director,
            Szinopszis = dto.Szinopszis,
            Length = dto.Length,
            Image = dto.Image,
            ListId = dto.ListId,
            ReleaseDate = dto.ReleaseDate,
            ScreeningTimes = dto.ScreeningTimes,
        };

        public static explicit operator MovieDto(MovieViewModel vm) => new MovieDto
        {
            Id = vm.Id,
            Title = vm.Title,
            Director = vm.Director,
            Szinopszis = vm.Szinopszis,
            Length = vm.Length,
            Image = vm.Image,
            ListId = vm.ListId,
            ReleaseDate = vm.ReleaseDate,
            ScreeningTimes = vm.ScreeningTimes,
        };
    }
}
