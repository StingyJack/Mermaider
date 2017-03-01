using System;

namespace Mermaider.UI
{
    using System.Diagnostics;
    using System.IO;
    using System.Linq;

    public class MermaidCaller
    {
        private string _workingDirectory;
        private string nodeCommandPath = "node";//assuming its in the system path
        private string mermaidPath = @"C:\Users\Andrew\AppData\Roaming\npm\node_modules\mermaid\bin\mermaid.js";
        public string OutputDir { get; set; }

        public const string EXTENSION_SVG = ".svg";
        public const string EXTENSION_PNG = ".png";


        //Css ?
        public MermaidCaller(string outDir)
        {
            OutputDir = outDir;
            _workingDirectory = Path.GetFullPath(outDir);
            Directory.CreateDirectory(_workingDirectory);

        }

        public MermaidResult GetSvg(string inputText)
        {
            var graphFileName = WriteGraphFile(inputText);
            var args = $"\"{mermaidPath}\" -v -o \"{OutputDir}\" --svg \"{graphFileName}\"";
            var expectedFilePath = $"{graphFileName}{EXTENSION_SVG}";

            var result = RunCommand(args, expectedFilePath);

            if (result.Errors.Any())
            {
                return result;
            }

            result.SvgContent = GetFileContent(expectedFilePath);

            return result;

        }

        private MermaidResult RunCommand(string args, string expectedFilePath)
        {
            string stdOut = null;
            string stdErr = null;

            var ps = new ProcessStartInfo(nodeCommandPath)
            {
                Arguments = args,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                WorkingDirectory = _workingDirectory,
                CreateNoWindow = true
            };

            using (var p = Process.Start(ps))
            {
                stdOut = p.StandardOutput.ReadToEnd();
                stdErr = p.StandardError.ReadToEnd();
                p.WaitForExit();
            }

            var result = new MermaidResult();

            if (string.IsNullOrWhiteSpace(stdOut))
            {
                result.Errors.Add($"No StdOut from the command '{ps.FileName} {ps.Arguments}'");
            }

            Trace.WriteLine($"stdOut: {stdOut}");

            if (string.IsNullOrWhiteSpace(stdErr) == false)
            {
                result.Errors.Add($"Errors: {stdErr}");
            }

            if (File.Exists(expectedFilePath) == false)
            {
                result.Errors.Add($"Expected file {expectedFilePath}, but it was not found");
            }

          

            return result;
        }

        public MermaidResult GetImage(string inputText)
        {
            var graphFileName = WriteGraphFile(inputText);
            var args = $"\"{mermaidPath}\" -o \"{OutputDir}\" --png \"{graphFileName}\"";
            var expectedFilePath = $"{graphFileName}{EXTENSION_PNG}";

            var result = RunCommand(args, expectedFilePath);

            if (result.Errors.Any())
            {
                return result;
            }

            result.ImagePath = expectedFilePath;

            return result;
        }

        private string GetFileContent(string expectedFilePath)
        {
            return File.ReadAllText(expectedFilePath);
        }

        private string WriteGraphFile(string inputText, string existingFile = null)
        {
            var testFilePath = existingFile ?? $"{Path.GetRandomFileName()}.graph";
            var fileInfo = new FileInfo(testFilePath);
            var targetFilePath = Path.Combine(_workingDirectory, fileInfo.Name);

            File.WriteAllText(targetFilePath, inputText);
            return targetFilePath;
        }

       
    }
}
