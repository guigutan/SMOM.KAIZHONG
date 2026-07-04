using SIE.CrossPlatform.Collect.Models;
using SIE.CrossPlatform.Collect.Models.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.CrossPlatform.Collect.Models
{
    /// <summary>
    /// 本地化资源 
    /// </summary>
    [Serializable]
    public class Resource
    {
        public Resource()
        {
            ResourceType = ResourceType.Simplify;
        }
        /// <summary>
        /// 数据Id
        /// </summary>
        public double Id
        {
            get; set;
        }

        /// <summary>
        /// 本地化ID
        /// </summary>
        public double CultureId
        {
            get; set;
        }

        /// <summary>
        /// 原文
        /// </summary>
        public string Key
        {
            get; set;
        }


        /// <summary>
        /// 译文
        /// </summary>
        public string Value
        {
            get; set;
        }
        /// <summary>
        /// 平台的类型
        /// </summary>
        public ResourceType ResourceType
        {
            get; set;
        }

        /// <summary>
        /// 文化名称
        /// </summary>
        public string CultureName
        {
            get; set;
        }

        public PersistenceStatus PersistenceStatus
        {
            get; set;
        }
    }

    /// <summary>
    /// 平台类型
    /// </summary>
    public enum ResourceType
    {
        /// <summary>
        /// 标准版
        /// </summary>
        [Label("标准版")]
        Standard = 0,
        /// <summary>
        /// 简化版
        /// </summary>
        [Label("简化版")]
        Simplify = 1,
    }
}
