using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Cinema.Persistence;
using Cinema.Persistence.Services;
using Cinema.Persistence.DTO;
using Microsoft.AspNetCore.Authorization;
using Cinema.Desktop.ViewModel;

namespace Cinema.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScreeningsController : ControllerBase
    {
            private readonly ICinemaService _service;

            public ScreeningsController(ICinemaService service)
            {
                _service = service;
            }

             
            
            // GET: api/Movies
            [HttpGet("List/{movieId}")]
            public ActionResult<IEnumerable<ScreeningDto>> GetScreenings(int movieId)
            {
                try
                {
                    return _service.GetScreeningsByMovieID(movieId).Select(screening => (ScreeningDto)screening).ToList();
                    //return _service.GetListByID(listId).Movies.Screenings.Select(movie => (ScreeningDto)movie).ToList();
                }
                catch (Exception)
                {

                    return NotFound();
                }
            }
            

            // GET: api/Movies/5
            [HttpGet("{id}")]
            public ActionResult<ScreeningDto> GetScreening(int id)
            {
                try
                {
                    return (ScreeningDto)_service.GetScreeningOnlybyId(id);
                }
                catch (InvalidOperationException)
                {
                    return NotFound();
                }
            }



        // POST : api/Screenings
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.

        /*
        [HttpPost]
        public ActionResult<ScreeningDto> PostScreening(ScreeningDto screeningDto)
        {
            System.Diagnostics.Debug.WriteLine("PostScreening");
            var screening = _service.CreateScreening((Screening)screeningDto);
            if (screening == null)
            {
                System.Diagnostics.Debug.WriteLine("Itt nuull");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            else
            {
                return CreatedAtAction(nameof(GetScreening), new { id = screening.Id }, (ScreeningDto)screening);
            }


        }*/

        // POST: api/Movies
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [Authorize]
        [HttpPost]
        public ActionResult<ScreeningDto> PostScreening(ScreeningDto screeningDto)
        {
            
            System.Diagnostics.Debug.WriteLine("PostMovie");
            var movie = _service.CreateScreening((Screening)screeningDto);
            if (movie == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            else
            {
                return CreatedAtAction(nameof(GetScreening), new { id = movie.Id }, (ScreeningDto)movie);
            }
            
            return StatusCode(StatusCodes.Status500InternalServerError);

        }

        [Authorize]
        [HttpPut("{id}")]
        public IActionResult PutSeat(int id, ScreeningDto screeningDto)
        {
            System.Diagnostics.Debug.WriteLine("Id: " + id);
            System.Diagnostics.Debug.WriteLine("AnotherId: " + screeningDto.Id);

            if (_service.UpdateSeat(id, screeningDto.Id))
            {
                return Ok();
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

        }
    }
}
