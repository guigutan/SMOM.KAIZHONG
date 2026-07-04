using SIE.Domain;
using SIE.Resources.Enterprises;
using SIE.Wpf.Command;
using SIE.Wpf.Resources.Enterprises.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Wpf.Resources.Enterprises
{
    /// <summary>
    /// 生产单元视图的配置类
    /// </summary>
    public class ProductionCellViewModelViewConfig : WPFViewConfig<ProductionCellViewModel>
    {
        /// <summary>
        /// 生产单元视图的列表视图配置
        /// </summary>
        protected override void ConfigListView()
        {
            View.InlineEdit();
            View.UseDefaultBehaviors();
            View.UseCommands(typeof(ListAddCommand), WPFCommandNames.ListEdit, WPFCommandNames.ListDelete, typeof(SaveProductCellCommand));

            View.Property(p => p.Code).Readonly(false);
            View.Property(p => p.Name).Readonly(false);
            View.Property(p => p.Shop).UseResourceWorkShopEditor().Readonly(DataEntityStatus.IsEditStatusProperty); //UseShopEditor
            View.Property(p => p.Shop.Name).HasLabel("车间名称").Readonly(true);
            View.AttachDetailChildrenProperty(typeof(ProductionCellExt), e =>
            {
                var arg = e as ChildDataArgs;
                var pcVM = arg.Parent as ProductionCellViewModel;
                if (pcVM != null)
                {
                    var pdcellExt = RT.Service.Resolve<EnterpriseController>().GetProdCellExt(pcVM.ProductionCellId);
                    if (pdcellExt == null)
                    {
                        pdcellExt = new ProductionCellExt();
                    }

                    pcVM.ProductionCellExt = pdcellExt;
                    return pdcellExt;
                }

                return new ProductionCellExt() { ProductionCell = pcVM.ProductionCell };
            }).HasLabel("区域设置").Show(ChildShowInWhere.List).OrderNo = 8;
        }

        /*protected override void ConfigQueryView()
        {
            View.Property(p => p.ProductionCell.Code).HasLabel("产线编码");
            ////View.Property(p => p.ProductionCell.Name).HasLabel("名称");
            View.Property(p => p.Shop).UseShopEditor().HasLabel("车间编码");
        }*/

        /*protected override void ConfigSelectionView()
        {
            //base.ConfigSelectionView();
            View.Property(p => p.Shop.Code).HasLabel("车间编码").Readonly(DataEntityStatus.IsEditStatusProperty);
            View.Property(p => p.Shop.Name).HasLabel("车间名称").Readonly(true);
        }*/
    }
}
