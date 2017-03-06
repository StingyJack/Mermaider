namespace Mermaider.Tests
{
    using System;
    using System.IO;
    using System.Linq;
    using Core;
    using Core.IO;
    using Core.Utils;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class MermaidCallerTests
    {
        private const string SIMPLE_GRAPH = "graph TD \n\t A-->B";
        private readonly string _rootPath = AppContext.BaseDirectory + "\\outDir";
        private readonly string _tempFolder = Path.GetTempPath();

        [TestMethod]
        public void GetSvg()
        {
            var mc = new MermaidRenderer(_rootPath, _tempFolder);

            var result = mc.RenderAsSvg(SIMPLE_GRAPH);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Errors.Any() == false, result.Errors.ToSingle());
            Assert.IsNull(result.ImagePath);
            Assert.IsNotNull(result.SvgContent);
            Assert.IsTrue(result.SvgContent.Length > 10);
            FileUtils.ClearDir(_rootPath); //meh
        }

        [TestMethod]
        public void GetImage()
        {
            var mc = new MermaidRenderer(_rootPath, _tempFolder);

            var result = mc.RenderAsImage(SIMPLE_GRAPH);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Errors.Any() == false, result.Errors.ToSingle());
            Assert.IsNotNull(result.ImagePath);
            Assert.IsNull(result.SvgContent);
            Assert.IsTrue(result.ImagePath.Length > 10);
        }
    }
}
