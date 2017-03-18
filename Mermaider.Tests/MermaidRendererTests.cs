namespace Mermaider.Tests
{
    using System;
    using System.IO;
    using System.Linq;
    using Core;
    using Core.Utils;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class MermaidRendererTests
    {
        private const string SIMPLE_GRAPH = "graph TD \n\t A-->B";
        private readonly string _rootPath = AppContext.BaseDirectory + "\\outDir";
        private readonly string _tempFolder = Path.GetTempPath();

        [TestMethod]
        public void GetSvg()
        {
            var mc = new Renderer(_rootPath);

            var result = mc.RenderAsSvg("myFile", SIMPLE_GRAPH);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Errors.Any() == false, result.Errors.ToSingle());
            Assert.IsNull(result.LocalFileSystemImagePath);
            Assert.IsNotNull(result.SvgContent);
            Assert.IsTrue(result.SvgContent.Length > 10);
        }

        [TestMethod]
        public void GetImage()
        {
            var mc = new Renderer(_rootPath);

            var result = mc.RenderAsImage("myFile",SIMPLE_GRAPH);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Errors.Any() == false, result.Errors.ToSingle());
            Assert.IsNotNull(result.LocalFileSystemImagePath);
            Assert.IsNull(result.SvgContent);
            Assert.IsTrue(result.LocalFileSystemImagePath.Length > 10);
        }
    }
}
