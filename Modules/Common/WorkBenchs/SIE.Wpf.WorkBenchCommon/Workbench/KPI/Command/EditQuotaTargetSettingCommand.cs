using SIE.MetaModel.View;
using SIE.WorkBenchCommon.Workbench.KPI;
using SIE.Wpf.Command;
using System;

namespace SIE.Wpf.WorkBenchCommon.Workbench.KPI
{
    /// <summary>
    /// 指标定义设定修改命令
    /// </summary>
    [Command(Label = "修改", GroupType = 10, ImageName = "EditEntity", ToolTip = "修改当前行数据", Location = CommandLocation.All, Gestures = "Ctrl+Shift+E")]
    class EditQuotaTargetSettingCommand : ListEditCommand
    {
        /// <summary>
        /// 命令执行代码块
        /// </summary>
        /// <param name="view">详细逻辑视图</param>
        public override void Execute(ListLogicalView view)
        {
            QuotaTargetSetting quotatargetsetting = view.Current as QuotaTargetSetting;

            string key = CRT.Workbench.CreateKey(ViewConfig.DetailsView, typeof(QuotaTargetSetting), null);

            CRT.Workbench.ShowView(key, v =>
            {
                v.Title = Meta.Label.L10N();
                var template = new DetailsUITemplate<QuotaTargetSetting>(view.ModuleKey);
                var ui = template.CreateUI();
                var detailView = ui.MainView as DetailLogicalView;
                detailView.Data = quotatargetsetting;
                v.Closing += (o, e) =>
                {
                    if (ui.MainView.Data.IsDirty)
                    {
                        if (CRT.MessageService.AskQuestion("数据未保存，确定退出吗".L10N()))
                        {
                            e.Cancel = false;
                        }
                        else
                        {
                            e.Cancel = true;
                        }
                    }
                };
                return ui;
            });
        }
    }
}
