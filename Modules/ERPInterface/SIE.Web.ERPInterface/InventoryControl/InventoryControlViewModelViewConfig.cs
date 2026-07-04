using Newtonsoft.Json;
using SIE.Domain;
using SIE.ERPInterface.Common.InventoryControl;
using SIE.ERPInterface.Smom.InventoryControl;
using SIE.MetaModel.View;
using SIE.Web.ERPInterface.InventoryControl.Commands;
using SIE.WMS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.ERPInterface.InventoryControl
{
    /// <summary>
    /// 库存对照表视图
    /// </summary>
    public class InventoryControlViewModelViewConfig : WebViewConfig<InventoryControlViewModel>
    {
        /// <summary>
        /// 库存对照表
        /// </summary>
        protected override void ConfigListView()
        {
            View.AddBehavior("SIE.Web.ERPInterface.InventoryControl.Scripts.InventoryControlBehavior");
            View.ClearCommands();
            View.UseCommand(typeof(InventoryControlSettingCommand).FullName);
            View.UseCommands(WebCommandNames.ExportXls, WebCommandNames.ExportXlsSelection);
            View.WithoutPaging();
            using (View.OrderProperties())
            {
                View.Property(p => p.LineNo).Show().Readonly().DisableSort();
                View.Property(p => p.ItemCode).Show().Readonly().DisableSort();
                View.Property(p => p.ItemName).Show().Readonly().DisableSort();
                View.Property(p => p.ErpLotCode).Show().Readonly().DisableSort();
                View.Property(p => p.Qty).Show().Readonly().UseListSetting(f => f.HelpInfo = "不含暂收库存").DisableSort();
                View.Property(p => p.ErpQty).Show().Readonly().DisableSort();
                View.Property(p => p.DifferenceQty).Show().Readonly().DisableSort();
                View.Property(p => p.TemporaryQty).Show().Readonly().UseListSetting(f => f.HelpInfo = "ASN未上架库存").DisableSort();
                View.Property(p => p.WareHouseCode).Show().Readonly().DisableSort();
                View.Property(p => p.ErpWareHouseCode).Show().Readonly().DisableSort();
                View.Property(p => p.UnitCode).Show().Readonly().DisableSort();
                View.Property(p => p.SpecificationModel).Show().Readonly().DisableSort();
                //View.AttachChildrenProperty(typeof(InventoryControlDetailViewModel), (c) =>
                //{
                //    //var args = c as ChildPagingDataArgs;
                //    EntityList<InventoryControlDetailViewModel> packingLabelList = new EntityList<InventoryControlDetailViewModel>();
                //    return packingLabelList;
                //}).HasLabel("仓库明细");
                View.AttachChildrenProperty(typeof(InventoryControlDetailViewModel), o =>
                {
                    
                    var result = new EntityList<InventoryControlDetailViewModel>();
                    return result;
                }, InventoryControlDetailViewModelViewConfig.ListView).HasLabel("仓库明细");
            }
        }
    }
}
