using SIE.Domain;
using SIE.MetaModel.View;
using SIE.Resources.ShiftTypes;
using SIE.Web.Resources.ShiftTypes.Commands;

namespace SIE.Web.Resources.ShiftTypes
{
    /// <summary>
    /// 班制视图配置
    /// </summary>
    public class ShiftTypeViewConfig : WebViewConfig<ShiftType>
    {
        /// <summary>
        /// 列表视图配置
        /// </summary>
        protected override void ConfigListView()
        {
            View.InlineEdit().UseDefaultCommands();
            View.ReplaceCommands(WebCommandNames.Copy, typeof(SetDefaultCommand).FullName);
            View.UseCommands("SIE.Web.Resources.ShiftTypes.Commands.ShowCalendarSchemesCommand");
            View.UseChildrenAsHorizontal();
            View.Property(p => p.Code).Readonly(f => f.PersistenceStatus != PersistenceStatus.New)
                .UseListSetting(e => { e.HelpInfo = "新增状态可编辑"; });
            View.Property(p => p.Name);
            View.Property(p => p.IsDefault).DefaultValue(0).Readonly();
            //View.Property(p => p.IsWeekend).DefaultValue(0);
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            View.ChildrenProperty(p => p.ShiftList);
        }

        /// <summary>
        /// 查询视图配置
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Name);
        }

        /// <summary>
        /// 下拉视图配
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Name);
        }
    }
}
