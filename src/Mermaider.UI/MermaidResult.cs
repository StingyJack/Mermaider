using System.Collections.Generic;

namespace Mermaider.UI
{
    public class MermaidResult
    {
        public string SvgContent { get; set; }
        public string ImagePath { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
        public List<string> Messages { get; set; } = new List<string>();
        
    }
}
