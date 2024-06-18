using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BaseController : ControllerBase
    {
        public Guid UserId
        {
            get
            {
                return Guid.Parse(User.Claims.FirstOrDefault(q => q.Type == "userId").Value);
            }
        }
    }
}
