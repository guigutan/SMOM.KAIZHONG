using System;
using System.Collections.Generic;

namespace SIE.MES.TeamManagement.ApiInterfaces
{
    /// <summary>
    /// 评分项目分类API信息类
    /// </summary>
    [Serializable]
    public class RatedItemCategoryInfo
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public RatedItemCategoryInfo()
        {
            ItemList = new List<RatedItemInfo>();
        }

        /// <summary>
        /// 分类Id
        /// </summary>
        public double Id { get; set; }

        /// <summary>
        /// 分类编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 分类名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 评分项目列表
        /// </summary>
        public List<RatedItemInfo> ItemList { get; set; }
    }
}
