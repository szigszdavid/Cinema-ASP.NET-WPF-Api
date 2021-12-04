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

namespace Cinema.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly ICinemaService _service;

        public MoviesController(ICinemaService service)
        {
            _service = service;
        }

        // GET: api/Movies
        [HttpGet("List/{listId}")]
        public ActionResult<IEnumerable<MovieDto>> GetMovies(int listId)
        {
            try
            {
                return _service.GetListByID(listId).Movies.Select(movie => (MovieDto)movie).ToList();
            }
            catch (Exception)
            {

                return NotFound();
            }
        }
        
        // GET: api/Movies/5
        [HttpGet("{id}")]
        public ActionResult<MovieDto> GetMovie(int id)
        {
            try
            {
                return (MovieDto)_service.GetMovie(id);
            }
            catch (InvalidOperationException)
            {
                return NotFound();
            }
        }

        // PUT: api/Movies/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [Authorize]
        [HttpPut("{id}")]
        public IActionResult PutMovie(int id, MovieDto item)
        {
            if (id != item.Id)
            {
                return BadRequest();
            }

            if (_service.UpdateMovie((Movie)item))
            {
                return Ok();
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // POST: api/Movies
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [Authorize]
        [HttpPost]
        public ActionResult<MovieDto> PostMovie(MovieDto movieDto)
        {
            System.Diagnostics.Debug.WriteLine("PostMovie");
            var movie = _service.CreateMovie((Movie)movieDto);
            if (movie is null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            else
            {
                return CreatedAtAction(nameof(GetMovie), new { id = movie.Id }, (MovieDto)movie);
            }
            
        }

        // DELETE: api/Movies/5
        [Authorize]
        [HttpDelete("{id}")]
        public ActionResult DeleteMovie(int id)
        {
            if (_service.DeleteMovie(id))
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
