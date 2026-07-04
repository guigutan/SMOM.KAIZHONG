using SIE.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.Common.Utils
{
    /// <summary>
    /// 通用辅助类
    /// </summary>
    public class ToolController : DomainController
    {
        /// <summary>
        /// 批量生成实体ID
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="num">个数</param>
        /// <returns>返回新生成的ID</returns>
        public virtual List<double> GetBatchEntityId<T>(int num) where T : DataEntity
        {
            return BatchDataEntity.GetBatchEntityId<T>(999);
        }
    }
}
