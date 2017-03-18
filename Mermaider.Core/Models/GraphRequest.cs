namespace Mermaider.Core.Models
{
    using Utils;

    public class GraphRequest
    {
        public MermaidOutput OutputType { get; set; }

        public string GraphText { get; set; }
        public string UserIdent { get; set; }
        public string GraphIdent { get; internal set; }
    }
}