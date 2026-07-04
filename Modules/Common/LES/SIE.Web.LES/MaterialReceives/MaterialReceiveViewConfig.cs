using SIE.Domain;
using SIE.LES.MaterialReceives;
using System;
using SIE.Web.LES.MaterialReceives.Commands;
using SIE.MetaModel.View;

namespace SIE.Web.LES.MaterialReceives
{
    /// <summary>
    /// 物料接收视图配置
    /// </summary>
    public class MaterialReceiveViewConfig : WebViewConfig<MaterialReceive>
    {
        /// <summary>
        /// 列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseClientOrder();
            View.DisableEditing();
            View.UseCommands(typeof(ReceiveCommand).FullName, typeof(RejectCommand).FullName, WebCommandNames.ExportXls);
            using (View.OrderProperties())
            {
                View.Property(p => p.SoNo).ShowInList(width: 150);
                View.Property(p => p.MaterialPreparationNo).ShowInList(width: 150);
                View.Property(p => p.ShippingWarehouseName).ShowInList(width: 100);
                View.Property(p => p.ReceiveWarehouseName).ShowInList(width: 100);
                View.Property(p => p.WoNo).ShowInList(width: 150);
                View.Property(p => p.WorkShopName);
                View.Property(p => p.ResourceName);
                View.Property(p => p.DeliveryDate).ShowInList(width: 150);
                View.Property(p => p.TransactionName).ShowInList();
                View.Property(p => p.State).ShowInList();
                View.Property(p => p.CreateByName).ShowInList();
                View.Property(p => p.CreateDate).ShowInList(width: 150);

                View.ChildrenProperty(p => p.DetailList).Show(ChildShowInWhere.Hide);
                View.ChildrenProperty(p => p.LabelList).Show(ChildShowInWhere.Hide);

                View.AttachChildrenProperty(typeof(MaterialReceiveDetail), (w) =>
                {
                    var args = w as ChildPagingDataWithParentEntityArgs;
                    var bill = w.Parent as MaterialReceive;// args.ParentEntity.ToJsonObject<MaterialReceiveViewModel>();
                    if (bill == null)
                    {
                        return new EntityList<MaterialReceiveDetail>();
                    }

                    return RT.Service.Resolve<MaterialReceiveController>().GetMaterialReceiveDetails(bill.Id, ReceiveState.TobeReceived);
                }).HasLabel("接收明细").OrderNo = 1;
                View.AttachChildrenProperty(typeof(MaterialReceiveLabel), (w) =>
                {
                    var args = w as ChildPagingDataWithParentEntityArgs;
                    var bill = w.Parent as MaterialReceive; //args.ParentEntity.ToJsonObject<MaterialReceiveViewModel>();
                    if (bill == null)
                    {
                        return new EntityList<MaterialReceiveLabel>();
                    }

                    return RT.Service.Resolve<MaterialReceiveController>().GetMaterialReceiveLabels(bill.Id, ReceiveState.TobeReceived);
                }).HasLabel("标签列表").OrderNo = 2;
            }
        }

    }
}
