using SIE.Fixtures.MaintainTasks;
using SIE.MetaModel.View;

namespace SIE.Web.Fixtures.MaintainTasks
{
    /// <summary>
    /// 更换记录-界面
    /// </summary>
    public class MaintainChangeRecordViewConfig : WebViewConfig<MaintainChangeRecord>
    {
        /// <summary>
        /// 保养任务维护界面-更换记录
        /// </summary>
        public const string EditChangeRecord = "EditChangeRecord";

        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(EditChangeRecord);
            View.AssignAuthorize(typeof(MaintainTask));
            if (ViewGroup == EditChangeRecord)
                EditChangeRecordView();
        }

        /// <summary>
        /// 配置列表界面
        /// </summary>
        protected override void ConfigListView()
        {
            View.Property(p => p.Code).Readonly();
            View.Property(p => p.Name).Readonly();
            View.Property(p => p.Qty).Readonly();
            View.Property(p => p.Description).Readonly();
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
        }

        /// <summary>
        /// 保养任务维护界面-更换记录
        /// </summary>
        protected void EditChangeRecordView()
        {
            View.UseCommands(WebCommandNames.Add, WebCommandNames.Edit, WebCommandNames.Delete);
            using (View.OrderProperties())
            {
                View.Property(p => p.Code).Show();
                View.Property(p => p.Name).Show();
                View.Property(p => p.Qty).UseSpinEditor(p => { p.AllowDecimals = false; p.AllowNegative = false; }).Show();
                View.Property(p => p.Description).Show();
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            }
        }
    }
}
