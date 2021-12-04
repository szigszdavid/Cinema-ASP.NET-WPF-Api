using Cinema.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cinema.Persistence.Services
{
    public interface ICinemaService
    {
        public List<List> GetLists(String name = null);
        public List GetListByID(int id);

        public List<Movie> GetMoviesByListID(int id);

        public List GetListDetails(int id);

        public Movie GetMovie(int id);

        public Screening GetScreening(int id);

        //Lista módosító metódusok:

        List CreateList(List list); //Sikeres volt a létrehozás vagy nem -> ehhez kell a bool

        bool UpdateList(List list);

        bool DeleteList(int id);

        //Movie módosító metódusok:

        Movie CreateMovie(Movie movie); //Sikeres volt a létrehozás vagy nem -> ehhez kell a bool

        Screening CreateScreening(Screening screening);

        bool UpdateMovie(Movie movie);

        bool DeleteMovie(int id);

        public Int32 GetSteat(Screening screening, int i, int j);

        public bool SeatClicked(Screening screening, int i, int j);

        public bool PurchaseClicked(Screening screening, string name, string phoneNumber);

        //Sikeres volt a létrehozás vagy nem -> ehhez kell a bool

        public Screening GetScreening(int id, string time);

        public Screening GetScreeningOnlybyId(int id);

        public Screening GetScreeningBYIdAndTime(int id);


        bool UpdateScreening(Movie movie);

        bool DeleteScreening(int id);

        List<Screening> GetScreeningsByMovieID(int id);
        bool UpdateSeat(int id,int screeningID);
    }
}
