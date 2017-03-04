using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Mermaider.UI.Tests
{
    using System.IO;
    using IO;
    using Mermaider.UI;

    [TestClass]
    public class MermaidCallerTests
    {
        private const string SIMPLE_GRAPH = "graph TD \n\t A-->B";
        private string _rootPath = AppContext.BaseDirectory + "\\outDir";
        private string _tempFolder = Path.GetTempPath();

        [TestMethod]
        public void GetSvg()
        {
            var mc = new MermaidCaller(_rootPath, _tempFolder);

            var result = mc.GetSvg(SIMPLE_GRAPH);

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
            var mc = new MermaidCaller(_rootPath, _tempFolder);

            var result = mc.GetImage(SIMPLE_GRAPH);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Errors.Any() == false, result.Errors.ToSingle());
            Assert.IsNotNull(result.ImagePath);
            Assert.IsNull(result.SvgContent);
            Assert.IsTrue(result.ImagePath.Length > 10);
        }
    }
}
