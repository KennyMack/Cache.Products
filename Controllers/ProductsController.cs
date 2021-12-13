using Cache.Products.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cache.Products.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAll([FromServices] IProductsDto _ProductsDto)
        {
            return Ok(_ProductsDto.GetAll());
        }

        [Route("Id/{pId}")]
        [HttpGet]
        public IActionResult GetById([FromServices] IProductsDto _ProductsDto, Guid pId)
        {
            return Ok(_ProductsDto.GetById(pId));
        }

        [HttpPost]
        public IActionResult Post([FromServices] IProductsDto _ProductsDto, [FromBody]Models.Products pProduto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(_ProductsDto.New(new Models.Products
            {
                Id = Guid.NewGuid(),
                Name = pProduto.Name,
                Price = pProduto.Price
            }));
        }

        [HttpPut]
        public IActionResult Put([FromServices] IProductsDto _ProductsDto, [FromBody] Models.Products pProduto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(_ProductsDto.Update(pProduto));
        }

        [Route("Id/{pId}")]
        [HttpDelete]
        public IActionResult Delete([FromServices] IProductsDto _ProductsDto, Guid pId)
        {
            return Ok(_ProductsDto.RemoveById(pId));
        }
    }
}
