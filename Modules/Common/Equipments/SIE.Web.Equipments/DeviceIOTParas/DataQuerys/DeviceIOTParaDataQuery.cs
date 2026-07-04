using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Equipments.DeviceIOTParas.ViewModles;
using SIE.SMDC;
using SIE.Web.Data;
using System;

namespace SIE.Web.Equipments.DeviceIOTParas.DataQuerys
{
    /// <summary>
    /// 设备物联参数请求
    /// </summary>
    public class DeviceIOTParaDataQuery : DataQueryer
    {

        /// <summary>
        /// 根据设备型号获取MDC接口明细数据
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public EntityList<MDCDetailViewModle> GetMDCDetailByEquipModel(MDCDetailViewModleCriteria criteria)
        {
            try
            {
                var mdcDetailViewModle = RT.Service.Resolve<EquipmentSmdcController>().GetMDCDetailByEquipModel(criteria);
                return mdcDetailViewModle;
            }
            catch (Exception)
            {
                throw new ValidationException("接口异常：{0}".L10nFormat("从MDC接口中查找明细数据失败!"));
            }
        }

        /// <summary>
        /// 根据资产编码获取MDC的targs 
        /// </summary>
        /// <param name="assetsCode"></param>
        /// <returns></returns>
        public String GetEquipmentTags(string assetsCode)
        {
            try
            {
                var jsonResult = RT.Service.Resolve<EquipmentSmdcController>().GetEquipmentTags(assetsCode);
                return jsonResult;
            }
            catch (Exception)
            {
                throw new ValidationException("接口异常：{0}".L10nFormat("在MDC接口中获取设备Tags失败!"));
            }
        }
    }
}
