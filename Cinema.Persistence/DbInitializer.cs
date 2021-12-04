using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Cinema.Persistence
{
    public static class DbInitializer
    {
        public static void Initialize(CinemaDbContext context, string imageDirectory) //Az Image-hez módosítani kellett az appsettings.json-t
        {
            //context.Database.Migrate(); //Ha vannak migrációk, amik még nem kerültek alkalmazásra, akkor azokat alkalmazza
            //context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            if (context.Lists.Any()) //MEgnézhetjük, hogy vannak-e már addatok az adatbázisban
            {
                return; //Ha már van adat akkor return
            }

            var tablePath = Path.Combine(imageDirectory, "table.png");
            var helyzetPath = Path.Combine(imageDirectory, "helyzet7.png");
            var starrPath = Path.Combine(imageDirectory, "starr.png");

            var blackwidow = Path.Combine(imageDirectory, "BlackWidow.png");
            var gentlemans = Path.Combine(imageDirectory, "Gentlemans.png");
            var tenet = Path.Combine(imageDirectory, "Tenet.png");
            var avengers = Path.Combine(imageDirectory, "Avengers.png");
            var indiana = Path.Combine(imageDirectory, "Indiana.png");
            var swIV = Path.Combine(imageDirectory, "SW3.png");
            var swIII = Path.Combine(imageDirectory, "SW4.png");
            var batman = Path.Combine(imageDirectory, "Batman.png");
            var bohem = Path.Combine(imageDirectory, "Bohem.png");
            var nemo = Path.Combine(imageDirectory, "Nemo.png");
            var ragadozo = Path.Combine(imageDirectory, "Ragadozo.png");

            IList<List> defaultLists = new List<List>
            {
                new List
                {
                    Name= "Filmek",
                    Movies = new List<Movie>
                    {
                        new Movie
                        {
                            Title =  "Black Widow",
                            Length =  133,
                            Director= "Cate Shortland",
                            Szinopszis= "A Fekete Özvegy 2021 - es amerikai szuperhősfilm, gyártója a Marvel Studios, forgalmazója a Walt Disney Pictures.A Bosszúállókból megismert Natalia Romanova előzményfilmje, egyúttal a Marvel moziuniverzum huszonnegyedik filmje.A film rendezője Cate Shortland, írója Jac Schaeffer és Ned Benson.",
                            ReleaseDate= DateTime.Parse("July 9, 2021"),
                            Image = File.Exists(blackwidow) ? File.ReadAllBytes(blackwidow) : null, //Ha létezik a file az elérési úton, akkor az kerül be, ah nem akkor null lesz a helyén
                            ScreeningTimes = "12:20,14:20,15:20,17:20",
                            Screenings = new List<Screening>
                            {
                                new Screening { ScreenTime = "12:20", TakenSeats = 0, Seats = new List<Seat>(), Name = "", PhoneNumber="06-", ScreeningHall = new Hall{ RowCount = 10, ColumnCount = 10, Name = "A"} },
                                new Screening { ScreenTime = "14:20", TakenSeats = 0, Seats = new List<Seat>(), Name = "", PhoneNumber="06-", ScreeningHall = new Hall{ RowCount = 10, ColumnCount = 10, Name = "A"} },
                                new Screening { ScreenTime = "15:20", TakenSeats = 0, Seats = new List<Seat>(), Name = "", PhoneNumber="06-", ScreeningHall = new Hall{ RowCount = 10, ColumnCount = 10, Name = "A"} },
                                new Screening { ScreenTime = "17:20", TakenSeats = 0, Seats = new List<Seat>(), Name = "", PhoneNumber="06-", ScreeningHall = new Hall{ RowCount = 10, ColumnCount = 10, Name = "A"} }
                            }

                        },

                        new Movie
                        {
                            Title = "Tenet",
                            Length=150,
                            Director= "Cristopher Nolan",
                            Szinopszis= "A Tenet 2020-ban bemutatott brit-amerikai akciófilm, sci-fi, melyet Christopher Nolan írt és rendezett.A főszereplők John David Washington, Robert Pattinson, Elizabeth Debicki, Dimple Kapadia és Kenneth Branagh.",
                            ReleaseDate= DateTime.Parse("August 12, 2020"),
                            Image = File.Exists(tenet) ? File.ReadAllBytes(tenet) : null,
                            ScreeningTimes = "19:20,20:20",
                            Screenings = new List<Screening>
                            {
                                new Screening { ScreenTime = "19:20", TakenSeats = 0, Seats = new List<Seat>(), Name = "", PhoneNumber="06-", ScreeningHall = new Hall{ RowCount = 10, ColumnCount = 10, Name = "B"}},
                                new Screening { ScreenTime = "20:20", TakenSeats = 0, Seats = new List<Seat>(), Name = "", PhoneNumber="06-", ScreeningHall = new Hall{ RowCount = 10, ColumnCount = 10, Name = "B"}}
                            }
                        },

                        new Movie
                        {
                            Title= "Úriemberek",
                            Length= 113,
                            Director= "Guy Ritchie",
                            Szinopszis= "Az Úriemberek 2020-ban bemutatott brit-amerikai akció-vígjáték, melyet Guy Ritchie írt, rendezett és készített, Ivan Atkinson, Marn Davies és Ritchie történetéből.A főszereplők Matthew McConaughey, Charlie Hunnam, Henry Golding, Michelle Dockery, Jeremy Strong, Eddie Marsan, Colin Farrell és Hugh Grant.",
                            ReleaseDate= DateTime.Parse("Jan 1, 2020"),
                            Image = File.Exists(gentlemans) ? File.ReadAllBytes(gentlemans) : null,
                            ScreeningTimes = "18:20,21:20",
                            Screenings = new List<Screening>
                            {
                                new Screening { ScreenTime = "18:20", TakenSeats = 0, Seats = new List<Seat>(), Name = "", PhoneNumber="06-", ScreeningHall = new Hall{ RowCount = 10, ColumnCount = 10, Name = "C"}},
                                new Screening { ScreenTime = "21:20", TakenSeats = 0, Seats = new List<Seat>(), Name = "", PhoneNumber="06-", ScreeningHall = new Hall{ RowCount = 10, ColumnCount = 10, Name = "C"}}
                            }
                        },


                        new Movie
                        {
                            Title= "Bosszúállók:Végjáték",
                            Length= 182,
                            Director="Joe Russo, Anthony Russo",
                            Szinopszis= "A Bosszúállók: Végjáték 2019-ben bemutatott amerikai szuperhős film, mely a Marvel képregények szuperhős csapatán alapszik.",
                            ReleaseDate= DateTime.Parse("April 24, 2019"),
                            Image = File.Exists(avengers) ? File.ReadAllBytes(avengers) : null,
                            ScreeningTimes = "15:40,19:40",
                            Screenings = new List<Screening>
                            {
                                new Screening { ScreenTime = "15:40", TakenSeats = 0, Seats = new List<Seat>(), Name = "", PhoneNumber="06-", ScreeningHall = new Hall{ RowCount = 10, ColumnCount = 10, Name = "D"}},
                                new Screening { ScreenTime = "19:40", TakenSeats = 0, Seats = new List<Seat>(), Name = "", PhoneNumber="06-", ScreeningHall = new Hall{ RowCount = 10, ColumnCount = 10, Name = "D"}}
                            }
                        },

                        new Movie
                        {
                            Title= "Az elveszett frigyláda fosztogatói",
                            Length= 115,
                            Director= "Steven Spielberg",
                            Szinopszis= "Az elveszett frigyláda fosztogatói, ismert még Indiana Jones és az elveszett frigyláda fosztogatói címmel is, 1981-ben bemutatott amerikai kalandfilm Steven Spielberg rendezésében, George Lucas produceri közreműködésével és Harrison Ford főszereplésével.",
                            ReleaseDate= DateTime.Parse("October 27, 1985"),
                            Image = File.Exists(indiana) ? File.ReadAllBytes(indiana) : null,
                            ScreeningTimes = "22:20",
                            Screenings = new List<Screening>
                            {
                                new Screening { ScreenTime = "22:20", TakenSeats = 0, Seats = new List<Seat>(), Name = "", PhoneNumber="06-", ScreeningHall = new Hall{ RowCount = 10, ColumnCount = 10, Name = "E"}},
                            }
                        },


                        new Movie
                        {

                            Title= "Star Wars IV: New Hope",
                            Length= 125,
                            Director= "George Lucas",
                            Szinopszis= "A Star Wars IV: Egy új remény 1977-ben bemutatott amerikai sci-fi, amelynek címe az eredeti Csillagok háborúja című film retronim elnevezése. Ezzel a George Lucas által rendezett filmmel indult el a kilenc részből álló Csillagok háborúja-sorozat.",
                            ReleaseDate= DateTime.Parse("August 16, 1979"),
                            Image = File.Exists(swIV) ? File.ReadAllBytes(swIV) : null,
                            ScreeningTimes = "16:20,18:20",
                            Screenings = new List<Screening>
                            {
                                new Screening { ScreenTime = "16:20", TakenSeats = 0, Seats = new List<Seat>(), Name = "", PhoneNumber="06-", ScreeningHall = new Hall{ RowCount = 10, ColumnCount = 10, Name = "F"}},
                                new Screening { ScreenTime = "18:20", TakenSeats = 0, Seats = new List<Seat>(), Name = "", PhoneNumber="06-", ScreeningHall = new Hall{ RowCount = 10, ColumnCount = 10, Name = "F"}}
                            }
                        },

                        new Movie
                        {
                            Title= "Star Wars 3: A shitek bosszúja",
                            Length= 140,
                            Director= "George Lucas",
                            Szinopszis= "A Star Wars III. rész – A sithek bosszúja 2005-ben bemutatott amerikai sci-fi, amelyet George Lucas írt és rendezett. Ez a hatodik film a Csillagok háborúja sorozatból, amely a cselekmény alapján sorrendben a harmadik. A film három évvel a Klónháborúk megkezdése után játszódik.",
                            ReleaseDate= DateTime.Parse("May 19, 2005"),
                            Image = File.Exists(swIII) ? File.ReadAllBytes(swIII) : null,
                            ScreeningTimes = "19:20,20:20",
                            Screenings = new List<Screening>
                            {
                                new Screening { ScreenTime = "19:45", TakenSeats = 0, Seats = new List<Seat>(), Name = "", PhoneNumber="06-", ScreeningHall = new Hall{ RowCount = 10, ColumnCount = 10, Name = "G"}},
                                new Screening { ScreenTime = "20:20", TakenSeats = 0, Seats = new List<Seat>(), Name = "", PhoneNumber="06-", ScreeningHall = new Hall{ RowCount = 10, ColumnCount = 10, Name = "G"}},
                            }
                        },
                        /*
                        new Movie
                        {
                            Title= "Némó nyomában",
                            Length= 100,
                            Director= "Andrew Stanton",
                            Szinopszis= "A Némó nyomában 2003-ban bemutatott egész estés amerikai 3D-s számítógépes animációs film, amely az 5. Pixar-film. Az animációs filmet Lee Unkrich rendezett, a forgatókönyvet Andrew Stanton írta, a zenéjét Thomas Newman szerezte, a producere Graham Walters.",
                            ReleaseDate= DateTime.Parse("November 20, 2003"),
                            Image = File.Exists(nemo) ? File.ReadAllBytes(nemo) : null
                        },

                        new Movie
                        {
                            Title= "Bohém rapszódia",
                            Length= 133,
                            Director= "Bryan Singer",
                            Szinopszis= "A Bohém rapszódia című életrajzi film a brit Queen rockegyüttes történetéről szól, középpontban a frontember Freddie Mercury életével. A film végigköveti az együttes felemelkedését, Mercury szólókarrierjének indulását, és azt az időszakot, amikor az énekesnél diagnosztizálták az AIDS betegséget.",
                            ReleaseDate= DateTime.Parse("October 24, 2018"),
                            Image = File.Exists(bohem) ? File.ReadAllBytes(bohem) : null
                        },

                        new Movie
                        {
                            Title= "A sötét lovag",
                            Length= 152,
                            Director= "Cristopher Nolan",
                            Szinopszis= "A sötét lovag egy 2008-as amerikai–brit szuperhős-film, amelynek társírója és rendezője Christopher Nolan. A DC Comics képregénykiadó Batman szereplőjén alapuló film a Batman: Kezdődik! folytatása. A címszerepet ismét Christian Bale alakítja.",
                            ReleaseDate= DateTime.Parse("August 7, 2008"),
                            Image = File.Exists(batman) ? File.ReadAllBytes(batman) : null
                        }
                        */
                    }
                },

                /*new List
                {
                    Name= "Beadandók",
                    Items = new List<Item>
                    {
                        new Item
                        {
                            Name = "MVC beadandó",
                            Description = "Nem lesz nehéz.",
                            Deadline = DateTime.Now.AddDays(7)
                        },

                        new Item
                        {
                            Name = "Körte",
                            Description = "Ez még kevésbé lesz nehéz.",
                            Deadline = DateTime.Now.AddDays(10)
                        }
                    }
                }*/
            };

            for (int x = 0; x < defaultLists[0].Movies.Count; x++)
            {
                for (int y = 0; y < defaultLists[0].Movies.ElementAt(x).Screenings.Count; y++)
                {
                    for (int i = 0; i < 10; i++)
                    {
                        for (int j = 0; j < 10; j++)
                        {
                            defaultLists[0].Movies.ElementAt(x).Screenings[y].Seats.Add(new Seat
                            {
                                //Id = i * 10 + j,
                                RowID = i,
                                ColumnID = j,
                                SeatValue = 0,
                                PhoneNumber = "",
                                Name = ""
                            });

                            System.Diagnostics.Debug.WriteLine("Name: " + defaultLists[0].Movies.ElementAt(x).Title + " " + " Value: " + i + " " + j);
                        }

                    }
                }
            }

            /*
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    defaultLists[0].Movies.ElementAt(0).Screenings[0].Seats.Add(new Seat
                    {
                        //Id = i * 10 + j,
                        RowID = i,
                        ColumnID = j,
                        SeatValue = 0,
                        PhoneNumber="",
                        Name=""
                    });
                    defaultLists[0].Movies.ElementAt(0).Screenings[1].Seats.Add(new Seat
                    {
                        //Id = i * 10 + j,
                        RowID = i,
                        ColumnID = j,
                        SeatValue = 0,
                        PhoneNumber = "",
                        Name = ""
                    });
                    defaultLists[0].Movies.ElementAt(0).Screenings[2].Seats.Add(new Seat
                    {
                        //Id = i * 10 + j,
                        RowID = i,
                        ColumnID = j,
                        SeatValue = 0,
                        PhoneNumber = "",
                        Name = ""
                    });
                    defaultLists[0].Movies.ElementAt(0).Screenings[3].Seats.Add(new Seat
                    {
                        //Id = i * 10 + j,
                        RowID = i,
                        ColumnID = j,
                        SeatValue = 0,
                        PhoneNumber = "",
                        Name = ""
                    });
                    defaultLists[0].Movies.ElementAt(1).Screenings[0].Seats.Add(new Seat
                    {
                        //Id = i * 10 + j,
                        RowID = i,
                        ColumnID = j,
                        SeatValue = 0,
                        PhoneNumber = "",
                        Name = ""
                    });
                    defaultLists[0].Movies.ElementAt(1).Screenings[1].Seats.Add(new Seat
                    {
                        //Id = i * 10 + j,
                        RowID = i,
                        ColumnID = j,
                        SeatValue = 0,
                        PhoneNumber = "",
                        Name = ""
                    });
                    defaultLists[0].Movies.ElementAt(2).Screenings[0].Seats.Add(new Seat
                    {
                        //Id = i * 10 + j,
                        RowID = i,
                        ColumnID = j,
                        SeatValue = 0,
                        PhoneNumber = "",
                        Name = ""
                    });
                    defaultLists[0].Movies.ElementAt(2).Screenings[1].Seats.Add(new Seat
                    {
                        //Id = i * 10 + j,
                        RowID = i,
                        ColumnID = j,
                        SeatValue = 0,
                        PhoneNumber = "",
                        Name = ""
                    });
                    defaultLists[0].Movies.ElementAt(3).Screenings[0].Seats.Add(new Seat
                    {
                        //Id = i * 10 + j,
                        RowID = i,
                        ColumnID = j,
                        SeatValue = 0,
                        PhoneNumber = "",
                        Name = ""
                    });
                    defaultLists[0].Movies.ElementAt(3).Screenings[1].Seats.Add(new Seat
                    {
                        //Id = i * 10 + j,
                        RowID = i,
                        ColumnID = j,
                        SeatValue = 0,
                        PhoneNumber = "",
                        Name = ""
                    });
                    defaultLists[0].Movies.ElementAt(4).Screenings[0].Seats.Add(new Seat
                    {
                        //Id = i * 10 + j,
                        RowID = i,
                        ColumnID = j,
                        SeatValue = 0,
                        PhoneNumber = "",
                        Name = "",
                    });
                    defaultLists[0].Movies.ElementAt(5).Screenings[0].Seats.Add(new Seat
                    {
                        //Id = i * 10 + j,
                        RowID = i,
                        ColumnID = j,
                        SeatValue = 0,
                        PhoneNumber = "",
                        Name = ""
                    });
                    defaultLists[0].Movies.ElementAt(5).Screenings[1].Seats.Add(new Seat
                    {
                        //Id = i * 10 + j,
                        RowID = i,
                        ColumnID = j,
                        SeatValue = 0,
                        PhoneNumber = "",
                        Name = ""
                    });
                    defaultLists[0].Movies.ElementAt(6).Screenings[0].Seats.Add(new Seat
                    {
                        //Id = i * 10 + j,
                        RowID = i,
                        ColumnID = j,
                        SeatValue = 0,
                        PhoneNumber = "",
                        Name = ""
                    });
                    defaultLists[0].Movies.ElementAt(6).Screenings[1].Seats.Add(new Seat
                    {
                        //Id = i * 10 + j,
                        RowID = i,
                        ColumnID = j,
                        SeatValue = 0,
                        PhoneNumber = "",
                        Name = ""
                    });
                }
            }
            */
            context.AddRange(defaultLists); //Egy teljes listát ad hozzá
            context.SaveChanges(); // csak ez után kerülbe ténylegesen a lista
        }
    }
}
