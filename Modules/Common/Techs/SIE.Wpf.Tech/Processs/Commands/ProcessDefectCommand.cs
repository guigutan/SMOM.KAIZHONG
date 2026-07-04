using SIE.Domain;
using SIE.MetaModel.View;
using SIE.Tech.Processs;
using SIE.Wpf.Command;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Wpf.Tech.Processs.Commands
{
    /// <summary>
    /// 缺陷信息选择按钮（列表视图调用）
    /// </summary>
    [Command(ImageName = "PlaylistCheck",
        Label = "选择", ToolTip = "选择",
        GroupType = CommandGroupType.Edit,
        DisplayMode = CommandDisplayMode.LabelAndIcon)]
    public class ProcessDefectCommand : LookupCommand
    {
        /// <summary>
        /// 能否执行弹出
        /// </summary>
        /// <param name="view">当前列表逻辑视图</param>
        /// <returns>bool</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            //新增状态、装配、包装、维修类型工序不允许录缺陷信息 
            var parent = view.Parent.Current as Process;
            if (parent == null || parent.PersistenceStatus == PersistenceStatus.New || parent.Type == ProcessType.Assembly || parent.Type == ProcessType.Packing || parent.Type == ProcessType.Fix)
                return false;
            return true;
        }
    }

    /// <summary>
    /// 缺陷信息选择按钮(明细视图调用)
    /// </summary>
    [Command(ImageName = "PlaylistCheck",
        Label = "选择", ToolTip = "选择",
        GroupType = CommandGroupType.Edit,
        DisplayMode = CommandDisplayMode.LabelAndIcon)]
    public class SelProcessDefectCommand : ProcessDefectCommand
    {
        /// <summary>
        /// 当前选中的缺陷信息
        /// </summary>
        private EntityList<ProcessDefect> currProcessDefect = new EntityList<ProcessDefect>();

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="view">列表逻辑视图</param>
        public override void Execute(ListLogicalView view)
        {
            currProcessDefect = View.Data as EntityList<ProcessDefect>;
            base.Execute(view);
        }

        /// <summary>
        /// 点击确定
        /// </summary>
        protected override void OnAccept()
        {
            List<Entity> proDefectList = new List<Entity>();
            proDefectList.AddRange(SelectedView.Data.DeletedList);
            proDefectList.AddRange(SelectedView.Data.OfType<Entity>().Where(f => f.PersistenceStatus == PersistenceStatus.New));
            proDefectList.ForEach(p =>
            {
                if (!View.Data.DeletedList.Contains(p) && !View.Data.Contains(p))
                {
                    View.Data.Add(p);
                }
            });

            currProcessDefect = View.Data as EntityList<ProcessDefect>;
        }

        /// <summary>
        /// 加载已选择数据
        /// </summary>
        /// <param name="parent">父</param>
        /// <returns>已选择数据</returns>
        protected override IDomainComponent LoadSelectedViewDataCore(Entity parent)
        {
            return currProcessDefect;
        }
    }

    /// <summary>
    /// 缺陷信息明细时删除数据
    /// </summary>
    [Command(ImageName = "DeleteEntity", Label = "删除", ToolTip = "删除数据", Gestures = "Delete", Location = CommandLocation.All, GroupType = 10)]
    public class DelProcessDefectCommand : ListDeleteCommand
    {
        /// <summary>
        /// 执行删除逻辑
        /// </summary>
        /// <param name="view">视图对象</param>
        public override void Execute(ListLogicalView view)
        {
            ProcessDefect selProcessDefect = view.Current as ProcessDefect;
            if (selProcessDefect != null)
            {
                if (!CRT.MessageService.AskQuestion(string.Format("你确认删除选择的{0}条数据吗？".L10N(), view.SelectedEntities.Count)))
                {
                    return;
                }

                view.Data.Remove(selProcessDefect);
            }
        }
    }
}
