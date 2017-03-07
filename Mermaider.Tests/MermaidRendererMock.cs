using System;
using System.Collections.Generic;
using System.Text;

namespace Mermaider.Tests
{
    using Core;
    using Core.Abstractions;

    internal class MermaidRendererMock : MermaidRenderer
    {
        public MermaidRendererMock(IFileUtils fileUtils, string graphFilePath, string imageFilePath) : base(graphFilePath, imageFilePath)
        {
            _FileUtils = fileUtils;
        }
    }
}
