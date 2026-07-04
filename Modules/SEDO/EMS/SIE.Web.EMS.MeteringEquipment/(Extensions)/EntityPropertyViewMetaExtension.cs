using SIE.Common.Catalogs;
using SIE.Common.Sort;
using SIE.Domain;
using SIE.EMS.MeteringEquipment.Calibrations;
using SIE.EMS.MeteringEquipment.MeteringEquipmentAccounts;
using SIE.MetaModel.View;
using SIE.Web.ClientMetaModel;
using System;

namespace SIE.Web.EMS.MeteringEquipment
{
    /// <summary>
    /// 扩展
    /// </summary>
    public static class EntityPropertyViewMetaExtension
    {
    
        /// <summary>
        /// 计量设备台账ID
        /// </summary>
        private static string MeteringEquipmentAccountId = "MeteringEquipmentAccountId";

        /// <summary>
        /// 是否降级
        /// </summary>
        private static string IsDowngrade = "IsDowngrade";


        /// <summary>
        /// 获取精度级别类型下拉编辑器
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="meta">属性视图元数据</param>
        /// <param name="action">委托</param>
        /// <param name="meteringEquipmentAccountId"></param>
        ///  <param name="isDowngrade"></param>
        /// <returns>泛型属性视图元数据</returns>
        public static WebEntityPropertyViewMeta<T> UsePrecisionClassLevelEditor<T>(this WebEntityPropertyViewMeta<T> meta,
            Action<PagingLookUpBaseConfig> action = null, string meteringEquipmentAccountId = "", string isDowngrade = "")
        {
            meta.UseDataSource((source, pagingInfo, keyword) =>
            {
                EntityList<Catalog> CatalogList = new EntityList<Catalog>();

                if (!meteringEquipmentAccountId.IsNullOrEmpty())
                {
                    MeteringEquipmentAccountId = meteringEquipmentAccountId;
                }
                if (!isDowngrade.IsNullOrEmpty())
                {
                    IsDowngrade = isDowngrade;
                }
   
                var meteringEquipmentAccountIdProperty = source.PropertyContainer.FindProperty(MeteringEquipmentAccountId);

                var isDowngradeProperty = source.PropertyContainer.FindProperty(IsDowngrade);

                if (meteringEquipmentAccountIdProperty != null && isDowngradeProperty!=null)
                {
                    var meteringEquipmentAccountIdObject = source.GetProperty(meteringEquipmentAccountIdProperty);
                    var isDowngradeObject = source.GetProperty(isDowngradeProperty);

                    if (meteringEquipmentAccountIdObject != null && isDowngradeObject!=null)
                    {
                        var meteringEquipmentAccountId =Convert.ToDouble(meteringEquipmentAccountIdObject);

                        var isDowngrade= Convert.ToBoolean(isDowngradeObject);

                        var EquipAccount=RT.Service.Resolve<MeteringEquipmentAccountController>().GetMeteringEquipmentAccountById(meteringEquipmentAccountId);

                        Catalog aa = RT.Service.Resolve<CatalogController>().GetCatalog(CalibrationEquipment.PrecisionClassType, EquipAccount.PrecisionClass);

                        var index= aa.GetProperty(SortExtension.INDEX_Property);

                        if (isDowngrade)
                        {
                            CatalogList = RT.Service.Resolve<CalibrationController>().GetCatalogList(CalibrationEquipment.PrecisionClassType, index);
                        }
                        else {
                            CatalogList = RT.Service.Resolve<CatalogController>().GetCatalogList(CalibrationEquipment.PrecisionClassType);
                        }
                    }
                }
                return CatalogList;
            }).UsePagingLookUpEditor(action);

            return meta;
        }
    }
}
