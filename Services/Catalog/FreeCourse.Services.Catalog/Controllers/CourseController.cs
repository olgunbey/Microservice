using FreeCourse.Services.Catalog.Dtos;
using FreeCourse.Services.Catalog.Services;
using FreeCourse.Shared.ControllerBases;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FreeCourse.Services.Catalog.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CourseController : CustomBaseController
    {
        private readonly ICourseService courseService;
        public CourseController(ICourseService courseService)
        {
            this.courseService = courseService;
        }
        [HttpGet]
        public async Task<IActionResult> GetById([FromHeader]string Id)
        {
        var responseDto=  await  courseService.GetByIdAsync(Id);
            return CreateActionResultInstance(responseDto);
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await courseService.GetAllAsync();
            return CreateActionResultInstance(response);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllByUserId([FromHeader]string userId)
        {
            var response = await courseService.GetAllByUserIdAsync(userId);
            return CreateActionResultInstance(response);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CourseCreateDto courseCreateDto)
        {
            var response = await courseService.CreateAsync(courseCreateDto);
            return CreateActionResultInstance(response);
        }
        [HttpPost]
        public async Task<IActionResult> Update(CourseUpdateDto courseUpdateDto)
        {
            var response = await courseService.UpdateAsync(courseUpdateDto);
            return CreateActionResultInstance(response);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var response = await courseService.DeleteAsync(id);
            return CreateActionResultInstance(response);
        }
    }
}
