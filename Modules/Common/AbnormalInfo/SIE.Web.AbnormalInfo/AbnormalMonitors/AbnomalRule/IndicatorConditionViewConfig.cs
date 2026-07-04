using SIE.AbnormalInfo.AbnormalMonitors;
using SIE.AbnormalInfo.AnomalyMonitors; 

namespace SIE.Web.AbnormalInfo.AnomalyMonitors
{
	/// <summary>
	/// 规则计算指标条件视图配置
	/// </summary>
	internal class IndicatorConditionViewConfig : WebViewConfig<IndicatorCondition>
	{
        ///<summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(AbnormalDecisionRule));
        }

        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
		{
			View.ClearCommands();
			View.WithoutPaging();
			View.Property(p => p.Code).Readonly();
			View.Property(p => p.LayerName).ShowInList(width: 350).Readonly().ShowInList(width:200).HasLabel("层别条件");
			View.Property(p => p.IndicatorValue);
			View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
			View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
			View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
			View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
		}
		
	}
}