using SIE.EMS.InventoryTasks;
using SIE.Equipments.Enums;

namespace SIE.Web.EMS.InventoryTasks
{
    /// <summary>
    /// 备件盘点差异视图配置
    /// </summary>
    public class InventoryTaskSparePartDiffViewConfig : WebViewConfig<InventoryTaskSparePartDiff>
    {
        //单个字符宽度
        private readonly int SingleCharWidth = 20;

        /// <summary>
        /// 
        /// </summary>
        protected override void ConfigListView()
        {
            View.Property(p => p.SparePartId).ShowInList(SingleCharWidth * 8).Readonly();
            View.Property(p => p.SparePartName).ShowInList(SingleCharWidth * 8).Readonly();
            View.Property(p => p.Sn).ShowInList(SingleCharWidth * 10).Readonly();
            View.Property(p => p.Total).ShowInList(SingleCharWidth * 4).Readonly();
            View.Property(p => p.ActualTotal).ShowInList(SingleCharWidth * 4).Readonly();
            View.Property(p => p.InventoryResult).ShowInList(SingleCharWidth * 4).Readonly();
            View.Property(p => p.Diff).ShowInList(SingleCharWidth * 4).Readonly();
            View.Property(p => p.Remark).ShowInList(SingleCharWidth * 10)
                .Readonly(p => p.ApprovalStatus != ApprovalStatus.Draft && p.ApprovalStatus != ApprovalStatus.Reject);

            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
        }
    }
}
