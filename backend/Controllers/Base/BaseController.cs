using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace TaskManager.Controllers.Base
{
    public class BaseController : ControllerBase
    {
        protected int UserId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
    }
}
