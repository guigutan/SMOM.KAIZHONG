using SIE.Domain;
using SIE.Fixtures.Models;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Fixtures
{
    /// <summary>
    /// 同步工治具
    /// </summary>
    public class SyncFixtureController : DomainController
    {
        /// <summary>
        /// 获取所有工治具类型信息
        /// </summary>
        /// <returns>返回工治具类型信息</returns>
        public virtual List<FixtureTypeData> GetAllFixtureTypeInfo()
        {
            return Query<FixtureModel>().Where(p => p.ManageMode == ManageMode.Number).Select(p => new { TypeId = p.Id, TypeCode = p.Code, TypeName = p.Name }).ToList<FixtureTypeData>().ToList();
        }

        /// <summary>
        /// 获取所有工治具编码信息
        /// </summary>
        /// <returns>返回工治具编码信息</returns>
        public virtual List<FixtureEncodeData> GetAllFixtureEncodeInfo()
        {
            return Query<FixtureEncode>().Join<FixtureModel>((a, b) => a.FixtureModelId == b.Id)
                .Where<FixtureModel>((p, q) => q.ManageMode == ManageMode.Number)
                .Select<FixtureModel>((p, q) => new { FixtureId = p.Id, FixtureCode = p.Code, FixtureName = p.Code, TypeId = q.Id }).ToList<FixtureEncodeData>().ToList();
        }

        ///// <summary>
        ///// 获取所有设备类型信息
        ///// </summary>
        ///// <returns>返回所有设备类型信息</returns>
        //public virtual List<EquipTypeInfo> GetEquipTypeList()
        //{
        //    return Query<EquipType>().Select(p => new { TypeCode = p.TypeCode, TypeName = p.TypeName }).ToList<EquipTypeInfo>().ToList();
        //}

        ///// <summary>
        ///// 获取所有设备型号信息
        ///// </summary>
        ///// <returns>返回所有设备型号信息</returns>
        //public virtual List<EquipModelInfo> GetEquipModelList()
        //{
        //    return Query<EquipModel>().Join<EquipType>((m, t) => m.EquipTypeId == t.Id)
        //        .Select<EquipType>((p, q) => new { ModelCode = p.Code, ModelName = p.Name, TypeCode = q.TypeCode }).ToList<EquipModelInfo>().ToList();
        //}

        ///// <summary>
        ///// 获取所有设备台账信息
        ///// </summary>
        ///// <returns>返回所有设备台账信息</returns>
        //public virtual List<EquipAccountInfo> GetEquipAccountList()
        //{
        //    return Query<EquipAccount>().Join<EquipModel>((a, m) => a.EquipModelId == m.Id)
        //        .Select<EquipModel>((p, q) => new { AccountCode = p.Code, AccountName = p.Name, ModelCode = q.Code }).ToList<EquipAccountInfo>().ToList();
        //}
    }
}