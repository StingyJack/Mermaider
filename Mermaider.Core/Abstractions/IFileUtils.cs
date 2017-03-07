namespace Mermaider.Core.Abstractions
{
    public interface IFileUtils
    {
        void CreateDir(params string[] directories);
        void ClearDir(params string[] directories);
        string GetTempFile(string targetDirectory, string extension = "tmp");
        string GetFileContent(string expectedFilePath);
        void WriteAllText(string filePath, string content);
        string PathCombine(string path1, string path2);
    }
}