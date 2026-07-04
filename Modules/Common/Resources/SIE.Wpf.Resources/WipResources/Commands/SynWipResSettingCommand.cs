using SIE.Domain;
using SIE.Resources.WipResources;
using SIE.Wpf.Command;
using System;

namespace SIE.Wpf.Resources.WipResources.Commands
{
    /// <summary>
    /// 资源停用添加命令
    /// </summary>
    [Command(Label = "资源同步设置", GroupType = CommandGroupType.None)]
    internal class SynWipResSettingCommand : ListViewCommand
    {
        /// <summary>
        /// 命令执行
        /// </summary>
        /// <param name="view">逻辑视图</param>
        public override void Execute(ListLogicalView view)
        {
            var settingList = RF.GetAll<SynWipResSetting>(eagerLoad: new EagerLoadOptions().LoadWithViewProperty());

            ControlResult ui = CreateUI(view.ModuleKey);
            var listView = ui.MainView as ListLogicalView;
            listView.Data = settingList;

            string key = Guid.NewGuid().ToString("N");

            CRT.Workbench.ShowDialog(key, ui.Control, v =>
            {
                v.Title = this.Label.L10N();
                v.Width = 800;
                v.Height = 500;
                v.Closing += (o, e) =>
                {
                    if (v.Result == 0)
                        RF.Save(settingList);
                    else
                    {
                        if (listView.Data.IsDirty && !CRT.MessageService.AskQuestion("数据未保存，是否退出？".L10N()))
                            e.Cancel = true;
                    }
                };
            });
        }

        /// <summary>
        /// 创建UI控件
        /// </summary>
        /// <param name="moduleKey">ModuleKey</param>
        /// <param name="viewGroup">视图组</param>
        /// <returns>UI控件</returns>
        private ControlResult CreateUI(string moduleKey, string viewGroup = ViewConfig.ListView)
        {
            var template = new ListUITemplate(typeof(SynWipResSetting), viewGroup, moduleKey);
            template.BlocksDefined += (o, e) =>
            {
                var queryBlock = e.Blocks.Surrounders.Find(typeof(SynWipResSetting));
                if (queryBlock != null)
                    e.Blocks.Surrounders.Remove(queryBlock);

                if (e.Blocks.Children.Count > 0)
                    e.Blocks.Children.Clear();
            };

            var ui = template.CreateUI();
            ui.MainView.CommandsContainer.Visibility = System.Windows.Visibility.Collapsed;
            return ui;
        }
    }
}
