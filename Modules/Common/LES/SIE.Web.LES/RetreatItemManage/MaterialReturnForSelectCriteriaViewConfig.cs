using SIE.LES.RetreatItemManage.MaterialReturns;
using SIE.MetaModel.View;
using SIE.Web.Common;
using SIE.Web.Resources;

namespace SIE.Web.LES.RetreatItemManage
{
    /// <summary>
    /// 设备台账视图视图配置
    /// </summary>
    public class MaterialReturnForSelectCriteriaViewConfig : WebViewConfig<MaterialReturnForSelectCriteria>
    {

        protected override void ConfigView()
        {
            View.ClearCommands();
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.ClearCommands();
            View.RemoveCommands(WebCommandNames.ResetQuery,WebCommandNames.ClearQuery);
            View.Property(p => p.Sn).Show(ShowInWhere.All);
            View.Property(p => p.Factory).UseFactoryEditor().Show(ShowInWhere.All);
            View.Property(p => p.WorkOrder).Show(ShowInWhere.All);
        }
    }
}
