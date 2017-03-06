namespace Mermaider.Core
{
    using System.Collections.Generic;
    using Utils;

    public class MermaidRenderResult
    {
        public bool IsSuccessful
        {
            get
            {
                if (Errors.Count == 0 
                    && SvgContent.OnlyOneEmpty(ImagePath))
                {
                    return true;
                }
                return false;
            }
        }
        public string SvgContent { get; set; }
        public string ImagePath { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
        public List<string> Diagnostics { get; set; } = new List<string>();

    }
}
