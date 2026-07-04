using SIE.Domain;
using SIE.MES.WIP.Repairs;
using SIE.Wpf.MES.BatchWIP.Repairs.Commands;
using SIE.Wpf.MES.WIP.Repairs.Commands;
using System.Linq;

namespace SIE.Wpf.MES.WIP.Repairs
{
    /// <summary>
    /// 不良信息视图配置
    /// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    public class RepairDefectViewModelViewConfig : WPFViewConfig<RepairDefectViewModel>
    {
        #region 维修措施 Measure
        /// <summary>
        /// 维修措施
        /// </summary>
        public static readonly Property<string> MeasureProperty = P<RepairDefectViewModel>.RegisterExtensionReadOnly("Measure", typeof(RepairDefectViewModelViewConfig),
            GetMeasure, RepairDefectViewModel.MeasureListProperty);

        /// <summary>
        /// 获取维修措施
        /// </summary>
        /// <param name="me">不良信息视图模型</param>
        /// <returns>维修措施</returns>
        public static string GetMeasure(RepairDefectViewModel me)
        {
            string measure = string.Empty;
            me.MeasureList.ForEach(e => { measure += e.Name + ";"; });
            return measure;
        }
        #endregion

        #region 缺陷责任 Responsibility
        /// <summary>
        /// 缺陷责任
        /// </summary> 
        public static readonly Property<string> ResponsibilityProperty = P<RepairDefectViewModel>.RegisterExtensionReadOnly("Responsibility", typeof(RepairDefectViewModelViewConfig),
            GetResponsibility, RepairDefectViewModel.ResponsibilityListProperty);

        /// <summary>
        /// 获取缺陷责任
        /// </summary>
        /// <param name="me">不良信息视图模型</param>
        /// <returns>缺陷责任</returns>
        public static string GetResponsibility(RepairDefectViewModel me)
        {
            string responsibility = string.Empty;
            me.ResponsibilityList.ForEach(e => { responsibility += e.Description + ";"; });
            return responsibility;
        }
        #endregion

        /// <summary>
        /// 维修不良信息
        /// </summary>
        public const string RepairView = "RepairView";

        /// <summary>
        /// 批次不良信息
        /// </summary>
        public const string BatchRepairView = "BatchRepairView";

        /// <summary>
        /// 批次不良信息明细
        /// </summary>
        public const string BatchRepairDetailView = "BatchRepairDetailView";

        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(RepairView, BatchRepairView, BatchRepairDetailView);
            if (ViewGroup == RepairView)
            {
                RepairDefectView();
            }
            else if (ViewGroup == BatchRepairView)
            {
                BatchRepairDefectView();
            }
            else if (ViewGroup == BatchRepairDetailView)
            {
                BatchRepairDefectDetailView();
            }
        }

        /// <summary>
        /// 维修不良信息界面
        /// </summary>
        protected void RepairDefectView()
        {
            View.AssignAuthorize(typeof(RepairViewModel));
            View.UseCommands(typeof(RepairCommand));
            View.UseDetail(columnCount: 3);
            using (View.OrderProperties())
            {
                View.Property(p => p.WipProductDefect.Process.Name).HasLabel("工序").Show(ShowInWhere.All).Readonly();
                View.Property(p => p.WipProductDefect.NgQty).HasLabel("数量").Show(ShowInWhere.All).Readonly();
                View.Property(p => p.WipProductDefect.InspectionItem.Name).HasLabel("检验项描述").UseMemoEditor().Show(ShowInWhere.All).Readonly();
                View.Property(p => p.WipProductDefect.Defect.Code).HasLabel("缺陷编码").Show(ShowInWhere.All).Readonly();
                View.Property(p => p.WipProductDefect.Defect.Description).HasLabel("缺陷描述").UseMemoEditor().Show(ShowInWhere.All).Readonly();
                View.Property(MeasureProperty).HasLabel("维修措施").Show(ShowInWhere.List).Readonly();
                View.Property(ResponsibilityProperty).HasLabel("缺陷责任").Show(ShowInWhere.List).Readonly();
                View.Property(p => p.Remark).Show(ShowInWhere.List).Readonly();
                View.Property(p => p.ScrapQty).ShowInList().Readonly();
                View.Property(p => p.ScrapReason).ShowInList().Readonly();
                View.ChildrenProperty(p => p.MeasureList).Show(ChildShowInWhere.Hide);
                View.ChildrenProperty(p => p.ResponsibilityList).Show(ChildShowInWhere.Hide);
            }
        }

        /// <summary>
        /// 批次不良信息
        /// </summary>
        protected void BatchRepairDefectView()
        {
            View.UseCommands(typeof(BatchRepairCommand));
            View.UseDetail(columnCount: 3);
            using (View.OrderProperties())
            {
                View.Property(p => p.WipProductDefect.Process.Name).HasLabel("工序").Show(ShowInWhere.All).Readonly();
                View.Property(p => p.WipProductDefect.Defect.Code).HasLabel("缺陷编码").Show(ShowInWhere.All).Readonly();
                View.Property(p => p.WipProductDefect.NgQty).HasLabel("不良数量").Show(ShowInWhere.All).Readonly();
                View.Property(MeasureProperty).HasLabel("维修措施").Show(ShowInWhere.List).Readonly();
                View.Property(ResponsibilityProperty).HasLabel("缺陷责任").Show(ShowInWhere.List).Readonly();
                View.Property(p => p.Remark).Show(ShowInWhere.List).Readonly();
                View.Property(p => p.ScrapQty).HasLabel("报废数量").Show(ShowInWhere.All).Readonly();
                View.Property(p => p.ScrapReason).HasLabel("报废原因").Show(ShowInWhere.All).Readonly();
                View.ChildrenProperty(p => p.MeasureList).Show(ChildShowInWhere.Hide);
                View.ChildrenProperty(p => p.ResponsibilityList).Show(ChildShowInWhere.Hide);
            }
        }

        /// <summary>
        /// 维修弹窗明细
        /// </summary>
        protected void BatchRepairDefectDetailView()
        {
            View.UseCommands(typeof(BatchRepairCommand));
            View.UseDetail(columnCount: 3);
            using (View.OrderProperties())
            {
                View.Property(p => p.WipProductDefect.Process.Name).HasLabel("工序").Show(ShowInWhere.All).Readonly();
                View.Property(p => p.WipProductDefect.Defect.Code).HasLabel("缺陷编码").Show(ShowInWhere.All).Readonly();
                View.Property(p => p.WipProductDefect.NgQty).HasLabel("不良数量").Show(ShowInWhere.All).Readonly();
                View.Property(p => p.ScrapQty).HasLabel("报废数量").Show(ShowInWhere.All);
                View.Property(p => p.ScrapReason).HasLabel("报废原因").Show(ShowInWhere.All);
                View.Property(MeasureProperty).HasLabel("维修措施").Show(ShowInWhere.List).Readonly();
                View.Property(ResponsibilityProperty).HasLabel("缺陷责任").Show(ShowInWhere.List).Readonly();
                View.Property(p => p.Remark).Show(ShowInWhere.List).Readonly();
                View.ChildrenProperty(p => p.MeasureList).Show(ChildShowInWhere.Hide);
                View.ChildrenProperty(p => p.ResponsibilityList).Show(ChildShowInWhere.Hide);
            }
        }
    }
}
