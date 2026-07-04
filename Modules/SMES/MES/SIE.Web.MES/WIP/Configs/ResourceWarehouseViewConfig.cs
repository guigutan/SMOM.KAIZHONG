using SIE.MES.Configs;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.MES.WIP.Configs
{
    public class ResourceWarehouseViewConfig:WebViewConfig<ResourceWarehouse>
    {
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.Code);
            View.Property(p=>p.Name);
        }
        protected override void ConfigListView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Name);
        }
    }
}
