using DevExpress.Xpf.Core;
using SIE.Wpf.MES.BatchWIP.Packings;
using SIE.Wpf.MES.WIP.Packings;
using SIE.Wpf.MES.WIP.PackRecombine;
using System;
using System.Windows;

namespace SIE.Wpf.MES.BatchWIP.PackRecombine
{
    /// <summary>
    /// 批次包装拆合模板
    /// </summary>
    public class BatchPackRecombineUITemplate : PackRecombineBaseUITemplate
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public BatchPackRecombineUITemplate() 
        {

        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="entityType">实体类型</param>
        public BatchPackRecombineUITemplate(Type entityType) : base(entityType)
        {
        }

        /// <summary>
        /// 添加当前模块中 UI 的初始化逻辑。
        /// 当使用自动生成的 UI 时，此方法会被调用。
        /// </summary>
        /// <param name="ui">控件结果</param>
        protected override void OnUIGenerated(ControlResult ui)
        {
            var model = new BatchPackRecombineViewModel();
            var tabs = new DXTabControl();
            tabs.Margin = new Thickness(5, 5, 5, 0);
            tabs.Items.Add(CreateTabItem("操作记录", CreateListControl(ui.MainView, model.DetailList, ViewConfig.ListView)));
            tabs.Items.Add(CreateTabItem("包装规则明细", CreateListControl(ui.MainView, model.RuleDetailList, PackageRuleDetailViewConfig.RecombineView)));
            tabs.Items.Add(CreateTabItem("消息列表", CreateMessagerControl(model)));

            var layout = ui.Control as PackRecombineLayout;
            layout.InitChildrenControl(tabs);
            layout.InitRelationControl(CreateListControl(ui.MainView, model.PackingRelationList, BatchPackingRelationViewConfig.PackRecombineView));
            ui.MainView.Data = model;
            base.OnUIGenerated(ui);
        }
    }
}