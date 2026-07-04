using SIE.Domain;
using SIE.EMS.MeteringEquipment.EquipModelExtensions;
using SIE.Equipments.EquipModels;
using System.Collections.Generic;

namespace SIE.Web.EMS.MeteringEquipment.EquipModelExtensions
{
    /// <summary>
    /// 设备型号扩展
    /// </summary>
    public  class EquipModelExtensionViewConfig : WebViewConfig<EquipModel>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            #region 计量校验规程页签

            View.AssociateChildrenProperty(EquipModelExtension.EquipModelCalibrationListProperty, w =>
            {
                var args = w as ChildPagingDataArgs;
                var parent = args.Parent as EquipModel;
                if (parent == null)
                {
                    return new EntityList<EquipModelCalibration>();
                }
                return RT.Service.Resolve<EquipModelExtensionController>().GetEquipModelCalibrationList(parent.Id, args.PagingInfo, args.SortInfo);
            }, ViewConfig.ListView).HasLabel("计量校验规程").Show(ChildShowInWhere.All).HasOrderNo(136);

            #endregion
        }

        /// <summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.AssociateChildrenProperty(EquipModelExtension.EquipModelCalibrationListProperty, w =>
            {
                var args = w as ChildPagingDataArgs;
                var parent = args.Parent as EquipModel;
                if (parent == null)
                {
                    return new EntityList<EquipModelCalibration>();
                }
                return RT.Service.Resolve<EquipModelExtensionController>().GetEquipModelCalibrationList(parent.Id, args.PagingInfo, args.SortInfo);
            }, ViewConfig.ListView).HasLabel("计量校验规程").Show(ChildShowInWhere.All).HasOrderNo(136);
        }
    }
}
