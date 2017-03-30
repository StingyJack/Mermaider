namespace Mermaider.Core.IO
{
    using System;
    using System.Collections.Generic;
    using Abstractions;
    using Models;
    using Utils;


    /// <summary>
    ///     Responsible for tracking files et.al
    /// </summary>
    public class Manager : IManager
    {
        /*
         * This should be storing files, per user (user picks name, hold onto date, etc)
         * Getting images for a user's collection (by name or date)
         * Removing images for a user
         * 
         * Cleaning up temp directories periodically, or pruning files it doesnt know about.
         * 
         * 
         */

        private ManagerConfig _config;
        private IRenderer _renderer;

        public void Configure(ManagerConfig config)
        {
            _config = config;
            _renderer = new Renderer(_config.UnsavedGraphFilesPath, config.PathToNodeExe, config.PathToMermaidJsFile);
        }


        public InProgressGraph RenderGraphForUser(GraphRequest request)
        {
            var result = new InProgressGraph
            {
                GraphId = $"{request.UserIdent}_{DateTime.Now.ToFileTime()}"
            };
            switch (request.OutputType)
            {
                case MermaidOutput.Png:
                    result.RenderResult = _renderer.RenderAsImage(result.GraphId, request.GraphText);
                    result.LocalPathFile = result.RenderResult.LocalFileSystemImagePath;
                    break;
                case MermaidOutput.Svg:
                    break;
                case MermaidOutput.PngAndSvg:

                    break;
            }

            return result;
        }

        public SavedGraph SaveGraphForUser(InProgressGraph inProgressGraph)
        {
            throw new NotImplementedException();
        }

        public List<SavedGraph> GetGraphsForUser(string userIdent)
        {
            throw new NotImplementedException();
        }


    }
}
