namespace Mermaider.UI.Controllers
{
    using Microsoft.AspNetCore.Mvc;


    public class LiveEditorController : Controller
    {
        [Route("home/index")]
        public IActionResult Index()
        {
            return Ok("Dis am controller");
        }

        

    }
}
