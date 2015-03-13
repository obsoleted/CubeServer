namespace CubeServer.Models
{
    public class ModelMetadata
    {
        public int MinimumViewport { get; set; }
        public int MaximumViewport { get; set; }
        public string CubeTemplate { get; set; }
        public string MtlTemplate { get; set; }
        public string JpgTemplate { get; set; }
        public string MetadataTemplate { get; set; }
        public int TextureSubdivide { get; set; }
        public string TexturePath { get; set; }
    }
}