using DevExpress.Xpf.Grid;
using SIE.Domain;
using SIE.MES.WorkOrders;
using SIE.MetaModel.View;
using SIE.ObjectModel;
using SIE.Packages;
using SIE.Wpf.Command;
using System;
using System.Linq;

namespace SIE.Wpf.MES.WorkOrders.Commands
{
    /// <summary>
    /// 物料包装规则选择包装规则按钮
    /// </summary>
    [Command(ImageName = "PlaylistCheck", Label = "选择", ToolTip = "选择", GroupType = 10, DisplayMode = CommandDisplayMode.LabelAndIcon)]
    class SelectPackageRuleCommand : ListViewCommand
    {
        /// <summary>
        /// 执行方法
        /// </summary>
        /// <param name="view">视图</param>
        public override void Execute(ListLogicalView view)
        {
            var template = new ListUITemplate(typeof(ItemPackageRule), WorkOrderPackageRuleViewConfig.WoSelectPackageRule, view.Parent.ModuleKey);
            var ui = template.CreateUI();
            var listView = ui.MainView as ListLogicalView;
            var control = listView.Control as GridControl;
            var wo = view.Parent.Current as WorkOrder;
            ui.MainView.QueryView.DataProvider = (c) =>
            {
                return RT.Service.Resolve<PackageController>().GetItemPackageRules(wo.ProductId, c as CriteriaQuery);
            };

            var result = CRT.Workbench.ShowDialog(ui, w =>
            {
                w.Title = "选择 产品包装规则".L10N();
                w.MinWidth = 500;
                w.MinHeight = 400;
                ui.MainView.QueryView.TryExecuteQuery();
                w.Closing += (o, e) =>
                {
                    if (w.Result == 0)
                    {
                        if (listView.SelectedEntities.Count == 0)
                        {
                            CRT.MessageService.ShowMessage("请选择一行".L10N());
                            e.Cancel = true;
                        }
                        else
                            SaveItemPackageRule(view, listView);
                    }
                };
            });
        }

        /// <summary>
        /// 选择物料包装规则，保存规则明细到工单包装规则
        /// </summary>
        /// <param name="view"></param>
        /// <param name="listView"></param>
        protected void SaveItemPackageRule(ListLogicalView view, ListLogicalView listView)
        {
            var workorder = view.Parent.Current as WorkOrder;
            var selectedRule = listView.SelectedEntities.OfType<ItemPackageRule>().FirstOrDefault();
            var rules = RT.Service.Resolve<WorkOrderPropertyChanged>().SetPackageRuleDetail(selectedRule, workorder.Id);
            workorder.PackageRuleDetailList.Clear();
            workorder.PackageRuleDetailList.AddRange(rules);
            view.Data = workorder.PackageRuleDetailList;
            view.RefreshControl();
        }

        /// <summary>
        /// 选择了产品后才可用
        /// </summary>
        /// <param name="view">视图</param>
        /// <returns>true.false</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            var entity = view.Parent.Current as WorkOrder;
            return entity != null && entity.ProductId != 0;
        }
    }
}