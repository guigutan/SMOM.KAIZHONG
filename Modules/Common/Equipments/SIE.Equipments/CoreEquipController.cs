using SIE.Core.ApiModels;
using SIE.Domain;
using SIE.Equipments.EquipAccounts;
using SIE.Equipments.EquipModels;
using SIE.Equipments.EquipTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace SIE.Equipments
{
    /// <summary>
    /// 基类设备控制器
    /// </summary>
    public partial class CoreEquipController : DomainController
    {
        /// <summary>
        /// 根据条件获取设备类型数据
        /// </summary>
        /// <param name="exp">查询表达式</param>
        /// <returns></returns>
        public virtual EquipType GetEquipType(Expression<Func<EquipType, bool>> exp)
        {
            var query = Query<EquipType>();
            if (exp != null)
                query.Where(exp);
            return query.FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据设备编码和ID获取设备类型信息
        /// </summary>
        /// <param name="typeCode">编码</param>
        /// <param name="id">ID</param>
        /// <returns>设备类型信息</returns>
        public virtual EntityList<EquipType> GetEquipTypeList(string typeCode, double id)
        {
            var query = Query<EquipType>();
            var equipTypeList = query.Where(p => p.TypeCode == typeCode && p.Id != id).ToList();
            return equipTypeList;
        }

        /// <summary>
        /// 根据条件获取设备类型数据
        /// </summary>
        /// <param name="exp">查询表达式</param>
        /// <returns></returns>
        public virtual IEnumerable<EquipType> GetEquipTypeList(Expression<Func<EquipType, bool>> exp)
        {
            var query = Query<EquipType>();
            if (exp != null)
                query.Where(exp);
            return query.ToList();
        }

        /// <summary>
        /// 获取设备型号
        /// </summary>
        /// <param name="id">型号ID</param>
        /// <returns>设备型号</returns>
        public virtual EquipModel GetEquipModel(double id)
        {
            return Query<EquipModel>().Where(p => p.Id == id).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据台帐ID获取设备型号数据
        /// </summary>
        /// <param name="equipId">台帐Id</param>
        /// <returns>设备型号</returns>
        public virtual EquipModel GetEquipModelByEquipId(double equipId)
        {
            return Query<EquipModel>().Exists<EquipAccount>((a, b) => b.Where(c => c.EquipModelId == a.Id && c.Id == equipId)).FirstOrDefault();
        }

        /// <summary>
        /// 获取所有设备型号
        /// </summary> 
        /// <returns>设备型号列表</returns>
        public virtual EntityList<EquipModel> GetEquipModels()
        {
            return Query<EquipModel>().ToList();
        }

        /// <summary>
        /// 根据ID获取设备列表
        /// </summary>
        /// <param name="equipIds">设备Ids</param>
        /// <returns>设备列表</returns>
        public virtual EntityList<EquipAccount> GetEquipAccounts(List<double?> equipIds)
        {
            return Query<EquipAccount>().Where(w => equipIds.Contains(w.Id)).ToList();
        }

        /// <summary>
        /// 根据Code获取设备列表
        /// </summary>
        /// <param name="equipCodes">设备Ids</param>
        /// <returns>设备列表</returns>
        public virtual EntityList<EquipAccount> GetEquipAccounts(List<string> equipCodes)
        {
            return Query<EquipAccount>().Where(w => equipCodes.Contains(w.Code)).ToList();
        }

        /// <summary>
        /// 获取设备基础数据
        /// </summary>
        /// <param name="equipCodes"></param>
        /// <returns></returns>
        public virtual List<BaseDataInfo> GetEquipAccountBaseInfos(List<string> equipCodes)
        {
            List<BaseDataInfo> equipBaseInfos = new List<BaseDataInfo>();

            equipCodes.SplitDataExecute(temps =>
            {
                var list = Query<EquipAccount>().Where(p => temps.Contains(p.Code)).Select(p => new
                {
                    Id = p.Id,
                    Code = p.Code,
                    Name = p.Name,
                }).ToList<BaseDataInfo>();
                equipBaseInfos.AddRange(list);
            });

            return equipBaseInfos;
        }

        /// <summary>
        /// 通过设备编码获取设备
        /// </summary>
        /// <param name="code">设备编码 不可空</param>
        /// <returns>设备台账 可空</returns>
        public virtual EquipAccount GetEquipAccountByCode(string code)
        {
            return Query<EquipAccount>().Where(p => p.Code == code).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据设备ID获取设备信息
        /// </summary>
        /// <param name="equipID">设备Id</param>
        /// <returns>设备列表</returns>
        public virtual EntityList<EquipAccount> GetEquipAccount(double equipID)
        {
            return Query<EquipAccount>().Where(w => w.Id == equipID).ToList();
        }


        /// <summary>
        /// 查询设备类型信息
        /// </summary>
        /// <param name="pagingInfo"></param>
        /// <param name="keyword"></param>
        /// <param name="needView"></param>
        /// <returns></returns>
        public virtual EntityList<EquipType> GetEquipTypes(PagingInfo pagingInfo, string keyword, bool needView = false)
        {
            var q = Query<EquipType>();
            if (keyword.IsNotEmpty())
                q.Where(p => p.TypeCode.Contains(keyword) || p.TypeName.Contains(keyword));
            EagerLoadOptions opt = new EagerLoadOptions();
            if (needView)
            {
                opt.LoadWithViewProperty();
            }
            return q.ToList(pagingInfo, opt);
        }

        /// <summary>
        /// 根据设备类别获取设备类型信息
        /// </summary>
        /// <param name="typeCategory">设备类别</param>        
        /// <param name="pagingInfo"></param>
        /// <param name="keyword"></param>
        /// <returns>设备类型信息</returns>
        public virtual EntityList<EquipType> GetEquipTypes(string typeCategory,
            PagingInfo pagingInfo, string keyword)
        {
            var query = Query<EquipType>();

            var equipTypeList = query
                .WhereIf(keyword.IsNotEmpty(), p => p.TypeCode.Contains(keyword) || p.TypeName.Contains(keyword))
                .ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());

            return equipTypeList;
        }


        // <summary>
        /// 获取立库业务的设备类型
        /// </summary>
        /// <returns>返回设备类型列表</returns>
        public virtual EntityList<EquipType> GetArsEquipType()
        {
            var query = Query<EquipType>();
            query.Join<EquipModel>((a, b) => a.Id == b.EquipTypeId && b.IndustryCategory == Core.Enums.IndustryCategory.LogisticsEquipment);
            return query.Distinct().ToList();
        }

        /// <summary>
        /// 根据仓库编码获取立库应用的设备
        /// </summary>
        /// <param name="warehouseCode"></param>
        /// <returns></returns>
        public virtual EntityList<EquipAccount> GetArsEquipAccountByWHCode(string warehouseCode)
        {
            var query = Query<EquipAccount>().Where(p => p.InstallationLocation == warehouseCode);
            query.Join<EquipModel>((a, b) => a.EquipModelId == b.Id && b.IndustryCategory == Core.Enums.IndustryCategory.LogisticsEquipment);
            return query.OrderBy(p => new { p.EquipTypeCode, p.Code }).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据Id集合获取设备类型集合
        /// </summary>
        /// <param name="Ids"></param>
        /// <returns></returns>
        public virtual EntityList<EquipType> GetEquipTypesByIds(List<double> Ids)
        {
            return Ids.SplitContains(items =>
            {
                return Query<EquipType>().Where(m => items.Contains(m.Id)).ToList();
            });
        }

        /// <summary>
        /// 根据编码获取设备类型
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public virtual EquipType GetEquipTypeByCode(string code)
        {
            if (code.IsNullOrEmpty())
                return null;

            return Query<EquipType>().Where(c => c.TypeCode == code).FirstOrDefault();

        }
    }
}