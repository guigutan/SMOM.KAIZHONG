using SIE.ObjectModel;

namespace SIE.MES.OnOffDutyA
{
    /// <summary>
    /// 上下岗类型
    /// </summary>
    public enum OnOffDutyType
    {
        /// <summary>
        /// 上岗
        /// </summary>
        [Label("上岗")]
        OnDuty = 0,


        /// <summary>
        /// 下岗
        /// </summary>

        [Label("下岗")]
        OffDuty = 1
    }
}
