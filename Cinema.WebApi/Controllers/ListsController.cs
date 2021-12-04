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
using Microsoft.AspNetCore.Identity;

namespace Cinema.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ListsController : ControllerBase
    {
        private readonly ICinemaService _service;
        private readonly UserManager<ApplicationUser> _userManager;

        public ListsController(ICinemaService service, UserManager<ApplicationUser> userManager)
        {
            _service = service;
            _userManager = userManager;
        }

        // GET: api/Lists
        [HttpGet]
        public ActionResult<IEnumerable<ListDto>> GetLists()
        {
            return _service.GetLists().Select(list => (ListDto)list).ToList();
        }
        
        // GET: api/Lists/5
        [HttpGet("{id}")]
        public ActionResult<ListDto> GetList(int id)
        {
            try
            {
                return (ListDto)_service.GetListByID(id);
            }
            catch (InvalidOperationException)
            {

                return NotFound();
            }
        }

        // PUT: api/Lists/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [Authorize]
        [HttpPut("{id}")]
        public IActionResult PutList(int id, ListDto list)
        {
            if (id != list.Id)
            {
                return BadRequest();
            }

            if (_service.UpdateList((List)list))
            {
                return Ok();
            }
            else
                return StatusCode(StatusCodes.Status500InternalServerError);
            
        }

        // POST: api/Lists
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [Authorize]
        [HttpPost]//Lista létrehozás
        public ActionResult<ListDto> PostList(ListDto listDto)
        {
            var list = _service.CreateList((List)listDto);
            if (list == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            else
            {
                return CreatedAtAction("GetList", new { id = list.Id }, (ListDto)list);
            }
            
        }

        // DELETE: api/Lists/5
        [Authorize]
        [HttpDelete("{id}")]
        public ActionResult DeleteList(int id)
        {
            if (_service.DeleteList(id))
            {
                return Ok();
            }
            else
                return StatusCode(StatusCodes.Status500InternalServerError);

        }


    }
}
