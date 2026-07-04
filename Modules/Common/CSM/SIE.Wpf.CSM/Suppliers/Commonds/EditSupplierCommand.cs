using SIE.CSM.Suppliers;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MetaModel;
using SIE.WPF.Command;
using SIE.WPF.Common.Templates;
using SIE.WPF.Controls;
using System.Linq;
using System.Windows;

namespace SIE.WPF.CSM.Suppliers.Commonds
{
    /// <summary>
    /// 供应商修改按钮
    /// </summary>
    [Command(ImageName = "Edit.png", Label = "修改", GroupType = CommandGroupType.Edit)]
    public class EditSupplierCommand : EditDetailCommand
    {
        public override bool CanExecute(ListLogicalView view)
        {
            return (view.Current as Supplier) != null;
        }

        public override void Execute(ListLogicalView view)
        {
            var template = new DetailsUITemplate(typeof(Supplier), typeof(AddSupplierViewConfig));
            var ui = template.CreateUI();
            var supplier = View.Current as Supplier;
            ui.MainView.Data = supplier;

            bool isChanged = false;
            //isChanged判断是否变更属性
            supplier.PropertyChanged += (s, e) => isChanged = true;

            App.Windows.ShowDialog(ui.Control, w =>
            {
                w.Title = "修改".Translate();
                w.Width = 800;
                w.Height = 500;
                w.Buttons = ViewDialogButtons.YesNo;


                w.Accepting += (x, y) =>
                {
                    var broken = supplier.Validate();
                    if (broken.Count > 0)
                    {
                        App.MessageBox.Show(DisplayHelper.Display(broken.ToString()));
                        y.Cancel = true;
                        return;
                    }
                    supplier.PersistenceStatus = PersistenceStatus.Modified;
                    RF.Save(supplier);
                    view.Current = supplier;
                    View.RefreshControl();
                };

                w.Closing += (o, e) =>
                {
                    if (w.DialogResult == false && isChanged)
                    {
                        var result = App.MessageBox.Show("数据已修改,是否退出".Translate(), System.Windows.MessageBoxButton.YesNo);
                        if (result == System.Windows.MessageBoxResult.No)
                        {
                            e.Cancel = true;
                        }
                        if (result == System.Windows.MessageBoxResult.Yes)
                        {
                            view.ConditionQueryView.TryExecuteQuery();
                        }
                    }
                };
            });
        }
    }
}
