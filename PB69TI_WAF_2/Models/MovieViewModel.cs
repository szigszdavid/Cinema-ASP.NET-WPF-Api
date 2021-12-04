using Cinema.Persistence;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Cinema.Web.Models
{
    public class MovieViewModel //Innetől mindig ezen dolgozunk és utána konvetráljuk Movie-vá
    {
        [Key]
        public Int32 Id { get; set; }

        [Required(ErrorMessage = "A title megadása kötelező")]
        public String Title { get; set; }

        public String Director { get; set; }


        public String Szinopszis { get; set; }

        public Int32 Length { get; set; }

        public DateTime ReleaseDate { get; set; }

        //public Image Poster { get; set; }
        [DisplayName("List")]
        public Int32 ListId { get; set; }

        public byte[] Image { get; set; }

        public virtual List<Screening> Screenings { get; set; }

        public String ScreeningTimes { get; set; }


        public static explicit operator Movie(MovieViewModel vm) => new Movie //Movievá alakítjuk a MovieViewModelt
        {
            Id = vm.Id,
            Title = vm.Title,
            Director = vm.Director,
            Szinopszis = vm.Szinopszis,
            Length = vm.Length,
            ReleaseDate = vm.ReleaseDate,
            ListId = vm.ListId,
            ScreeningTimes = vm.ScreeningTimes,
            Screenings = vm.Screenings
        };

        public static explicit operator MovieViewModel(Movie m) => new MovieViewModel //MovieViewModellé alakítjuk a Moviet
        {
            Id = m.Id,
            Title = m.Title,
            Director = m.Director,
            Szinopszis = m.Szinopszis,
            Length = m.Length,
            ReleaseDate = m.ReleaseDate,
            ListId = m.ListId,
            ScreeningTimes = m.ScreeningTimes,
            Screenings = m.Screenings
        };
    }
}
