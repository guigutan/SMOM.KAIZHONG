namespace SIE.WebApiHost
{
    /// <summary>
    /// 配置类
    /// </summary>
    public static class Configuration
    {
        private const string PrefixServerConfiguration = "server";
        private const string KeyServiceName = PrefixServerConfiguration + ".serviceName";
        private const string KeyServiceDisplayName = PrefixServerConfiguration + ".serviceDisplayName";
        private const string KeyServiceDescription = PrefixServerConfiguration + ".serviceDescription";

        private const string DefaultServiceName = "S-Nest Server";
        private const string DefaultServiceDisplayName = "_S-Nest Server";
        private const string DefaultServiceDescription = "S-Nest Logical Service Host";

        /// <summary>
        /// Gets the name of the service.
        /// </summary>
        /// <value>The name of the service.</value>
        public static string ServiceName
        {
            get { return RT.Config.Get(KeyServiceName, DefaultServiceName); }
        }

        /// <summary>
        /// Gets the display name of the service.
        /// </summary>
        /// <value>The display name of the service.</value>
		public static string ServiceDisplayName
        {
            get { return RT.Config.Get(KeyServiceDisplayName, DefaultServiceDisplayName); }
        }

        /// <summary>
        /// Gets the service description.
        /// </summary>
        /// <value>The service description.</value>
		public static string ServiceDescription
        {
            get { return RT.Config.Get(KeyServiceDescription, DefaultServiceDescription); }
        }
    }
}
