using Cinema.Persistence.DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;
using Cinema.Desktop.Model;
using System.Linq;
using Cinema.Persistence;

namespace Cinema.Desktop.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private readonly CinemaApiService _service;
        private ObservableCollection<ListViewModel> _lists;
        private ObservableCollection<MovieViewModel> _movies;
        private ObservableCollection<ScreeningViewModel> _screenings;
        private ObservableCollection<SeatViewModel> _seats;
        private ObservableCollection<Hall> _hall;
        private MovieViewModel _selectedItem;
        private ListViewModel _selectedList;
        private MovieViewModel _editableMovie;
        private String _selectedListName;
        private ScreeningViewModel _selectedScreening;
        private Int32 TakenSeats;
        private string _buyerName;
        private string _buyerPhone;
        private int _buyerRowID;
        private int _buyerColumnID;

        #region Properties
        public ObservableCollection<ListViewModel> Lists
        {
            get { return _lists; }
            set { _lists = value; OnPropertyChanged(); }
        }   

        public ObservableCollection<MovieViewModel> Movies
        {
            get { return _movies; }
            set { _movies = value; OnPropertyChanged(); }
        }

        public ObservableCollection<ScreeningViewModel> Screenings
        {
            get { return _screenings; }
            set { _screenings = value; OnPropertyChanged(); }
        }

        public ObservableCollection<SeatViewModel> Seats
        {
            get { return _seats; }
            set { _seats = value; OnPropertyChanged(); }
        }

        public ObservableCollection<Hall> Halls
        {
            get { return _hall; }
            set { _hall = value; OnPropertyChanged(); }
        }

        public String BuyerName
        {
            get { return _buyerName; }
            set { _buyerName = value; OnPropertyChanged(); }
        }

        public String BuyerPhone
        {
            get { return _buyerPhone; }
            set { _buyerPhone = value; OnPropertyChanged(); }
        }

        public Int32 BuyerRowID
        {
            get { return _buyerRowID; }
            set { _buyerRowID = value; OnPropertyChanged(); }
        }

        public Int32 BuyerColumnID
        {
            get { return _buyerColumnID; }
            set { _buyerColumnID = value; OnPropertyChanged(); }
        }

        public MovieViewModel SelectedItem
        {
            get { return _selectedItem; }
            set { _selectedItem = value; OnPropertyChanged(); }
        }

        public ScreeningViewModel SelectedScreening
        {
            get { return _selectedScreening; }
            set { _selectedScreening= value; OnPropertyChanged(); }
        }

        public ListViewModel SelectedList
        {
            get { return _selectedList; }
            set { _selectedList = value; OnPropertyChanged(); }
        }

        public String SelectedListName
        {
            get { return _selectedListName; }
            set { _selectedListName = value; OnPropertyChanged(); }
        }

        public MovieViewModel EditableMovie
        {
            get { return _editableMovie; }
            set { _editableMovie = value; OnPropertyChanged(); }
        }

        #endregion

        #region Commands
        public DelegateCommand RefreshListsCommand { get; private set; }
        public DelegateCommand SelectCommand { get; private set; }
        public DelegateCommand LogoutCommand { get; private set; }
        public DelegateCommand AddListCommand { get; private set; }
        public DelegateCommand EditListCommand { get; private set; }
        public DelegateCommand DeleteListCommand { get; private set; }
        public DelegateCommand AddMovieCommand { get; private set; }
        public DelegateCommand EditMovieCommand { get; private set; }
        public DelegateCommand DeleteMovieCommand { get; private set; }
        public DelegateCommand SaveMovieEditCommand { get; private set; }
        public DelegateCommand CancelMovieEditCommand { get; private set; }
        public DelegateCommand ChangeImageCommand { get; private set; }

        


        #endregion

        #region Events

        public event EventHandler LogoutSucceeded;

        public event EventHandler StartingMovieEdit;

        public event EventHandler FinishingMovieEdit;

        public event EventHandler StartingImageChange;

        public event EventHandler RefreshLabel;

        #endregion

        public MainViewModel(CinemaApiService service)
        {
            _service = service;

            RefreshListsCommand = new DelegateCommand(_ => LoadListsAsync());
            SelectCommand = new DelegateCommand(_ => LoadMoviesAsync(SelectedList));
            LogoutCommand = new DelegateCommand(_ => LogoutAsync());

            AddListCommand = new DelegateCommand(_ => AddList());
            EditListCommand = new DelegateCommand(_ => !(SelectedList is null), _ => EditList());
            DeleteListCommand = new DelegateCommand(_ => !(SelectedList is null), _ => DeleteList(SelectedList));

            AddMovieCommand = new DelegateCommand(_ => !(SelectedList is null), _ => AddMovie());
            EditMovieCommand = new DelegateCommand(_ => !(SelectedItem is null), _ => StartEditMovie());
            DeleteMovieCommand = new DelegateCommand(_ => !(SelectedItem is null), _ => DeleteMovie(SelectedItem));

            SaveMovieEditCommand = new DelegateCommand(_ => SaveMovieEdit());
            CancelMovieEditCommand = new DelegateCommand(_ => CancelMovieEdit());
            ChangeImageCommand = new DelegateCommand(_ => StartingImageChange?.Invoke(this, EventArgs.Empty));

            TakenSeats = 0;
        }

        #region Logout
        private async void LogoutAsync()
        {
            try
            {
                await _service.LogoutAsync();

                LogoutSucceeded?.Invoke(this, EventArgs.Empty);


            }
            catch (Exception ex) when (ex is NetworkException || ex is HttpRequestException)
            {
                OnMessageApplication($"Unexpected error occured! ({ex.Message})");
            }
            
        }

        #endregion

        #region List

        private async void LoadListsAsync()
        {
            try
            {
                Lists = new ObservableCollection<ListViewModel>((await _service.LoadListsAsync())
                    .Select(list => (ListViewModel)list));
            }
            catch (Exception ex) when (ex is NetworkException || ex is HttpRequestException)
            {
                OnMessageApplication($"Unexpected error occured! ({ex.Message})");
            }
        }
        private async void AddList()
        {
            var newList = new ListViewModel
            {
                Name = "New List"
            };

            var listDto = (ListDto)newList;
            
            try
            {
                await _service.CreateListAsync(listDto);
                newList.Id = listDto.Id;
                Lists.Add(newList);
                SelectedList = newList;
            }

            catch (Exception ex) when (ex is NetworkException || ex is HttpRequestException)
            {
                OnMessageApplication($"Unexpected error occured! ({ex.Message})");
            }
        }
        private async void DeleteList(ListViewModel list)
        {
            try
            {
                await _service.DeleteListAsync(list.Id);
                Lists.Remove(SelectedList);
                SelectedList = null;
            }
            catch (Exception ex) when (ex is NetworkException || ex is HttpRequestException)
            {
                OnMessageApplication($"Unexpected error occured! ({ex.Message})");
            }
        }
        private async void EditList()
        {
            try
            {
                SelectedList.Name = SelectedListName;
                await _service.UpdateListAsync((ListDto)SelectedList);
            }
            catch (Exception ex) when (ex is NetworkException || ex is HttpRequestException)
            {
                OnMessageApplication($"Unexpected error occured! ({ex.Message})");
            }
        }

        #endregion

        #region Movie
        private async void LoadMoviesAsync(ListViewModel list) //Ez lehet, hogy ListDto
        {
            if (list is null)
            {
                Movies = null;
                return;
            }

            try
            {
                SelectedListName = list.Name;
                Movies = new ObservableCollection<MovieViewModel>((await _service.LoadMoviesAsync(list.Id))
                    .Select(item => (MovieViewModel)item));
                    //.Select(item => (MovieViewModel)item));

            }
            catch (Exception ex) when (ex is NetworkException || ex is HttpRequestException)
            {
                OnMessageApplication($"Unexpected error occured! ({ex.Message})");
            }
        }

        public async void LoadScreeningsAsync(MovieViewModel movie)
        {
            if(movie is null)
            {
                Screenings = null;
                return;
            }

            try
            {
                Screenings = new ObservableCollection<ScreeningViewModel>((await _service.LoadScreeningsAsync(SelectedItem.Id))
                    .Select(item => (ScreeningViewModel)item));
            }
            catch (Exception ex) when (ex is NetworkException || ex is HttpRequestException)
            {
                OnMessageApplication($"Unexpected error occured! ({ex.Message})");
            }
        }

        public void LoadSeatsAsync(ScreeningViewModel screening)
        {
            if (screening is null)
            {
                Seats = null;
                return;
            }

            TakenSeats = 0;

            OnPropertyChanged();

            try
            {
                Seats = new ObservableCollection<SeatViewModel>();
                for (int i = 0; i < Screenings.ElementAt(0).Seats.Count; i++)
                {
                    //Seats.Add(Screenings.ElementAt(0).Seats.ElementAt(i));
                    Seats.Add(new SeatViewModel
                    {
                        SeatData = Screenings.First(screening => screening.Id == SelectedScreening.Id).Seats.ElementAt(i),
                        Id = Screenings.First(screening => screening.Id == SelectedScreening.Id).Seats.ElementAt(i).Id,
                        Number = i,
                        SeatValue = Screenings.First(screening => screening.Id == SelectedScreening.Id).Seats.ElementAt(i).SeatValue,
                        StepCommand = new DelegateCommand(param => ChangeColor(Convert.ToInt32(param))),
                        Text = (i).ToString()
                    }) ;
                }
                    
            }
            catch (Exception ex) when (ex is NetworkException || ex is HttpRequestException)
            {
                OnMessageApplication($"Unexpected error occured! ({ex.Message})");
            }
        }

        private void ChangeColor(int v)
        {
            if(Seats[v].SeatValue == 2)
            {
                BuyerName = Seats[v].SeatData.Name;
                OnPropertyChanged();
                System.Diagnostics.Debug.WriteLine(BuyerName);
                BuyerPhone = Seats[v].SeatData.PhoneNumber;
                OnPropertyChanged();
                System.Diagnostics.Debug.WriteLine(BuyerPhone);
                BuyerRowID = Seats[v].SeatData.RowID;
                OnPropertyChanged();
                System.Diagnostics.Debug.WriteLine(BuyerRowID);
                OnPropertyChanged();
                BuyerColumnID = Seats[v].SeatData.ColumnID;
                OnPropertyChanged();
                System.Diagnostics.Debug.WriteLine(BuyerColumnID);
                TakenSeats--;
                Seats[v].SeatValue = 0;
                OnPropertyChanged("Seats");

                if (RefreshLabel != null)
                    RefreshLabel(this, EventArgs.Empty);

            }
            else if (Seats[v].SeatValue == 0 && TakenSeats < 6)
            {
                TakenSeats++;
                Seats[v].SeatValue = 2;
                OnPropertyChanged("Seats");

            }
        }

        public async void PurchaseButtonCliked()
        {
            for (int i = 0; i < Screenings.ElementAt(0).Seats.Count; i++)
            {
                //Seats.Add(Screenings.First(screening => screening.Id == SelectedScreening.Id).Seats.ElementAt(i));
                if((Seats[i].SeatValue == 1 && TakenSeats < 6) || (Seats[i].SeatValue == 2 && TakenSeats < 6))
                {
                    Seats[i].SeatValue = 3;
                    OnPropertyChanged("Seats");
                    int j = 0;
                    while (j < Screenings.Count && Screenings[j].Id != Seats[i].SeatData.ScreeningId)
                    {
                        j++;
                    }
                    
                    ScreeningDto onlyId = (ScreeningDto)Screenings[j];
                    ScreeningDto screeningViewModel = new ScreeningDto { Id = onlyId.Id };
                    System.Diagnostics.Debug.WriteLine("ScreeningId " + Seats[i].SeatData.ScreeningId);
                    await _service.ApiServicePurchaseAsync(i, screeningViewModel);
                    OnPropertyChanged();
                    TakenSeats = 0;
                }
                
            }
        }

        private async void AddMovie()
        {
            var newItem = new MovieViewModel
            {
                Title = "New Title",
                ReleaseDate = DateTime.Now,
                ListId = SelectedList.Id,
                ScreeningTimes = "12:30"
            };

            var movieDto = (MovieDto)newItem;

            try
            {
                await _service.CreateMovieAsync(movieDto);
                newItem.Id = movieDto.Id;
                Movies.Add(newItem);
                SelectedItem = newItem;
            }

            catch (Exception ex) when (ex is NetworkException || ex is HttpRequestException)
            {
                OnMessageApplication($"Unexpected error occured! ({ex.Message})");
            }
        }

        public async void AddScreening(string movieName, string screenTime, string hallName)
        {
            Screenings = new ObservableCollection<ScreeningViewModel>((await _service.LoadScreeningsAsync(SelectedItem.Id))
                    .Select(item => (ScreeningViewModel)item));

            for (int i = 0; i < Screenings.Count; i++)
            {
                if(Screenings[i].ScreeningHall.Name == hallName)
                {
                    System.Diagnostics.Debug.WriteLine("Ellenőrzés....");
                    String[] newScreenTimePieces = screenTime.Split(':');
                    Int32 newScreenTimeBegin = (Convert.ToInt32(newScreenTimePieces[0])) * 60 + Convert.ToInt32(newScreenTimePieces[1]);
                    String[] lowerLimitPieces = Screenings[i].ScreenTime.Split(":");
                    Int32 lowerLimit = (Convert.ToInt32(lowerLimitPieces[0])) * 60 + Convert.ToInt32(lowerLimitPieces[1]);
                    int j = 0;
                    while (j < Movies.Count && Movies[j].Id != Screenings[i].MovieId)
                    {
                        j++;
                    }
                    Int32 higherLimit = lowerLimit + 15 + Convert.ToInt32(Movies[j].Length);
                    Int32 newScreenTimeEnd = newScreenTimeBegin + 15 + Convert.ToInt32(SelectedItem.Length);

                    System.Diagnostics.Debug.WriteLine("The lower limit is: " + lowerLimit);
                    System.Diagnostics.Debug.WriteLine("The higher limit is: " + higherLimit);
                    System.Diagnostics.Debug.WriteLine("The screentime begin is: " + newScreenTimeBegin);
                    System.Diagnostics.Debug.WriteLine("The screentime end is: " + newScreenTimeEnd);

                    if (!(newScreenTimeEnd + 15 <= lowerLimit || higherLimit <= newScreenTimeBegin))
                    {
                        System.Diagnostics.Debug.WriteLine("Kilépés");
                        return;
                    }
                }
            }

            var newHall = new Hall
            {
                Name = hallName.ToString(),
                RowCount = 10,
                ColumnCount = 10
            };

            var newScreening = new ScreeningViewModel
            {
                MovieId = SelectedItem.Id,
                ScreenTime = screenTime.ToString(),
                Name = " ",
                PhoneNumber = " ",
                ScreeningHall = newHall,
                Seats = new List<Seat>(100)

            };


            /*
            for (int i = 0; i < newHall.RowCount; i++)
            {
                for (int j = 0; j < newHall.ColumnCount; j++)
                {
                    newScreening.Seats.Add(new Seat
                    {
                        RowID = i,
                        ColumnID = j,
                        SeatValue = 0,
                        PhoneNumber = "",
                        Name = ""
                    });

                }

            }*/

            var screeningDto = (ScreeningDto)newScreening;

            try
            {
                await _service.CreateScreeningAsync(screeningDto);
            }

            catch (Exception ex) when (ex is NetworkException || ex is HttpRequestException)
            {
                OnMessageApplication($"Unexpected error occured! ({ex.Message})");
            }


        }

        private async void DeleteMovie(MovieViewModel item)
        {
            try
            {
                await _service.DeleteMovieAsync(item.Id);
                Movies.Remove(SelectedItem);
                SelectedItem = null;
            }
            catch (Exception ex) when (ex is NetworkException || ex is HttpRequestException)
            {
                OnMessageApplication($"Unexpected error occured! ({ex.Message})");
            }
        }
        private void StartEditMovie()
        {
            EditableMovie = SelectedItem.ShallowClone();
            StartingMovieEdit?.Invoke(this, EventArgs.Empty);
        }
        private void CancelMovieEdit()
        {
            EditableMovie = null;
            FinishingMovieEdit?.Invoke(this, EventArgs.Empty);
        }
        private async void SaveMovieEdit()
        {
            try
            {
                SelectedItem.CopyFrom(EditableMovie);
                await _service.UpdateMovieAsync((MovieDto)SelectedItem);
                if (SelectedItem.ListId != SelectedList.Id)
                {
                    Movies.Remove(SelectedItem);
                    SelectedItem = null;
                }
            }
            catch (Exception ex) when (ex is NetworkException || ex is HttpRequestException)
            {
                OnMessageApplication($"Unexpected error occured! ({ex.Message})");
            }
            FinishingMovieEdit?.Invoke(this, EventArgs.Empty);
        }

        #endregion
    }
}
