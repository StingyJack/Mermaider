namespace Mermaider.Core.IO
{
    using System.IO;
    using Abstractions;

    /// <summary>
    ///     General file utilities
    /// </summary>
    /// <remarks>These are probably not necessary with .net core</remarks>
    public class FileUtils : IFileUtils
    {
        public void CreateDir(params string[] directories)
        {
            foreach (var dir in directories)
            {
                Directory.CreateDirectory(dir);
            }
        }

        public void ClearDir(params string[] directories)
        {
            foreach (var dir in directories)
            {
                foreach (var file in Directory.EnumerateFileSystemEntries(dir))
                {
                    File.Delete(file);
                }
            }
        }

        public string GetTempFile(string targetDirectory, string extension = "tmp")
        {
            var testFilePath = $"{Path.GetRandomFileName()}.{extension}";
            var targetFilePath = Path.Combine(targetDirectory, testFilePath);
            return targetFilePath;
        }

        public string GetFileContent(string expectedFilePath)
        {
            return File.ReadAllText(expectedFilePath);
        }

        public void WriteAllText(string filePath, string content)
        {
            File.WriteAllText(filePath, content);
        }

        public string PathCombine(string path1, string path2)
        {
            return Path.Combine(path1, path2);
        }
    }
}