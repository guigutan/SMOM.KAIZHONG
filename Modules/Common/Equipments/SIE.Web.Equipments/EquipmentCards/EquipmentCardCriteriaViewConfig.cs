using SIE.CSM.Suppliers;
using SIE.Equipments.EquipmentCards;
using SIE.Equipments.EquipModels;
using SIE.Web.Resources;

namespace SIE.Web.Equipments.EquipmentCards
{
    /// <summary>
    /// 设备立卡查询视图
    /// </summary>
    internal class EquipmentCardCriteriaViewConfig : WebViewConfig<EquipmentCardCriteria>
    {
        /// <summary>
        /// 主体
        /// </summary>
        protected override void ConfigView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.FactoryId).UseFactoryEditor().Show(ShowInWhere.All);
                View.Property(p => p.Code).Show(ShowInWhere.All);
                View.Property(p => p.EquipmentCardSource).Show(ShowInWhere.All);
                View.Property(p => p.EquipModelId).UseDataSource((source, pagingInfo, keyword) =>
                {
                    return RT.Service.Resolve<EquipModelController>().GetEquipModels(pagingInfo, keyword);
                }).HasLabel("型号编码").Show(ShowInWhere.All);
                View.Property(p => p.ApprovalStatus).Show(ShowInWhere.All);
                View.Property(p => p.SupplierId).UseDataSource((source, pagingInfo, keyword) =>
                {
                    return RT.Service.Resolve<SupplierController>().GetSuppliers(pagingInfo, keyword);
                }).HasLabel("供应商").Show(ShowInWhere.All);
                View.Property(p => p.PurchaseOrderNo).Show(ShowInWhere.All);
                View.Property(p => p.CreateDate).UseDateRangeEditor(p =>
                {
                    p.DateRangeType = ObjectModel.DateRangeType.All;
                }).Show(ShowInWhere.All);
            }
        }
    }
}