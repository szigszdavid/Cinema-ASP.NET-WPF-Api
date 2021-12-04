using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Cinema.Persistence
{
    public class Movie
    {

        [Key]
        public Int32 Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        public String Title { get; set; }

        public String Director { get; set; }


        public String Szinopszis { get; set; }

        public Int32 Length { get; set; }

        [Required(ErrorMessage = "Release Date is required")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime ReleaseDate { get; set; }

        //public Image Poster { get; set; }

        public Int32 ListId { get; set; }

        [Required]
        public virtual List List { get; set; } //virtual nem muszáj //A Movie Create miatt kell és emiatt kell egy ViewModel -> mInden propertyt hozzáadunk kivéve ezt

        [Required]
        public String ScreeningTimes { get; set; }

        public int ScreeningSize { get; set; }

        public virtual List<Screening> Screenings { get; set; }
        public byte[] Image { get; set; }
    }
}
