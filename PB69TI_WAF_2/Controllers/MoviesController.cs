using Cinema.Persistence.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cinema.Persistence;
using Microsoft.AspNetCore.Http;
using System.IO;
using Cinema.Web.Models;
using Microsoft.AspNetCore.Authorization;

namespace Cinema.Web.Controllers
{
    [Authorize]
    public class MoviesController : Controller
    {
        private readonly ICinemaService _service;
        public MoviesController(ICinemaService service)
        {
            _service = service;
        }

        [AllowAnonymous]
        public IActionResult DisplayImage(int id)
        {
            var item = _service.GetMovie(id);
            if (item == null)
            {
                return null;
            }
            return File(item.Image, "image/png"); //byte array és a típus, pl:docx is lehetne
        }

        //Movie kezeló metódusok:

        public IActionResult Create()
        {
            //Kell az, hogy  melyik listához legyen hozzáadva -> legördülő lista <- lista nevekkel Fontos!!: A Create.htmlcs-ben a ListId tagnál a ViewBag.Lists legyen ListId helyett
            ViewBag.Lists = new SelectList(_service.GetLists(), "Id", "Name", TempData["ListId"]);//ViewBag a controllerek és nézetek között szállítanAK adatokat
            //SelectList paraméterek: 1.értékek 2.melyik mező alapján sorol fel 3.Amit látni fogunk 4. Alapértelmezetten mi lesz kiválasztva(objektum)
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(MovieViewModel vm, IFormFile image) //az add Viewnál a Model: Movie //Kell majd egy action a ListControllerbe is
        {
            var movie = (Movie)vm; //Ez az operatoros rész miatt működik

            if(image != null && image.Length > 0) //Átkonvertáljuk a képet byte[]-re
            {
                using (var stream = new MemoryStream())
                {
                    image.CopyTo(stream);
                    movie.Image = stream.ToArray(); //byte tömbbé konvertáltuk a képet és odaadtuk a movienak
                }
            }

            if(ModelState.IsValid)
            {
                _service.CreateMovie(movie);
                return RedirectToAction("Details", "Lists", new { id = movie.ListId}); //Visszatérünk az elemek felsorolásához
                //Details nevű action, a ListsControlerben, átadjuk a movie ListIdját mert kell a Details actionnek egy id paraméter
            }

            ViewBag.Lists = new SelectList(_service.GetLists(), "Id", "Name", TempData["ListId"]);//ViewBag a controllerek és nézetek között szállítanAK adatokat
            //SelectList paraméterek: 1.értékek 2.melyik mező alapján sorol fel 3.Amit látni fogunk 4. Alapértelmezetten mi lesz kiválasztva(objektum)
            return View();

            //enctype-t be kell állítani
        }


        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                
                return NotFound();
            }

            var movie = _service.GetMovie((int)id); //Próbáljuk lekérni a listet

            if (movie == null) //Ha nem találjuk a listet, akkor ez
            {
                return NotFound();
            }

            System.Diagnostics.Debug.WriteLine("Editben");
            ViewBag.Lists = new SelectList(_service.GetLists(), "Id", "Name", movie.ListId);//ViewBag a controllerek és nézetek között szállítanAK adatokat
            return View(movie);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, MovieViewModel vm, IFormFile image)
        {
            //Lehet, hogy a két id nem egyezik meg

            if (id != vm.Id)
            {
                return NotFound();
            }

            var movie = (Movie)vm;

            if (image != null && image.Length > 0) //Átkonvertáljuk a képet byte[]-re
            {
                using (var stream = new MemoryStream())
                {
                    image.CopyTo(stream);
                    movie.Image = stream.ToArray(); //byte tömbbé konvertáltuk a képet és odaadtuk a movienak
                }
            }

            if (ModelState.IsValid)
            {
                _service.UpdateMovie(movie);
                return RedirectToAction("Details", "Lists", new { id = movie.ListId }); //Visszatérünk az elemek felsorolásához
                //Details nevű action, a ListsControlerben, átadjuk a movie ListIdját mert kell a Details actionnek egy id paraméter
            }


            ViewBag.Lists = new SelectList(_service.GetLists(), "Id", "Name", movie.ListId);//ViewBag a controllerek és nézetek között szállítanAK adatokat
            //SelectList paraméterek: 1.értékek 2.melyik mező alapján sorol fel 3.Amit látni fogunk 4. Alapértelmezetten mi lesz kiválasztva(objektum)
            return View(movie);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = _service.GetMovie((int)id); //Próbáljuk lekérni a listet

            if (movie == null) //Ha nem találjuk a listet, akkor ez
            {
                return NotFound();
            }

            return View(movie);
        }

        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var movie = _service.GetMovie(id);
            
            if(movie != null)
            {
                System.Diagnostics.Debug.WriteLine("Törlés");
                _service.DeleteMovie(id);
                return RedirectToAction("Details", "Lists", new { id = movie.ListId }); //Visszaugrunk az index oldalra minden esetben
            }
            else
            {
                return NotFound();
            }

        }

        public IActionResult Details(int id)
        {
            System.Diagnostics.Debug.WriteLine("Detailsben");
            var movie = _service.GetMovie(id);

            return View(movie);
        }

        public IActionResult GetSeat(int id, int rowindex, int columnindex,string time)
        {
            var screening = _service.GetScreening(id,time);

            if (screening != null)
            {
                _service.GetSteat(screening, rowindex, columnindex);
            }

            return View(screening);
        }

        public IActionResult SeatClicked(int id, int rowindex, int columnindex,string time)
        {
            var screening = _service.GetScreening(id,time);
            var firstScreening = _service.GetScreeningOnlybyId(id);

            System.Diagnostics.Debug.WriteLine("Seat MovieId: " + id);
            System.Diagnostics.Debug.WriteLine("Seat ScreeningId: " + screening.Id);
            System.Diagnostics.Debug.WriteLine("Seat FirstScreeningId: " + firstScreening.Id);

            if (screening != null)
            {
                _service.SeatClicked(screening, rowindex, columnindex);
            }

            return RedirectToAction("Edit", "Screenings", new { id = screening.MovieId, screeningId = screening.Id - firstScreening.Id, screeningTime = screening.ScreenTime });
        }

        public IActionResult PurchaseClicked(int id,string time)
        {
            System.Diagnostics.Debug.WriteLine("PurchaseId: " + id);
            System.Diagnostics.Debug.WriteLine("PurchaseTime: " + time);
            var Screening = _service.GetScreening(id, time);
            var firstScreening = _service.GetScreeningOnlybyId(id);

            if (Screening != null)
            {
                System.Diagnostics.Debug.WriteLine("itt is");
                //_service.PurchaseClicked(Screening);
            }

            return RedirectToAction("Edit", "Screenings", new { id = Screening.MovieId, screeningId = Screening.Id - firstScreening.Id, screeningTime = Screening.ScreenTime});
        }

        public IActionResult ScreeningDetails(int id, int screeningId)
        {
            System.Diagnostics.Debug.WriteLine("id: " + id);
            System.Diagnostics.Debug.WriteLine("screeningId: " + screeningId);
            var movie = _service.GetMovie(id);
            var screening = movie.Screenings[screeningId];
            return View(screening);
        }
    }
}
