namespace Mermaider.Core
{
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Abstractions;
    using IO;
    using Utils;

    public class MermaidRenderer : IMermaidRenderer
    {
        #region "fields and consts"

        private FileUtils _fileUtils;
        private readonly string _graphFileDirectory;
        private readonly string _outputDirectory;
        private readonly string nodeCommandPath = "node"; //assuming its in the system path
        private readonly string mermaidPath = @"C:\Users\Andrew\AppData\Roaming\npm\node_modules\mermaid\bin\mermaid.js";

        public const string EXTENSION_SVG = ".svg";
        public const string EXTENSION_PNG = ".png";

        #endregion //#region "fields and consts"

        #region  "initialization"

        /// <summary>
        /// 
        /// </summary>
        /// <param name="outputDirectory"></param>
        /// <param name="tempDir"></param>
        public MermaidRenderer(string outputDirectory, string tempDir)
        {
            _outputDirectory = Path.GetFullPath(outputDirectory);
            _graphFileDirectory = Path.GetFullPath(tempDir);
            if (Directory.Exists(_outputDirectory) == false)
            {
                throw new DirectoryNotFoundException($"no {_outputDirectory} found");
            }
            if (Directory.Exists(_graphFileDirectory) == false)
            {
                throw new DirectoryNotFoundException($"no {_graphFileDirectory} found");
            }
        }

        #endregion //#region  "initialization"

        #region "public members"

        public MermaidRenderResult RenderAsSvg(string inputText)
        {
            var graphFileName = WriteGraphFile(inputText);
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


        public MermaidRenderResult RenderAsImage(string inputText)
        {
            var graphFilePath = WriteGraphFile(inputText);
            var args = $"\"{mermaidPath}\" -o \"{_outputDirectory}\" --png \"{graphFilePath}\"";
            var graphFileName = new FileInfo(graphFilePath).Name;
            var expectedFilePath = Path.Combine(_outputDirectory, $"{graphFileName}{EXTENSION_PNG}");

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

        private MermaidRenderResult RunCommand(string args, string expectedFilePath)
        {
            string stdOut;
            string stdErr;

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

            var result = new MermaidRenderResult();

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
                _fileUtils = new FileUtils(_outputDirectory, _graphFileDirectory);
            }
            return _fileUtils;
        }

        private string WriteGraphFile(string inputText)
        {
            var targetFilePath = GetFileUtils().GetTempFile("graph");
            File.WriteAllText(targetFilePath, inputText);
            return targetFilePath;
        }

        #endregion //#region "helpers"
    }
}