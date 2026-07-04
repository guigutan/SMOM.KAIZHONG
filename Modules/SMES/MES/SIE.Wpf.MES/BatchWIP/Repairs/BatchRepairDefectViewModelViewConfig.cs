using SIE.Domain;
using SIE.Wpf.MES.BatchWIP.Repairs.Commands;
using System.Linq;

namespace SIE.Wpf.MES.BatchWIP.Repairs
{
    /// <summary>
    /// 不良信息视图配置
    /// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    public class BatchRepairDefectViewModelViewConfig : WPFViewConfig<BatchRepairDefectViewModel>
    {
        #region 维修措施 Measure
        /// <summary>
        /// 维修措施
        /// </summary>
        public static readonly Property<string> MeasureProperty = P<BatchRepairDefectViewModel>.RegisterExtensionReadOnly("Measure", typeof(BatchRepairDefectViewModelViewConfig),
            GetMeasure, BatchRepairDefectViewModel.MeasureListProperty);

        /// <summary>
        /// 获取维修措施
        /// </summary>
        /// <param name="me">不良信息视图模型</param>
        /// <returns>维修措施</returns>
        public static string GetMeasure(BatchRepairDefectViewModel me)
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
        public static readonly Property<string> ResponsibilityProperty = P<BatchRepairDefectViewModel>.RegisterExtensionReadOnly("Responsibility", typeof(BatchRepairDefectViewModelViewConfig),
            GetResponsibility, BatchRepairDefectViewModel.ResponsibilityListProperty);

        /// <summary>
        /// 获取缺陷责任
        /// </summary>
        /// <param name="me">不良信息视图模型</param>
        /// <returns>缺陷责任</returns>
        public static string GetResponsibility(BatchRepairDefectViewModel me)
        {
            string responsibility = string.Empty;
            me.ResponsibilityList.ForEach(e => { responsibility += e.Description + ";"; });
            return responsibility;
        }
        #endregion

        #region 缺陷编码 DefectCode
        /// <summary>
        /// 维修措施
        /// </summary>
        public static readonly Property<string> DefectProperty = P<BatchRepairDefectViewModel>.RegisterExtensionReadOnly("DefectCode", typeof(BatchRepairDefectViewModelViewConfig),
            GetDefect, BatchRepairDefectViewModel.WipProductDefectProperty);

        /// <summary>
        /// 获取维修措施
        /// </summary>
        /// <param name="me">不良信息视图模型</param>
        /// <returns>维修措施</returns>
        public static string GetDefect(BatchRepairDefectViewModel me)
        {
            //string deft = string.Empty;
            //me.WipProductDefect.DetailList.ForEach(e => { deft += e.Defect.Code + ";"; });
            //return deft;
            var codes = me.WipProductDefect.DetailList.Select(p => p.Defect.Code);
            return string.Join(";", codes);
        }
        #endregion

        #region 缺陷编码 DefectCode
        /// <summary>
        /// 维修措施
        /// </summary>
        public static readonly Property<string> DefectDescriptionProperty = P<BatchRepairDefectViewModel>.RegisterExtensionReadOnly("DefectDescription", typeof(BatchRepairDefectViewModelViewConfig),
            GetDefectDescription, BatchRepairDefectViewModel.WipProductDefectProperty);

        /// <summary>
        /// 获取维修措施
        /// </summary>
        /// <param name="me">不良信息视图模型</param>
        /// <returns>维修措施</returns>
        public static string GetDefectDescription(BatchRepairDefectViewModel me)
        {
            var descriptions = me.WipProductDefect.DetailList.Select(p => p.Defect.Description);
            return string.Join(";", descriptions);
        }
        #endregion

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
            View.DeclareExtendViewGroup(BatchRepairView, BatchRepairDetailView);
            if (ViewGroup == BatchRepairView)
            {
                BatchRepairDefectView();
            }
            else if (ViewGroup == BatchRepairDetailView)
            {
                BatchRepairDefectDetailView();
            }
        }

        /// <summary>
        /// 批次不良信息
        /// </summary>
        protected void BatchRepairDefectView()
        {
            View.AssignAuthorize(typeof(BatchRepairViewModel));
            View.UseCommands(typeof(BatchRepairCommand), typeof(SaveBatchRepairRecord));
            View.UseDetail(columnCount: 3);
            using (View.OrderProperties())
            {
                View.Property(p => p.WipProductDefect.Process.Name).HasLabel("工序").Show(ShowInWhere.All).Readonly();
                View.Property(DefectProperty).HasLabel("缺陷编码").Show(ShowInWhere.List).Readonly();
                View.Property(DefectDescriptionProperty).HasLabel("缺陷描述").Show(ShowInWhere.List).Readonly();
                View.Property(p => p.Qty).HasLabel("不良数量").Show(ShowInWhere.All).Readonly();
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
            View.AssignAuthorize(typeof(BatchRepairViewModel));
            View.UseCommands(typeof(BatchRepairCommand));
            View.UseDetail(columnCount: 3);
            using (View.OrderProperties())
            {
                View.Property(p => p.WipProductDefect.Process.Name).HasLabel("工序").Show(ShowInWhere.All).Readonly();
                View.Property(DefectProperty).HasLabel("缺陷编码").Show(ShowInWhere.List).Readonly();
                View.Property(DefectDescriptionProperty).HasLabel("缺陷描述").Show(ShowInWhere.List).Readonly();
                View.Property(p => p.Qty).HasLabel("不良数量").Show(ShowInWhere.All).Readonly();
                View.Property(p => p.ScrapQty).HasLabel("报废数量").Show(ShowInWhere.All).UseCalcurateEditor(p => { p.MinWidth = 300; p.MinHeight = 300; });
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
