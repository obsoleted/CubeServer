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
    using System.Web.Caching;
    using System.Web.Http;
    using CubeServer.Models;
    using Data;
    using Filters;
    using WebApi.OutputCache.V2;

    [StaticTokenAuthentication]
    [Authorize]
    public class ModelController : ApiController
    {
        private readonly IModelRepository modelRepository;

        private const int ClientCacheTimeInSeconds = 120;
        private const int ServerCacheTimeInSeconds = 600;

        public ModelController(IModelRepository modelRepository)
        {
            this.modelRepository = modelRepository;
        }

        [Route("models/{modelName}")]
        public async Task<IHttpActionResult> GetMetadataAsync(string modelName)
        {
            var metadata = await modelRepository.GetModelMetadataAsync(modelName);
            if (metadata == null)
            {
                return NotFound();
            }

            return Ok(metadata);
        }

        [Route("models/{modelName}/{viewport}")]
        public async Task<IHttpActionResult> GetViewportMetadataAsync(string modelName, int viewport)
        {
            var viewportMetadata = await modelRepository.GetViewportMetadataAsync(modelName, viewport);
            if (viewportMetadata == null)
            {
                return NotFound();
            }

            return Ok(viewportMetadata);
        }

        [CacheOutput(ClientTimeSpan = ClientCacheTimeInSeconds, ServerTimeSpan = ServerCacheTimeInSeconds)]
        [Route("models/{modelName}/v{viewport}/{data}")]
        public async Task<IHttpActionResult> GetVertexDataAsync(string modelName, int viewport, string data)
        {
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            var stream = await modelRepository.GetVertexData(modelName, viewport, data);
            if (stream == null)
            {
                return NotFound();
            }

            result.Content = new StreamContent(stream);
            result.Content.Headers.ContentType =
                new MediaTypeHeaderValue("application/octet-stream");
            result.Content.Headers.ContentLength = stream.Length;
            return ResponseMessage(result);
        }


        [CacheOutput(ClientTimeSpan = ClientCacheTimeInSeconds, ServerTimeSpan = ServerCacheTimeInSeconds)]
        [Route("models/{modelName}/t{textureIndex}/{data}")]
        public async Task<IHttpActionResult> GetTextureDataAsync(string modelName, int textureIndex, string data)
        {
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            var stream = await modelRepository.GetTexture(modelName, textureIndex, data);
            if (stream == null)
            {
                return NotFound();
            }
            result.Content = new StreamContent(stream);
            result.Content.Headers.ContentType =
                new MediaTypeHeaderValue("application/octet-stream");
            result.Content.Headers.ContentLength = stream.Length;
            return ResponseMessage(result);
        }
    }
}
