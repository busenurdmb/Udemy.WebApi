using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Udemy.WebApi.Data;

namespace Udemy.WebApi.Controllers
{
    [EnableCors]
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController:ControllerBase
    {
        private readonly ProductContext _Context;

        public CategoriesController(ProductContext context)
        {
            _Context = context;
        }
        [HttpGet("{id}/products")]
        public IActionResult GetWithProducts(int id)
        {
         var data=   _Context.Categories.Include(x => x.products).SingleOrDefault(x => x.Id == id);
            if (data == null)
            {
                return NotFound(id);
            }
            return Ok(data);
        }
    }
}
