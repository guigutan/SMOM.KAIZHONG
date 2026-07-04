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
    /// 项目号工艺卡-工艺参数视图配置
    /// </summary>
    public class ProjectDesignCardParamterViewConfig : WPFViewConfig<ProjectDesignCardParamter>
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
            using (View.OrderProperties())
            {
                View.Property(p => p.ParameterCode).ShowInList(gridWidth: 120);
                View.Property(p => p.ParameterName).ShowInList(gridWidth: 120);
                View.Property(p => p.ParameterType).ShowInList(gridWidth: 120);
                View.Property(p => p.ProcessStDtlValueType).ShowInList(gridWidth: 120);
                View.Property(p => p.Unit).ShowInList(gridWidth: 120);
                View.Property(p => p.SingleValue).ShowInList(gridWidth: 120);
                View.Property(p => p.RangeMaxValue).ShowInList(gridWidth: 120);
                View.Property(p => p.RangeMinValue).ShowInList(gridWidth: 120);
            }
        }
    }
}
