namespace CubeServer
{
    using Microsoft.WindowsAzure;

    public static class Configuration
    {
        private const string StaticTokenKey = "StaticToken";
        private static string _staticToken;

        public static string StaticToken
        {
            get
            {
                if (string.IsNullOrEmpty(_staticToken))
                {
                    _staticToken = CloudConfigurationManager.GetSetting(StaticTokenKey);
                }
                return _staticToken;
            }
        }

        private const string StorageConnectionStringKey = "StorageConnectionString";
        private static string _storageConnectionString;
        public static string StorageConnectionString
        {
            get
            {
                if (string.IsNullOrEmpty(_storageConnectionString))
                {
                    _storageConnectionString = CloudConfigurationManager.GetSetting(StorageConnectionStringKey);
                }
                return _storageConnectionString;
            }
        }
    }
}