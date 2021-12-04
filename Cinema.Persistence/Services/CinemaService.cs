using Cinema.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cinema.Persistence.Services
{
    public class CinemaService : ICinemaService
    {
        private readonly CinemaDbContext _context;

        public CinemaService(CinemaDbContext context)
        {
            _context = context;
        }

        public List<List> GetLists(String name = null)
        {
            return _context.Lists
                .Where(l => l.Name.Contains(name ?? ""))
                .OrderBy(l => l.Name)
                .ToList();
        }

        public List GetListByID(int id)
        {
            return _context.Lists
                .Single(l => l.Id == id);
        }

        public List<Movie> GetMoviesByListID(int id)
        {
            /*
            var list = _context.Lists
                .Include(l => l.Movies)
                .Where(l => l.Id == id)
                .Select(l => l.Movies);

            return list.Movies.ToList();*/

            return _context.Movies
                .Where(i => i.ListId == id)
                .ToList();
        }

        public List<Screening> GetScreeningsByMovieID(int id)
        {
            return _context.Screenings
                .Where(i => i.MovieId == id)
                .ToList();
        }

        public List GetListDetails(int id)
        {
            return _context.Lists
                .Include(l => l.Movies)
                .Single(l => l.Id == id);
        }

        public Movie GetMovie(int id)
        {
            return _context.Movies
                .FirstOrDefault(i => i.Id == id); //Visszadja az első olyat, amelyiknek az id-ja megegyezik a paraméterben megadott id-val
        }

        public Screening GetScreening(int id)
        {
            return _context.Screenings
                .FirstOrDefault(i => i.Id == id); //Visszadja az első olyat, amelyiknek az id-ja megegyezik a paraméterben megadott id-val
        }

        //Lista módosító metódusok

        public List CreateList(List list)
        {
            try
            {
                _context.Add(list); //Még csak a trackerhez adtuk hozzá
                _context.SaveChanges(); //Ez dobhat Exceptioneket
            }
            catch (DbUpdateConcurrencyException) //Ha az  adatbzáist úgy módosítanák, hogy az előző módosítások nem lettek elmentve
            {
                return null;
            }
            catch (DbUpdateException)
            {
                return null;
            }

            return list; //Ezután ListController kell
        }

        public bool UpdateList(List list)
        {
            try
            {
                _context.Update(list); //Még csak a trackerhez adtuk hozzá
                _context.SaveChanges(); //Ez dobhat Exceptioneket
            }
            catch (DbUpdateConcurrencyException) //Ha az  adatbzáist úgy módosítanák, hogy az előző módosítások nem lettek elmentve
            {
                return false;
            }
            catch (DbUpdateException)
            {
                return false;
            }

            return true; //Ezután ListController kell
        }

        public bool DeleteList(int id)
        {
            var list = _context.Lists.Find(id); //A Find visszaadhat null-t

            if (list == null)
            {
                return false;
            }

            try
            {
                _context.Remove(list); //Még csak a trackerhez adtuk hozzá
                _context.SaveChanges(); //Ez dobhat Exceptioneket
            }
            catch (DbUpdateConcurrencyException) //Ha az  adatbzáist úgy módosítanák, hogy az előző módosítások nem lettek elmentve
            {
                return false;
            }
            catch (DbUpdateException)
            {
                return false;
            }

            return true; //Ezután ListController kell

        }

        public Movie CreateMovie(Movie movie)
        {
            System.Diagnostics.Debug.WriteLine("CreateMovie");
            var screenings = movie.ScreeningTimes;
            System.Diagnostics.Debug.WriteLine(screenings);
            string[] screeningTimes = screenings.Split(",");

            movie.Screenings = new List<Screening>();
            for (int i = 0; i < screeningTimes.Length; i++)
            {
                movie.Screenings.Add(new Screening { ScreenTime = screeningTimes[i], TakenSeats = 0, Seats = new List<Seat>(),
                    PhoneNumber = "",
                    Name = ""
                });

                for (int z = 0; z < 10; z++)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        movie.Screenings[i].Seats.Add(new Seat
                        {
                            //Id = i * 10 + j,
                            RowID = z,
                            ColumnID = j,
                            SeatValue = 0,
                            PhoneNumber = "",
                            Name = ""
                        });
                    }
                }
            }

            movie.ScreeningSize = screeningTimes.Length;

            System.Diagnostics.Debug.Write("Screenings: " + screenings);



            try
            {
                _context.Add(movie); //Még csak a trackerhez adtuk hozzá
                _context.SaveChanges(); //Ez dobhat Exceptioneket
            }
            catch (DbUpdateConcurrencyException) //Ha az  adatbzáist úgy módosítanák, hogy az előző módosítások nem lettek elmentve
            {
                System.Diagnostics.Debug.WriteLine("DbUpdateConcurrency");
                return null;
            }
            catch (DbUpdateException)
            {
                System.Diagnostics.Debug.WriteLine("DbUpdateException");
                return null;
            }

            return movie; //Ezután ListController kell
        }

        public Screening CreateScreening(Screening screening)
        {
            System.Diagnostics.Debug.WriteLine("CreateScreening");
            try
            {
                _context.Screenings.Add(screening); //Még csak a trackerhez adtuk hozzá
                _context.SaveChanges(); //Ez dobhat Exceptioneket
            }
            catch (DbUpdateConcurrencyException) //Ha az  adatbzáist úgy módosítanák, hogy az előző módosítások nem lettek elmentve
            {
                System.Diagnostics.Debug.WriteLine("DbUpdateConcurrency");
                return null;
            }
            catch (DbUpdateException)
            {
                System.Diagnostics.Debug.WriteLine("DbUpdateException");
                return null;
            }

            return screening;
        }

        public bool DeleteMovie(int id)
        {
            var movie = _context.Movies.Find(id); //A Find visszaadhat null-t

            if (movie == null)
            {
                System.Diagnostics.Debug.WriteLine("Törlés2");
                return false;
            }

            try
            {
                System.Diagnostics.Debug.WriteLine("Törlés3");
                _context.Remove(movie); //Még csak a trackerhez adtuk hozzá

                _context.SaveChanges(); //Ez dobhat Exceptioneket
            }
            catch (DbUpdateConcurrencyException) //Ha az  adatbzáist úgy módosítanák, hogy az előző módosítások nem lettek elmentve
            {
                return false;
            }
            catch (DbUpdateException)
            {
                return false;
            }

            return true; //Ezután ListController kell
        }

        public bool UpdateMovie(Movie movie)
        {
            try
            {
                var screenings = movie.ScreeningTimes;

                string[] screeningTimes = screenings.Split(",");

                System.Diagnostics.Debug.WriteLine("Count: " + movie.ScreeningSize);
                System.Diagnostics.Debug.WriteLine("Length: " + screeningTimes.Length);

                if (movie.ScreeningSize != screeningTimes.Length)
                {
                    for (int i = 0; i < screeningTimes.Length; i++)
                    {
                        bool sameTime = false;
                        for (int j = 0; j < movie.ScreeningSize; j++)
                        {
                            if (screeningTimes[i] == movie.Screenings[j].ScreenTime)
                            {
                                sameTime = true;
                            }
                        }
                        /*
                        if (sameTime == false)
                        {
                            int screeningSize = movie.ScreeningSize;
                            movie.Screenings.Add(new Screening { ScreenTime = screeningTimes[i], TakenSeats = 0, Seats = new List<Seat>() });

                            for (int z = 0; z < 10; z++)
                            {
                                for (int j = 0; j < 10; j++)
                                {
                                    movie.Screenings[screeningSize + 1].Seats.Add(new Seat
                                    {
                                        //Id = i * 10 + j,
                                        RowID = z,
                                        ColumnID = j,
                                        SeatValue = 0
                                    });
                                }
                            }
                        }*/
                    }
                }



                _context.Update(movie); //Még csak a trackerhez adtuk hozzá


                if (movie.Image == null) //Ha nem akarnánk updatelni a képet, akkor megnézzük, hogy a paraméter null-e
                {
                    _context.Entry(movie).Property("Image").IsModified = false; //Tehát a movie Image propertije nem lett módosítva, ezt jelenti
                }
                _context.SaveChanges(); //Ez dobhat Exceptioneket
            }
            catch (DbUpdateConcurrencyException) //Ha az  adatbzáist úgy módosítanák, hogy az előző módosítások nem lettek elmentve
            {
                return false;
            }
            catch (DbUpdateException)
            {
                return false;
            }

            return true;
        }

        public Int32 GetSteat(Screening screening, int i, int j)
        {
            return screening.Seats[i * 10 + j].SeatValue;
        }

        public bool SeatClicked(Screening screening, int i, int j)
        {
            if (screening.Seats[i * 10 + j].SeatValue == 0 && screening.TakenSeats < 6)
            {
                screening.Seats[i * 10 + j].SeatValue = 2;
                System.Diagnostics.Debug.WriteLine("Ez már sárga: " + i * 10 + " " + j);
                screening.TakenSeats++;
            }
            else if (screening.Seats[i * 10 + j].SeatValue == 2)
            {
                screening.Seats[i * 10 + j].SeatValue = 0;
                screening.TakenSeats--;
            }
            else
            {
                return true;
            }


            try
            {
                _context.Update(screening); //Még csak a trackerhez adtuk hozzá

                _context.SaveChanges(); //Ez dobhat Exceptioneket
            }
            catch (DbUpdateConcurrencyException) //Ha az  adatbzáist úgy módosítanák, hogy az előző módosítások nem lettek elmentve
            {
                return false;
            }
            catch (DbUpdateException)
            {
                return false;
            }

            //System.Diagnostics.Debug.Write("takenseats: " + movie.TakenSeats);
            return true;

        }

        public bool PurchaseClicked(Screening screening, string name, string phoneNumber)
        {
            System.Diagnostics.Debug.WriteLine("Id-ja: " + screening.Id);
            System.Diagnostics.Debug.WriteLine("Ez a screeming:" + screening.Seats);
            System.Diagnostics.Debug.WriteLine("Pname: " + name);
            System.Diagnostics.Debug.WriteLine("PPnumber: " + phoneNumber);

            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (screening.Seats[i * 10 + j].SeatValue == 2)
                    {
                        screening.Seats[i * 10 + j].SeatValue = 1;
                        screening.Seats[i * 10 + j].PhoneNumber = phoneNumber;
                        screening.Seats[i * 10 + j].Name = name;

                        System.Diagnostics.Debug.WriteLine("i: " + i + " j: " + j + " name: " + screening.Seats[i * 10 + j].Name + " RowID: " + screening.Seats[i * 10 + j].RowID + " ColumnID: " + screening.Seats[i * 10 + j].ColumnID);


                        System.Diagnostics.Debug.WriteLine("Purchased");
                    }
                    else if (screening.Seats[i * 10 + j].SeatValue == 1)
                    {
                        System.Diagnostics.Debug.WriteLine("1");
                    }
                }
            }

            screening.TakenSeats = 0;

            try
            {
                _context.Update(screening); //Még csak a trackerhez adtuk hozzá

                _context.SaveChanges(); //Ez dobhat Exceptioneket
            }
            catch (DbUpdateConcurrencyException) //Ha az  adatbzáist úgy módosítanák, hogy az előző módosítások nem lettek elmentve
            {
                return false;
            }
            catch (DbUpdateException)
            {
                return false;
            }

            return true;
        }

        public Screening GetScreening(int id, string time)
        {
            System.Diagnostics.Debug.WriteLine("GetScreening: " + id);
            return _context.Screenings
            .FirstOrDefault(i => i.MovieId == id && i.ScreenTime == time);
        }

        public bool UpdateScreening(Movie movie)
        {
            int i = 0;
            while (movie.Id == movie.Screenings[i].MovieId)
            {
                i++;
            }
            try
            {
                _context.Update(movie); //Még csak a trackerhez adtuk hozzá

                if (movie.Image == null) //Ha nem akarnánk updatelni a képet, akkor megnézzük, hogy a paraméter null-e
                {
                    _context.Entry(movie).Property("Image").IsModified = false; //Tehát a movie Image propertije nem lett módosítva, ezt jelenti
                }
                _context.SaveChanges(); //Ez dobhat Exceptioneket
            }
            catch (DbUpdateConcurrencyException) //Ha az  adatbzáist úgy módosítanák, hogy az előző módosítások nem lettek elmentve
            {
                return false;
            }
            catch (DbUpdateException)
            {
                return false;
            }

            return true;
        }

        public bool DeleteScreening(int id)
        {
            throw new NotImplementedException();
        }

        public Screening GetScreeningOnlybyId(int id)
        {
            return _context.Screenings
            .FirstOrDefault(i => i.MovieId == id);
        }

        public Screening GetScreeningBYIdAndTime(int id)
        {
            var screening = _context.Screenings.FirstOrDefault(i => i.Id == id);
            return screening;
        }

        public bool UpdateSeat(int id, int screeningId)
        {
            var screening = _context.Screenings.FirstOrDefault(i => i.Id == screeningId);

            screening.Seats[id].SeatValue = 3;
            /*
            var seat = _context.Seats.FirstOrDefault(seat => seat.Id == id);
            seat.SeatValue = 2;*/
            try
            {
                _context.Update(screening); //Még csak a trackerhez adtuk hozzá
                _context.SaveChanges(); //Ez dobhat Exceptioneket
            }
            catch (DbUpdateConcurrencyException) //Ha az  adatbzáist úgy módosítanák, hogy az előző módosítások nem lettek elmentve
            {
                return false;
            }
            catch (DbUpdateException)
            {
                return false;
            }
            
            return true;
        }
    }
}
