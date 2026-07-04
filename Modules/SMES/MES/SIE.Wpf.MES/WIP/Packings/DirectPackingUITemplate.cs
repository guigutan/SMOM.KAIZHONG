using DevExpress.Xpf.Core;
using SIE.Domain;
using SIE.Packages;
using SIE.Packages.Packings;
using SIE.Packages.Packings.Strategys;
using SIE.Wpf.MES.WIP.NewPackages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SIE.Wpf.MES.WIP.Packings
{
    /// <summary>
    /// 直接包装采集UI模板
    /// </summary>
    public class DirectPackingUITemplate : CollectionUITemplate
    {
        /// <summary>
        /// 包装关系视图
        /// </summary>
        ListLogicalView packingRelationView;

        /// <summary>
        /// 包装ViewModel
        /// </summary>
        DirectPackingViewModel model;

        /// <summary>
        /// 构造函数
        /// </summary>
        public DirectPackingUITemplate() : base(typeof(DirectPackingViewModel))
        {

        }

        /// <summary>
        /// UI创建
        /// </summary>
        /// <param name="ui"></param>
        protected override void OnUIGenerated(ControlResult ui)
        {
            model = new DirectPackingViewModel();
            var layout = ui.Control as DockLayout;

            var tabs = new DXTabControl();
            tabs.Margin = new Thickness(5, 5, 5, 0);
            tabs.Items.Add(CreateTabItem("条码明细", CreateDetailListControl(ui.MainView, model.PackageSnRecordList, WPFViewConfig.ListView)));

            //tabs.Items.Add(CreateTabItem("物料标签", CreateDetailListControl(ui.MainView, model.PackingRelations, PackingRelationViewConfig.DirectPackingView)));
            tabs.Items.Add(CreateTabItem("包装规则", CreateDetailListControl(ui.MainView, model.PackageRuleDetailList, "Packing")));
            tabs.Items.Add(CreateTabItem("消息列表", CreateMessagerControl(model)));
            layout.Children.Add(CreateOperationControl(model.Workstation));
            layout.Children.Add(tabs);
            ui.MainView.Data = model;
            model.ReloadDirectPackingSnRelation();
            base.OnUIGenerated(ui);
        }
    }
}
