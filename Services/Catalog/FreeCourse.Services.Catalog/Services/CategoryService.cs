using FreeCourse.Services.Catalog.Dtos;
using FreeCourse.Services.Catalog.Models;
using FreeCourse.Services.Catalog.Settings;
using FreeCourse.Shared.Dtos;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace FreeCourse.Services.Catalog.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IMongoCollection<Category> mongoCollection;
        public CategoryService(IDatabaseSetting databaseSetting)
        {
            var client = new MongoClient(databaseSetting.ConnectionString);

            var database=  client.GetDatabase(databaseSetting.DatabaseName);

            mongoCollection= database.GetCollection<Category>(databaseSetting.CategoryCollectionName);

        }

        public async Task<ResponseDto<List<CategoryDto>>> GetAllAsync()
        {
            
            var categories = await mongoCollection.FindAsync(Builders<Category>.Filter.Empty);

            return ResponseDto<List<CategoryDto>>.Success(categories.ToList().Select(y => new CategoryDto
            {
                Id=y.Id,
                Name=y.Name,
            }).ToList(), 200);
        }

        public async Task<ResponseDto<CategoryDto>> CreateAsync(CategoryDto category)
        {
            Category category1 = new Category()
            {
                Name = category.Name,
            };
           await mongoCollection.InsertOneAsync(category1);


            return ResponseDto<CategoryDto>.Success(category,200); 
        }

        public async Task<ResponseDto<CategoryDto>> GetByIdAsync(string id)
        {
          Category category= await (await mongoCollection.FindAsync(y => y.Id == id)).FirstOrDefaultAsync();

            if (category == null)
            {
                return ResponseDto<CategoryDto>.Fail("not category", 404);
            }

            return ResponseDto<CategoryDto>.Success(new CategoryDto() { Name=category.Name }, 200);
        }

    }
}
