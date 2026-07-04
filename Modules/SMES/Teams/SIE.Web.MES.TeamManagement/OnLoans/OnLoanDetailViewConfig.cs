using SIE.MES.TeamManagement.OnLoans;

namespace SIE.Web.MES.TeamManagement.OnLoans
{
    /// <summary>
    /// 借调明细视图配置类
    /// </summary>
    internal class OnLoanDetailViewConfig : WebViewConfig<OnLoanDetail>
    {
        /// <summary>
        /// 配置通用视图
        /// </summary>
        protected override void ConfigView()
        {
            View.ClearCommands();
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            using (View.OrderProperties())
            {
                View.Property(x => x.RowIndex);
                View.Property(x => x.State);
                View.Property(x => x.OperatorName);
                View.Property(x => x.Remark);
                View.Property(x => x.OperateDate);
            }
        }

        /// <summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.FormEdit(); //.UseDetail(1);
            View.Property(x => x.Id).UseDisplayEditor(p => { p.XType = "flowEditor"; }).ShowInDetail(width: "auto", height: "auto", columnSpan: 10, hideLabel: true).HasLabel(string.Empty);
        }

        /// <summary>
        /// 配置选择视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            using (View.OrderProperties())
            {
                View.Property(x => x.RowIndex);
                View.Property(x => x.State);
                View.Property(x => x.Operator);
                View.Property(x => x.Remark);
                View.Property(x => x.OperateDate);
            }
        }
    }
}
