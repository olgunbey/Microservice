using FreeCourse.Services.Basket.Dtos;
using FreeCourse.Shared.Dtos;
using Microsoft.AspNetCore.DataProtection.XmlEncryption;

namespace FreeCourse.Services.Basket.Services
{
    public interface IBasketService
    {
        Task<ResponseDto<BasketDto>> GetBasket(string userId);
        Task<ResponseDto<bool>> SaveOrUpdate(BasketDto basketDto);
        Task<ResponseDto<bool>> Delete(string userId);
    }
}
