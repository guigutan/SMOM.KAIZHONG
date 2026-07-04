using SIE.CSM.Suppliers;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MetaModel;
using SIE.Wpf.Command;
using SIE.WPF.Common.Templates;
using SIE.WPF.Controls;
using System.Linq;
using System.Windows;
using SIE.Wpf;
using System;

namespace SIE.WPF.CSM.Suppliers.Commonds
{
    /// <summary>
    /// 供应商添加按钮
    /// </summary>
    [Command(ImageName = "add.png", Label = "添加", GroupType = CommandGroupType.Edit)]
    public class AddSupplierCommand : ListViewCommand
    {
        public override void Execute(ListLogicalView view)
        {
            var template = new DetailsUITemplate(typeof(Supplier), typeof(AddSupplierViewConfig));
            var supplier = new Supplier();
            var ui = template.CreateUI();
            ui.MainView.Data = supplier;

            var result = CRT.Workbench.ShowDialog(ui, w =>
            {
                w.Title = "添加";
                w.Width = 800;
                w.Height = 500;
                w.Closing += (o, e) =>
                {
                    if (w.DialogResult != true)
                    {
                        if (supplier.IsDirty)
                        {
                            var res = CRT.MessageService.AskQuestion("直接退出将不会保存数据，是否退出？".L10N(), MessageBoxButton.YesNo, MessageBoxImage.Warning);
                            e.Cancel = false;
                        }
                    }
                    else
                    {
                        var broken = supplier.Validate();
                        if (broken.Count > 0)
                        {
                            CRT.MessageService.AskQuestion(DisplayHelper.Display(broken.ToString()));
                            e.Cancel = true;
                            return;
                        }
                        RF.Save(supplier);
                        view.AddItem(supplier, true);
                        view.Current = view.Data.OfType<Supplier>().FirstOrDefault(p => p.Id == supplier.Id);
                    }
                };
            });
        }
    }
}
