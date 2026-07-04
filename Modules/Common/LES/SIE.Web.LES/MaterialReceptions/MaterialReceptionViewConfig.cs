using SIE.Domain;
using SIE.LES.MaterialReceptions;
using SIE.LES.MaterialReceptions.ViewModels;
using SIE.MetaModel.View;
using SIE.Web.Items._Extentions_;
using SIE.Web.LES.MaterialReceptions.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.LES.MaterialReceptions
{
    /// <summary>
    /// 物料接收视图配置
    /// </summary>
    public class MaterialReceptionViewConfig : WebViewConfig<MaterialReception>
    {
        /// <summary>
        /// 列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.FormEdit();
            View.DisableEditing();
            View.UseCommands("SIE.Web.LES.MaterialReceptions.Commands.MaterialReceptionAddCommand");
            View.UseCommands(typeof(OneKeySubmitByDetailCommand).FullName, WebCommandNames.ExportXls, WebCommandNames.ExportXlsSelection, WebCommandNames.ExportXlsAll);
            using (View.OrderProperties())
            {
                View.Property(p => p.StockOrder).ShowInList(width: 100);
                View.Property(p => p.LineNo);
                View.Property(p => p.State);
                View.Property(p => p.Item).UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> dic = new Dictionary<string, string>();
                    dic.Add(nameof(e.ItemName), nameof(e.Item.Name));
                    m.DicLinkField = dic;
                }).HasLabel("物料编码").ShowInList(width: 100);
                View.Property(p => p.ItemName).ShowInList(width: 100);
                View.Property(p => p.ItemExtProp).ShowInList(width: 100);
                View.Property(p => p.LabelNo).ShowInList(width: 100);
                View.Property(p => p.LotNo).ShowInList(width: 100);
                View.Property(p => p.Qty).ShowInList(width: 100);
                View.Property(p => p.ShipQty).ShowInList(width: 100);
                View.Property(p => p.Warehouse).ShowInList(width: 100);
                View.Property(p => p.StorageLocation).ShowInList(width: 100);
                View.Property(p => p.WorkOrder).ShowInList(width: 100);
                View.Property(p => p.ResourceName).HasLabel("生产资源").ShowInList(width: 100);
                View.Property(p => p.SoNo).ShowInList(width: 100);
                View.Property(p => p.SoLineNo).ShowInList(width: 100);
                View.Property(p => p.IsManualRec).Readonly().ShowInList(width: 200);
                View.Property(p => p.Receiver).ShowInList(width: 100);
                View.Property(p => p.ReceiveTime).ShowInList(width: 100);
            }
        }

        /// <summary>
        /// 列表视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.ClearCommands();
            View.AddBehavior("SIE.Web.LES.MaterialReceptions.Behaviors.TabBehavior");
            View.AttachChildrenProperty(typeof(MaterialReceptionAddViewModel), (e) =>
            {
                return new EntityList<MaterialReceptionAddViewModel>();
            }).UseViewGroup(MaterialReceptionViewModelViewConfig.AddDetailPageView).HasLabel("按明细接收").Show(ChildShowInWhere.Detail).HasOrderNo(1);
            View.AttachChildrenProperty(typeof(MaterialReceptionAddViewModel), (e) =>
            {
                return new EntityList<MaterialReceptionAddViewModel>();
            }).UseViewGroup(MaterialReceptionViewModelViewConfig.AddOrderPageView).HasLabel("按单接收").Show(ChildShowInWhere.Detail).HasOrderNo(2);
        }
    }
}
