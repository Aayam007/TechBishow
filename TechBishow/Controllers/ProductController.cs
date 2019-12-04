using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechBishow.Data;
using TechBishow.Models;

namespace TechBishow.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _db;
        public ProductController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet("[action]")]
        public IActionResult GetProducts()
        {
            return Ok(_db.Products.ToList());
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> AddProduct([FromBody] ProductModel productModel)
        {
            var newproduct = new ProductModel
            {
                Name = productModel.Name,
                ImageUrl = productModel.ImageUrl,
                Description = productModel.Description,
                OutOfStock = productModel.OutOfStock,
                Price = productModel.Price
            };
            await _db.Products.AddAsync(newproduct);
            await _db.SaveChangesAsync();
            return Ok();
        }
        [HttpPut("[action]")]
        public async Task<IActionResult> UpdateProduct([FromRoute] int id, [FromBody] ProductModel product)
        {
            if (ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var findproduct = _db.Products.FirstOrDefault(p => p.ProductId == id );
            if (findproduct  == null)
            {
                return NotFound();
            }

            //if the Product product
            findproduct.Name = product.Name;
            findproduct.Description = product.Description;
            findproduct.ImageUrl = product.ImageUrl;
            findproduct.OutOfStock = product.OutOfStock;
            findproduct.Price = product.Price;
            _db.Entry(findproduct).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return Ok(new JsonResult("The product with id" + id + "is updated"));
        }
    }
}