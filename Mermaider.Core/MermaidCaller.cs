namespace Mermaider.UI
{
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Text;
    using IO;

    public class MermaidCaller
    {
        #region "fields and consts"

        private FileUtils _fileUtils;
        private readonly string _tempDirectory;
        private readonly string _outputDirectory;
        private readonly string nodeCommandPath = "node"; //assuming its in the system path
        private readonly string mermaidPath = @"C:\Users\Andrew\AppData\Roaming\npm\node_modules\mermaid\bin\mermaid.js";

        public const string EXTENSION_SVG = ".svg";
        public const string EXTENSION_PNG = ".png";

        #endregion //#region "fields and consts"

        #region  "initialization"

        public MermaidCaller(string outputDirectory, string tempDir)
        {
            _outputDirectory = Path.GetFullPath(outputDirectory);
            _tempDirectory = Path.GetFullPath(tempDir);
            GetFileUtils().CreateDir(_outputDirectory, _tempDirectory);
        }

        #endregion //#region  "initialization"

        #region "public members"

        public MermaidResult GetSvg(string inputText)
        {
            var graphFileName = WriteGraphTempFile(inputText);
            var args = BuildMermaidArgs(graphFileName, MermaidOutput.Svg, false);
            var expectedFilePath = $"{graphFileName}{EXTENSION_SVG}";

            var result = RunCommand(args, expectedFilePath);

            if (result.Errors.Any())
            {
                return result;
            }

            result.SvgContent = GetFileUtils().GetFileContent(expectedFilePath);
            
            return result;
        }


        public MermaidResult GetImage(string inputText)
        {
            var graphFileName = WriteGraphTempFile(inputText);
            var args = $"\"{mermaidPath}\" -o \"{_outputDirectory}\" --png \"{graphFileName}\"";
            var expectedFilePath = $"{graphFileName}{EXTENSION_PNG}";

            var result = RunCommand(args, expectedFilePath);

            if (result.Errors.Any())
            {
                return result;
            }

            result.ImagePath = expectedFilePath;

            return result;
        }

        #endregion //#region "public members"

        #region "command execution"

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
                WorkingDirectory = _outputDirectory,
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

            //Trace.WriteLine($"stdOut: {stdOut}");

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

        private string BuildMermaidArgs(string graphFileName, MermaidOutput mermaidOutput, bool verbose)
        {
            var sb = new StringBuilder($"\"{mermaidPath}\" ");
            if (verbose)
            {
                sb.Append(" -v ");
            }
            sb.Append($" -o \"{_outputDirectory}\" ");
            switch (mermaidOutput)
            {
                case MermaidOutput.Png:
                    //nothing to add, this is default
                    break;
                case MermaidOutput.Svg:
                    sb.Append(" --svg ");
                    break;
                case MermaidOutput.PngAndSvg:
                    sb.Append(" --svg --png ");
                    break;
            }

            sb.Append($" \"{graphFileName}\" ");
            return sb.ToString();
        }

        #endregion //#region "command execution"

        #region "helpers"

        protected virtual FileUtils GetFileUtils()
        {
            if (_fileUtils == null)
            {
                _fileUtils = new FileUtils(_outputDirectory);
            }
            return _fileUtils;
        }

        private string WriteGraphTempFile(string inputText)
        {
            var targetFilePath = GetFileUtils().GetTempFile("graph");
            File.WriteAllText(targetFilePath, inputText);
            return targetFilePath;
        }

        #endregion //#region "helpers"
    }
}