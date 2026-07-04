using DevExpress.Xpf.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SIE.Wpf.MES.WIP.NewPackages
{
    public class NewPackageUITemplate : CollectionUITemplate
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public NewPackageUITemplate() : base(typeof(NewPackageViewModel))
        {
        }

        /// <summary>
        /// UI创建
        /// </summary>
        /// <param name="ui">UI</param>
        protected override void OnUIGenerated(ControlResult ui)
        {
            var model = new NewPackageViewModel();
            var layout = ui.Control as DockLayout;
            var tabs = new DXTabControl();
            tabs.Margin = new Thickness(5, 5, 5, 0);
            tabs.Items.Add(CreateTabItem("条码明细", CreateDetailListControl(ui.MainView, model.PackageSnRecordList, WPFViewConfig.ListView)));
            tabs.Items.Add(CreateTabItem("包装规则", CreateDetailListControl(ui.MainView, model.PackageRuleDetailList, "Packing")));
            tabs.Items.Add(CreateTabItem("采集记录", CreateDetailListControl(ui.MainView, model.CollectDetailList)));
            //如果不用显示消息列表，则注释下面这句
            tabs.Items.Add(CreateTabItem("消息列表", CreateMessagerControl(model)));
            
            layout.Children.Add(CreateOperationControl(model.Workstation));
            layout.Children.Add(tabs);
            ui.MainView.Data = model;
            model.ReloadPackingRelation();
            base.OnUIGenerated(ui);
        }
    }
}