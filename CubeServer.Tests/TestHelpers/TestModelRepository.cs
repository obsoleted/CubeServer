namespace CubeServer.Tests.TestHelpers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web.Http;
    using Data;
    using Models;

    class TestModelRepository : IModelRepository
    {
        public Dictionary<string, ModelMetadata> Models { get; set; }
        public Dictionary<Tuple<string, int>, ViewportMetadata> ModelViewports { get; set; }

        public Dictionary<Tuple<string, int, string>, Stream> VertexData { get; set; }

        public Dictionary<Tuple<string, int, string>, Stream> TextureData { get; set; }

        public TestModelRepository()
        {
            Models = new Dictionary<string, ModelMetadata>();
            ModelViewports = new Dictionary<Tuple<string, int>, ViewportMetadata>();
            VertexData = new Dictionary<Tuple<string, int, string>, Stream>();
            TextureData = new Dictionary<Tuple<string, int, string>, Stream>();
        }

        public Task<ModelMetadata> GetModelMetadataAsync(string modelName)
        {
            return Models.ContainsKey(modelName) ? Task.FromResult(Models[modelName]) : Task.FromResult<ModelMetadata>(null);
        }

        public Task<ViewportMetadata> GetViewportMetadataAsync(string modelName, int viewport)
        {
            var key = new Tuple<string, int>(modelName, viewport);
            return ModelViewports.ContainsKey(key) ? Task.FromResult(ModelViewports[key]) : Task.FromResult<ViewportMetadata>(null);
        }

        public Task<Stream> GetTexture(string modelName, int textureIndex, string texturePath)
        {
            var key = new Tuple<string, int, string>(modelName, textureIndex, texturePath);
            return TextureData.ContainsKey(key) ?  Task.FromResult(TextureData[key]) : Task.FromResult<Stream>(null);
        }

        public Task<Stream> GetVertexData(string modelName, int viewport, string vertexDataPath)
        {
            var key = new Tuple<string, int, string>(modelName, viewport, vertexDataPath);
            return VertexData.ContainsKey(key) ? Task.FromResult(VertexData[key]) : Task.FromResult<Stream>(null);
        }
    }
}
