using SIE.MetaModel.View;
using System;
using System.Collections.Generic;

namespace SIE.Core.NavMenus
{
    /// <summary>
    /// 向导菜单数据
    /// </summary>    
    public class NavMenuDto
    {
        /// <summary>
        /// 树结构菜单key，根节点的时候这个为空，子节点才需要
        /// </summary>        
        public string TreeKey { get; set; }

        /// <summary>
        /// 向导菜单标签
        /// </summary>        
        public string Label { get; set; }

        /// <summary>
        /// 实体类型
        /// </summary>   
        public Type EntityType { get; set; }

        /// <summary>
        /// 图标
        /// </summary>  
        public string Icon { get; set; }

        /// <summary>
        /// 打开方式，默认 “标签页” 打开
        /// </summary> 
        public ViewTarget Target { get; set; }
    }

    /// <summary>
    /// Web向导菜单配置接口
    /// </summary>    
    public interface IWebNavMenuInit : INavMenuInit
    {

    }
    /// <summary>
    /// 向导菜单配置接口
    /// </summary>    
    public interface INavMenuInit
    {
        /// <summary>
        /// 获取向导菜单列表
        /// </summary>
        /// <returns></returns>        
        List<NavMenuDto> GetNavMenuDtos();
    }
}
