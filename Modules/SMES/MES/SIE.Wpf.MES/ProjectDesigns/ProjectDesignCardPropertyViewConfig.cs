using SIE.MES.ProjectDesigns.ViewModels;
using SIE.Wpf.MES.BatchWIP.Assemblys;
using SIE.Wpf.MES.BatchWIP.Inspects;
using SIE.Wpf.MES.BatchWIP.Repairs;
using SIE.Wpf.MES.WIP.Assemblys;
using SIE.Wpf.MES.WIP.Inspects;
using SIE.Wpf.MES.WIP.Repairs;
using SIE.Wpf.MES.WIP.TemporaryRepairs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Wpf.MES.ProjectDesigns
{
    /// <summary>
    /// 项目号工艺卡-基础属性视图配置
    /// </summary>
    public class ProjectDesignCardPropertyViewConfig : WPFViewConfig<ProjectDesignCardProperty>
    {
        /// <summary>
        /// 视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(AssemblyViewModel), typeof(BatchAssemblyViewModel), typeof(RepairViewModel), typeof(BatchRepairViewModel), typeof(TemporaryRepairViewModel), typeof(InspectViewModel), typeof(BatchInspectViewModel), typeof(InspectByItemViewModel));
        }

        /// <summary>
        /// 列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.DisableEditing();
            View.Property(p => p.PropertyName).ShowInList(gridWidth: 120);
            View.Property(p => p.PropertyValue).ShowInList(gridWidth: 120);
            View.Property(p => p.PropertyUnit).ShowInList(gridWidth: 120);
        }
    }
}
