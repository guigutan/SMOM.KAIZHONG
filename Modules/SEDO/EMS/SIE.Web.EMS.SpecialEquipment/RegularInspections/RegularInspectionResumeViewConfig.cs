using SIE.EMS.SpecialEquipment.RegularInspections;

namespace SIE.Web.EMS.SpecialEquipment.RegularInspections
{
    /// <summary>
    /// 操作记录视图配置
    /// </summary>
    internal class RegularInspectionResumeViewConfig : WebViewConfig<RegularInspectionResume>
    {
        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.OperationType);
                View.Property(p => p.OperationDateTime);
                View.Property(p => p.InspectionResult);
                View.Property(p => p.OperatorId).HasLabel("操作人");
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            }
        }

    }
}