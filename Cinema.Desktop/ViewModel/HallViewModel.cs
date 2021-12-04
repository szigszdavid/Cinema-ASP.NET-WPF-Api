using System;
using System.Collections.Generic;
using System.Text;

namespace Cinema.Desktop.ViewModel
{
    public class HallViewModel : ViewModelBase
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

        private int beginTime;

        public int BeginTime
        {
            get { return beginTime; }
            set { beginTime = value; OnPropertyChanged(); }
        }

        private int endTime;

        public int EndTime
        {
            get { return endTime; }
            set { endTime = value; OnPropertyChanged(); }
        }




    }
}
