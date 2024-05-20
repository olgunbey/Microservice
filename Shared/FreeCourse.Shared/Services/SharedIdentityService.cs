using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeCourse.Shared.Services
{
    public class SharedIdentityService : ISharedIdentityService
    {
        private readonly IHttpContextAccessor _contextAccessor;
        public SharedIdentityService(IHttpContextAccessor httpContextAccessor)
        {
            _contextAccessor = httpContextAccessor;
            
        }
        public string GetUserId => _contextAccessor.HttpContext.User.FindFirst("sub").Value;
    }
}
