namespace CubeServer.Data
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Models;

    public interface IModelRepository
    {
        Task<ModelMetadata> GetModelMetadataAsync(string modelName);

        Task<ViewportMetadata> GetViewportMetadataAsync(string modelName, int viewport);

        Task<Stream> GetCubeData(string modelName, string directory, string data);
    }
}
