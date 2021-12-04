using System;
using System.Collections.Generic;
using System.Text;

namespace Cinema.Persistence.DTO
{
    public class MovieDto
    {

        public Int32 Id { get; set; }


        public String Title { get; set; }

        public String Director { get; set; }


        public String Szinopszis { get; set; }

        public Int32 Length { get; set; }

        public DateTime ReleaseDate { get; set; }

        public Int32 ListId { get; set; }

        public byte[] Image { get; set; }

        public virtual List<Screening> Screenings { get; set; }

        public String ScreeningTimes { get; set; }


        public static explicit operator Movie(MovieDto dto) => new Movie
        {
            Id = dto.Id,
            Title = dto.Title,
            Director = dto.Director,
            Szinopszis = dto.Szinopszis,
            Length = dto.Length,
            ReleaseDate = dto.ReleaseDate,
            ListId = dto.ListId,
            ScreeningTimes = dto.ScreeningTimes,
            Image = dto.Image,
            Screenings = dto.Screenings,
            
        };

        public static explicit operator MovieDto(Movie m) => new MovieDto
        {
            Id = m.Id,
            Title = m.Title,
            Director = m.Director,
            Szinopszis = m.Szinopszis,
            Length = m.Length,
            ReleaseDate = m.ReleaseDate,
            ListId = m.ListId,
            ScreeningTimes = m.ScreeningTimes,
            Image = m.Image,
            Screenings = m.Screenings,
            
        };
    }
}
