namespace Mermaider.Core
{
    using System;

    public class ManagerConfig
    {
        private string _pathToMermaidJsFile;
        public string SavedGraphFilesPath { get; set; }
        public string UnsavedGraphFilesPath { get; set; }
        public string PathToNodeExe { get; set; }

        public string PathToMermaidJsFile
        {
            get => _pathToMermaidJsFile;
            set
            {
                var expandedPath = Environment.ExpandEnvironmentVariables(value);
                _pathToMermaidJsFile = expandedPath;
            }
        }
    }
}
