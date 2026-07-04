using SIE.Defects;
using SIE.Wpf.Command;
using System;

namespace SIE.Wpf.Defects.Commands
{
    /// <summary>
    /// 数据维护命令
    /// </summary>
    public abstract class ServiceDataCommand : ClientCommand
    {
        /// <summary>
        /// 维护数据的实体类型
        /// </summary>
        protected abstract Type EntityType { get; }

        /// <summary>
        /// 自动加载数据，默认true
        /// </summary>
        protected virtual bool TryAutoLoadData { get { return true; } }

        /// <summary>
        /// 视图组，默认ListView
        /// </summary>
        protected virtual string ViewGroup { get { return ViewConfig.ListView; } }

        /// <summary>
        /// 标题，默认取Label，内部实现已调用L10N()
        /// </summary>
        protected virtual string Title { get; }

        /// <summary>
        /// 执行视图创建
        /// </summary>
        protected override void ExecuteCore()
        {
            string key = CRT.Workbench.CreateKey(ViewGroup, EntityType, null);
            CRT.Workbench.ShowView(key, w =>
            {
                w.Title = Title.IsNullOrEmpty() ? Meta.Label.L10N() : Title.L10N();
                var ui = CreateUI();
                if (TryAutoLoadData)
                    (ui.MainView.QueryView)?.TryExecuteQuery();
                w.Closing += (s, e) =>
                {
                    if (ui.MainView.Data.IsDirty)
                        e.Cancel = !CRT.MessageService.AskQuestion("数据未保存，确定退出吗".L10N());
                };
                return ui;
            });
        }

        /// <summary>
        /// 创建控件结果
        /// </summary>
        /// <returns>控件结果</returns>
        protected virtual ControlResult CreateUI()
        {
            var template = new ListUITemplate(EntityType, ViewGroup);
            var ui = template.CreateUI();
            return ui;
        }
    }

    /// <summary>
    /// 缺陷代码分类维护命令
    /// </summary>
    [Command(ImageName = "Repair", Label = "分类维护", ToolTip = "缺陷代码分类维护", GroupType = 50)]
    public class DefectCategoryCommand : ServiceDataCommand
    {
        /// <summary>
        /// 维护数据的实体类型
        /// </summary>
        protected override Type EntityType => typeof(DefectCategory);

        /// <summary>
        /// 标题
        /// </summary>
        protected override string Title => "缺陷代码分类维护";
    }
}