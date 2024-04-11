using FreeCourse.Services.Catalog.Dtos;
using FreeCourse.Services.Catalog.Models;
using FreeCourse.Services.Catalog.Settings;
using FreeCourse.Shared.Dtos;
using MongoDB.Driver;

namespace FreeCourse.Services.Catalog.Services
{
    public class CourseService:ICourseService
    {
        private readonly IMongoCollection<Course> mongoCollection;
        private readonly IMongoCollection<Category> mongoCollectioncategory;
        public CourseService(IDatabaseSetting databaseSetting)
        {
            var client = new MongoClient(databaseSetting.ConnectionString);

            var database = client.GetDatabase(databaseSetting.DatabaseName);

            mongoCollection= database.GetCollection<Course>(databaseSetting.CourseCollectionName);

            mongoCollectioncategory=database.GetCollection<Category>(databaseSetting.CategoryCollectionName);
        }

        public async Task<ResponseDto<List<CourseDto>>> GetAllAsync()
        {
         var Courses = await mongoCollection.Find(y => true).ToListAsync();

            if (Courses.Any())
            {
                foreach (var course in Courses)
                {
                    course.Category= await mongoCollectioncategory.Find(x=> x.Id==course.CategoryId).FirstAsync();
                }
            }
            else
            {
                Courses=new List<Course>();
            }

            return ResponseDto<List<CourseDto>>.Success(Courses.Select(y => new CourseDto()
            {
                Id=y.Id,
                CreateTime=y.CreateTime,
                Description=y.Description,
                Name=y.Name,
                CategoryId=y.CategoryId,
                Price=y.Price,
                UserId=y.UserId,
                Category = new CategoryDto() { Name=y.Category.Name},
                Feature = new FeatureDto() { Duration=y.Feature.Duration },
                Picture = y.Picture
            }).ToList(), 200);
        }


        public async Task<ResponseDto<CourseDto>> GetByIdAsync(string id)
        {
            var course=  await mongoCollection.Find(y => y.Id == id).FirstOrDefaultAsync();

            if (course == null)
            {
                return ResponseDto<CourseDto>.Fail("not course data",404);

            }
            course.Category=await mongoCollectioncategory.Find(y=>y.Id==course.CategoryId).FirstAsync();

            return ResponseDto<CourseDto>.Success(new CourseDto() //odada doldur
            {
                Id = course.Id,
                CreateTime = course.CreateTime,

            }, 200);



        }


        public async Task<ResponseDto<List<CourseDto>>> GetAllByUserIdAsync(string userId)
        {
            var course= await mongoCollection.Find<Course>(y=>y.Id == userId).ToListAsync();
            if(course.Any())
            {
                foreach (var item in course)
                {
                    item.Category =await (await mongoCollectioncategory.FindAsync<Category>(x => x.Id == item.CategoryId)).FirstAsync();
                }
            }
            else
            {
                course = new List<Course>();
            }
            return ResponseDto<List<CourseDto>>.Success(course.Select(y => new CourseDto()
            {
                Id=y.Id, CreateTime = y.CreateTime,Description=y.Description,
                Feature=new FeatureDto() { Duration=y.Feature.Duration },
                Name=y.Name,
                Price=y.Price,
                Picture=y.Picture,
                UserId=y.UserId,
                CategoryId=y.CategoryId,

            }).ToList(),200);
        }

        public async Task<ResponseDto<NoContent>> UpdateAsync(CourseUpdateDto courseUpdateDto)
        {
            var updateDtos = new Course()
            {
                Id = courseUpdateDto.Id,
                Description = courseUpdateDto.Description,
                Feature=new Feature()
                {
                    Duration=courseUpdateDto.Feature.Duration
                },
                Name=courseUpdateDto.Name,
                Price=courseUpdateDto.Price,
                Picture=courseUpdateDto.Picture,
                UserId=courseUpdateDto.UserId,
                CategoryId=courseUpdateDto.CategoryId,
            };

          var result=  await mongoCollection.FindOneAndReplaceAsync(y => y.Id == courseUpdateDto.Id, updateDtos);

            if(result == null)
            {
                return ResponseDto<NoContent>.Fail("Course not found", 404);
            }
            return ResponseDto<NoContent>.Success(204);
        }

        public async Task<ResponseDto<NoContent>> DeleteAsync(string id)
        {
            var result =await mongoCollection.DeleteOneAsync(y => y.Id == id);

            if (result.DeletedCount > 0)
            {
                return ResponseDto<NoContent>.Success(204);
            }
            return ResponseDto<NoContent>.Fail("Course not found", 404);
        }

        public async Task<ResponseDto<NoContent>> CreateAsync(CourseCreateDto courseCreateDto)
        {
            var course = new Course()
            {
                CategoryId = courseCreateDto.CategoryId,
                Name = courseCreateDto.Name,
                Price = courseCreateDto.Price,
                Picture = courseCreateDto.Picture,
                Description = courseCreateDto.Description,
                UserId = courseCreateDto.UserId,
                Feature = new Feature() { Duration = courseCreateDto.Feature.Duration },
            };


          await  mongoCollection.InsertOneAsync(course);

            return ResponseDto<NoContent>.Success(200);
        }
    }
}
