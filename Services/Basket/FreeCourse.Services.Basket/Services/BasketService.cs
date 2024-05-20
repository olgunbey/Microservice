using FreeCourse.Services.Basket.Dtos;
using FreeCourse.Shared.Dtos;
using System.Text.Json;

namespace FreeCourse.Services.Basket.Services
{
    public class BasketService : IBasketService
    {
        private readonly RedisService _basketService;
        public BasketService(RedisService redis)
        {
            _basketService = redis;
        }
        public async Task<ResponseDto<bool>> Delete(string userId)
        {
            var existBasket= await _basketService.GetDb().KeyDeleteAsync(userId);
            return existBasket ? ResponseDto<bool>.Success(204) : ResponseDto<bool>.Fail("basket not found", 404);
        }

        public async Task<ResponseDto<BasketDto>> GetBasket(string userId)
        {
            var existBasket = await _basketService.GetDb().StringGetAsync(userId);

            if(String.IsNullOrEmpty(existBasket))
            {
                return ResponseDto<BasketDto>.Fail("basket not found", 404);
            }

            return ResponseDto<BasketDto>.Success(JsonSerializer.Deserialize<BasketDto>(existBasket), 200);

        }

        public async Task<ResponseDto<bool>> SaveOrUpdate(BasketDto basketDto)
        {
            var status = await _basketService.GetDb().StringSetAsync(basketDto.UserId,JsonSerializer.Serialize(basketDto));

            return status ? ResponseDto<bool>.Success(204) : ResponseDto<bool>.Fail("basket not found update or save", 500);
        }
    }
}
