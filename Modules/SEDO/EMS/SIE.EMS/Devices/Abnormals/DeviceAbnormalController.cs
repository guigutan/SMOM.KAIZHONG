using SIE.Core.ApiModels;
using SIE.Domain;
using SIE.EMS.Equipments.Accounts;
using SIE.EMS.EquipRepairs.Enums;
using SIE.EMS.EquipRepairs;
using SIE.Equipments.EquipTypes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace SIE.EMS.Devices.Abnormals
{
    /// <summary>
    ///设备异常维护控制器
    /// </summary>
    public partial class DeviceAbnormalController : DomainController
    {
        /// <summary>
        /// 获取设备异常维护列表
        /// </summary>
        /// <param name="criteria">设备异常维护查询体</param>
        /// <returns>设备异常维护列表</returns>
        public virtual EntityList<DeviceAbnormal> GetDeviceAbnormalsByCriteria(DeviceAbnormalCriteria criteria)
        {
            var query = Query<DeviceAbnormal>();
            if (criteria.Code.IsNotEmpty())
                query.Where(p => p.Code.Contains(criteria.Code));
            if (criteria.Description.IsNotEmpty())
                query.Where(p => p.Description.Contains(criteria.Description));
            if (criteria.EquipType != null)
                query.Where(p => p.EquipType.TypeCode == criteria.EquipType.TypeCode);
            if (criteria.AbnormalType.HasValue)
                query.Where(p => p.AbnormalType == criteria.AbnormalType);
            return query.OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 查询对应设备类型的异常信息
        /// </summary>
        /// <param name="pagingInfo"></param>
        /// <param name="keyWord"></param>
        /// <param name="abnormalType"></param>
        /// <param name="equipTypeId"></param>
        /// <param name="repairType">设备类型</param>
        /// <returns></returns>
        private EntityList<DeviceAbnormal> QueryTypeRepairBill(PagingInfo pagingInfo, string keyWord, AbnormalType? abnormalType, double? equipTypeId, int? repairType)
        {
            var queryer = DB.Query<DeviceAbnormal>().WhereIf(abnormalType != null, p => p.AbnormalType == abnormalType);
            if (repairType == 0)
            {
                queryer.Where(p => p.EquipTypeId == equipTypeId);
            }
            if (keyWord.IsNotEmpty())
            {
                queryer.Where(p => p.Code.Contains(keyWord) || p.Description.Contains(keyWord));
            }
            return queryer.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 查询设备类型为空的异常信息
        /// </summary>
        /// <param name="pagingInfo"></param>
        /// <param name="keyWord"></param>
        /// <param name="abnormalType"></param>
        /// <param name="repairType">报修类型（0 设备   1 备件）</param>
        /// <returns></returns>
        private EntityList<DeviceAbnormal> QueryNoTypeRepairBill(PagingInfo pagingInfo, string keyWord, AbnormalType? abnormalType, int? repairType)
        {
            var queryer = DB.Query<DeviceAbnormal>().Where(p => p.AbnormalType == abnormalType);
            if (repairType == 0)
            {
                queryer.Where(p => p.EquipTypeId == null);
            }

            if (keyWord.IsNotEmpty())
            {
                queryer.Where(p => p.Code.Contains(keyWord) || p.Description.Contains(keyWord));
            }
            return queryer.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取设备异常信息
        /// </summary>
        /// <param name="pagingInfo"></param>
        /// <param name="keyWord"></param>
        /// <param name="abnormalType"></param>
        /// <param name="equipTypeId"></param>
        /// <param name="repairType">维修类型（0,设备  1,备件）</param>
        /// <returns></returns>
        public virtual EntityList<DeviceAbnormal> GetDeviceAbnormals(PagingInfo pagingInfo, string keyWord, AbnormalType? abnormalType, double? equipTypeId, int? repairType = null)
        {

            var list = QueryTypeRepairBill(pagingInfo, keyWord, abnormalType, equipTypeId, repairType);
            if (list.Count <= 0)
            {
                list = QueryNoTypeRepairBill(pagingInfo, keyWord, abnormalType, repairType);
            }

            return list;
        }

        #region 获取集合
        /// <summary>
        /// 获取设备异常维护集合  根据异常类型获取
        /// </summary>
        /// <param name="pagingInfo"></param>
        /// <returns></returns>
        public virtual EntityList<DeviceAbnormal> GetDeviceAbnormalsByAbnormalType(PagingInfo pagingInfo)
        {
            var result = DB.Query<DeviceAbnormal>().Where(p => p.AbnormalType == AbnormalType.Fault).ToList(pagingInfo, null);
            return result;
        }
        #endregion

        /// <summary>
        /// 获取数据库故障名称
        /// </summary>
        /// <param name="names"></param>
        /// <returns></returns>
        public virtual List<string> GetAbnormalNames(List<string> names)
        {
            List<string> nameList = new List<string>();
            names.SplitDataExecute(temp =>
            {
                var list = Query<DeviceAbnormal>().Where(p => temp.Contains(p.Code)).Select(p => new
                {
                    p.Code,
                }).ToList<string>().ToList();
                nameList.AddRange(list);
            });
            return nameList;
        }
    }
}
