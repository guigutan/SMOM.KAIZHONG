using SIE.Andon.Andons;
using SIE.Domain;
using SIE.Items;
using SIE.Warehouses;
using SIE.Wpf.Andon.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Wpf.Andon
{
    /// <summary>
    /// 安灯叫料生成备料单
    /// </summary>
    internal class AndonManageCallMaterialViewConfig : WPFViewConfig<AndonManageCallMaterial>
    {
        /// <summary>
        /// 列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.ClearCommands();
            View.UseCommands(typeof(AndonManageMaterialCommand), WPFCommandNames.ListDelete);
            View.Property(p => p.Item).UseDataSource((s, p, k) =>
            {
                var source = s as AndonManageCallMaterial;
                if (source != null)
                {
                    return RT.Service.Resolve<AndonManageController>().ChoseItems(source, p, k);
                }
                else
                {
                    return new EntityList<Item>();
                }
            }).ShowInList().HasLabel("物料编码");
            View.Property(p => p.ItemName).ShowInList().Readonly();
            View.Property(p => p.ConsumeModeView).ShowInList().Readonly();
            View.Property(p => p.Qty).UseSpinEditor(p => { p.MinValue = 0; }).ShowInList();
            View.Property(p => p.TimeNeed).ShowInList();
            View.Property(p => p.WareHouse).ShowInList().Readonly(p => !p.Hand);
            View.Property(p => p.StorageLocation).UseDataSource((s, p, k) =>
            {
                var source = s as AndonManageCallMaterial;
                if (source != null)
                {
                    return RT.Service.Resolve<AndonManageController>().GetStorageLocations(source, p, k);
                }
                else
                {
                    return new EntityList<StorageLocation>();
                }
            }).ShowInList().Readonly(p => !p.Hand);
            View.Property(p => p.No).ShowInList();
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
        }
    }

    internal class AndonManageCallMaterialWarehouseViewConfig : WPFViewConfig<Warehouse>
    {
        /// <summary>
        /// 列表视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.Code).ShowInList();
            View.Property(p => p.Name).ShowInList();

        }
    }

    internal class AndonManageCallMaterialStorageLocationViewConfig : WPFViewConfig<StorageLocation>
    {
        /// <summary>
        /// 列表视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.Code).ShowInList();
            View.Property(p => p.Name).ShowInList();

        }
    }
}
