namespace CubeServer.Tests.Controllers
{
    using System;
    using System.IO;
    using System.Net;
    using System.Net.Http;
    using System.Runtime.InteropServices.WindowsRuntime;
    using System.Threading.Tasks;
    using System.Web.Http.Results;
    using CubeServer.Controllers;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Models;
    using TestHelpers;

    [TestClass]
    public class ModelControllerTests
    {
        [TestMethod]
        public async Task GetNonExistentModelMetadataReturns404()
        {
            var testRepository = new TestModelRepository();
            var modelController = new ModelController(testRepository);
            var result = await modelController.GetMetadataAsync("doesnotexist") as NotFoundResult;

            Assert.IsNotNull(result, "No result returned or result was not of expected type (NotFoundResult)");
        }

        [TestMethod]
        public async Task GetNonExistentViewportMetadataReturns404()
        {
            var testRepository = new TestModelRepository();
            var modelController = new ModelController(testRepository);
            var result = await modelController.GetViewportMetadataAsync("doesnotexist", 3) as NotFoundResult;

            Assert.IsNotNull(result, "No result returned or result was not of expected type (NotFoundResult)");
        }

        [TestMethod]
        public async Task GetNonExistentVertexDataReturns404()
        {
            var testRepository = new TestModelRepository();
            var modelController = new ModelController(testRepository);
            var result = await modelController.GetVertexDataAsync("doesnotexist",1,"doesnotexist.file") as NotFoundResult;

            Assert.IsNotNull(result, "No result returned or result was not of expected type (NotFoundResult)");
        }

        [TestMethod]
        public async Task GetNonExistentTextureDataReturns404()
        {
            var testRepository = new TestModelRepository();
            var modelController = new ModelController(testRepository);
            var result = await modelController.GetTextureDataAsync("doesnotexist", 1, "doesnotexist.file") as NotFoundResult;

            Assert.IsNotNull(result, "No result returned or result was not of expected type (NotFoundResult)");
        }

        [TestMethod]
        public async Task GetModelMetadataOk()
        {
            const string modelName = "getmodelmetadataok";
            ModelMetadata expectedMetadata = new ModelMetadata()
            {
                CubeTemplate = "cubetemplate",
                JpgTemplate = "jpgTemplate",
                MaximumViewport = 5,
                MetadataTemplate = "metadataTemplate",
                MinimumViewport = 1,
                MtlTemplate = "mtlTemplate",
                TexturePath = "texturePath",
                TextureSubdivide = 4
            };

            var testRepository = new TestModelRepository();
            testRepository.Models[modelName] = expectedMetadata;

            var modelController = new ModelController(testRepository);
            var result = await modelController.GetMetadataAsync(modelName) as OkNegotiatedContentResult<ModelMetadata>;

            Assert.IsNotNull(result, "No result returned or result was not of expected type (OkNegotiatedContentResult<ModelMetadata>)");
            Assert.IsNotNull(result.Content, "Result content");
            Assert.AreEqual(expectedMetadata, result.Content, "Result content");
        }

        [TestMethod]
        public async Task GetViewportMetdataOk()
        {
            const string modelName = "getmodelmetadataok";
            const int viewport = 2;

            Tuple<string, int> viewportKey = new Tuple<string, int>(modelName, viewport);

            ViewportMetadata expectedViewportMetadata = new ViewportMetadata()
            {
                GridSize = new Gridsize() { X = 1, Y = 2, Z = 3},
                CubeExists = new bool[1,2,3],
                Extents = new Extents() {  XMax = 100, XMin = 0, XSize = 100, YMax = 100, YMin = 0, YSize = 100, ZMax = 77, ZMin = -1, ZSize = 78 },
            };

            var testRepository = new TestModelRepository();
            testRepository.ModelViewports[viewportKey] = expectedViewportMetadata;

            var modelController = new ModelController(testRepository);
            var result = await modelController.GetViewportMetadataAsync(modelName, viewport) as OkNegotiatedContentResult<ViewportMetadata>;

            Assert.IsNotNull(result, "No result returned or result was not of expected type (OkNegotiatedContentResult<ModelMetadata>)");
            Assert.IsNotNull(result.Content, "Result content");
            Assert.AreEqual(expectedViewportMetadata, result.Content, "Result content");
        }

        private Stream GenerateStreamFromString(string s)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        [TestMethod]
        public async Task GetVertexDataOk()
        {
            const string modelName = "getmodelmetadataok";
            const int viewport = 2;
            const string dataName = "data";

            var dataKey = new Tuple<string, int, string>(modelName, viewport, dataName);


            using (var expectedStream = GenerateStreamFromString(modelName))
            {
                var testRepository = new TestModelRepository();
                testRepository.VertexData[dataKey] = expectedStream;

                var modelController = new ModelController(testRepository);
                var result =
                    await modelController.GetVertexDataAsync(modelName, viewport, dataName) as
                        ResponseMessageResult;

                Assert.IsNotNull(result,
                    "No result returned or result was not of expected type (ResponseMessageResult)");

                Assert.IsNotNull(result.Response, "Result response");
                Assert.AreEqual(HttpStatusCode.OK, result.Response.StatusCode, "Response status");
                var streamContent = result.Response.Content as StreamContent;
                Assert.IsNotNull(streamContent, "No response content or response was not of expected type (StreamContent)");
                Assert.AreEqual(modelName,await streamContent.ReadAsStringAsync(), "Stream contents");
            }
        }

        [TestMethod]
        public async Task GetTextureDataOk()
        {
            const string modelName = "getmodelmetadataok";
            const int viewport = 2;
            const string dataName = "data";

            var dataKey = new Tuple<string, int, string>(modelName, viewport, dataName);


            using (var expectedStream = GenerateStreamFromString(modelName))
            {
                var testRepository = new TestModelRepository();
                testRepository.TextureData[dataKey] = expectedStream;

                var modelController = new ModelController(testRepository);
                var result =
                    await modelController.GetTextureDataAsync(modelName, viewport, dataName) as
                        ResponseMessageResult;

                Assert.IsNotNull(result,
                    "No result returned or result was not of expected type (ResponseMessageResult)");

                Assert.IsNotNull(result.Response, "Result response");
                Assert.AreEqual(HttpStatusCode.OK, result.Response.StatusCode, "Response status");
                var streamContent = result.Response.Content as StreamContent;
                Assert.IsNotNull(streamContent, "No response content or response was not of expected type (StreamContent)");
                Assert.AreEqual(modelName, await streamContent.ReadAsStringAsync(), "Stream contents");
            }
        }


    }
}
