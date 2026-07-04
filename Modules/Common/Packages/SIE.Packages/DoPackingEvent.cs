using System;

namespace SIE.Packages.Packings
{
    /// <summary>
    /// 打包事件
    /// </summary>
    [Serializable]
    public class DoPackingEvent
    {
        /// <summary>
        /// 包装事件
        /// </summary>
        /// <param name="action">包装</param>
        /// <param name="group">组</param>
        /// <param name="data">包装关系</param>
        public DoPackingEvent(DoPackingAction action, string group, params PackingRelation[] data)
        {
            OuterPackingRelations = data;
            DoPackingAction = action;
            Group = group;
        }

        /// <summary>
        /// 组
        /// </summary>
        public string Group { get; }

        /// <summary>
        /// 输出包装关系
        /// </summary>
        public PackingRelation[] OuterPackingRelations { get; }

        /// <summary>
        /// 包装
        /// </summary>
        public DoPackingAction DoPackingAction { get; }
    }

    /// <summary>
    /// 包装
    /// </summary>
    public enum DoPackingAction
    {
        /// <summary>
        /// 打包
        /// </summary>
        DoPacking = 0,

        /// <summary>
        /// 完成打包
        /// </summary>
        Packed = 1
    }
}
