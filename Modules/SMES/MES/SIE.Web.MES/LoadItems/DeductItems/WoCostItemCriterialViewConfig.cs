using SIE.Domain;
using SIE.MES.LoadItems;
using SIE.Resources.Enterprises;
using SIE.Resources.WipResources;
using SIE.Web.Resources;
using System;

namespace SIE.Web.MES.LoadItems.DeductItems
{
    /// <summary>
    /// 扣料查询实体视图
    /// </summary>
    public class WoCostItemCriterialViewConfig : WebViewConfig<WoCostItemCriterial>
    {
        /// <summary>
        /// 配置
        /// </summary>
        protected override void ConfigView()
        {
        }

        /// <summary>
        /// 查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.CostNo).ShowInList(width: 150);
                View.Property(p => p.RecordType).ShowInList(width: 150);
                View.Property(p => p.State).ShowInList(width: 150);
                View.Property(p => p.WoNo).ShowInList(width: 150);
                View.Property(p => p.ProductName).ShowInList(width: 150);
                View.Property(p => p.CostItemCode).ShowInList(width: 150);
                View.Property(p => p.CostItemName).ShowInList(width: 150);
                View.Property(p => p.Label).ShowInList(width: 150);
                View.Property(p => p.Lot).ShowInList(width: 150);
                View.Property(p => p.BarCode).ShowInList(width: 150);
                View.Property(p => p.BatchNo).ShowInList(width: 150);
                View.Property(p => p.Factory).UseDataSource((s, p, k) =>
                {
                    var source = s as WoCostItemCriterial;
                    if (source == null)
                    {
                        return new EntityList<Enterprise>();
                    }
                    else
                    {
                        return RT.Service.Resolve<WoCostItemController>().QueryEnterprises(source, p, k);
                    }
                }).ShowInList(width: 150);
                View.Property(p => p.WipResource).UseDataSource((s, p, k) =>
                {
                    var source = s as WoCostItemCriterial;
                    if (source == null)
                    {
                        return new EntityList<WipResource>();
                    }
                    else
                    {
                        return RT.Service.Resolve<WoCostItemController>().QueryWipResources(source, p, k);
                    }
                }).UseListSetting(p => p.HelpInfo = "显示启用状态不失效且企业类型为企业模型的生产资源".L10N()).ShowInList(width: 150);
                View.Property(p => p.Submiter).ShowInList(width: 150);
                View.Property(p => p.SubmitTime).UseDateRangeEditor(p => p.DateRangeType = ObjectModel.DateRangeType.All).ShowInList(width: 150);
            }
        }
    }
}
