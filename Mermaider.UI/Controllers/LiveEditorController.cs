namespace Mermaider.UI.Controllers
{
    using System;
    using System.Security.Claims;
    using Core;
    using Core.Abstractions;
    using Core.IO;
    using Core.Models;
    using Core.Utils;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    public class LiveEditorController : Controller
    {
        protected IFileUtils _FileUtils;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IManager _manager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LiveEditorController(IHostingEnvironment environment, IManager manager,
            IHttpContextAccessor httpContextAccessor)
        {
            _manager = manager;
            _hostingEnvironment = environment;
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        //[ValidateAntiForgeryToken]
        public ActionResult RenderAsPng(string graphText)
        {
            var userIdent = "not sure";
            var nameClaim = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            if (nameClaim != null)
            {
                userIdent = nameClaim.Value;
            }

            var graphRequest = new GraphRequest
            {
                OutputType = MermaidOutput.Png,
                GraphText = graphText,
                UserIdent = userIdent
            };

            var inProgressGraph = _manager.RenderGraphForUser(graphRequest);

            var renderResult = inProgressGraph.RenderResult;
            //need to Url-ize the path, and use the library
            if (renderResult.IsSuccessful)
            {
                renderResult.LocalUrlImagePath = Urlizer(renderResult.LocalFileSystemImagePath);
            }
            return new JsonResult(renderResult);
        }
        

        private string Urlizer(string filePath)
        {
            var pathWithoutRoot = filePath.Replace(_hostingEnvironment.WebRootPath, string.Empty);
            return $"{pathWithoutRoot.Replace("\\", "/").Remove(0,1)}";
        }
        
    }
}