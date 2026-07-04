using SIE.ObjectModel;

namespace SIE.EMS.EarlierStage.Enums
{
    /// <summary>
    /// 变更操作
    /// </summary>
    public enum ChangeOperate
    {
        /// <summary>
        /// 增加
        /// </summary>
        [Label("增加")]
        Add = 10,
        /// <summary>
        /// 修改
        /// </summary>
        [Label("修改")]
        Modify = 20,
        /// <summary>
        /// 删除
        /// </summary>
        [Label("删除")]
        Delete = 30,
        /// <summary>
        /// 暂停
        /// </summary>
        [Label("暂停")]
        Suspend = 40,
        /// <summary>
        /// 暂停恢复
        /// </summary>
        [Label("暂停恢复")]
        Recovery = 50
    }
}
