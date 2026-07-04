using SIE.MES.ProjectDesigns;
using SIE.MES.ProjectDesigns.ChildInfos;
using SIE.MetaModel.View;
using SIE.Tech.Processs;
using SIE.Web.MES.ProjectDesigns.ChildCommands.RoutingCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.ProjectDesigns.ChildInfos
{
    /// <summary>
    /// 工艺资料-产品工艺路线明细视图配置
    /// </summary>
    public class DesignTreeRoutingDetailViewConfig : WebViewConfig<DesignTreeRoutingDetail>
    {
        /// <summary>
        /// 查询视图
        /// </summary>
        public const string LookUpViewGroup = "LookUpViewGroup";

        /// <summary>
        /// 视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(ProjectDesign));
            View.DeclareExtendViewGroup(LookUpViewGroup);
            View.InlineEdit();
            if (ViewGroup == LookUpViewGroup)
            {
                ReadOnlyView();
            }
        }

        /// <summary>
        /// 列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands(typeof(TreeRoutingDetailAddCommand).FullName, WebCommandNames.Edit, WebCommandNames.Delete);

            using (View.OrderProperties())
            {
                View.Property(p => p.Index).UseSpinEditor(p => { p.MinValue = 10; p.Step = 10; p.AllowNegative = false; }).ShowInList();
                View.Property(p => p.Process).UseDataSource((e, p, k) =>
                {
                    return RT.Service.Resolve<ProcessController>().GetProcessList(p, k);
                }).UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
                    keyValuePairs.Add(nameof(e.ProcessName), nameof(e.Process.Name));
                    keyValuePairs.Add(nameof(e.ProcessType), nameof(e.Process.Type));
                    keyValuePairs.Add(nameof(e.SegmentName), nameof(e.Process.SegmentName));
                    m.DicLinkField = keyValuePairs;
                }).ShowInList();
                View.Property(p => p.ProcessType).Readonly().ShowInList();
                View.Property(p => p.SegmentName).Readonly().ShowInList();
                View.Property(p => p.IsOptional).ShowInList();
                View.Property(p => p.Outsourcing).ShowInList();
                View.Property(p => p.IsGenerateTask).ShowInList(width: 120);
                View.Property(p => p.IsRequirementTask).ShowInList(width: 120);
                
                View.Property(p => p.Beat).ShowInList();
                View.Property(p => p.DirectCost).ShowInList();
                View.Property(p => p.InDirectCost).ShowInList();
                View.Property(p => p.EnergyCost).ShowInList();
                View.Property(p => p.OtherCost).ShowInList();
            }
        }

        /// <summary>
        /// 下拉选择视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.Process).ShowInList(width: 120);
                View.Property(p => p.Index).ShowInList(width: 120);
                View.Property(p => p.ProcessType).ShowInList(width: 120);
                View.Property(p => p.SegmentName).ShowInList(width: 120);
            }
        }

        /// <summary>
        /// 查看界面视图
        /// </summary>
        private void ReadOnlyView()
        {
            View.ClearCommands();
            View.DisableEditing();
            using (View.OrderProperties())
            {
                View.Property(p => p.Index).ShowInList();
                View.Property(p => p.Process).ShowInList();
                View.Property(p => p.ProcessType).Readonly().ShowInList();
                View.Property(p => p.SegmentName).Readonly().ShowInList();
                View.Property(p => p.IsOptional).ShowInList();
                View.Property(p => p.Outsourcing).ShowInList();
                View.Property(p => p.IsGenerateTask).ShowInList(width: 120);
                View.Property(p => p.IsRequirementTask).ShowInList(width: 120);
                View.Property(p => p.Beat).ShowInList();
                View.Property(p => p.DirectCost).ShowInList();
                View.Property(p => p.InDirectCost).ShowInList();
                View.Property(p => p.EnergyCost).ShowInList();
                View.Property(p => p.OtherCost).ShowInList();
            }
        }
    }
}
