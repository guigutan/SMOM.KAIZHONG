using SIE.EMS.EquipRepair.EquipRepairs;

namespace SIE.Web.EMS.EquipRepair.EquipRepairs
{
    /// <summary>
    /// 设备维修操作记录视图配置
    /// </summary>
    internal class EquipRepairOpRecViewConfig : WebViewConfig<EquipRepairOperationRec>
    {
        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.Property(p => p.OperationType).Readonly();
            View.Property(p => p.Operationer).Readonly();
            View.Property(p => p.OperationDate).Readonly().ShowInList(width: 150);
            View.Property(p => p.OriginalRepairMasterId).Readonly();
            View.Property(p => p.OriginalRepairer).Readonly();
            View.Property(p => p.HandoverConfirmResult).Readonly();
            View.Property(p => p.EngineerConfirmResult).Readonly();
            View.Property(p => p.Remark).Readonly().ShowInList(width: 250);

            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
        }
    }
}
