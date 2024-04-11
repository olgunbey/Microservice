using FreeCourse.Shared.ControllerBases;
using FreeCourse.Shared.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FreeCourse.Services.Photo.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PhotoController : CustomBaseController
    {
        [HttpPost]
        public async Task<IActionResult> PhotoSave(IFormFile photo,CancellationToken cancellation)
        {
            if(photo != null&& photo.Length>0)
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot/photos",photo.FileName);

                var stream = new FileStream(path, FileMode.Create);
                await photo.CopyToAsync(stream,cancellation);

                var returnPath= "photos/"+photo.FileName;

                return CreateActionResultInstance(ResponseDto<object>.Success(new { returnPath = returnPath }, 200));
            }
            return CreateActionResultInstance(ResponseDto<NoContent>.Fail("photo is empty",400));
        
        }

        [HttpGet] 
        public IActionResult PhotoDelete(string photoPath)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwroot/photos",photoPath);

            if (System.IO.File.Exists(path))
            {

                return CreateActionResultInstance<NoContent>(ResponseDto<NoContent>.Fail("photo not found",400));
            }

            System.IO.File.Delete(path);

            return CreateActionResultInstance<NoContent>(ResponseDto<NoContent>.Success(204));
        }
    }
}
