using SIE.Domain;
using SIE.Items;
using SIE.Items.ViewModels;
using SIE.Wpf.Command;
using System;
using System.Linq;

namespace SIE.Wpf.Items
{
    /// <summary>
    /// 添加物料属性值按钮
    /// </summary>
    [Command(ImageName = "AddEntity", Label = "添加", GroupType = CommandGroupType.Edit)]
    public class ItemPropertyValueAddCommand : ListViewCommand
    {
        /// <summary>
        /// 是否执行命令
        /// </summary>
        /// <param name="view">List逻辑视图</param>
        /// <returns>bool</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            return view.CanAddItem() && view.Parent?.Current != null;
        }

        /// <summary>
        /// 命令执行块
        /// </summary>
        /// <param name="view">List逻辑视图</param>
        public override void Execute(ListLogicalView view)
        {
            var propertyValue = new ItemPropertyValue();
            propertyValue.GenerateId();
            propertyValue.Item = View.Parent?.Current as Item;
            var itemPropertyViewModel = new ItemPropertyViewModel(propertyValue);

            if (PopSelectionView(itemPropertyViewModel) == 0)
            {
                itemPropertyViewModel.AcceptChanges();
                var data = view.Data as EntityList<PropertyValueViewModel>;
                var vm = data.FirstOrDefault(p => p.DefinitionId == propertyValue.DefinitionId);
                if (vm != null)
                {
                    vm.ParentId = propertyValue.ItemId;
                    vm.Values.Add(propertyValue.Value);
                }
                else
                {
                    vm = new PropertyValueViewModel() { Definition = propertyValue.Definition, Value = propertyValue.Value, ParentId = propertyValue.ItemId };
                    vm.Values.Add(propertyValue.Value);
                    data.Add(vm);
                }

                RF.Save(itemPropertyViewModel.PropertyValue);
            }

            view.RefreshControl();
        }

        /// <summary>
        /// 弹出填写视图
        /// </summary>
        /// <param name="itemPropertyViewModel">物料属性模板</param>
        /// <returns>结果int</returns>
        int PopSelectionView(ItemPropertyViewModel itemPropertyViewModel)
        {
            var template = new DetailsUITemplate<ItemPropertyViewModel>();
            template.ViewGroup = ViewConfig.DetailsView;
            var ui = template.CreateUI();
            ui.MainView.CommandsContainer.Visibility = System.Windows.Visibility.Collapsed;
            ui.MainView.Data = itemPropertyViewModel;

            var result = CRT.Workbench.ShowDialog(ui, (v) =>
            {
                v.Title = "请填写物料属性".L10N();
                v.Width = 300;
                v.Height = 200;
                v.Closing += (o, e) =>
                {
                    if (v.Result != 0)
                    {
                        if (ui.MainView.Data.IsDirty)
                        {
                            if (CRT.MessageService.AskQuestion("数据未保存，确定退出吗".L10N()))
                            {
                                e.Cancel = false;
                            }
                            else
                            {
                                e.Cancel = true;
                            }
                        }
                    }
                };
            });
            return result;
        }
    }

    //[Command(ImageName = "Save", Label = "保存", GroupType = CommandGroupType.Edit)]
    //public class ItemPropertyValueSaveCommand : ListAddCommand
    //{
    //    public override bool CanExecute(ListLogicalView view)
    //    {
    //        var data = view.Data as IDirtyAware;
    //        return data != null && data.IsDirty;
    //    }

    //    public override void Execute(ListLogicalView view)
    //    {
    //        var listView = view.CastTo<ListLogicalView>();
    //        var list = listView.Data;
    //        if (list.IsDirty)
    //        {
    //            if (!this.ValidateData(listView)) return;
    //            List<ItemPropertyValueViewModel> values = new List<ItemPropertyValueViewModel>();
    //            double parentId = 0;
    //            var parent = view.Parent.Current as Item;
    //            if (parent != null)
    //                parentId = parent.Id;
    //            foreach (PropertyValueViewModel item in list)
    //            {
    //                foreach (var value in item.Values)
    //                {
    //                    ItemPropertyValueViewModel propertyValue = new ItemPropertyValueViewModel()
    //                    {
    //                        DefinitionId = item.DefinitionId,
    //                        Value = value,
    //                    };
    //                    values.Add(propertyValue);
    //                }
    //            }
    //            var propertyValues = RT.Service.Resolve<ItemController>().GetItemPropertys(parentId);  //物料属性值
    //            foreach (var value in propertyValues)
    //            {
    //                var result = values.Where(p => p.DefinitionId == value.DefinitionId && p.Value == value.Value).FirstOrDefault() as ItemPropertyValueViewModel;
    //                if (result == null)
    //                {
    //                    var propertyValue = RT.Service.Resolve<ItemController>().GetItemPropertyValue(value.DefinitionId, value.Value, parentId);
    //                    if (propertyValue != null) //删除移除的属性值
    //                    {
    //                        propertyValue.PersistenceStatus = PersistenceStatus.Deleted;
    //                        RF.Save(propertyValue);
    //                    }
    //                }
    //            }
    //            var currentItem = RF.Find<Item>().GetById(parentId) as Item;
    //            var resultValues = currentItem.PropertyValueList.GroupBy(p => p.DefinitionId).Select(f => new PropertyValueViewModel { DefinitionId = f.Key, Values = f.Select(p => p.Value).ToList(), Type = f.Select(p => p.Item).FirstOrDefault().GetType(), ParentId = f.Select(p => p.ItemId).FirstOrDefault() });
    //            var itemList = new EntityList<PropertyValueViewModel>();
    //            itemList.AddRange(resultValues);
    //            view.Data = itemList;
    //            view.Data.MarkSaved();
    //        }
    //    }
    //}

    /// <summary>
    /// 删除命令
    /// </summary>
    [Command(ImageName = "DeleteEntity", Label = "删除", GroupType = CommandGroupType.Edit)]
    public class ItemPropertyValueDeleteCommand : ListDeleteCommand
    {
        /// <summary>
        /// 是否可执行命令
        /// </summary>
        /// <param name="view">List逻辑视图</param>
        /// <returns>bool</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            return view.Current != null;
        }

        /// <summary>
        /// 命令执行块
        /// </summary>
        /// <param name="view">List逻辑视图</param>
        public override void Execute(ListLogicalView view)
        {
            if (!CRT.MessageService.AskQuestion(string.Format("你确认删除选择的{0}条数据吗？".L10N(), view.SelectedEntities.Count)))
                return;
            var selectedItems = view.SelectedEntities.OfType<PropertyValueViewModel>();
            foreach (var item in selectedItems)
            {
                foreach (var value in item.Values)
                {
                    var propertyValue = RT.Service.Resolve<ItemController>().GetItemPropertyValue(item.DefinitionId, value, item.ParentId);
                    if (propertyValue != null)
                    {
                        propertyValue.PersistenceStatus = PersistenceStatus.Deleted;
                        RF.Save(propertyValue);
                    }
                }
            }

            var parent = RF.Find<Item>().GetById(selectedItems.FirstOrDefault().ParentId) as Item;
            var result = parent.PropertyValueList.GroupBy(p => p.DefinitionId).Select(f => new PropertyValueViewModel { DefinitionId = f.Key, Values = f.Select(p => p.Value).ToList(), Type = f.Select(p => p.Item).FirstOrDefault().GetType(), ParentId = f.Select(p => p.ItemId).FirstOrDefault() });
            var list = new EntityList<PropertyValueViewModel>();
            list.AddRange(result);
            view.Data = list;
            view.RefreshControl();
        }
    }
}
