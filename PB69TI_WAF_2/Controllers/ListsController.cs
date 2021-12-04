using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Cinema.Persistence;
using Cinema.Persistence.Services;
using Microsoft.AspNetCore.Authorization;

namespace ELTE.Cinema.Web.Controllers
{
    public enum SortOrder { RELEASE_DESC, RELEASE_ASC}
    
    [Authorize]
    public class ListsController : Controller
    {
        private readonly ICinemaService _service;

        public ListsController(ICinemaService service)
        {
            _service = service;
        }


        // GET: Lists
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View(_service.GetLists());
        }

        // GET: Lists/Details/5
        [AllowAnonymous]
        public IActionResult Details(int id, SortOrder sortOrder = SortOrder.RELEASE_DESC)
        {
            try
            {
                //ViewData["TitleSortParam"] = sortOrder == SortOrder.RELEASE_DESC ? SortOrder.RELEASE_ASC : SortOrder.RELEASE_DESC;

                ViewData["ReleaseSortParam"] = sortOrder == SortOrder.RELEASE_DESC ? SortOrder.RELEASE_ASC : SortOrder.RELEASE_DESC;

                var list = _service.GetListDetails(id);

                switch (sortOrder)
                {
                    case SortOrder.RELEASE_DESC:
                        list.Movies = list.Movies.OrderByDescending(i => i.ReleaseDate).ToList(); //ThenBy-al lehetne második sorbállítási oszlopot megadni
                        break;
                    case SortOrder.RELEASE_ASC:
                        list.Movies = list.Movies.OrderBy(i => i.ReleaseDate).ToList();
                        break;
                    default:
                        break;
                }

                //list.Movies = list.Movies.Take(5).ToList();

                return View(list);
            }
            catch
            {

                return NotFound();
            }
        }

        //Lista módosításokhoz

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(List list) //Ez már egy kész listát kap paraméterül //Kell neki egy view-> jobbklikk create view-> TEmplate.Create, Model:Lists
        {
            //MEg kell nézni, hogy  a kapott adatok valósak-e

            if(ModelState.IsValid) //Egy HTTP kéréssből kapott adatok validak-e
            {
                
                if(ModelState.IsValid)
                {
                    var result = _service.CreateList(list);
                    if (result == null)
                        return NotFound();
                    return RedirectToAction(nameof(Index)); //Viiszamegyünk a kezdőoldalra
                }
                else
                {
                    return NotFound();
                }
            }

            else
            {
                return View(list); //Ha nem validak az értékek, akkor térjünk vissza oda , ahol bevittük őket
            }
        }


        public IActionResult Edit(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            var list = _service.GetListByID((int)id); //Próbáljuk lekérni a listet

            if(list == null) //Ha nem találjuk a listet, akkor ez
            {
                return NotFound();
            }

            return View(list);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Edit(int id, List list) //Kell ehhez is egy nézet //A List/Index.cshtml-ben kell egy új link neki a Details alá
        {
            if (id != list.Id) //Ha a fentebbi metódusban hazsnált Id és az itt behúzott id nem ugyanaz, akkr NotFound()
            {
                return NotFound();
            }

            if (ModelState.IsValid) //Egy HTTP kéréssből kapott adatok validak-e
            {
                bool result = _service.UpdateList(list);
                if (result)
                {
                    return RedirectToAction(nameof(Index)); //Viiszamegyünk a kezdőoldalra
                }
                else
                {
                    ModelState.AddModelError("", "Hiba a mentés során"); 
                }
            }

        
            return View(list); //Ha nem validak az értékek, akkor térjünk vissza oda , ahol bevittük őket
            
        }

        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var list = _service.GetListByID((int)id); //Próbáljuk lekérni a listet

            if (list == null) //Ha nem találjuk a listet, akkor ez
            {
                return NotFound();
            }

            return View(list);
        }

        [HttpPost, ActionName("Delete")] //Ez a Delete Viewja miatt kell
        [ValidateAntiForgeryToken]

        public IActionResult DeleteConfimred(int id) //A fenti miatt Delete lesz a neve
        {
            _service.DeleteList(id);
            return RedirectToAction(nameof(Index)); //Visszaugrunk az index oldalra minden esetben
        }

        public IActionResult CreateMovie(int id)// Kell a linkhez -> Details.htmlcs-ben
        {
            TempData["ListId"] = id;
            return RedirectToAction("Create","Movies"); //A Create action a MoviesControllerben
        }
    }
}
