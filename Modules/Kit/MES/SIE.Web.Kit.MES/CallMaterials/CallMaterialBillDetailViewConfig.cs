using SIE.Kit.MES.CallMaterials;

namespace SIE.Web.Kit.MES.CallMaterials
{
    /// <summary>
    /// 叫料单明细视图配置
    /// </summary>
    public class CallMaterialBillDetailViewConfig : WebViewConfig<CallMaterialBillDetail>
    {
        /// <summary>
        /// 工位物料视图
        /// </summary>
        public const string StationCallMateriaView = "StationCallMateriaView";

        /// <summary>
        /// 视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(CallMaterialWorkOrder));
            View.DeclareExtendViewGroup(StationCallMateriaView);
            if (ViewGroup == StationCallMateriaView)
                ConfigStationCallMateriaView();
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.RemoveCommands();
            using (View.OrderProperties())
            {
                View.Property(p => p.RowNo).Readonly();
                View.Property(p => p.ItemCode).Readonly().HasLabel("物料编码");
                View.Property(p => p.ItemName).Readonly().HasLabel("物料名称");
                View.Property(p => p.CalledQty).Readonly();
                View.Property(p => p.ActualQty).Readonly();
                View.Property(p => p.ReceiveByName).Readonly().HasLabel("接收人");
                View.Property(p => p.ReceiveDate).Readonly();
                View.Property(p => p.IsLoaded).Readonly();
                View.Property(p => p.CreateBy).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateBy).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            }
        }

        /// <summary>
        /// 工位叫料视图
        /// </summary>
        void ConfigStationCallMateriaView()
        {
            View.ClearCommands();
            View.Property(p => p.ItemCode).HasLabel("物料编码").Show();
            View.Property(p => p.ItemName).HasLabel("物料名称").Show();
            View.Property(p => p.CalledQty).Show();
            View.Property(p => p.BillNo).HasLabel("叫料单").Show();
            View.Property(p => p.BillPriority).UseEnumEditor().HasLabel("优先级").Show();
            View.Property(p => p.BillStatus).UseEnumEditor().HasLabel("状态").Show();
            View.Property(p => p.CreateDate).HasLabel("叫料时间").Show();
            View.Property(p => p.IsLoaded).Show();
            View.Property(p => p.CreateBy).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateBy).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
        }
    }
}
