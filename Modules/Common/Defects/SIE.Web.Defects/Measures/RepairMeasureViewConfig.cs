using SIE.Defects.Measures;
using SIE.MetaModel.View;

namespace SIE.Web.Defects.Measures
{
    /// <summary>
    /// 维修措施视图配置
    /// </summary>
    internal class RepairMeasureViewConfig : WebViewConfig<RepairMeasure>
    {
        protected override void ConfigListView()
        {
            View.InlineEdit();
            View.UseDefaultCommands();
            View.ReplaceCommands(WebCommandNames.Copy, "SIE.Web.Defects.Measures.Commands.RepairMeasureCopyCommand");
            View.Property(p => p.Code).ShowInList(150).Readonly(p => p.PersistenceStatus != Domain.PersistenceStatus.New)
                .UseListSetting(e => { e.HelpInfo = "新增状态可编辑"; });
            View.Property(p => p.Name).ShowInList(150);
            View.Property(p => p.Description).ShowInList(200);
            View.Property(p => p.CreateByName);
            View.Property(p => p.CreateDate).ShowInList(150);
            View.Property(p => p.UpdateByName);
            View.Property(p => p.UpdateDate).ShowInList(150);
        }
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Name);
            View.Property(p => p.Description);
        }
        protected override void ConfigQueryView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Name);
        }
    }
}
