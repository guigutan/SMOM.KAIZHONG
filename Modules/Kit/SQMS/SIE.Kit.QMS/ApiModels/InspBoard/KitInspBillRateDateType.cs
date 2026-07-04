using SIE.QMS.ApiModel.DataAnalysis;

namespace SIE.Kit.QMS.ApiModels.InspBoard
{
    /// <summary>
    /// 看板-检验单时间类型
    /// </summary>
    public class KitInspBillRateDateType : InspBillRateDateType
    {
        /// <summary>
        /// 检验组Id
        /// </summary>
        public double? InspGroupId { get; set; }
    }
}
