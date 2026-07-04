using SIE.EMS.Common;

namespace SIE.Web.EMS.Common
{
    /// <summary>
    /// 部门的视图配置
    /// </summary>
    public class DepartmentSelectViewConfig : WebViewConfig<DepartmentSelect>
    {
        /// <summary>
        /// 列表视图配置
        /// </summary>
        protected override void ConfigListView()
        {
            View.DisableEditing();

            View.Property(p => p.Code).ShowInList(width: 300);
            View.Property(p => p.Name);
            View.Property(p => p.Level);
            View.Property(p => p.IsResource).UseCheckEditor().Readonly(p => !p.LevelIsResource).HasLabel("资源").Show()
                .UseListSetting(e => { e.HelpInfo = "设为资源可编辑"; });
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
        }


        /// <summary>
        /// 查询视图配置
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Name);
        }
    }
}
