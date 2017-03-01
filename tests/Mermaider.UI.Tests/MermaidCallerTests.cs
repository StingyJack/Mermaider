using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Mermaider.UI.Tests
{
    [TestClass]
    public class MermaidCallerTests
    {
        private string _rootPath = AppContext.BaseDirectory + "\\outDir";

        [TestMethod]
        public void GetSvg()
        {
            var mc = new MermaidCaller(_rootPath);

            var result = mc.GetSvg("graph TD A-->B");

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Errors.Any() == false, string.Join(",\n", result.Errors));
        }

        [TestMethod]
        public void GetImage()
        {
            var mc = new MermaidCaller(_rootPath);

            var result = mc.GetImage("graph TD \n\t A-->B");

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Errors.Any() == false, string.Join(",\n", result.Errors));
        }
    }
}
