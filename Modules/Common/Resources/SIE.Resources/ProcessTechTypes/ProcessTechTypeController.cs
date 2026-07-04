using SIE.Domain;
using SIE.Resources.CalendarSchemes;
using SIE.Resources.WipResources;
using System;

namespace SIE.Resources.ProcessTechTypes
{
    /// <summary>
    /// 制程工艺类型控制器
    /// </summary>
    public class ProcessTechTypeController : DomainController
    {
        /// <summary>
        /// 获取所有制程工艺类型数据
        /// </summary>
        /// <returns></returns>
        public virtual EntityList<ProcessTechType> GetProcessTechType()
        {
            var query = Query<ProcessTechType>();

            return query.ToList();
        }

        /// <summary>
        /// 根据编码/名称获取对应的制程工艺类型
        /// </summary>
        /// <returns></returns>
        public virtual ProcessTechType GetProcessTechType(string context)
        {
            return Query<ProcessTechType>().Where(p => p.Code == context || p.Name == context).FirstOrDefault();
        }

        /// <summary>
        /// 根据指定id获取对应的制程工艺类型
        /// </summary>
        /// <param name="typeId">制程工艺类型</param>
        /// <returns>返回制程工艺类型</returns>
        public virtual ProcessTechType GetProcessTechTypeById(double typeId)
        {
            return Query<ProcessTechType>().Where(p => p.Id == typeId).FirstOrDefault();
        }

        /// <summary>
        /// 根据工厂ID获取制程工艺类型
        /// </summary>
        /// <param name="factoryId">工厂ID</param>
        /// <returns>返回制程工艺类型</returns>
        public virtual EntityList<ProcessTechType> GetProcessTechTypeByFactory(double factoryId)
        {
            return Query<ProcessTechType>()
                .Exists<WipResource>((a, b) => b.Where(p => p.ProcessTechTypeId == a.Id && p.FactoryId == factoryId))
                .ToList();
        }

        /// <summary>
        /// 根据工厂ID获取制程工艺类型
        /// </summary>
        /// <param name="factoryId">工厂ID</param>
        /// <returns>返回制程工艺类型</returns>
        public virtual EntityList<ProcessTechType> GetProcessTechTypeByFactory(double factoryId, string code)
        {
            return Query<ProcessTechType>()
                .Exists<WipResource>((a, b) => b.Where(p => p.ProcessTechTypeId == a.Id && p.FactoryId == factoryId).WhereIf<ProcessTechType>(code.IsNotEmpty(), (c, d) => d.Code.Contains(code) || d.Name.Contains(code)))
                .ToList();
        }

        /// <summary>
        /// 获取可用的制程工艺类型
        /// </summary>
        /// <param name="pagingInfo">分页</param>
        /// <param name="keyword">关键字</param>
        /// <returns>列表</returns>
        public virtual EntityList<ProcessTechType> GetProcessTechTypeList(PagingInfo pagingInfo, string keyword)
        {
            var query = Query<ProcessTechType>();
            query.WhereIf(keyword.IsNotEmpty(), p => p.Name.Contains(keyword) || p.Code.Contains(keyword));
            return query.ToList(pagingInfo);
        }
    }
}