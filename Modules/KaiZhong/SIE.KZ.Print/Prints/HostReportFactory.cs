using SIE.Domain.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Common.Prints
{
    /// <summary>
    /// 报表工厂
    /// </summary>
    public class HostReportFactory
    {
        /// <summary>
        /// 报表工厂
        /// </summary>
        public static HostReportFactory Current = new HostReportFactory();
        /// <summary>
        /// 打印提供者的容器
        /// </summary>
        private static Dictionary<Type, IHostReport> container = new Dictionary<Type, IHostReport>();
        /// <summary>
        /// 当前所有支持的报表类型
        /// </summary>
        public IEnumerable<IHostReport> ReportTypes
        {
            get
            {
                return container.Values;
            }
        }
        /// <summary>
        /// 注册报表提供者
        /// </summary>
        /// <param name="report"></param>
        public virtual void Register(IHostReport report)
        {
            if (report == null)
                throw new ArgumentNullException(nameof(report));
            var type = report.GetType();
            if (!container.ContainsKey(type))
            {
                container.Add(type, report);
            }
        }
        /// <summary>
        /// 获取报表提供者
        /// </summary>
        /// <param name="name">报表名</param>
        /// <returns>报表提供者</returns>
        public virtual IHostReport GetReport(string name)
        {
            var report = container.Values.FirstOrDefault(p => p.Name == name);
            return GetReport(report);
        }
        /// <summary>
        /// 获取报表提供者
        /// </summary>
        /// <param name="extension">报表扩展名</param>
        /// <returns>报表提供者</returns>
        public virtual IHostReport GetReportByExtension(string extension)
        {
            var report = container.Values.FirstOrDefault(p => p.Extension.EqualsIgnoreCase(extension));
            return GetReport(report);
        }
        /// <summary>
        /// 获取报表提供者
        /// </summary>
        /// <param name="report">报表提供者</param>
        /// <returns>报表提供者</returns>
        IHostReport GetReport(IHostReport report)
        {
            if (report == null)
                throw new ValidationException("未找到：对应的报表".L10N());
            return report;
        }

    }
}
