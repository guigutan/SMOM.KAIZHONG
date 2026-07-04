using SIE.EMS.Purchases.EquipmentSetups.ViewModels;
using SIE.Resources.Enterprises;
using System.Linq;

namespace SIE.Web.EMS.Purchases.EquipmentSetups.ViewModels
{
    /// <summary>
    /// 选择设备台账界面
    /// </summary>
    internal class SelEquipDetailViewModelViewConfig : WebViewConfig<SelEquipDetailViewModel>
    {
        /// <summary>
        /// 列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.WithoutPaging();
            View.Property(p => p.Code).Readonly();
            View.Property(p => p.Name).Readonly();
            View.Property(p => p.Alias).Readonly();
        }
    }

    /// <summary>
    /// 选择设备台账查询界面
    /// </summary>
    internal class SelEquipDetailCriteriaViewModelViewConfig : WebViewConfig<SelEquipDetailCriteriaViewModel>
    {
        /// <summary>
        /// 查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Name);
            View.Property(p => p.ModelCode);
            View.Property(p => p.ModelName);
            View.Property(p => p.WorkShopId).UseDataSource((e, c, r) =>
            {
                var workShopList = RT.Service.Resolve<EnterpriseController>().GetResourceWorkShops(c, r);
                workShopList.ForEach(enterprise => { enterprise.TreePId = null; });
                return workShopList;
            });
            View.Property(p => p.ResourceId).UseDataSource((e, c, r) =>
            {
                var resourcesList = RT.Service.Resolve<EnterpriseController>().GetLines(c, r, null);
                resourcesList.ForEach(p => p.TreePId = null);
                return resourcesList;
            });
            View.Property(p => p.ProcessId);
            View.Property(p => p.PurchaseOrder);
        }
    }
}
