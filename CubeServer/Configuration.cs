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

        private const string ServiceRootUrlKey = "ServiceRootUrl";
        private static string _serviceRootUrl;

        public static string ServiceRootUrl
        {
            get
            {
                if (string.IsNullOrEmpty(_serviceRootUrl))
                {
                    _serviceRootUrl = CloudConfigurationManager.GetSetting(ServiceRootUrlKey);
                }
                return _serviceRootUrl;
            }
        }

        private const string UrlReplacementTargetKey = "UrlReplacementTarget";
        private static string _urlReplacementTarget;

        public static string UrlReplacementTarget
        {
            get
            {
                if (string.IsNullOrEmpty(_urlReplacementTarget))
                {
                    _urlReplacementTarget = CloudConfigurationManager.GetSetting(UrlReplacementTargetKey);
                }
                return _urlReplacementTarget;
            }
        }
    }
}