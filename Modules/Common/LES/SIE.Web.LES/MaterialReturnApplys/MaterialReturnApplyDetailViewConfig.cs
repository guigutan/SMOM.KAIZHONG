using SIE.LES.MaterialReturnApplys;
using SIE.MetaModel.View;
using SIE.Web.Items._Extentions_;

namespace SIE.Web.LES.MaterialReturnApplys
{
    /// <summary>
    /// 退料申请视图配置
    /// </summary>
    public class MaterialReturnApplyDetailViewConfig : WebViewConfig<MaterialReturnApplyDetail>
    {
        /// <summary>
        /// 编辑
        /// </summary>
        public const string EditViewStr = "EditViewStr";

        /// <summary>
        /// 视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(EditViewStr);
            if (ViewGroup == EditViewStr)
            {
                EditView();
            }
        }

        /// <summary>
        /// 列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.DisableEditing();
            using (View.OrderProperties())
            {
                View.Property(p => p.LineNo).Readonly().ShowInList();
                View.Property(p => p.ReDetailStatus).Readonly().ShowInList();
                View.Property(p => p.ItemCode).Readonly().ShowInList();
                View.Property(p => p.ItemName).Readonly().ShowInList();
                View.Property(p => p.UnitName).Readonly().ShowInList();
                View.Property(p => p.ReDetailQuality).Readonly().ShowInList();
                View.Property(p => p.CtrlMode).Readonly().ShowInList();
                View.Property(p => p.ItemExtPropName).Readonly().ShowInList();
                View.Property(p => p.ItemLabel).Readonly().ShowInList();
                View.Property(p => p.ReturnQty).Readonly().ShowInList();
                View.Property(p => p.OnWayQty).Readonly().ShowInList();
                View.Property(p => p.CollectQty).Readonly().ShowInList();
            }
        }

        /// <summary>
        /// 编辑视图
        /// </summary>
        private void EditView()
        {
            View.AddBehavior("SIE.Web.LES.MaterialReturnApplys.Scripts.MaterialReturnDetailBehavior");
            View.UseCommands("SIE.Web.LES.MaterialReturnApplys.Commands.SelectReDetailCommand", WebCommandNames.Delete);
            using (View.OrderProperties())
            {
                View.Property(p => p.LineNo).Readonly().Show();
                View.Property(p => p.ItemCode).Readonly().Show();
                View.Property(p => p.ItemName).Readonly().Show();
                View.Property(p => p.UnitName).Readonly().Show();
                View.Property(p => p.ReDetailQuality).HasLabel("状态").Show();
                View.Property(p => p.CtrlMode).Readonly().Show();
                View.Property(p => p.ItemExtPropName).UseItemExtPropRecordsFieldEditor(p =>
                {
                    p.IsAllRequired = false;
                    p.SourceEntityType = "MaterialReturnApplyDetail";
                    p.ItemIdField = "ItemId";
                    p.DbField = "ItemExtProp";
                }).Readonly(p => !p.EnableExtendProperty && !p.CanExtProp).Show();
                View.Property(p => p.Label).Readonly().Show();
                View.Property(p => p.Lot).Readonly().ShowInList(width: 150);
                View.Property(p => p.ReturnQty).UseSpinEditor(p => { p.MinValue = 0; p.AllowDecimals = false; }).Show();
                View.Property(p => p.LabelQty).Readonly().Show();
                View.Property(p => p.LabelNgQty).Readonly().Show();
                View.Property(p => p.AvailableQty).HasLabel("工单剩余可用数").Readonly().Show();
                View.Property(p => p.NgQty).HasLabel("工单剩余不良数").Readonly().Show();
                View.Property(p => p.LpnQty).Readonly().Show();
                View.Property(p => p.NgLpnQty).Readonly().Show();
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            }
        }
    }
}
