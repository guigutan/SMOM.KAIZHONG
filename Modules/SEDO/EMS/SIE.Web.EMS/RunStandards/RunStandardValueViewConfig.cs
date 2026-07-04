using SIE.EMS.Enums;
using SIE.EMS.RunStandards;
using SIE.MetaModel.View;

namespace SIE.Web.EMS.RunStandards
{
    /// <summary>
    /// 定标量视图
    /// </summary>
    public class RunStandardValueViewConfig : WebViewConfig<RunStandardValue>
    {
        /// <summary>
        /// 编辑视图
        /// </summary>
        public const string EditView = "EditView";

        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(EditView);
            if (ViewGroup == EditView)
            {
                DetailListView();
            }
        }

        /// <summary>
        /// 明细列表视图
        /// </summary>
        public void DetailListView()
        {
            View.AddBehavior("SIE.Web.EMS.RunStandards.RunStandardsValueListBehavior");
            View.UseCommands("SIE.Web.EMS.RunStandards.Commands.AddRunStandardValuesCommand", WebCommandNames.Delete);
            using (View.OrderProperties())
            {
                View.Property(p => p.StandardType).ShowInList(80);
                View.Property(p => p.StandardUnit).ShowInList(80).Readonly();
                View.Property(p => p.Amount).ShowInList(80).UseSpinEditor(m => m.MinValue = 0);
                View.Property(p => p.AmountOfRound).ShowInList(80).Readonly(p => p.StandardType == StandardType.Period).UseSpinEditor(m => m.MinValue = 0);
                View.Property(p => p.TotalAmount).ShowInList(80).Readonly().UseSpinEditor(m => m.MinValue = 0);
                View.Property(p => p.LastExecuteDate).UseDateEditor().ShowInList(130).Readonly(p => p.StandardType != StandardType.Period);
                View.Property(p => p.NextExecuteDate).UseDateEditor().ShowInList(130).Readonly();
                View.Property(p => p.LeadTime).ShowInList(100);
            }
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.DisableEditing();
            using (View.OrderProperties())
            {
                View.Property(p => p.StandardType).ShowInList(80);
                View.Property(p => p.StandardUnit).ShowInList(80).Readonly();
                View.Property(p => p.Amount).ShowInList(80);
                View.Property(p => p.AmountOfRound).ShowInList(80);
                View.Property(p => p.TotalAmount).ShowInList(80);
                View.Property(p => p.LastExecuteDate).ShowInList(130);
                View.Property(p => p.NextExecuteDate).ShowInList(130);
                View.Property(p => p.LeadTime).ShowInList(100);
                View.Property(p => p.RunStandardNo).ShowInList(100);
            }
        }
    }
}
