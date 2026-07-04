using SIE.Domain;
using SIE.EMS.SpecialEquipment.Models;
using SIE.Equipments.EquipModels;

namespace SIE.Web.EMS.SpecialEquipment.Models
{
    /// <summary>
    /// 设备型号维护视图配置
    /// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    public class EquipModelExtensionViewConfig : WebViewConfig<EquipModel>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            #region 标准产品页签

            View.AssociateChildrenProperty(EquipModelExtension.EquipModelRegularInspectionListProperty, w =>
            {
                var args = w as ChildPagingDataArgs;
                var parent = args.Parent as EquipModel;
                if (parent == null)
                {
                    return new EntityList<EquipModelRegularInspection>();
                }
                return RT.Service.Resolve<EquipModelExtensionController>().GetEquipModelRegularInspectionList(parent.Id, args.PagingInfo, args.SortInfo);
            }, ViewConfig.ListView).HasLabel("设备定检规程").Show(ChildShowInWhere.All).HasOrderNo(135);

            #endregion
        }

        /// <summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.AssociateChildrenProperty(EquipModelExtension.EquipModelRegularInspectionListProperty, w =>
            {
                var args = w as ChildPagingDataArgs;
                var parent = args.Parent as EquipModel;
                if (parent == null)
                {
                    return new EntityList<EquipModelRegularInspection>();
                }
                return RT.Service.Resolve<EquipModelExtensionController>().GetEquipModelRegularInspectionList(parent.Id, args.PagingInfo, args.SortInfo);
            }, ViewConfig.ListView).HasLabel("设备定检规程").Show(ChildShowInWhere.All).HasOrderNo(135);
        }
    }
}
