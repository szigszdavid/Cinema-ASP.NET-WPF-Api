using Cinema.Persistence;
using Cinema.Persistence.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Tasks.Deployment.Bootstrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cinema.Web.Controllers
{
    public class ScreeningsController : Controller
    {
        private readonly ICinemaService _service;
        public ScreeningsController(ICinemaService service)
        {
            _service = service;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Edit(int? id, int screeningId, string screeningTime)
        {
            if (id == null)
            {

                return NotFound();
            }

            var movie = _service.GetMovie((int)id); //Próbáljuk lekérni a listet
            var screening = movie.Screenings[screeningId];
            //var screening = _service.GetScreening(screeningId, screeningTime);

            System.Diagnostics.Debug.WriteLine("HTTP előtt: " + screening.Id);
            if (screening == null) //Ha nem találjuk a listet, akkor ez
            {
                return NotFound();
            }

            System.Diagnostics.Debug.WriteLine("Editben");
            //ViewBag.Lists = new SelectList(_service.GetLists(), "Id", "Name", movie.ListId);//ViewBag a controllerek és nézetek között szállítanAK adatokat
            return View(screening);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Screening _screening)
        {
            //Lehet, hogy a két id nem egyezik meg
            if (id != _screening.Id)
            {
                return NotFound();
            }

            var movie = _service.GetMovie(id);
            System.Diagnostics.Debug.WriteLine("ScreeningIDDD: " + _screening.Id);
            var formerScreening = _service.GetScreeningBYIdAndTime(_screening.Id);
            //var formerScreening = _service.GetScreening(_screening.MovieId, _screening.ScreenTime);
            var screening = _screening;

            System.Diagnostics.Debug.WriteLine("Időpont: " + formerScreening.ScreenTime);
            System.Diagnostics.Debug.WriteLine("Former Id: " + formerScreening.Id);
            System.Diagnostics.Debug.WriteLine("Name: " + screening.Name);
            System.Diagnostics.Debug.WriteLine("PhoneNumber: " + screening.PhoneNumber);
            

            if (ModelState.IsValid)
            {
                //_service.UpdateMovie(movie);
                
                _service.PurchaseClicked(formerScreening, screening.Name, screening.PhoneNumber);
                return View(formerScreening);
                //return RedirectToAction("Details", "Lists", new { id = movie.ListId }); //Visszatérünk az elemek felsorolásához
                //Details nevű action, a ListsControlerben, átadjuk a movie ListIdját mert kell a Details actionnek egy id paraméter
            }


            //ViewBag.Lists = new SelectList(_service.GetLists(), "Id", "Name", movie.ListId);//ViewBag a controllerek és nézetek között szállítanAK adatokat
            //SelectList paraméterek: 1.értékek 2.melyik mező alapján sorol fel 3.Amit látni fogunk 4. Alapértelmezetten mi lesz kiválasztva(objektum)
            return View(screening);
        }
    }
}
