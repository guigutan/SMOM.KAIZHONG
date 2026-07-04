using SIE.Domain;
using SIE.MES.TaskManagement.Reports;
using SIE.MetaModel;

namespace SIE.MES.TaskManagement.Dispatchs
{
    /// <summary>
    /// 附加关联属性生产批次规则
    /// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    public static class ReportExt
    {
        #region ReportRecord ReportRecord (附加关联属性生产批次)
        /// <summary>
        /// 附加关联属性生产批次 扩展属性。
        /// </summary>
        public static readonly Property<ReportRecord> ReportRecordProperty =
            P<DispatchTask>.RegisterExtension<ReportRecord>("ReportRecord", typeof(ReportExt));

        /// <summary>
        /// 获取 附加关联属性生产批次 属性的值。
        /// </summary>
        /// <param name="me">要获取扩展属性值的对象。</param>
        /// <returns>生产批次</returns>
        public static ReportRecord GetReportRecord(DispatchTask me)
        {
            return me.GetProperty(ReportRecordProperty);
        }

        /// <summary>
        /// 设置 附加关联属性生产批次 属性的值。
        /// </summary>
        /// <param name="me">要设置扩展属性值的对象。</param>
        /// <param name="value">设置的值。</param>
        public static void SetReportRecord(DispatchTask me, ReportRecord value)
        {
            me.SetProperty(ReportRecordProperty, value);
        }
        #endregion
    }

    /// <summary>
    /// 作用是映射的时候能找到对应的实体
    /// </summary>
    internal class ReportExtConfig : EntityConfig<DispatchTask>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.Property(ReportExt.ReportRecordProperty).DontMapColumn();
        }
    }
}
