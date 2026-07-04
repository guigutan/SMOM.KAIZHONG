using SIE.CSM.Customers;
using SIE.CSM.Suppliers;
using SIE.Domain;
using SIE.EMS.Purchases.EquipmentAcceptances;
using SIE.Equipments.EquipModels;
using SIE.Web.EMS.Extensions;
using SIE.Web.Resources;

namespace SIE.Web.EMS.Purchases.EquipmentAcceptances
{
    /// <summary>
    /// 采购订单查询实体界面
    /// </summary>
    internal class EquipmentAcceptanceCriteriaViewConfig : WebViewConfig<EquipmentAcceptanceCriteria>
    {
        /// <summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.FactoryId).UseFactoryEditor();
            View.Property(p => p.DepartmentId).UseUserBudgetDepartmentEditor();
            View.Property(p => p.EquipModelId).UseDataSource((e, p, k) =>
            {
                var equipmentAccount = e as EquipmentAcceptanceCriteria;
                if (equipmentAccount == null)
                {
                    return new EntityList<EquipModel>();
                }

                return RT.Service.Resolve<SIE.EMS.Equipments.EquipController>()
                      .GetEquipModelsOfUserHasPermission(p, k);
            });
            View.Property(p => p.EquipAccountCode);
            View.Property(p => p.No);
            View.Property(p => p.ReceiveType);
            View.Property(p => p.SupplierId).UseDataSource((source, pagingInfo, keyword) =>
            {
                return RT.Service.Resolve<SupplierController>().GetSuppliers(pagingInfo, keyword);
            });
            View.Property(p => p.CustomerId).UseDataSource((source, pagingInfo, keyword) =>
            {
                return RT.Service.Resolve<CustomerController>().GetCustomers(pagingInfo, keyword);
            });
            View.Property(p => p.ApprovalStatus);
            View.Property(p => p.CreateDate).UseDateRangeEditor(p => p.DateRangeType = ObjectModel.DateRangeType.All);
        }
    }
}
