using Newtonsoft.Json;
using SIE.Core.ApiModels;
using SIE.Core.Logs;
using SIE.Domain;
using SIE.EMS.Equipments.Accounts;
using SIE.Equipments.EquipAccounts;
using SIE.EventMessages.EMS.Equipments;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.EMS.Equipments
{
    /// <summary>
    /// 设备查询接口实现控制器
    /// </summary>
    public class EquipmentQueryController : DomainController, IEquipmentQuery
    {
        private const string I_EQUIPMENT_QUERY_KEY = "IEquipmentQuery";

        #region 接口实现 
        /// <summary>
        /// 获取设备列表
        /// </summary>
        /// <param name="resourceId">产线ID</param>
        /// <returns>设备列表</returns>
        public virtual List<BaseDataInfo> GetEquipAccounts(double resourceId)
        {
            SaveGetEquipAccounts(resourceId);
            var equips = RT.Service.Resolve<EquipController>().GetEquipAccountsByResourceId(resourceId, null, null);
            var infos = new List<BaseDataInfo>();
            equips.ForEach(workshop =>
            {
                infos.Add(new BaseDataInfo()
                {
                    Id = workshop.Id,
                    Code = workshop.Code,
                    Name = workshop.Name
                });
            });
            return infos;
        }

        /// <summary>
        /// 保存获取设备列表日志
        /// </summary>
        /// <param name="resourceId">产线ID</param>
        private void SaveGetEquipAccounts(double resourceId)
        {
            using (var tran = DB.AutonomousTransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                var inputValue = "产线ID:{0}".L10nFormat(resourceId);
                var log = new InterfaceLog()
                {
                    Name = I_EQUIPMENT_QUERY_KEY,
                    Method = "GetEquipAccounts",
                    ControllerName = "EquipmentQueryController",
                    InputValue = inputValue,
                };

                RF.Save(log);
                tran.Complete();
            }
        }

        /// <summary>
        /// 获取设备列表
        /// </summary>
        /// <param name="resourceId">产线ID</param>
        /// <param name="equipIdList">排除的设备Id集合</param>
        /// <param name="key">查询字段</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <returns>设备列表</returns>
        public virtual PagingBaseDataInfo GetLoadItemEquipAccounts(double resourceId, List<double> equipIdList, string key, PagingInfo pagingInfo)
        {
            SaveGetLoadItemEquipAccounts(resourceId, equipIdList, key, pagingInfo);
            var equips = RT.Service.Resolve<EquipController>().GetLoadItemEquipAccountList(resourceId, equipIdList, key, pagingInfo);
            var infos = new List<BaseDataInfo>();
            equips.ForEach(workshop =>
            {
                infos.Add(new BaseDataInfo()
                {
                    Id = workshop.Id,
                    Code = workshop.Code,
                    Name = workshop.Name
                });
            });
            PagingBaseDataInfo result = new PagingBaseDataInfo()
            {
                PageNumber = pagingInfo.PageNumber,
                PageSize = pagingInfo.PageSize,
                TotalCount = equips.TotalCount
            };
            result.DataInfos.AddRange(infos);
            return result;
        }

        /// <summary>
        /// 保存获取设备列表日志
        /// </summary>
        /// <param name="resourceId">产线ID</param>
        /// <param name="equipIdList">排除的设备Id集合</param>
        /// <param name="key">查询字段</param>
        /// <param name="pagingInfo">分页信息</param>
        private void SaveGetLoadItemEquipAccounts(double resourceId, List<double> equipIdList, string key, PagingInfo pagingInfo)
        {
            using (var tran = DB.AutonomousTransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                var strEquipIdList = JsonConvert.SerializeObject(equipIdList);
                var strPageInfo = JsonConvert.SerializeObject(pagingInfo);
                var inputValue = "产线ID:{0};排除的设备Id集合:{1};查询字段:{2};分页信息:{3}".L10nFormat(resourceId, strEquipIdList, key, strPageInfo);
                var log = new InterfaceLog()
                {
                    Name = I_EQUIPMENT_QUERY_KEY,
                    Method = "GetLoadItemEquipAccounts",
                    ControllerName = "EquipmentQueryController",
                    InputValue = inputValue,
                };

                RF.Save(log);
                tran.Complete();
            }
        }

        /// <summary>
        /// 根据设备获取设备产线ID
        /// </summary>
        /// <param name="equipId">设备ID</param>
        /// <returns>产线ID</returns>
        public virtual double? GetEquipResource(double equipId)
        {
            SaveGetEquipResource(equipId);
            return RF.GetById<EquipAccount>(equipId)?.ResourceId;
        }

        /// <summary>
        /// 保存根据设备获取设备产线ID日志
        /// </summary>
        /// <param name="equipId">设备ID</param>
        private void SaveGetEquipResource(double equipId)
        {
            using (var tran = DB.AutonomousTransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                var inputValue = "设备ID:{0}".L10nFormat(equipId);
                var log = new InterfaceLog()
                {
                    Name = I_EQUIPMENT_QUERY_KEY,
                    Method = "GetEquipResource",
                    ControllerName = "EquipmentQueryController",
                    InputValue = inputValue,
                };

                RF.Save(log);
                tran.Complete();
            }
        }

        /// <summary>
        /// 根据设备ID虚拟设备ID
        /// </summary>
        /// <param name="equipIds">设备ID</param>
        /// <returns>设备ID</returns>
        public virtual List<double> GetEquipIdByIds(List<double?> equipIds)
        {
            SaveGetEquipIdByIds(equipIds);
            var query = Query<EquipAccount>().Where(w => equipIds.Contains(w.Id) && w.IsVirtual == YesNo.Yes).ToList();
            return query.Select(s => s.Id).ToList();
        }

        /// <summary>
        /// 保存根据设备ID虚拟设备ID日志
        /// </summary>
        /// <param name="equipIds">设备ID列表</param>
        private void SaveGetEquipIdByIds(List<double?> equipIds)
        {
            using (var tran = DB.AutonomousTransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                var strEquipIds = JsonConvert.SerializeObject(equipIds);
                var inputValue = "设备ID列表:{0}".L10nFormat(strEquipIds);
                var log = new InterfaceLog()
                {
                    Name = I_EQUIPMENT_QUERY_KEY,
                    Method = "GetEquipIdByIds",
                    ControllerName = "ElecLoadItemController",
                    InputValue = inputValue,
                };

                RF.Save(log);
                tran.Complete();
            }
        }
        #endregion
    }
}