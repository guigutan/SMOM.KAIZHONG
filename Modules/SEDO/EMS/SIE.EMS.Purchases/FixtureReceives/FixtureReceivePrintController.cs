using SIE.Common.Prints;
using SIE.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.Purchases.FixtureReceives
{
    /// <summary>
    /// 工治具接收打印控制器
    /// </summary>
    public class FixtureReceivePrintController : DomainController
    {

        /// <summary>
        /// 获取打印模板
        /// </summary>
        /// <param name="entityType">打印条码类型</param>
        /// <param name="info">分页信息</param>
        /// <param name="keyword">关键词</param>
        /// <returns>打印模板列表</returns>
        public virtual EntityList<PrintTemplate> GetPrintTemplatesByType(string entityType, PagingInfo info = null, string keyword = "")
        {
            var query = Query<PrintTemplate>().Where(p => p.EntityType.Contains(entityType));
            if (keyword.IsNotEmpty())
            {
                query.Where(p => p.FileName.Contains(keyword));
            }
            return query.ToList(info);
        }
    }
}
