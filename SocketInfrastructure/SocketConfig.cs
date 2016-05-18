using System.Configuration;


namespace SocketInfrastructure
{
    public class SocketConfig
    {
        #region Methods
        public static int GetBacklog()
            => int.Parse(ConfigurationManager.AppSettings["backlog"]);

        public static int GetBufferSize()
            => int.Parse(ConfigurationManager.AppSettings["bufferSize"]);

        public static string GetEof()
            => ConfigurationManager.AppSettings["eof"];

        public static string GetIpAddress()
            => ConfigurationManager.AppSettings["ipAddress"];

        public static int GetPort()
            => int.Parse(ConfigurationManager.AppSettings["port"]);
        #endregion
    }
}