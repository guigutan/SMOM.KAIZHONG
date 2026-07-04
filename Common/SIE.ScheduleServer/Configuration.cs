using Hangfire;
using Hangfire.DM;
using Hangfire.MySql;
using Hangfire.Oracle.Core;
using Hangfire.PostgreSql;
using Hangfire.SqlServer;
using SIE;
using SIE.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.ScheduleServer
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

        private const string DefaultServiceName = "_S-MOM_ScheduleServer";
        private const string DefaultServiceDisplayName = "S-MOM_ScheduleServer";
        private const string DefaultServiceDescription = "S-MOM Schedule Server";

        private const string KeyQueuePollIntervalSeconds = "schedule.QueuePollIntervalSeconds";
        private const string KeyJobExpirationCheckIntervalMinutes = "schedule.JobExpirationCheckIntervalMinutes";

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
        /// 队列轮询间隔
        /// </summary>
        public static int QueuePollIntervalSeconds
        {
            get { return RT.Config.Get(KeyQueuePollIntervalSeconds, 30); }
        }

        /// <summary>
        /// 任务job清理时间
        /// </summary>
        public static double JobExpirationCheckIntervalMinutes
        {
            get { return RT.Config.Get(KeyJobExpirationCheckIntervalMinutes, 60d); }
        }
        /// <summary>
        /// Gets the service description.
        /// </summary>
        /// <value>The service description.</value>
		public static string ServiceDescription
        {
            get { return RT.Config.Get(KeyServiceDescription, DefaultServiceDescription); }
        }
        public static IGlobalConfiguration UseStorage(this IGlobalConfiguration configuration, string connectionStringName)
        {
            var cfg = RT.Config.GetConnectionString(connectionStringName);
            if (cfg == null)
                throw new PlatformException("找不到链接字符串[{0}]".FormatArgs(connectionStringName));
            if (cfg.ProviderName.Contains("Oracle", StringComparison.OrdinalIgnoreCase))
            {
                configuration.UseStorage(new OracleStorage(cfg.ConnectionString,
                    new OracleStorageOptions
                    {
                        QueuePollInterval = TimeSpan.FromSeconds(QueuePollIntervalSeconds),
                        PrepareSchemaIfNecessary = false,
                        JobExpirationCheckInterval = TimeSpan.FromMinutes(JobExpirationCheckIntervalMinutes)
                    }));
            }
            else if (cfg.ProviderName.Contains("SqlClient", StringComparison.OrdinalIgnoreCase)
                || cfg.ProviderName.Contains("SqlServer", StringComparison.OrdinalIgnoreCase))
            {
                configuration.UseSqlServerStorage(
                    cfg.ConnectionString,
                    new SqlServerStorageOptions
                    {
                        CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                        QueuePollInterval = TimeSpan.FromSeconds(QueuePollIntervalSeconds),
                        UseRecommendedIsolationLevel = true,
                        SlidingInvisibilityTimeout = TimeSpan.FromMinutes(1),
                        JobExpirationCheckInterval = TimeSpan.FromMinutes(JobExpirationCheckIntervalMinutes)
                    });
            }
            else if (cfg.ProviderName.Contains("MySql.Data", StringComparison.OrdinalIgnoreCase))
            {
                if (!cfg.ConnectionString.Contains("Allow User Variables"))
                    cfg.ConnectionString += ";Allow User Variables=true";
                configuration.UseStorage(new MySqlStorage(cfg.ConnectionString, new MySqlStorageOptions
                {
                    QueuePollInterval = TimeSpan.FromSeconds(QueuePollIntervalSeconds),
                    PrepareSchemaIfNecessary = true,
                    JobExpirationCheckInterval = TimeSpan.FromMinutes(JobExpirationCheckIntervalMinutes)
                }));
            }
            else if (cfg.ProviderName.IndexOf("Npgsql", StringComparison.OrdinalIgnoreCase) > -1)
            {
                configuration.UsePostgreSqlStorage(
                    cfg.ConnectionString,
                    new PostgreSqlStorageOptions
                    {
                        PrepareSchemaIfNecessary = true,
                    });
            }
            else if (cfg.ProviderName.IndexOf("Dm.DmClient", StringComparison.OrdinalIgnoreCase) > -1)
            {
                configuration.UseStorage(new DMStorage(cfg.ConnectionString,
                     new DMStorageOptions
                     {
                         QueuePollInterval = TimeSpan.FromSeconds(QueuePollIntervalSeconds),
                         PrepareSchemaIfNecessary = false,
                         JobExpirationCheckInterval = TimeSpan.FromMinutes(JobExpirationCheckIntervalMinutes),
                         TimeZoneResolver = TimeZoneInfo.Local
                     }));
            }
            else if (cfg.ProviderName.IndexOf("VastData", StringComparison.OrdinalIgnoreCase) > -1)
            {
                configuration.UseStorage(new PostgreSqlStorage(cfg.ConnectionString,
                    new PostgreSqlStorageOptions
                    {
                        QueuePollInterval = TimeSpan.FromSeconds(1),
                        PrepareSchemaIfNecessary = false,
                        JobExpirationCheckInterval = TimeSpan.FromMinutes(JobExpirationCheckIntervalMinutes)
                    }));
            }
            return configuration;
        }
    }
}
