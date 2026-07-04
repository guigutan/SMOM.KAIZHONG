using SIE.Domain;
using SIE.MetaModel.View;
using SIE.Resources.ProcessTechs;
using SIE.Web.Resources.ProcessTechs.Commands;

namespace SIE.Web.Resources.ProcessTechs
{
    /// <summary>
    /// 制程工艺视图配置
    /// </summary>
    internal class ProcessTechViewConfig : WebViewConfig<ProcessTech>
    {
        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.HasDelegate(ProcessTech.CodeProperty);
            View.InlineEdit().UseDefaultCommands();
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.ReplaceCommands(WebCommandNames.Add, typeof(ProcessTechAddCommand).FullName);
            View.ReplaceCommands(WebCommandNames.Copy, typeof(ProcessTechCopyCommand).FullName);
            View.ReplaceCommands(WebCommandNames.Edit, "SIE.Web.Resources.ProcessTechs.Commands.ProcessTechEditCommand");
            View.ReplaceCommands(WebCommandNames.Save, typeof(ProcessTechSaveCommand).FullName);
            View.UseCommands(typeof(ProcessTechImportCommand).FullName);
            View.Property(p => p.Code).HasLabel("制程编号").Readonly(p => p.PersistenceStatus != PersistenceStatus.New)
                .UseListSetting(e => { e.HelpInfo = "新增状态可编辑"; });
            View.Property(p => p.Name).HasLabel("制程名称");
            View.Property(p => p.ProcessTechTypeId).HasLabel("制程类型");
            View.Property(p => p.ProcessSegmentId).HasLabel("工段");
            View.Property(p => p.IsBottleneck);
            View.Property(p => p.TransferTime).HasLabel("转款时间(秒)").Readonly(p => !p.IsScheduling).UseSpinEditor(p =>
            {
                  p.MinValue = 0;
                  p.AllowDecimals = true;
              });
            View.Property(p => p.SAM).HasLabel("默认工艺定额(秒/单位)").Readonly(p => !p.IsScheduling ).UseSpinEditor(p =>
            {
                p.MinValue = 0;
                p.AllowDecimals = true;
            });
#pragma warning restore S1125 // Boolean literals should not be redundant
            View.Property(p => p.WorkingHours).UseSpinEditor(p => { p.MinValue = 1; p.DecimalPrecision = 0; }).DefaultValue(1);
            View.Property(p => p.OutAssistDay).UseSpinEditor(p => { p.MinValue = 1; p.DecimalPrecision = 0; });
        }

        /// <summary>
        /// 配置下拉视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.Code).HasLabel("制程编号");
            View.Property(p => p.Name).HasLabel("制程名称");
            View.Property(p => p.ProcessTechTypeId).HasLabel("制程类型");
            //View.Property(p => p.IsScheduling);
        }

        /// <summary>
        /// 配置数据导入的视图
        /// </summary>
        protected override void ConfigImportView()
        {
            View.Property(p => p.Code).HasLabel("制程编号");
            View.Property(p => p.Name).HasLabel("制程名称");
            View.Property(p => p.ProcessTechTypeId).HasLabel("制程类型");
            View.Property(p => p.ProcessSegmentId).HasLabel("工段");
            View.Property(p => p.IsBottleneck).HasLabel("是否瓶颈");
            View.Property(p => p.TransferTime).HasLabel("转款时间(秒)");
            View.Property(p => p.SAM).HasLabel("默认工艺定额(秒/单位)");
            View.Property(p => p.WorkingHours).HasLabel("工作时长(时)");
            View.Property(p => p.OutAssistDay).HasLabel("外协时长(天)");
        }
    }
}
