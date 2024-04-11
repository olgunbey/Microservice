using FreeCourse.Services.Catalog.Dtos;
using FreeCourse.Services.Catalog.Models;
using FreeCourse.Services.Catalog.Services;
using FreeCourse.Shared.ControllerBases;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FreeCourse.Services.Catalog.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CategoriesController : CustomBaseController
    {
        private readonly ICategoryService categoryService;
        public CategoriesController(ICategoryService categoryService = null)
        {
            this.categoryService = categoryService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categories = await categoryService.GetAllAsync();
            return CreateActionResultInstance(categories);
        }
        [HttpGet]
        public async Task<IActionResult> GetById([FromHeader]string id)
        {
            var categories = await categoryService.GetByIdAsync(id);
            return CreateActionResultInstance(categories);
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody]CategoryDto category)
        {
            var categories= await categoryService.CreateAsync(category);

            return CreateActionResultInstance(categories);
        }
    }
}
