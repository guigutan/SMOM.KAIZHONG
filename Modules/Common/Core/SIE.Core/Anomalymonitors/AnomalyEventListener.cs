
using Microsoft.Extensions.Caching.Memory;
using SIE.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Core.AnomalyMonitors
{
	/// <summary>
	/// 异常监控EventBus监听类
	/// </summary>
	public class AnomalyEventListener
    {
        /// <summary>
        /// 示例
        /// </summary>
        public static AnomalyEventListener Instance { get; set; } = new AnomalyEventListener();
        private static int TicketExpired = AppRuntime.Config.Get(ConfigKeys.DataPortalTicketExpiredMinutes, 30);
        //线程锁
        private readonly object _locker = new object();

        /// <summary>
        /// 订阅WMS来料接收信息
        /// </summary>
        public void Start()
        {

            var anomalyMonitorSessionKey = typeof(AnomalyMonitoringAttribute).FullName;
            //缓存
            lock (_locker)
            {
                RT.Cache.TryGet(anomalyMonitorSessionKey, out List<Type> anomalyType);

                if (anomalyType == null)
                {
                    //获取所有的异常监控类
                    anomalyType = ReadAnomalyModule();
                    RT.Cache.Add(anomalyMonitorSessionKey, anomalyType, DateTime.Now.AddMinutes(TicketExpired));
                }
            }
        }

        /// <summary>
        /// 读取监控项，并缓存监控
        /// </summary>

        public List<Type> ReadAnomalyModule()
        {
            Func<Attribute[], bool> IsAnomalyAttribute = o =>
            {

                foreach (Attribute a in o)
                {
                    if (a is AnomalyMonitoringAttribute)
                    {
                        return true;
                    }
                }
                return false;
            };
            var types = RT.GetAllModules().SelectMany(p => p.Assembly.GetTypes())
                        .Where(p => {
                            return IsAnomalyAttribute(System.Attribute.GetCustomAttributes(p, true));
                        }).ToList();
            return types;
        }
    }
}
