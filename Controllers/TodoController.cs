using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApp.Data;
using TodoApp.Models;

namespace TodoApp.Controllers
{
    [Route("api/[controller]")] // We define the routing that our controller going to use
    [ApiController] // We need to specify the type of the controller to let .Net core know
    public class TodoController : ControllerBase
    {
        private readonly ApiDbContext _context;
        public TodoController(ApiDbContext context)
        {
            _context = context;

        }

        [HttpGet]
        public ActionResult GetItems()
        {
            var items = _context.Items.ToList();
            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetItem(int id)
        {
            var item = await _context.Items.FirstOrDefaultAsync(item => item.Id == id);

            if (item == null)
            {
                return NotFound();
            }

            return Ok(item);
        }

        [HttpPost]
        public async Task<ActionResult> CreateItem(ItemData data)
        {
            if (ModelState.IsValid)
            {
                await _context.Items.AddAsync(data);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetItem", new { data.Id }, data);
            }

            return new JsonResult("Something was wrong")
            {
                StatusCode = 500
            };
        }

        [HttpPut]
        public async Task<ActionResult> UpdateItem(int id, ItemData item)
        {
            //Id sai
            if (id != item.Id)
            {
                return BadRequest();
            }

            var existItem = await _context.Items.FirstOrDefaultAsync(item => item.Id == id);

            if (existItem == null)
            {
                return NotFound();
            }

            //Truyen vao item ==> Lay data tu CSDL = item
            existItem.Id = id;
            existItem.Details = item.Details;
            existItem.Title = item.Title;
            existItem.Done = item.Done;

            await _context.SaveChangesAsync();
            return Ok(item);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteItem(int id)
        {
            var existItem = await _context.Items.FirstOrDefaultAsync(item => item.Id == id);

            if (existItem == null)
            {
                return NotFound();
            }

            _context.Items.Remove(existItem);
            await _context.SaveChangesAsync();

            return Ok(existItem);
        }
    }
}