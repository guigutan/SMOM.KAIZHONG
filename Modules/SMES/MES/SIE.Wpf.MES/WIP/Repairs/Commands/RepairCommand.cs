using SIE.Defects;
using SIE.Defects.Measures;
using SIE.Domain;
using SIE.MES.WIP.Repairs;
using SIE.Security;
using SIE.Wpf.Command;
using SIE.Wpf.MES.Controls;
using System;
using System.Linq;
using System.Windows.Controls;

namespace SIE.Wpf.MES.WIP.Repairs.Commands
{
    /// <summary>
    /// 维修命令
    /// </summary>
    [Command(ImageName = "Repair", Label = "维修", ToolTip = "录入维修信息")]
    public class RepairCommand : ListViewCommand
    {
        /// <summary>
        /// 判断维修命令能否执行
        /// </summary>
        /// <param name="view">列表逻辑视图</param>
        /// <returns>能执行返回true，不能执行返回false</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            var defect = view.Current as RepairDefectViewModel;
            return defect != null && !defect.IsFixed;
        }

        /// <summary>
        /// 维修命令执行方法
        /// </summary>
        /// <param name="view">列表逻辑视图</param>
        public override void Execute(ListLogicalView view)
        {
            var defect = view.Current as RepairDefectViewModel;
            if (defect == null) return;
            var panel = CreateControl(defect);
            var result = CRT.Workbench.ShowDialog(Guid.NewGuid().ToString(), panel, w =>
             {
                 w.Title = "维修录入".L10N();
                 w.Height = 600;
                 w.Width = 1100;
             });
            if (result == 0)
            {
                var measureControl = (panel.Children[1] as MeasureControl);
                var measures = measureControl.SelectedValues;
                var control = (panel.Children[2] as ResponsibilityControl);
                var reponsibilities = control.SelectedValue.Select(p => p.Responsibility);
                defect.MeasureList.Clear();
                defect.ResponsibilityList.Clear();
                defect.MeasureList.AddRange(measures);
                defect.ResponsibilityList.AddRange(reponsibilities);
                defect.Remark = control.Remark;
                defect.NotifyAllPropertiesChanged();
            }
        }

        /// <summary>
        /// 创建维修换料控件
        /// </summary>
        /// <param name="defect">不良信息视图模型</param>
        /// <returns>控件</returns>
        private DockPanel CreateControl(RepairDefectViewModel defect)
        {
            var moduleKey = RT.Service.Resolve<IFindModule>().FindModuleKey(typeof(RepairViewModel));
            var panel = new DockPanel();
            var detailView = AutoUI.ViewFactory.CreateDetailView(typeof(RepairDefectViewModel), RepairDefectViewModelViewConfig.RepairView, moduleKey);
            detailView.Data = defect;
            DockPanel.SetDock(detailView.Control, Dock.Top);
            panel.Children.Add(detailView.Control);
            var measureEditor = MeasureControlFactory.CreateControl();
            measureEditor.AllowMultiple = true;
            DockPanel.SetDock(measureEditor, Dock.Left);
            panel.Children.Add(measureEditor);
            var editor = ResponsibilityControlFactory.CreateControl();
            editor.AllowMultiple = true;
            panel.Children.Add(editor);
            BindingData(defect, measureEditor, editor);
            return panel;
        }

        /// <summary>
        /// 数据绑定
        /// </summary>
        /// <param name="defect">不良信息视图模型</param>
        /// <param name="measureEditor">维修措施编辑器</param>
        /// <param name="editor">缺陷责任编辑器</param>
        private void BindingData(RepairDefectViewModel defect, MeasureControl measureEditor, ResponsibilityControl editor)
        {
            var repairmeasures = RF.GetAll<RepairMeasure>();
            measureEditor.Measures.AddRange(repairmeasures);
            var responsibilities = RF.GetAll<DefectResponsibility>();
            editor.Responsibilities.AddRange(responsibilities);
            defect.MeasureList.ForEach(e =>
            {
                var repairmeasure = repairmeasures.FirstOrDefault(p => p.Id == e.Id);
                if (repairmeasure != null)
                    measureEditor.SelectedValues.Add(repairmeasure);
            });
            defect.ResponsibilityList.ForEach(e =>
            {
                var responsibility = responsibilities.FirstOrDefault(p => p.Id == e.Id);
                if (responsibility != null)
                {
                    var item = new DefectResponsibilityItem() { Control = editor, Responsibility = responsibility };
                    editor.SelectedValue.Add(item);
                }
            });
            editor.Remark = defect.Remark;
        }
    }
}
