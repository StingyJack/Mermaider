namespace Mermaider.Core.Abstractions
{
    using System.Collections.Generic;
    using IO;
    using Models;

    /// <summary>
    /// 
    /// </summary>
    public interface IManager
    {

        InProgressGraph RenderGraphForUser(GraphRequest request);

        SavedGraph SaveGraphForUser(InProgressGraph inProgressGraph);

        List<SavedGraph> GetGraphsForUser(string userIdent);

        void Configure(ManagerConfig config);
    }
}