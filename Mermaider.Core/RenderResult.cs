namespace Mermaider.Core
{
    using System.Collections.Generic;
    using Utils;

    public class RenderResult
    {
        public bool IsSuccessful
        {
            get
            {
                if (Errors.Count == 0 
                    && SvgContent.OnlyOneEmpty(LocalFileSystemImagePath))
                {
                    return true;
                }
                return false;
            }
        }
        public string SvgContent { get; internal set; }
        public string LocalFileSystemImagePath { get; internal set; }
        public string LocalUrlImagePath { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
        public List<string> Diagnostics { get; set; } = new List<string>();

    }
}
