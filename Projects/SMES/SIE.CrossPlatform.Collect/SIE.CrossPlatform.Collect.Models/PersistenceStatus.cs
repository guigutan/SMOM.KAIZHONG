using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.CrossPlatform.Collect.Models
{
    [Serializable]
    public enum PersistenceStatus
    {
        /// <summary>
        /// 未改动
        /// </summary>
        Unchanged = 0,
        /// <summary>
        /// 数据变更。仓库保存时，需要执行更新操作。
        /// </summary>
        Modified = 1,
        /// <summary>
        /// 新对象。仓库保存时，需要执行添加操作。
        /// </summary>
        New = 2,
        /// <summary>
        /// 已删除。仓库保存时，需要执行删除操作。
        /// </summary>
        Deleted = 3
    }
}
