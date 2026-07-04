using SIE.Defects;
using SIE.Domain;
using SIE.Wpf.Command;
using SIE.Wpf.MES.Controls;
using System;
using System.Linq;

namespace SIE.Wpf.MES.WIP.Inspects.Commands
{
    /// <summary>
    /// 缺陷代码增加命令
    /// </summary>
    [Command(ImageName = "NetworkError",
      Label = "不良录入",
      ToolTip = "不良录入",
      GroupType = CommandGroupType.Edit)]
    public class DefectItemAddCommand : ListViewCommand
    {
        /// <summary>
        /// 是否可执行的逻辑
        /// </summary>
        /// <param name="view">视图对象</param>
        /// <returns>返回是否可执行</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            var model = view.Current as InspectionItemViewModel;
            if (model == null)
                return false;

            return model.IsNg;
        }

        /// <summary>
        /// 执行具体的逻辑。
        /// </summary>
        /// <param name="view">视图对象</param>
        public override void Execute(ListLogicalView view)
        {
            var inspection = view.Current as InspectionItemViewModel;
            var editor = DefectControlFactory.CreateControl();
            var mainView = view.Relations.Find("mainView").Current as InspectByItemViewModel;

            editor.AllowMultiple = true;
            editor.AllowQty = false;
            editor.Defects.AddRange(RF.GetAll<Defect>());

            //获取当前检验项目对应不良信息
            var selectItem = mainView.DefectItemList.Where(p => p.ModelInspectionItemId == inspection.ModelInspecitonItemId).ToList();

            editor.SelectedValue.Clear();
            selectItem.ForEach(e =>
            {
                var item = new DefectItem() { Defect = e.Defect };
                editor.SelectedValue.Add(item);
            });

            var result = CRT.Workbench.ShowDialog(Guid.NewGuid().ToString(), editor, w =>
              {
                  w.Title = "不良代码录入".L10N();
                  w.Height = 500;
                  w.Width = 750;
                  w.MinHeight = 350;
                  w.MinWidth = 400;
                  w.Closing += (s, e) =>
                  {
                      if (editor.SelectedValue.Count == 0)
                      {
                          e.Cancel = true;
                      }
                  };
              });
            if (result == 0)
            {
                try
                {
                    var defects = editor.SelectedValue.Select(p => p.Defect).ToList();
                    selectItem.ForEach(d =>
                    {
                        mainView.DefectItemList.Remove(d);
                    });
                    mainView.DefectItemList.AddRange(defects.Select(p => new DefectItemViewModel
                    {
                        Defect = p,
                        ModelInspectionItem = inspection.ModelInspecitonItem,
                        PersistenceStatus = PersistenceStatus.Unchanged
                    }));
                }
                catch (Exception exc)
                {
                    exc.Alert();
                }
            }
        }
    }
}
