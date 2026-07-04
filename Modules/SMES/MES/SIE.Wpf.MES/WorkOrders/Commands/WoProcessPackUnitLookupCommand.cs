using DevExpress.Xpf.Grid;
using SIE.Common;
using SIE.Domain;
using SIE.MES.WorkOrders;
using SIE.MetaModel.View;
using SIE.Tech.Processs;
using SIE.Wpf.Command;
using System;
using System.Linq;

namespace SIE.Wpf.MES.WorkOrders.Commands
{
    /// <summary>
    /// 关系选择命令
    /// </summary>
    [Command(ImageName = "PlaylistCheck", Label = "选择", ToolTip = "选择", GroupType = CommandGroupType.Edit, DisplayMode = CommandDisplayMode.LabelAndIcon)]
    public class WoProcessPackUnitLookupCommand : ListViewCommand
    {
        /// <summary>
        /// 执行条件
        /// </summary>
        /// <param name="view">逻辑视图</param>
        /// <returns>条件结果</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            return base.CanExecute(view) && view.Parent?.Current != null;
        }

        /// <summary>
        /// 执行方法
        /// </summary>
        /// <param name="view">逻辑视图</param>
        public override void Execute(ListLogicalView view)
        {
            var detail = view.Parent.Current as WorkOrderPackageRuleDetail;
            var wo = detail?.WorkOrder;
            if (wo == null || wo.Version == null)
                return;
            var processList = wo.Version.ProcessList.Where(x => x.Process != null && (x.Process.Type == ProcessType.Packing || x.Process.Type == ProcessType.BatchPacking))
                .Select(p => p.Process).AsEntityList();
            ShowSelectionDialog(processList);
        }

        /// <summary>
        /// 弹出选择框
        /// </summary>
        /// <param name="process">待选择数据</param>
        /// <returns>视图结果</returns>
        int ShowSelectionDialog(EntityList<Process> process)
        {
            var selelctionView = AutoUI.ViewFactory.CreateListView(typeof(Process), PackingSelectProcessViewConfig.PackingSelectProcessView, moduleKey: View.ModuleKey);
            selelctionView.Control.SelectionMode = MultiSelectMode.Row;
            (selelctionView.Control.View as TableView).ShowCheckBoxSelectorColumn = true;
            selelctionView.Data = process;

            var selectedUnits = View.Data as EntityList<WorkOrderProcessPackingUnit>;
            if (selectedUnits != null && selectedUnits.Count > 0)
            {
                var temp = process.Where(p => selectedUnits.Select(q => q.ProcessId).Contains(p.Id)).ToArray();
                selelctionView.SelectEntities(temp);
            }

            var key = Guid.NewGuid().ToString("N");
            return CRT.Workbench.ShowDialog(key, selelctionView.Control, v =>
            {
                v.Title = "选择 工序".L10N();
                v.Width = 500;
                v.Height = 300;
                v.Closing += (o, e) =>
                {
                    if (v.Result == 0)
                        OnAceppting(selelctionView);
                };
            });
        }

        /// <summary>
        /// 确定后事件
        /// </summary>
        /// <param name="selelctionView">选择视图</param>
        void OnAceppting(ListLogicalView selelctionView)
        {
            var detail = View.Parent.Current as WorkOrderPackageRuleDetail;
            var selectingUnits = selelctionView.SelectedEntities.OfType<Process>();
            var packingUnitList = new EntityList<WorkOrderProcessPackingUnit>();
            selectingUnits.ForEach(p =>
            {
                var woPackingUnit = new WorkOrderProcessPackingUnit()
                {
                    PackageRuleId = detail.Id,
                    ProcessId = p.Id
                };

                woPackingUnit.ExtValues["ProcessId_Display"] = p.Name;
                packingUnitList.Add(woPackingUnit);
            });

            View.Data.Clear();
            View.Data.AddRange(packingUnitList);
        }
    }

    /// <summary>
    /// 包装选择工序视图配置
    /// </summary>
    internal class PackingSelectProcessViewConfig : WPFViewConfig<Process>
    {
        /// <summary>
        /// 包装选择工序视图
        /// </summary>
        public const string PackingSelectProcessView = "PackingSelectProcessView";

        /// <summary>
        /// 配置默认视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(PackingSelectProcessView);
            if (ViewGroup == PackingSelectProcessView)
                ConfigPackingSelectProcessView();
        }

        /// <summary>
        /// 配置包装选择工序视图
        /// </summary>
        private void ConfigPackingSelectProcessView()
        {
            View.Property(p => p.Name).ShowInList();
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
        }
    }
}