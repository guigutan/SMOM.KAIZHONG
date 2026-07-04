using SIE.ObjectModel;

namespace SIE.MES.WorkOrders
{
    /// <summary>
    /// 操作类型
    /// </summary>
    public enum WorkOrderLogType
    {
        /// <summary>
        /// 关闭
        /// </summary>
        [Label("关闭")]
        Close = 0,

        /// <summary>
        /// 取消
        /// </summary>
        [Label("取消")]
        Cancel = 1,

        /// <summary>
        /// 暂停
        /// </summary>
        [Label("暂停")]
        Pause = 2,

        /// <summary>
        /// 创建
        /// </summary>
        [Label("创建")]
        Create = 3,

        /// <summary>
        /// 恢复
        /// </summary>
        [Label("恢复")]
        Resume = 4,

        /// <summary>
        /// 修改工艺路线
        /// </summary>
        [Label("修改工艺路线")]
        UpdateRouting = 5,

        /// <summary>
        /// 发放
        /// </summary>
        [Label("发放")]
        Release = 6,

        /// <summary>
        /// 取消发放
        /// </summary>
        [Label("取消发放")]
        CancelRelease = 7,

        /// <summary>
        /// 生产中
        /// </summary>
        [Label("生产中")]
        Producing = 8,

        /// <summary>
        /// 完工
        /// </summary>
        [Label("完工")]
        Finish = 9,

        /// <summary>
        /// 拆分
        /// </summary>
        [Label("拆分")]
        Split = 10,

        /// <summary>
        /// 校验程序
        /// </summary>
        [Label("校验程序")]
        Verify = 11,

        /// <summary>
        /// 其它
        /// </summary>
        [Label("其它")]
        Other =12
    }
}