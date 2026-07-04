using SIE.MES.Routings.RoutingSettings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.Routings.RoutingSettings
{
    /// <summary>
    /// 产品工艺路线设置-产品工艺路线明细视图配置
    /// </summary>
    public class RoutingProcessViewModelViewConfig : WebViewConfig<RoutingProcessViewModel>
    {
        /// <summary>
        /// 列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.RemoveCommands();
            View.DisableEditing();
            using (View.OrderProperties())
            {
                View.Property(p => p.Index).ShowInList();
                View.Property(p => p.ProcessName).ShowInList();
                View.Property(p => p.ProcessType).UseEnumEditor().ShowInList();
                View.Property(p => p.SegmentName).ShowInList();
                View.Property(p => p.IsOptional).ShowInList();
                View.Property(p => p.Outsourcing).ShowInList();
                View.Property(p => p.IsGenerateTask).ShowInList();
                View.Property(p => p.IsRequirementTask).ShowInList();
                
            }
        }
    }
}
