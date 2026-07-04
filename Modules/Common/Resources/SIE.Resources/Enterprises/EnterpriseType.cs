using SIE.ObjectModel;

namespace SIE.Resources.Enterprises
{
    /// <summary>
    /// 企业类型
    /// </summary>
    public enum EnterpriseType
    {
        /// <summary>
        /// 集团
        /// </summary>
        [Label("集团")]
        Group,

        /// <summary>
        /// 公司
        /// </summary>
        [Label("公司")]
        Company,

        /// <summary>
        /// 事业部
        /// </summary>
        [Label("事业部")]
        Profit,

        /// <summary>
        /// 工厂
        /// </summary>
        [Label("工厂")]
        Plant,

        /// <summary>
        /// 部门
        /// </summary>
        [Label("部门")]
        Department,

        /// <summary>
        /// 车间
        /// </summary>
        [Label("车间")]
        Shop,

        /// <summary>
        /// 线体
        /// </summary>
        [Label("线体")]
        Line,

        /// <summary>
        /// 科室
        /// </summary>
        [Label("科室")]
        Course,

        /// <summary>
        /// 组
        /// </summary>
        [Label("组")]
        Series,

        /// <summary>
        /// 区域
        /// </summary>
        [Label("区域")]
        Area,
    }
}