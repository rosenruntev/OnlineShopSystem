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
    public class UsersController : ControllerBase
    {
        private readonly UserService userService;

        public UsersController()
        {
            userService = new UserService();
        }

        // GET: api/Users
        [HttpGet]
        public IEnumerable<UserDto> GetAll()
        {
            return userService.GetAll();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public ActionResult<UserDto> Get([FromRoute] int id)
        {
            var result = userService.GetById(id);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        // GET: api/Users/search?name
        [HttpGet("search")]
        public ActionResult<UserDto> GetAllByName([FromQuery(Name = "name")] string name)
        {
            var result = userService.GetAllByName(name);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        // POST: api/Users
        [HttpPost]
        public IActionResult Create([FromBody] UserDto userDto)
        {
            if (!userDto.IsValid())
            {
                return BadRequest();
            }

            if (userService.Create(userDto))
            {
                return NoContent();
            }

            return BadRequest();
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        public IActionResult Update([FromRoute] int id, [FromBody] UserDto userDto)
        {
            if (!userDto.IsValid())
            {
                return BadRequest();
            }

            userDto.Id = id;

            if (userService.Update(userDto))
            {
                return NoContent();
            }

            return BadRequest();
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (userService.Delete(id))
            {
                return NoContent();
            }

            return BadRequest();
        }
    }
}
