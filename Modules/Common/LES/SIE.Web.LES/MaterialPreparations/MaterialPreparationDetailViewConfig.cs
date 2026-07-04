using SIE.LES.MaterialPreparations;
using SIE.MetaModel.View;
using SIE.Web.Items._Extentions_;
using SIE.Web.LES.MaterialPreparations.Commands;
using System;

namespace SIE.Web.LES.MaterialPreparations
{
    /// <summary>
    /// 备料需求单明细视图配置
    /// </summary>
    public class MaterialPreparationDetailViewConfig : WebViewConfig<MaterialPreparationDetail>
    {
        /// <summary>
        /// 工单备料
        /// </summary>
        public const string WorkOrderModeStr = "WorkOrderMode";

        /// <summary>
        /// 车间备料模式
        /// </summary>
        public const string WorkShopModeStr = "WorkShopMode";


        /// <summary>
        /// 视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(WorkOrderModeStr, WorkShopModeStr);
            if (ViewGroup == WorkShopModeStr)
            {
                WorkShopMode();
            }
            else if (ViewGroup == WorkOrderModeStr)
            {
                WorkOrderMode();
            }
        }

        /// <summary>
        /// 列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.DisableEditing();
            View.UseCommands(typeof(WithDrawPreDetailCommand).FullName);
            using (View.OrderProperties())
            {
                View.Property(p => p.PreDetailStatus).ShowInList();
                View.Property(p => p.LineNo).ShowInList();
                View.Property(p => p.Item).ShowInList();
                View.Property(p => p.ItemName).ShowInList();
                View.Property(p => p.UnitName).ShowInList();
                View.Property(p => p.ItemExtPropName).HasLabel("物料扩展属性").ShowInList();
                View.Property(p => p.Qty).ShowInList();
                View.Property(p => p.ReceiveQty).ShowInList();
                View.Property(p => p.RefuseQty).ShowInList();
                View.Property(p => p.ToReceiveQty).ShowInList();
                View.Property(p => p.ShippingQty).ShowInList();
                View.Property(p => p.CancelQty).ShowInList();
                View.Property(p => p.ShippingDetailNo).ShowInList();
            }
        }

        /// <summary>
        /// 车间模式
        /// </summary>
        private void WorkShopMode()
        {
            View.AssignAuthorize(typeof(MaterialPreparation));
            View.UseCommands("SIE.Web.LES.MaterialPreparations.Commands.SelectItemCommand", WebCommandNames.Delete);
            using (View.OrderProperties())
            {
                View.Property(p => p.LineNo).Readonly().ShowInList();
                View.Property(p => p.Item).Readonly().ShowInList();
                View.Property(p => p.ItemName).Readonly().ShowInList();
                View.Property(p => p.ItemConsumeMode).Readonly().ShowInList();
                View.Property(p => p.UnitName).Readonly().ShowInList();
                View.Property(p => p.ItemExtPropName).UseItemExtPropRecordsFieldEditor(p =>
                {
                    p.IsAllRequired = false;
                    p.SourceEntityType = "MaterialPreparationDetail";
                    p.ItemIdField = "ItemId";
                    p.DbField = "ItemExtProp";
                }).Readonly(p => !p.EnableExtendProperty).ShowInList();
                View.Property(p => p.Qty).UseItemUnitEditor(p => p.MinValue = 0).ShowInList();
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            }
        }

        /// <summary>
        /// 工单模式
        /// </summary>
        private void WorkOrderMode()
        {
            View.AssignAuthorize(typeof(MaterialPreparation));
            View.AddBehavior("SIE.Web.LES.MaterialPreparations.Scripts.WorkOrderPrepareDetailBehavior");
            View.UseCommands("SIE.Web.LES.MaterialPreparations.Commands.SelectPrepareDetailCommand", WebCommandNames.Delete, "SIE.Web.LES.MaterialPreparations.Commands.SyncAllPrepareQtyCommand", "SIE.Web.LES.MaterialPreparations.Commands.ClearAllPrepareQtyCommand");
            using (View.OrderProperties())
            {
                View.Property(p => p.LineNo).Readonly().ShowInList();
                View.Property(p => p.Item).Readonly().ShowInList();
                View.Property(p => p.ItemName).Readonly().ShowInList();
                View.Property(p => p.UnitName).Readonly().ShowInList();
                View.Property(p => p.ItemExtPropName).Readonly().ShowInList();
                View.Property(p => p.ItemConsumeMode).Readonly().ShowInList();
                View.Property(p => p.BomNeedQty).Readonly().ShowInList();
                View.Property(p => p.CanPrepareQty).UseListSetting(p => p.HelpInfo = "需求量-已建单备料数+取消数+挪出数-挪入数+退料数").Readonly().ShowInList();
                View.Property(p => p.Qty).UseItemUnitEditor(p => p.MinValue = 0).ShowInList();
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            }
        }
    }
}
