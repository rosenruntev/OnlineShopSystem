using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OSS.Business.DTOs;
using OSS.Business.Services;
using System.Collections.Generic;

namespace OSS.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly OrderService orderService;

        public OrdersController()
        {
            orderService = new OrderService();
        }

        // GET: api/Orders
        [HttpGet]
        public IEnumerable<OrderDto> GetAll()
        {
            return orderService.GetAll();
        }

        // GET: api/Orders/5
        [HttpGet("{id}")]
        public ActionResult<OrderDto> Get([FromRoute] int id)
        {
            var result = orderService.GetById(id);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        // GET: api/Orders/search
        [HttpGet("search")]
        public ActionResult<OrderDto> GetAllByUser([FromBody] UserDto userDto)
        {
            var result = orderService.GetAllByUser(userDto);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        // POST: api/Orders
        [HttpPost]
        public IActionResult Create([FromBody] OrderDto orderDto)
        {
            if (!orderDto.IsValid())
            {
                return BadRequest();
            }

            if (orderService.Create(orderDto))
            {
                return NoContent();
            }

            return BadRequest();
        }

        // PUT: api/Orders/5
        [HttpPut("{id}")]
        public IActionResult Update([FromRoute] int id, [FromBody] OrderDto orderDto)
        {
            if (!orderDto.IsValid())
            {
                return BadRequest();
            }

            orderDto.Id = id;

            if (orderService.Update(orderDto))
            {
                return NoContent();
            }

            return BadRequest();
        }

        // DELETE: api/Orders/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (orderService.Delete(id))
            {
                return NoContent();
            }

            return BadRequest();
        }
    }
}
