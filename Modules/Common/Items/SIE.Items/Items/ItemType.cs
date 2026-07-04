using SIE.ObjectModel;
using System;
using System.ComponentModel;

namespace SIE.Items
{
    /// <summary>
    /// 物料类型
    /// </summary>    
    [Serializable]
    public enum ItemType
    {
        /// <summary>
        /// 成品
        /// </summary>
        [Category("CadManage")]
        [Label("成品")]
        Product=0,

        /// <summary>
        /// 原材料
        /// </summary>
        [Label("原材料")]
        Material=1,

        /// <summary>
        /// 半成品
        /// </summary>
        [Category("CadManage")]
        [Label("半成品")]
        SemiFinished=2,

        /// <summary>
        /// 备件
        /// </summary>
        [Category("CadManage")]
        [Label("备件")]
        SparePart=3,

        /// <summary>
        /// 其他
        /// </summary>
        [Label("其他")]
        Other = 9,

    }

    /// <summary>
    /// 物料消耗类型
    /// </summary>
    public enum ConsumeMode
    {
        /// <summary>
        /// 拉式物料
        /// </summary>
        [Label("拉式物料")]
        [Category("PullPush")]
        Pull=0,

        /// <summary>
        /// 推式物料
        /// </summary>
        [Label("推式物料")]
        [Category("PullPush")]
        Push=1,

        /// <summary>
        /// 储备物料
        /// </summary>
        [Label("储备物料")]
        Reserve=2
    }

    /// <summary>
    /// 进位类型
    /// </summary>
    public enum CarryType
    {
        /// <summary>
        /// 四舍五入
        /// </summary>
        [Label("四舍五入")]
        Round = 0,

        /// <summary>
        /// 舍位
        /// </summary>
        [Label("舍位")]
        Floor = 1,

        /// <summary>
        /// 进位
        /// </summary>
        [Label("进位")]
        Ceiling = 2
    }
}