namespace CubeServer.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using System.Web.Http;
    using CubeServer.Models;
    using Data;
    using Filters;

    [StaticTokenAuthentication]
    [Authorize]
    public class ObjectController : ApiController
    {
        private IModelRepository modelRepository;

        public ObjectController(IModelRepository modelRepository)
        {
            this.modelRepository = modelRepository;
        }

        [Route("models/{modelName}")]
        public Task<ModelMetadata> GetMetadata(string modelName)
        {
            return modelRepository.GetModelMetadataAsync(modelName);
        }

        [Route("models/{modelName}/{viewport}")]
        public Task<ViewportMetadata> GetViewportMetadata(string modelName, int viewport)
        {
            return modelRepository.GetViewportMetadataAsync(modelName, viewport);
        }

        [Route("models/{modelName}/v{viewport}/{data}")]
        public async Task<HttpResponseMessage> GetVertexData(string modelName, int viewport, string data)
        {
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            //string dataPath = string.Format("{0}_{1}_{2}.obj", x, y, z);
            var stream = await modelRepository.GetCubeData(modelName, "v" + viewport, data);
            result.Content = new StreamContent(stream);
            result.Content.Headers.ContentType =
                new MediaTypeHeaderValue("application/octet-stream");
            result.Content.Headers.ContentLength = stream.Length;
            return result;
        }

        [Route("models/{modelName}/t{textureIndex}/{data}")]
        public async Task<HttpResponseMessage> GetTextureData(string modelName, int textureIndex, string data)
        {
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            //string dataPath = string.Format("{0}_{1}_{2}.obj", x, y, z);
            var stream = await modelRepository.GetCubeData(modelName, "t" + textureIndex, data);
            result.Content = new StreamContent(stream);
            result.Content.Headers.ContentType =
                new MediaTypeHeaderValue("application/octet-stream");
            result.Content.Headers.ContentLength = stream.Length;
            return result;
        }
    }
}
