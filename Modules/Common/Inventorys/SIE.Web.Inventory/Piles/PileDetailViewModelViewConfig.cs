using SIE.Inventory.Piles;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.Inventory.Piles
{
    /// <summary>
    /// 垛物料明细视图
    /// </summary>
    public class PileDetailViewModelViewConfig : WebViewConfig<PileDetailViewModel>
    {
        /// <summary>
        /// 列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.DisableEditing();
            View.WithoutPaging();
            View.Property(p => p.Sn).ShowInList(150);
            View.Property(p => p.ItemCode);
            View.Property(p => p.ItemName);
            View.Property(p => p.SpecificationModel);
            View.Property(p => p.Qty);
            View.Property(p => p.ItemUnitName);
            View.Property(p => p.ItemExtPropName);
            View.Property(p => p.LotCode);
            View.Property(p => p.Warehouse);
            View.Property(p => p.StorageArea);
            View.Property(p => p.StorageLocation);
            View.Property(p => p.OnhandState);
            View.Property(p => p.ItemState);
            View.Property(p => p.StorerCode);
            View.Property(p => p.ProjectNo);
            View.Property(p => p.TaskNo);
        }
    }
}
