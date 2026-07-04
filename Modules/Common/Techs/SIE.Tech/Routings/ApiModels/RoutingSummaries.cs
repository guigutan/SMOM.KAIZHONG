using System;

namespace SIE.Tech.Routings.ApiModels
{
    /// <summary>
    /// 工艺路线概述信息
    /// </summary>
    [Serializable]
    public class RoutingSummaries
    {
        /// <summary>
        /// 产品族分类Id
        /// </summary>
        public double CategoryId
        {
            get;
            set;
        }

        /// <summary>
        /// 产品族分类
        /// </summary>
        public string Category
        {
            get;
            set;
        }
        
        /// <summary>
        /// 工艺路线ID
        /// </summary>
        public double? RoutingId
        {
            get;
            set;
        }
        
        /// <summary>
        /// 工艺路线名称
        /// </summary>
        public string RoutingName
        {
            get;
            set;
        }

        /// <summary>
        /// 工艺路线描述
        /// </summary>
        public string RoutingDesc
        {
            get;
            set;
        }

        /// <summary>
        /// 行号
        /// </summary>
        public int RowNum
        {
            get;
            set;
        }
        /// <summary>
        /// 是否验证通过
        /// </summary>
        public bool IsPass
        {
            get;
            set;
        }
    }
}
