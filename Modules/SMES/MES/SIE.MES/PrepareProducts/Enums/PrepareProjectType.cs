using SIE.ObjectModel;

namespace SIE.MES.PrepareProducts.Enums
{
    /// <summary>
    /// 产前准备项目维护类型
    /// </summary>
    public enum PrepareProjectType
    {
        /// <summary>
        /// 人
        /// </summary>
        [Label("人")]
        Man = 0,

        /// <summary>
        /// 机
        /// </summary>
        [Label("机")]
        Machine = 1,

        /// <summary>
        /// 料
        /// </summary>
        [Label("料")]
        Material = 2,

        /// <summary>
        /// 法
        /// </summary>
        [Label("法")]
        Method = 3,

        /// <summary>
        /// 环
        /// </summary>
        [Label("环")]
        Environments = 4,

        /// <summary>
        /// 测
        /// </summary>
        [Label("测")]
        Measure = 5,
    }
}
