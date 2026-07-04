
using SIE.ERPInterface.Common.InventoryControl;
using SIE.ERPInterface.Smom.InventoryControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.ERPInterface.InventoryControl
{
    public class InventoryControlSettingViewConfig:WebViewConfig<InventoryControlSetting>
    {
        /// <summary>
        /// 视图配置
        /// </summary>
        protected override void ConfigDetailsView()
        {
            //View.HasDetailColumnsCount(3);
            using (View.OrderProperties())
            {
                using (View.DeclareGroup("库存维度",3,false))
                {
                    View.Property(p => p.IsItem).HasLabel("物料").Readonly();
                    View.Property(p => p.IsLot).HasLabel("批次");
                    View.Property(p => p.IsWareHouse).HasLabel("仓库");
                }
                using (View.DeclareGroup("现有量范围",3,false))
                {
                    View.Property(p => p.IsOkInv).HasLabel("合格库存").Readonly();
                    View.Property(p => p.IsNgInv).HasLabel("不合格库存");
                }
                using (View.DeclareGroup("ERP批次对照"))
                {
                    View.Property(p => p.EbsToLot).HasLabel("ERP批次号=");
                }
                using (View.DeclareGroup("ERP子库对照"))
                {
                    View.Property(p => p.EbsToWarehouse).HasLabel("仓库:ERP子库=");
                }
            }
        }
    }
}
