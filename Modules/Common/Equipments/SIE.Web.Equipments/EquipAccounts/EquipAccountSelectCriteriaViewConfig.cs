using SIE.Domain;
using SIE.Equipments.EquipAccounts;
using SIE.Equipments.EquipTypes;
using SIE.MetaModel.View;
using SIE.Resources.Enterprises;
using SIE.Web.Common;
using System.Linq;

namespace SIE.Web.Equipments.EquipAccounts
{
    /// <summary>
    /// 设备台账查询实体视图配置
    /// </summary>
    internal class EquipAccountSelectCriteriaViewConfig : WebViewConfig<EquipAccountSelectCriteria>
    {
        /// <summary>
        /// 默认查询视图
        /// </summary>
        protected override void ConfigView()
        {
            View.UseDefaultCommands();
            View.RemoveCommands(WebCommandNames.ClearQuery);
            using (View.OrderProperties())
            {
                View.Property(p => p.Code).Show(ShowInWhere.All);
                View.Property(p => p.Name).Show(ShowInWhere.All);
                View.Property(p => p.ModelCode).Show(ShowInWhere.All);

                View.Property(p => p.WorkShop).UseDataSource((e, c, r) =>
                {                    
                    var workShopList = RT.Service.Resolve<EnterpriseController>().GetResourceWorkShops(c, r);
                    workShopList.ForEach(enterprise => { enterprise.TreePId = null; });
                    return workShopList;
                }).Show(ShowInWhere.All).HasLabel("车间");

                View.Property(p => p.Resource)
                    .UseDataSource((e, c, r) =>
                    {
                        var resourcesList = RT.Service.Resolve<EnterpriseController>()
                            .GetLines(c, r, null);

                        resourcesList.ForEach(p => p.TreePId = null);

                        return resourcesList;
                    }).UsePagingLookUpEditor()
                    .Show(ShowInWhere.All).HasLabel("产线");

                View.Property(p => p.Process).Show(ShowInWhere.All).HasLabel("工序");

                View.Property(p => p.TypeCategory)
                    .UseCatalogEditor(c => { c.CatalogType = EquipType.EquipTypeCatalogType;c.CatalogReloadData = true; })
                    .Show(ShowInWhere.All).HasLabel("设备类别");

                View.Property(p => p.State).Show(ShowInWhere.All);
                View.Property(p => p.AccountUseState).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).UseDateRangeEditor(p =>
                {
                    p.DateRangeType = ObjectModel.DateRangeType.All;
                }).Show(ShowInWhere.All);
            }
        }
    }
}
