using SIE.Traces.Common;

namespace SIE.Web.Traces.Common
{
    /// <summary>
    /// 品质追溯视图配置
    /// </summary>
    internal class QmsTraceViewModelViewConfig : WebViewConfig<QmsTraceViewModel>
    {
        /// <summary>
        ///  正向追溯-产品品质追溯视图
        /// </summary>
        public const string ProductQmsViewGroup = "ProductQmsViewGroup";

        /// <summary>
        /// 配置默认视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(new string[] { ProductQmsViewGroup });
            if (ViewGroup == ProductQmsViewGroup)
            {
                ConfigProductQmsView();
            }
        }

        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.WithoutPaging();
            View.ClearCommands();
            View.DisableEditing();
            View.Property(p => p.InspectionType).ShowInList().DisableSort();
            View.Property(p => p.InspectionNo).ShowInList().DisableSort();
            View.Property(p => p.InspectionBy).ShowInList().DisableSort();
            View.Property(p => p.InspectionTime).ShowInList().DisableSort();
            View.Property(p => p.InspectionResult).ShowInList().DisableSort();
            View.Property(p => p.DefectRecord).ShowInList().DisableSort();
            View.Property(p => p.FailedAuditWorkflowCode).ShowInList().DisableSort();
            View.Property(p => p.FailedAuditResult).ShowInList().DisableSort();
            View.Property(p => p.QualityWorkflowCode).ShowInList().DisableSort();
        }


        ///<summary>
        /// 关联产品配置列表视图
        /// </summary>
        protected void ConfigProductQmsView()
        {
            View.WithoutPaging();
            View.DisableEditing();
            View.ClearCommands();
            using (View.OrderProperties())
            {
                View.Property(p => p.InspectionType).ShowInList().DisableSort();
                View.Property(p => p.InspectionNo).ShowInList().DisableSort();
                View.Property(p => p.InspectionBy).ShowInList().DisableSort();
                View.Property(p => p.InspectionTime).ShowInList().DisableSort();
                View.Property(p => p.InspectionResult).ShowInList().DisableSort();
                View.Property(p => p.DefectRecord).ShowInList().DisableSort();                     
                View.Property(p => p.FailedAuditWorkflowCode).ShowInList().DisableSort();
                View.Property(p => p.FailedAuditResult).ShowInList().DisableSort();
                View.Property(p => p.QualityWorkflowCode).ShowInList().DisableSort();
            }
        }
    }
}