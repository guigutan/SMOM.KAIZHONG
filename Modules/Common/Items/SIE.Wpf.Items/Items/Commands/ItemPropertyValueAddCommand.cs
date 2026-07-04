using SIE.Domain;
using SIE.Items;
using SIE.Items.ViewModels;
using SIE.Wpf.Command;
using System;

namespace SIE.Wpf.Items.Items.Commands
{
    /// <summary>
    /// 添加命令
    /// </summary>
    [Command(ImageName = "AddEntity",
        Label = "添加",
        ToolTip = "添加数据",
        Gestures = "Ctrl+Shift+N",
        GroupType = CommandGroupType.Edit)]
    public class ItemPropertyValueAddCommand : ListAddCommand
    {
        /// <summary>
        /// 添加命令
        /// </summary>
        /// <param name="entity"></param>
        protected override void ShowView(Entity entity)
        {
            var model = new ItemPropertyViewModel();
            model.MarkSaved();
            var ui = new DetailsUITemplate<ItemPropertyViewModel>(View.ModuleKey).CreateUI();
            var detailView = ui.MainView as DetailLogicalView;
            detailView.Data = model;

            CRT.Workbench.ShowDialog(Guid.NewGuid().ToString("N"), ui.Control, v =>
            {
                v.Title = this.Meta.Label.L10N();

                v.Width = 400;
                v.Height = 200;

                v.Closing += (o, e) =>
                {
                    if (v.Result != 0 && detailView.Data.IsDirty && !CRT.MessageService.AskQuestion("数据未保存，是否退出？".L10N()))
                        e.Cancel = true;
                    else if (v.Result == 0 && model.Definition != null)
                    {
                        var value = entity as ItemPropertyValue;
                        value.DefinitionId = model.DefinitionId;
                        if (model.Definition.PropertyType == ItemPropertyType.Text)
                            value.Value = model.Value;
                        else if (model.Definition.PropertyType == ItemPropertyType.Number)
                            value.Value = model.NumberValue.ToString();
                        else if (model.Definition.PropertyType == ItemPropertyType.Catalog)
                            value.Value = model.Catalog?.Name;

                        value.PropertyGroup = model.PropertyGroup;
                        if (value.Item.PersistenceStatus != PersistenceStatus.New)
                        {
                            try
                            {
                                RF.Save(value);
                            }
                            catch
                            {
                                e.Cancel = true;
                                throw;
                            }
                        }

                        View.Data.Add(value);
                    }
                };
            });
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="entity"></param>
        protected override void FromDoSave(Entity entity)
        {
            base.FromDoSave(entity);
        }
    }
}
