using Cinema.Persistence.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cinema.Desktop.ViewModel
{
    public class ListViewModel : ViewModelBase
    {
        private int _id;

        public int Id
        {
            get { return _id; }
            set { _id = value; OnPropertyChanged(); }
        }

        private String _name;

        public String Name
        {
            get { return _name; }
            set { _name = value; OnPropertyChanged(); }
        }



        public static explicit operator ListViewModel(ListDto dto) => new ListViewModel
        {
            Id = dto.Id,
            Name = dto.Name
        };

        public static explicit operator ListDto(ListViewModel vm) => new ListDto
        {
            Id = vm.Id,
            Name = vm.Name
        };
    }
}
