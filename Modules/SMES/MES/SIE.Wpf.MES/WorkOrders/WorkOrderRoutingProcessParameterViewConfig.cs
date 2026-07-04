using SIE.Domain;
using SIE.ManagedProperty;
using SIE.MES.WorkOrders;
using SIE.Tech.Routings;
using System;

namespace SIE.Wpf.MES.WorkOrders
{
    /// <summary>
    /// 工单工序清单参数视图配置
    /// </summary>
    [CompiledPropertyDeclarer]
    internal class WorkOrderRoutingProcessParameterViewConfig : WPFViewConfig<WorkOrderRoutingProcessParameter>
    {
        /// <summary>
        /// 下一个工序名称
        /// </summary>
        public static readonly Property<string> NextProcessNameProperty = P<WorkOrderRoutingProcessParameter>.RegisterExtensionReadOnly("NextProcessName", typeof(WorkOrderRoutingProcessParameterViewConfig),
             GetNextProcessName, WorkOrderRoutingProcessParameter.NextProcessProperty);

        /// <summary>
        /// 获取下一个工序名称
        /// </summary>
        /// <param name="me">工单工序清单参数</param>
        /// <returns>下一个工序名称</returns>
        public static string GetNextProcessName(WorkOrderRoutingProcessParameter me)
        {
            if (me.ProcessName.IsNullOrEmpty())
                return RoutingProcessSign.End.ToLabel();
            return me.ProcessName;
        }

        /// <summary>
        /// 通用视图配置
        /// </summary>
        protected override void ConfigView()
        {
            using (View.OrderProperties())
            {
                View.Property(NextProcessNameProperty).HasLabel("下一工序").Show(ShowInWhere.All);
                View.Property(p => p.ResultType).Show(ShowInWhere.All).Readonly();
            }
        }
    }
}