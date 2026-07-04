using SIE.EMS.Tpms;

namespace SIE.Web.EMS.Tpms
{
    /// <summary>
    /// TPM记录查询实体视图配置
    /// </summary>
    internal class TpmRecordCriteriaViewConfig : WebViewConfig<TpmRecordCriteria>
    {
        ///<summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.EquipCode);
            View.Property(p => p.MachineNo);
            View.Property(p => p.Workshop);
            View.Property(p => p.Process);
            View.Property(p => p.CreateDate).UseDateRangeEditor(p => p.DateRangeType = ObjectModel.DateRangeType.Month);
        }
    }
}
