using SIE.Domain;
using SIE.Resources.Enterprises;
using SIE.Web.WorkBenchCommon._Extensions_;
using SIE.WorkBenchCommon.Workbench.KPI;
using System.Linq;

namespace SIE.Web.WorkBenchCommon.Workbench.KPI
{
    /// <summary>
    /// 指标定义查询视图实体
    /// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    public class QuotaTargetSettingCriteriaViewConfig : WebViewConfig<QuotaTargetSettingCriteria>
    {
        #region 层级类型变更清除企业模型 ClearEnterprise
        /// <summary>
        /// 层级类型变更清除企业模型
        /// </summary> 
        public static readonly Property<bool> ClearEnterpriseProperty = P<QuotaTargetSettingCriteria>.RegisterExtensionReadOnly("ClearEnterprise", typeof(QuotaTargetSettingCriteriaViewConfig),
            GetClearEnterprise, QuotaTargetSettingCriteria.EntTypeProperty);

        /// <summary>
        /// 层级类型变更清除企业模型
        /// </summary>
        public static bool GetClearEnterprise(QuotaTargetSettingCriteria me)
        {
            me.Enterprise = null;
            return false;
        }
        #endregion

        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.AddBehavior("SIE.Web.WorkBenchCommon.Workbench.KPI.Behaviors.QuotaTargetSettingCriteriaBehavior");
            using (View.OrderProperties())
            {
                View.Property(p => p.Code).UseDropDownEditor(() => { return RT.Service.Resolve<QuotaTargetSettingController>().GetQuotaTargetSettingCodeDic(); }).HasLabel("指标分类").Show(ShowInWhere.All);
                View.Property(p => p.Name).UseKpiDropDownEditor(() => { return RT.Service.Resolve<QuotaTargetSettingController>().GetQuotaTargetSettingNameDic(string.Empty); }).HasLabel("指标名称").Show(ShowInWhere.All);
                View.Property(p => p.Dimension).UseEnumEditor().Show(ShowInWhere.All);
                View.Property(p => p.EntType).UseEnumEditor().Show(ShowInWhere.All);
                View.Property(p => p.Enterprise).UsePagingLookUpEditor(e => { e.ReloadDataOnPopping = true; }).UseDataSource((o, p, e) =>
                {
                    var item = o as QuotaTargetSettingCriteria;
                    if (!item.EntType.HasValue) return new EntityList<Enterprise>();
                    var rst = RT.Service.Resolve<EnterpriseController>().GetEnterprises(item.EntType.Value, p, e);
                    rst.ForEach(c => c.TreePId = null);//不需要层级关系
                    return rst;
                }).Show(ShowInWhere.All);
            }
        }
    }
}