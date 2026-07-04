using System;

namespace SIE.Core.NavMenus
{
    /// <summary>
    /// 向导菜单配置信息
    /// </summary>
    [Serializable]
    public class NavMenuConfig
    {
        /// <summary>
        /// 向导菜单(唯一)key，
        /// </summary>        
        public string key { get; set; }

        /// <summary>
        /// 向导菜单描述
        /// </summary>        
        public string desc { get; set; }

        /// <summary>
        /// 实体类型
        /// </summary>   
        public string type { get; set; }

        /// <summary>
        /// 向导菜单父级key
        /// </summary>        
        public string parent { get; set; }
    }
}
