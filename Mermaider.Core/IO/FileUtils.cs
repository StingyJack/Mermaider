using System.IO;

namespace Mermaider.UI.IO
{
    using System.Collections.Generic;

    /// <summary>
    ///     General file utilisies
    /// </summary>
    public class FileUtils
    {
        private string _tempPath;

        public FileUtils(string tempPath)
        {
            _tempPath = tempPath;
        }

        public void CreateDir(params string[] directories)
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
            var targetFilePath = Path.Combine(_tempPath, testFilePath);
            return targetFilePath;
        }

        public string GetFileContent(string expectedFilePath)
        {
            return File.ReadAllText(expectedFilePath);
        }

        

        public void ClearFolderContents(string dierctory)
        {
            
        }

    }
}
