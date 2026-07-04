using SIE.MES.ProcessProperty;

namespace SIE.Web.MES.ProcessProperty
{
    /// <summary>
    /// 工序参数数采查询视图配置
    /// </summary>
    public class ProcessParamCollectCriteriaViewConfig : WebViewConfig<ProcessParamCollectCriteria>
    {
        /// <summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.SN);
            View.Property(p => p.ProcessCode);
            View.Property(p => p.EquipmentName);
            View.Property(p => p.QualityStatus);
            View.Property(p => p.CreateDate).UseDateRangeEditor(p =>
            {
                p.DateRangeType = ObjectModel.DateRangeType.All;
            });
        }
    }
}
