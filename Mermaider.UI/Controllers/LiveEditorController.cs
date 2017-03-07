namespace Mermaider.UI.Controllers
{
    using Core;
    using Core.Abstractions;
    using Core.IO;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;

    public class LiveEditorController : Controller
    {
        protected IFileUtils _FileUtils;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly string _graphFilesDirectory;
        private readonly string _imageFilesDirectory;

        public LiveEditorController(IHostingEnvironment environment)
        {
            _hostingEnvironment = environment;
            _graphFilesDirectory = SetDirectory("graphFiles");
            _imageFilesDirectory = SetDirectory("images/graphs");
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        //[ValidateAntiForgeryToken]
        public ActionResult RenderAsPng(string graphText)
        {
            var mc = new MermaidRenderer(_imageFilesDirectory, _graphFilesDirectory);
            var renderResult = mc.RenderAsImage(graphText);
            //need to Url-ize the path, and use the library
            renderResult.ImagePath = Urlizer(renderResult.ImagePath);
            return new JsonResult(renderResult);
        }

        // figure out action result and what to put on the client side to deserialie


        private string Urlizer(string filePath)
        {
            var pathWithoutRoot = filePath.Replace(_hostingEnvironment.WebRootPath, string.Empty);
            return $"{pathWithoutRoot.Replace("\\", "/").Remove(0,1)}";
        }

        private string SetDirectory(string subfolderName)
        {
            var directory = GetFileUtils().PathCombine(_hostingEnvironment.WebRootPath, subfolderName);
            GetFileUtils().CreateDir(directory);
            return directory;
        }

        private IFileUtils GetFileUtils()
        {
            if (_FileUtils == null)
            {
                _FileUtils = new FileUtils();
            }
            return _FileUtils;
        }
    }
}