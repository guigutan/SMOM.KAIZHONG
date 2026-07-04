using SIE.Domain;
using SIE.MetaModel.View;
using SIE.Resources.ShiftTypes;

namespace SIE.Web.Resources.ShiftTypes
{
    /// <summary>
    /// 班次视图配置
    /// </summary>
    public class ShiftViewConfig : WebViewConfig<Shift>
    {
        /// <summary>
        /// 列表视图配置
        /// </summary>
        protected override void ConfigListView()
        {
            View.InlineEdit().UseDefaultCommands()
                .RemoveCommands(WebCommandNames.Save, WebCommandNames.Copy);
            View.AddBehavior("SIE.Web.Resources.ShiftTypes.Behaviors.ShiftBehavior");
            View.ReplaceCommands(WebCommandNames.Add, "SIE.Web.Resources.ShiftTypes.Commands.ShiftAddCommand");
            View.ReplaceCommands(WebCommandNames.Edit, "SIE.Web.Resources.ShiftTypes.Commands.ShiftEditCommand");
            View.Property(p => p.Code).Readonly(f => f.PersistenceStatus != PersistenceStatus.New)
                .UseListSetting(e => { e.HelpInfo = "新增状态可编辑"; });
            View.Property(p => p.Name);
            View.Property(p => p.BeginTime).UseTimeEditor();
            View.Property(p => p.EndTime).UseTimeEditor();
            View.Property(p => p.IsOverDay).Readonly();
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            View.ChildrenProperty(p => p.ShiftRestList);
        }

        /// <summary>
        /// 下拉视图配置
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.Name);
            View.Property(p => p.BeginTime).UseTimeEditor();
            View.Property(p => p.EndTime).UseTimeEditor();
        }
    }
}
