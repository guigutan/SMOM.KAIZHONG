using SIE.Domain;
using System;
using System.Collections.Generic;

namespace SIE.Defects.InspectionItems
{
    /// <summary>
    /// 检验项目控制器
    /// </summary>
    public partial class InspectionItemController : DomainController
    {
        #region 检验方式 
        /// <summary>
        /// 获取名称为正常的检验方式，供来料接收创建来料检验单时使用
        /// </summary>
        /// <returns>检验方式</returns>
        public virtual InspectionMode GetInspectionModes()
        {
            var query = Query<InspectionMode>();
            return query.Where(p => p.Name == "正常").FirstOrDefault();
        }

        /// <summary>
        /// 获取检验方式
        /// </summary>
        /// <param name="type">检验类型</param>
        /// <param name="pagingInfo">pagingInfo</param>
        /// <param name="keyword">keyword</param>
        /// <returns>InspectionMode列表</returns>
        public virtual EntityList<InspectionMode> GetInspectionModes(PagingInfo pagingInfo, string keyword)
        {
            var query = Query<InspectionMode>();
            query.WhereIf(keyword.IsNotEmpty(), p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            return query.ToList(pagingInfo);
        }

        /// <summary>
        /// /获取检验方式
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns>检验方式</returns>
        public virtual InspectionMode GetInspectionMode(string name)
        {
            return Query<InspectionMode>().Where(p => p.Name == name).FirstOrDefault();
        }

        public virtual EntityList<InspectionMode> GetInspectionMode(List<string> names)
        {
            return Query<InspectionMode>().Where(p => names.Contains(p.Name)).ToList();
        }
        /// <summary>
        /// 获取指定名称的检验方式，供来料接收创建来料检验单时使用
        /// </summary>
        /// <returns>检验方式</returns>
        public virtual InspectionMode GetInspectionModesByName(string name)
        {
            var query = Query<InspectionMode>();
            return query.Where(p => p.Name == name).FirstOrDefault();
        }

        /// <summary>
        /// 获取所有检验方式的数据
        /// </summary>
        /// <returns>返回所有检验方式的数据</returns>
        public virtual EntityList<InspectionMode> GetInspectionModeList()
        {
            return Query<InspectionMode>().ToList();
        }
        #endregion
    }
}