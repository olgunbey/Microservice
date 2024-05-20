using FreeCourse.Services.Basket.Dtos;
using FreeCourse.Services.Basket.Services;
using FreeCourse.Shared.ControllerBases;
using FreeCourse.Shared.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FreeCourse.Services.Basket.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BasketsController : CustomBaseController
    {
        private readonly IBasketService basketService;
        private readonly ISharedIdentityService sharedIdentityService;

        public BasketsController(IBasketService basketService, ISharedIdentityService sharedIdentityService)
        {
            this.basketService = basketService;
            this.sharedIdentityService = sharedIdentityService;
        }
        [HttpGet]
        [Authorize("PolicyBasket")]
        public async Task<IActionResult> GetBasket()
        {
            var claims = User.Claims;
            return CreateActionResultInstance(await basketService.GetBasket(sharedIdentityService.GetUserId));
        }
        [HttpPost]
        public async Task<IActionResult> SaveOrUpdateBasket(BasketDto basketDto)
        {
            var response = await basketService.SaveOrUpdate(basketDto);
            return CreateActionResultInstance(response);
        }
        [HttpGet]
        public async Task<IActionResult> DeleteBasket()
        {
          return CreateActionResultInstance(await basketService.Delete(sharedIdentityService.GetUserId));
        }
    }
}
