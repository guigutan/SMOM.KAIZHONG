using SIE.MetaModel.View;
using SIE.Resources.ShiftTypes;
using SIE.Web.Resources.ShiftTypes.Commands;

namespace SIE.Web.Resources.ShiftTypes
{
    /// <summary>
    /// 班次休息
    /// </summary>
    public class ShiftRestViewConfig : WebViewConfig<ShiftRest>
    {
        /// <summary>
        /// 列表视图配置
        /// </summary>
        protected override void ConfigListView()
        {
            View.InlineEdit().UseDefaultCommands()
                .RemoveCommands(WebCommandNames.Save, WebCommandNames.Copy);
            View.ReplaceCommands(WebCommandNames.Add, typeof(ShiftResetAddCommand).FullName);
            View.Property(p => p.Type);
            View.Property(p => p.BeginTime).UseTimeEditor();
            View.Property(p => p.EndTime).UseTimeEditor();
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
        }
    }
}
