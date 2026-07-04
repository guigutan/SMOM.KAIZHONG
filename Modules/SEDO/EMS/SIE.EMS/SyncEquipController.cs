using SIE.Domain;
using SIE.EMS.Equipments.Accounts;
using SIE.EMS.Equipments.Models;
using SIE.Equipments.EquipAccounts;
using SIE.Equipments.EquipModels;
using SIE.Equipments.EquipTypes;
using SIE.Equipments.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace SIE.EMS
{
    /// <summary>
    /// 设置控制器
    /// </summary>
    public class SyncEquipController : DomainController
    {
        /// <summary>
        /// 获取所有设备类型信息
        /// </summary>
        /// <returns>返回所有设备类型信息</returns>
        public virtual List<EquipTypeInfo> GetEquipTypeList()
        {
            return Query<EquipType>().Select(p => new { TypeCode = p.TypeCode, TypeName = p.TypeName }).ToList<EquipTypeInfo>().ToList();
        }

        /// <summary>
        /// 获取所有设备型号信息
        /// </summary>
        /// <returns>返回所有设备型号信息</returns>
        public virtual List<EquipModelInfo> GetEquipModelList()
        {
            return Query<EquipModel>().Join<EquipType>((m, t) => m.EquipTypeId == t.Id)
                .Select<EquipType>((p, q) => new { ModelCode = p.Code, ModelName = p.Name, TypeCode = q.TypeCode }).ToList<EquipModelInfo>().ToList();
        }

        /// <summary>
        /// 获取所有设备台账信息
        /// </summary>
        /// <returns>返回所有设备台账信息</returns>
        public virtual List<EquipAccountInfo> GetEquipAccountList()
        {
            return Query<EquipAccount>().Join<EquipModel>((a, m) => a.EquipModelId == m.Id)
                .Select<EquipModel>((p, q) => new
                {
                    AccountCode = p.Code,
                    AccountName = p.Name,
                    ModelCode = q.Code
                }).ToList<EquipAccountInfo>().ToList();
        }
    }
}