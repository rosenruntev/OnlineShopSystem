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
    public class ProductsController : ControllerBase
    {
        private readonly ProductService productService;

        public ProductsController()
        {
            productService = new ProductService();
        }

        // GET: api/Products
        [HttpGet]
        public IEnumerable<ProductDto> GetAll()
        {
            return productService.GetAll();
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public ActionResult<ProductDto> Get([FromRoute] int id)
        {
            var result = productService.GetById(id);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        // GET: api/Products/search?name
        [HttpGet("search")]
        public ActionResult<ProductDto> GetAllByName([FromQuery(Name = "name")] string name)
        {
            var result = productService.GetAllByName(name);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        // POST: api/Products
        [HttpPost]
        public IActionResult Create([FromBody] ProductDto productDto)
        {
            if (!productDto.IsValid())
            {
                return BadRequest();
            }

            if (productService.Create(productDto))
            {
                return NoContent();
            }

            return BadRequest();
        }

        // PUT: api/Products/5
        [HttpPut("{id}")]
        public IActionResult Update([FromRoute] int id, [FromBody] ProductDto productDto)
        {
            if (!productDto.IsValid())
            {
                return BadRequest();
            }

            productDto.Id = id;

            if (productService.Update(productDto))
            {
                return NoContent();
            }

            return BadRequest();
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (productService.Delete(id))
            {
                return NoContent();
            }

            return BadRequest();
        }
    }
}
