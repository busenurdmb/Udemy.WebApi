using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Udemy.WebApi.Data;
using Udemy.WebApi.Interfaces;

namespace Udemy.WebApi.Controllers
{
    [EnableCors]
    [ApiController]
    [Route("api/[Controller]")]
    public class ProductsController : ControllerBase
    {
        //api/products ~ GET
        //api/products/id ~GET/DELETE
        //api/products ~ POST/PUT


        private readonly IProductRepository _productRepository;

        public ProductsController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
       [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _productRepository.GetAllAsync();
            return Ok(result);
        }
        [HttpGet("{id}")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> GetById(int id)
        {
            var data = await _productRepository.GetByIdAsync(id);
            if (data == null)
            {
                return NotFound(id);
            }
            return Ok(data);
        }
        //api/products/id?1&name=telefon fromgueryde
     //   Method BİR NESNE ALIYORSA SİZ önüne herhangi bir şey koymadıysanız frombody dir
        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            var addedProduct = await _productRepository.CreateAsync(product);
            return Created(string.Empty, addedProduct);
        }
        [HttpPut]
        public async Task<IActionResult> Update(Product product)
        {
            var checkProduct = await _productRepository.GetByIdAsync(product.Id);
            if (checkProduct == null)
            {
                return NotFound(product.Id);
            }
            await _productRepository.UpdateAsync(product);
            return NoContent();

        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            var checkRemove = await _productRepository.GetByIdAsync(id);
            if (checkRemove == null)
            {
                return NotFound(id);
            }
            await _productRepository.RemoveAsync(id);
            return NoContent();
        }
        //api/products/upload
        [HttpPost("upload")]
        public async Task<IActionResult> Upload([FromForm]IFormFile formFile)
        {
            var newName = Guid.NewGuid() + "." + Path.GetExtension(formFile.FileName);
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", newName);
            var stream = new FileStream(path, FileMode.Create);
            await formFile.CopyToAsync(stream);
            return Created(string.Empty, formFile);
        }
        [HttpGet("[action]")]
        public IActionResult Test(/*[FromForm] string name,[FromHeader] string auth,*/[FromServices] IDummyRepository _dummyRepository)
        {
            //request=> response
            //header,body
            //var authetication = HttpContext.Request.Headers[auth];

            //var value=HttpContext.Request.Form["name"];
            return Ok(_dummyRepository.Getname());
        }
    }
}
