using DevExpress.Xpf.Core;
using SIE.Wpf.MES.WIP.Packings;
using System;
using System.Windows;

namespace SIE.Wpf.MES.WIP.PackRecombine
{
    /// <summary>
    /// 单体包装拆合模板
    /// </summary>
    public class PackRecombineUITemplate : PackRecombineBaseUITemplate
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public PackRecombineUITemplate() 
        {

        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="entityType">实体类型</param>
        public PackRecombineUITemplate(Type entityType) : base(entityType)
        {
        }

        /// <summary>
        /// 添加当前模块中 UI 的初始化逻辑。
        /// 当使用自动生成的 UI 时，此方法会被调用。
        /// </summary>
        /// <param name="ui">控件结果</param>
        protected override void OnUIGenerated(ControlResult ui)
        {
            var model = new PackRecombineViewModel();
            var tabs = new DXTabControl();
            tabs.Margin = new Thickness(5, 5, 5, 0);
            tabs.Items.Add(CreateTabItem("物料标签", CreateListControl(ui.MainView, model.ItemLabelList, ItemLabelViewConfig.PackRecombineView)));
            tabs.Items.Add(CreateTabItem("操作记录", CreateListControl(ui.MainView, model.DetailList, ViewConfig.ListView)));
            tabs.Items.Add(CreateTabItem("包装规则明细", CreateListControl(ui.MainView, model.RuleDetailList, PackageRuleDetailViewConfig.RecombineView)));
            var layout = ui.Control as PackRecombineLayout;
            layout.InitChildrenControl(tabs);
            layout.InitRelationControl(CreateListControl(ui.MainView, model.PackingRelationList, PackingRelationViewConfig.RecombineView));
            ui.MainView.Data = model;
            base.OnUIGenerated(ui);
        }
    }
}