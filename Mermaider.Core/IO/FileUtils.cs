namespace Mermaider.Core.IO
{
    using System.IO;

    /// <summary>
    ///     General file utilities
    /// </summary>
    /// <remarks>These are probably not necessary with .net core</remarks>
    public class FileUtils
    {
        private readonly string _renderedOutputDirectory;
        private readonly string _graphFileDirectory;

        public FileUtils(string renderedOutputDirectory, string graphFileDirectory)
        {
            _renderedOutputDirectory = renderedOutputDirectory;
            _graphFileDirectory = graphFileDirectory;
        }

        public static void CreateDir(params string[] directories)
        {
            foreach (var dir in directories)
            {
                Directory.CreateDirectory(dir);
            }
        }

        public static void ClearDir(params string[] directories)
        {
            foreach (var dir in directories)
            {
                foreach (var file in Directory.EnumerateFileSystemEntries(dir))
                {
                    File.Delete(file);
                }
            }
        }

        public string GetTempFile(string extension = "tmp")
        {
            var testFilePath = $"{Path.GetRandomFileName()}.{extension}";
            var targetFilePath = Path.Combine(_graphFileDirectory, testFilePath);
            return targetFilePath;
        }

        public string GetFileContent(string expectedFilePath)
        {
            return File.ReadAllText(expectedFilePath);
        }
        

    }
}
