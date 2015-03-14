namespace CubeServer.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.IO;
    using System.Net;
    using System.Threading.Tasks;
    using System.Web.Http;
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Blob;
    using Models;

    public class AzureModelRepository : IModelRepository
    {
        public async Task<Models.ModelMetadata> GetModelMetadataAsync(string modelName)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(Configuration.StorageConnectionString);
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer modelContainer = blobClient.GetContainerReference(modelName);
            if (!await modelContainer.ExistsAsync())
            {
                return null;
            }

            ICloudBlob indexBlob = await modelContainer.GetBlobReferenceFromServerAsync("index.json");
            if (!await indexBlob.ExistsAsync())
            {
                return null;
            }
            string indexJson;
            using (var memoryStream = new MemoryStream())
            {
                indexBlob.DownloadToStream(memoryStream);
                indexJson = System.Text.Encoding.UTF8.GetString(memoryStream.ToArray());
            }

            return Newtonsoft.Json.JsonConvert.DeserializeObject<ModelMetadata>(indexJson);
        }


        public async Task<ViewportMetadata> GetViewportMetadataAsync(string modelName, int viewport)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(Configuration.StorageConnectionString);
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer modelContainer = blobClient.GetContainerReference(modelName);
            if (!await modelContainer.ExistsAsync())
            {
                return null;
            }

            var viewportDirectory = modelContainer.GetDirectoryReference("v" + viewport);
            var metadataBlob = viewportDirectory.GetBlockBlobReference("metadata.json");
            if (!await metadataBlob.ExistsAsync())
            {
                return null;
            }

            string metadataJson;
            using (var memoryStream = new MemoryStream())
            {
                metadataBlob.DownloadToStream(memoryStream);
                metadataJson = System.Text.Encoding.UTF8.GetString(memoryStream.ToArray());
            }

            return Newtonsoft.Json.JsonConvert.DeserializeObject<ViewportMetadata>(metadataJson);
        }

        public async Task<Stream> GetCubeData(string modelName, string directory, string data)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(Configuration.StorageConnectionString);
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer modelContainer = blobClient.GetContainerReference(modelName);
            if (!await modelContainer.ExistsAsync())
            {
                return null;
            }

            var viewportDirectory = modelContainer.GetDirectoryReference(directory);
            var metadataBlob = viewportDirectory.GetBlockBlobReference(data);
            if (!await metadataBlob.ExistsAsync())
            {
                return null;
            }

            return await metadataBlob.OpenReadAsync();
        }


        public Task<Stream> GetTexture(string modelName, int textureIndex, string texturePath)
        {
            return GetCubeData(modelName, "t" + textureIndex, texturePath);
        }

        public Task<Stream> GetVertexData(string modelName, int viewport, string vertexDataPath)
        {
            return GetCubeData(modelName, "v" + viewport, vertexDataPath);
        }
    }
}