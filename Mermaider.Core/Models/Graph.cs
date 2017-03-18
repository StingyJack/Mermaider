namespace Mermaider.Core.Models
{
    public abstract class Graph
    {
        public string GraphId { get; set; }
        public string UserIdent { get; set; }
        public string GraphText { get; set; }
        public string LocalPathUrl { get; set; }

        internal string LocalPathFile { get; set; }
        internal bool IsPermanent { get; set; }
    }
}