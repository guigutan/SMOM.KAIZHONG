using SIE.Core.Common;
using SIE.Defects;
using SIE.Domain;
using SIE.Equipments.EquipModels;
using System;
using System.Collections.Generic;

namespace SIE.Equipments.EquipFaults
{
    /// <summary>
    /// 设备故障与系统缺陷对应关系控制器
    /// </summary>
    public partial class EquipFaultAndDefectController : DomainController
    {
        /// <summary>
        /// 获取设备故障与系统缺陷对应关系
        /// </summary>
        /// <param name="criteria">设备故障与系统缺陷对应关系查询实体</param>
        /// <returns>设备故障与系统缺陷对应关系列表</returns>
        public virtual EntityList<EquipFaultAndDefect> GetEquipFaultAndDefects(EquipFaultAndDefectCriteria criteria)
        {
            var query = Query<EquipFaultAndDefect>();
            if (!string.IsNullOrEmpty(criteria.EquipBadCode))
            {
                query.Where(p => p.EquipBadCode.Contains(criteria.EquipBadCode));
            }

            if (criteria.EquipModelId.HasValue)
            {
                query.Where(p => p.EquipModelId == criteria.EquipModelId);
            }

            if (!string.IsNullOrEmpty(criteria.EquipModelName))
            {
                query.Join<EquipModel>((f, m) => f.EquipModelId == m.Id && m.Name.Contains(criteria.EquipModelName));
            }

            if (criteria.DefectCategoryId.HasValue)
            {
                query.Where(p => p.Defect.DefectCategoryId == criteria.DefectCategoryId);
            }

            if (criteria.DefectId.HasValue)
            {
                query.Where(p => p.DefectId == criteria.DefectId);
            }

            if (criteria.CreateDate!=null&&criteria.CreateDate.BeginValue.HasValue)
            {
                query.Where(p => p.CreateDate >= criteria.CreateDate.BeginValue.Value);
            }

            if (criteria.CreateDate != null && criteria.CreateDate.EndValue.HasValue)
            {
                query.Where(p => p.CreateDate <= criteria.CreateDate.EndValue.Value);
            }

            return query.ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }


        /// <summary>
        /// 根据设备缺陷不良编码获取缺陷关系
        /// </summary>
        /// <param name="equipBadCodes">设备不良编码集合</param>
        /// <returns></returns>
        public virtual EntityList<EquipFaultAndDefect> GetEquipFaultAndDefects(List<string> equipBadCodes)
        {
            if (equipBadCodes != null && equipBadCodes.Count > 0)
            {

                return Query<EquipFaultAndDefect>().Join<Defect>((x, p) => x.DefectId == p.Id).Where(equipBadCodes.CreateContainsExpression<EquipFaultAndDefect>("x", nameof(EquipFaultAndDefect.EquipBadCode))).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            }
            return new EntityList<EquipFaultAndDefect>();
        }

        /// <summary>
        /// 根据设备缺陷不良Id获取缺陷关系
        /// </summary>
        /// <param name="ids">设备不良Id集合</param>
        /// <returns></returns>
        public virtual EntityList<EquipFaultAndDefect> GetEquipFaultAndDefects(List<double> ids)
        {
            if (ids != null && ids.Count > 0)
            {
                return Query<EquipFaultAndDefect>().Join<Defect>((x, p) => x.DefectId == p.Id).Where(ids.CreateContainsExpression<EquipFaultAndDefect>("x", nameof(EquipFaultAndDefect.Id))).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            }
            return new EntityList<EquipFaultAndDefect>();
        }

        /// <summary>
        /// 保存设备故障与系统缺陷对应关系
        /// </summary>
        /// <param name="faultDefect">设备故障与系统缺陷对应关系</param>
        public virtual void SaveEquipFaultAndDefect(EquipFaultAndDefect faultDefect)
        {
            RF.Save(faultDefect);
        }

        /// <summary>
        /// 根据产品编码获取产品
        /// </summary>
        /// <param name="context">产品编码</param>
        /// <returns>返回产品</returns>
        public virtual EquipFaultAndDefect GetEquipFromCode(string context)
        {
            var equipEntity = Query<EquipFaultAndDefect>().Where(p => p.EquipBadCode == context).FirstOrDefault();

            return equipEntity;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="equipBadCodes"></param>
        /// <returns></returns>
        public virtual EquipFaultAndDefect GetEquipFaultAndDefect(string equipBadCodes)
        {
            return Query<EquipFaultAndDefect>().Where(c => c.EquipBadCode == equipBadCodes).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }
    }
}