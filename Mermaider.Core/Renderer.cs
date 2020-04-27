namespace Mermaider.Core
{
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Abstractions;
    using IO;
    using Utils;

    public class Renderer : IRenderer
    {
        #region "fields and consts"

        protected IFileUtils _FileUtils;
        private readonly string _workingDirectory;
        private readonly string _pathToNodeExe; 
        private readonly string _pathToMermaidJs;

        public const string EXTENSION_SVG = ".svg";
        public const string EXTENSION_PNG = ".png";

        #endregion //#region "fields and consts"

        #region  "initialization"

        /// <summary>
        /// 
        /// </summary>
        /// <param name="workingDirectory"></param>
        /// <param name="pathToNodeExe"></param>
        /// <param name="pathToMermaidJsFile"></param>
        public Renderer(string workingDirectory, string pathToNodeExe, string pathToMermaidJsFile)
        {
            _workingDirectory = Path.GetFullPath(workingDirectory);
            
            if (Directory.Exists(_workingDirectory) == false)
            {
                throw new DirectoryNotFoundException($"no {_workingDirectory} found");
            }

            _pathToNodeExe = pathToNodeExe;
            if (Path.IsPathRooted(_pathToNodeExe) && File.Exists(_pathToNodeExe) == false)
            {
                throw new FileNotFoundException($"node path '{_pathToNodeExe}' invalid");
            }

            _pathToMermaidJs = pathToMermaidJsFile;
            if (Path.IsPathRooted(_pathToMermaidJs) && File.Exists(_pathToMermaidJs) == false)
            {
                throw new FileNotFoundException($"path to mermaid.js '{_pathToMermaidJs}' is invalid. Did you `npm install -g mermaid` yet?");
            }
        }

        #endregion //#region  "initialization"

        #region "public members"

        public RenderResult RenderAsSvg(string fileName, string graphText)
        {
            var graphFileName = WriteGraphFile(fileName,graphText);
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


        public RenderResult RenderAsImage(string fileName, string graphText)
        {
            var graphFilePath = WriteGraphFile(fileName, graphText);
            var args = $"\"{_pathToMermaidJs}\" -o \"{_workingDirectory}\" --png \"{graphFilePath}\"";
            var graphFileName = new FileInfo(graphFilePath).Name;
            var expectedFilePath = Path.Combine(_workingDirectory, $"{graphFileName}{EXTENSION_PNG}");

            var result = RunCommand(args, expectedFilePath);

            if (result.Errors.Any())
            {
                return result;
            }

            result.LocalFileSystemImagePath = expectedFilePath;

            return result;
        }

        #endregion //#region "public members"

        #region "command execution"

        private RenderResult RunCommand(string args, string expectedFilePath)
        {
            string stdOut;
            string stdErr;

            var ps = new ProcessStartInfo(_pathToNodeExe)
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

            var result = new RenderResult();

            if (string.IsNullOrWhiteSpace(stdOut))
            {
                result.Errors.Add($"No StdOut from the command '{ps.FileName} {ps.Arguments}'");
            }
            else
            {
                var parsedDiags = RawResultParser.ParseStdOut(stdOut);
                result.Diagnostics.AddRange(parsedDiags.Item1);
                result.Errors.AddRange(parsedDiags.Item2);
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
            var sb = new StringBuilder($"\"{_pathToMermaidJs}\" ");
            if (verbose)
            {
                sb.Append(" -v ");
            }
            sb.Append($" -o \"{_workingDirectory}\" ");
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

        private IFileUtils GetFileUtils()
        {
            if (_FileUtils == null)
            {
                _FileUtils = new FileUtils();
            }
            return _FileUtils;
        }

        private string WriteGraphFile(string fileName, string graphText)
        {
            var targetFilePath = GetFileUtils().PathCombine(_workingDirectory, $"{fileName}.graph");
            GetFileUtils().WriteAllText(targetFilePath, graphText);
            return targetFilePath;
        }

        #endregion //#region "helpers"
    }
}