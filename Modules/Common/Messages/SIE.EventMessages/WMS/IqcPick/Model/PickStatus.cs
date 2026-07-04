using SIE.ObjectModel;

namespace SIE.EventMessages.WMS.IqcPick.Model
{
    public enum PickStatus
    {
        /// <summary>
        /// 待挑选
        /// </summary>
        [Label("待挑选")]
        ToPick,
        /// <summary>
        /// 挑选中
        /// </summary>
        [Label("挑选中")]
        Picking,
        /// <summary>
        /// 已挑选
        /// </summary>
        [Label("已挑选")]
        Picked,
        /// <summary>
        /// 终止挑选
        /// </summary>
        [Label("终止挑选")]
        StopPicked,
    }
}
