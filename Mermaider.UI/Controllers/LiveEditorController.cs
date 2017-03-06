namespace Mermaider.UI.Controllers
{
    using System;
    using System.IO;
    using Core;
    using Core.IO;
    using Microsoft.AspNetCore.Mvc;

    public class LiveEditorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        //[ValidateAntiForgeryToken]
        public ActionResult RenderAsPng(string graphText)
        {
            var graphFilesDirectory = Path.Combine(AppContext.BaseDirectory, "graphFiles");
            var outputDirectory = Path.Combine(AppContext.BaseDirectory, "renderedOutput");
            FileUtils.CreateDir(graphFilesDirectory, outputDirectory);
            var mc = new MermaidRenderer(outputDirectory, graphFilesDirectory);
            var renderResult = mc.RenderAsImage(graphText);
            //need to Url-ize the path, and use the library
            return new JsonResult(renderResult);
        }
        // figure out action result and what to put on the client side to deserialie
    }
}
