using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Items;
using SIE.Items.ProductBoms;
using SIE.Items.ViewModels;
using SIE.Wpf.Command;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Wpf.Items
{
    /// <summary>
    /// 添加产品BOM属性值
    /// </summary>
    [Command(Label = "添加", ImageName = "AddEntity", GroupType = CommandGroupType.Edit)]
    public class BomPropertyValueAddCommand : ListAddCommand
    {
        /// <summary>
        /// 是否可执行
        /// </summary>
        /// <param name="view">逻辑视图</param>
        /// <returns>bool</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            var parent = view.Parent?.Current as ProductBom;
            return view.CanAddItem() && parent != null && parent.PersistenceStatus != PersistenceStatus.New && parent.Product != null;
        }

        /// <summary>
        /// 创建实体
        /// </summary>
        /// <returns>Entity</returns>
        protected override Entity CreateNewItem()
        {
            var property = base.CreateNewItem() as PropertyValueViewModel;
            var parent = View.Parent.Current as ProductBom;
            property.Type = parent.GetType();
            property.ParentId = parent.Id;
            if (parent.Product != null)
            {
                property.ItemId = parent.ProductId;
            }

            return property;
        }
    }

    /// <summary>
    /// 产品Bom属性值保存命令
    /// </summary>
    [Command(Label = "保存", ImageName = "SaveEntity", GroupType = CommandGroupType.Edit)]
    public class BomPropertyValuesSaveCommand : ListSaveCommand
    {
        /// <summary>
        /// 是否可执行
        /// </summary>
        /// <param name="view">逻辑视图</param>
        /// <returns>bool</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            var parent = view.Parent.Current;
            return view.Data != null && view.Data.Count > 0 && view.Data.IsDirty && parent.PersistenceStatus != PersistenceStatus.New;
        }

        /// <summary>
        /// 命令执行块
        /// </summary>
        /// <param name="view">逻辑视图</param>
        public override void Execute(ListLogicalView view)
        {
            var list = view.Data;
            if (list.IsDirty)
            {
                try
                {
                    Validate(list.OfType<PropertyValueViewModel>());
                    List<ItemPropertyValue> values = new List<ItemPropertyValue>();
                    double parentId = 0;
                    var parent = view.Parent.Current as ProductBom;
                    if (parent != null)
                        parentId = parent.Id;
                    foreach (PropertyValueViewModel item in list)
                    {
                        foreach (var value in item.Values)
                        {
                            if (values.Any(p => p.DefinitionId == item.DefinitionId && p.Value == value)) continue;

                            ItemPropertyValue propertyValue = new ItemPropertyValue()
                            {
                                DefinitionId = item.DefinitionId,
                                Value = value,
                            };
                            values.Add(propertyValue);
                        }
                    }

                    RT.Service.Resolve<ItemController>().SaveBomPropertyValues(values, parentId);

                    var productBom = RF.Find<ProductBom>().GetById(parentId) as ProductBom;
                    var resultValues = productBom.PropertyValueList.GroupBy(p => p.DefinitionId).Select(f => new PropertyValueViewModel { DefinitionId = f.Key, Values = f.Select(p => p.Value).ToList(), Type = f.Select(p => p.ProductBom).FirstOrDefault().GetType(), ParentId = f.Select(p => p.ProductBomId).FirstOrDefault(), ItemId = productBom.ProductId });
                    var itemList = new EntityList<PropertyValueViewModel>();
                    itemList.AddRange(resultValues);
                    view.Data = itemList;
                    view.Data.MarkSaved();
                }
                catch (Exception exc)
                {
                    if (exc is ValidationException)
                        CRT.MessageService.ShowError("验证不通过".L10N() + ":\r\n" + exc.Message);
                }
            }
        }

        /// <summary>
        /// 验证属性名称与属性值
        /// </summary>
        /// <param name="vm">PropertyValueViewModel列表</param>
        private void Validate(IEnumerable<PropertyValueViewModel> vm)
        {
            foreach (var item in vm)
            {
                if (item.Definition == null)
                    throw new ValidationException("属性名称不能为空".L10N());
                if (item.Values.Count == 0)
                    throw new ValidationException("属性值不能为空".L10N());
            }
        }
    }

    /// <summary>
    /// 删除命令
    /// </summary>
    [Command(Label = "删除", ImageName = "DeleteEntity", GroupType = CommandGroupType.Edit)]
    public class BomPropertyValueDeleteCommand : ListDeleteCommand
    {
        /// <summary>
        /// 是否可执行
        /// </summary>
        /// <param name="view">逻辑视图</param>
        /// <returns>bool</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            return view.Current != null;
        }

        /// <summary>
        /// 命令执行块
        /// </summary>
        /// <param name="view">逻辑视图</param>
        public override void Execute(ListLogicalView view)
        {
            if (!CRT.MessageService.AskQuestion("你确认删除选择的{0}条数据吗？".L10nFormat(view.SelectedEntities.Count)))
            {
                return;
            }

            var selectedItems = view.SelectedEntities.OfType<PropertyValueViewModel>();
            foreach (var item in selectedItems)
            {
                foreach (var value in item.Values)
                {
                    var propertyValue = RT.Service.Resolve<ProductBomController>().GetProductBomPropertyValue(item.DefinitionId, value, item.ParentId);
                    if (propertyValue != null)
                    {
                        propertyValue.PersistenceStatus = PersistenceStatus.Deleted;
                        RF.Save(propertyValue);
                    }
                }
            }

            var bomId = selectedItems.FirstOrDefault().ParentId;
            var bom = RF.Find<ProductBom>().GetById(bomId) as ProductBom;
            var resultValues = bom.PropertyValueList.GroupBy(p => p.DefinitionId).Select(f => new PropertyValueViewModel { DefinitionId = f.Key, Values = f.Select(p => p.Value).ToList(), Type = f.Select(p => p.ProductBom).FirstOrDefault().GetType(), ParentId = f.Select(p => p.ProductBomId).FirstOrDefault() });
            var values = new EntityList<PropertyValueViewModel>();
            values.AddRange(resultValues);
            view.Data = values;
            view.RefreshControl();
        }
    }
}
