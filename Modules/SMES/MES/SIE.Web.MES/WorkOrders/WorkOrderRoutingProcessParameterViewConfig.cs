using SIE.Domain;
using SIE.ManagedProperty;
using SIE.MES.WorkOrders;
using SIE.Tech.Routings;
using SIE.Utils;
using System;

namespace SIE.Web.MES.WorkOrders
{
    /// <summary>
    /// 工单工序清单参数视图配置
    /// </summary>
    [CompiledPropertyDeclarer]
    internal class WorkOrderRoutingProcessParameterViewConfig : WebViewConfig<WorkOrderRoutingProcessParameter>
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
            if (me.NextProcess != null)
            {
                return me.NextProcess.Name;
            }
            return EnumViewModel.EnumToLabel(RoutingProcessSign.End).L10N();
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
