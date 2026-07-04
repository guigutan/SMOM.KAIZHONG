using SIE.Items;
using SIE.Wpf.Command;
using System;

namespace SIE.Wpf.Items.Commands
{
    /// <summary>
    /// 族分类维护命令
    /// </summary>
    [Command(Label = "族分类维护", GroupType = CommandGroupType.Edit)]
    public class ProductFamilyCommand : ListViewCommand
    {
        /// <summary>
        /// 按钮运行中心
        /// </summary>
        /// <param name="view">逻辑视图</param>
        public override void Execute(ListLogicalView view)
        {
            string key = CRT.Workbench.CreateKey(ViewConfig.ListView, typeof(ProductFamilyCategoryViewConfig), null);

            CRT.Workbench.ShowView(key, v =>
            {
                v.Title = "产品族分类维护".L10N();
                var ui =
                    new ListUITemplate(typeof(ProductFamilyCategory), ViewConfig.ListView, view.ModuleKey).CreateUI();
                var query = ui.MainView.QueryView;
                if (query != null)
                {
                    query.TryExecuteQuery();
                }

                //窗口在数据改变后再关闭窗口，需要提示用户是否保存。
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
