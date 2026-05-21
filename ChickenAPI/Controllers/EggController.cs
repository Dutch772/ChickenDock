using Microsoft.AspNetCore.Mvc;

namespace ChickenAPI.Controllers
{
    [ApiController]
[Route("api/[controller]")]
public class EggController : Controller
{
    [HttpGet("GetMeEggs")]
    public ActionResult<string> GetMeEggs()
    {
        return Ok("Here are some eggs! :)");
    }
}
}