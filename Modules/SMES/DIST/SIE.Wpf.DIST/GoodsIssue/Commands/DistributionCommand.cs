using SIE.DIST;
using SIE.Wpf.Command;
using System;

namespace SIE.Wpf.DIST
{
    /// <summary>
    /// 载具关联命令
    /// </summary>
    [Command(Label = "载具关联", GroupType = CommandGroupType.View)]
    public class DistributionCommand : ListViewCommand
    {
        /// <summary>
        /// 用于判断载具关联界面是否已经弹出
        /// </summary>
        protected bool isDisplay = false;

        /// <summary>
        /// 判断载具关联命令能否执行
        /// </summary>
        /// <param name="view">列表逻辑视图</param>
        /// <returns>能执行返回true，不能执行返回false</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            if (view.Current == null)
                return false;
            return true;
        }

        /// <summary>
        /// 载具关联命令执行方法
        /// </summary>
        /// <param name="view">列表逻辑视图</param>
        public override void Execute(ListLogicalView view)
        {
            var goodsIssue = view.Current as GoodsIssue;
            string key = CRT.Workbench.CreateKey(ViewConfig.ListView, typeof(GoodsIssue), goodsIssue);
            CRT.Workbench.ShowView(key, w =>
             {
                 w.Title = "载具关联".L10N();
                 var template = new GoodsIssueTemplate(goodsIssue);
                 template.ModuleKey = view.ModuleKey;
                 var ui = template.CreateUI();
                 return ui;
             });
        }
    }
}
