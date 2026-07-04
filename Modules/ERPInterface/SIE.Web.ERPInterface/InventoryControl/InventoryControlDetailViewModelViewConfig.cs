using SIE.Domain;
using SIE.ERPInterface.Common.InventoryControl;
using SIE.ERPInterface.Smom.InventoryControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.ERPInterface.InventoryControl
{
    /// <summary>
    /// 库存对照表仓库明细子表
    /// </summary>
    public class InventoryControlDetailViewModelViewConfig:WebViewConfig<InventoryControlDetailViewModel>
    {
        protected override void ConfigListView()
        {
            View.ClearCommands();
            //View.UseCommand(typeof(InventoryControlSettingCommand).FullName);
            View.UseChildrenAsHorizontal();
            using (View.OrderProperties())
            {
                View.Property(p => p.LineNo).Show().Readonly().DisableSort();
                View.Property(p => p.WareHouseCode).Show().HasLabel("仓库").Readonly().DisableSort();
                View.Property(p => p.Qty).Show().Readonly().DisableSort();
                View.Property(p => p.ErpWareHouseCode).Show().Readonly().DisableSort();
                View.Property(p => p.ErpQty).Show().Readonly().DisableSort();
                View.Property(p => p.DifferenceQty).Show().Readonly().DisableSort();
                View.AttachChildrenProperty(typeof(InventoryControlErpDetaiViewModel), o =>
                {

                    var result = new EntityList<InventoryControlErpDetaiViewModel>();
                    return result;
                }, InventoryControlDetailViewModelViewConfig.ListView).HasLabel("子库明细");
            }
        }
    }
}
