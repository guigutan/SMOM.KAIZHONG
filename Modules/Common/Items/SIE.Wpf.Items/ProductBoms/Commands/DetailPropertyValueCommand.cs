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
    /// 添加产品BOM明细属性值
    /// </summary>
    [Command(Label = "添加", ImageName = "AddEntity", GroupType = CommandGroupType.Edit)]
    public class BomDetailPropertyValueAddCommand : ListAddCommand
    {
        /// <summary>
        /// 是否可执行
        /// </summary>
        /// <param name="view">逻辑视图</param>
        /// <returns>bool</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            return true;
        }

        /// <summary>
        /// 编辑前
        /// </summary>
        /// <param name="editEntity">编辑实体</param>
        protected override void OnEditting(Entity editEntity)
        {
            base.OnEditting(editEntity);
        }
    }

    /// <summary>
    /// 保存产品BOM明细属性值
    /// </summary>
    [Command(Label = "保存", ImageName = "SaveEntity", GroupType = CommandGroupType.Edit)]
    public class DetailPropertyValuesSaveCommand : ListSaveCommand
    {
        /// <summary>
        /// 是否可执行
        /// </summary>
        /// <param name="view">逻辑视图</param>
        /// <returns>bool</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            var current = view.Current;
            return current != null && current.IsDirty;
        }

        /// <summary>
        /// 命令执行块
        /// </summary>
        /// <param name="view">逻辑视图</param>
        public override void Execute(ListLogicalView view)
        {
            var listView = view.CastTo<ListLogicalView>();
            var list = listView.Data;
            if (list.IsDirty)
            {
                Validate(list.OfType<PropertyValueViewModel>());
                List<ItemPropertyValue> values = new List<ItemPropertyValue>();
                double parentId = 0;
                var parent = view.Parent.Current as ProductBomDetail;
                if (parent != null)
                {
                    parentId = parent.Id;
                }

                foreach (PropertyValueViewModel item in list)
                {
                    foreach (var value in item.Values)
                    {
                        ItemPropertyValue propertyValue = new ItemPropertyValue()
                        {
                            DefinitionId = item.DefinitionId,
                            Value = value,
                        };
                        values.Add(propertyValue);
                    }
                }

                var propertyValues = RT.Service.Resolve<ProductBomController>().GetProductBomDetailPropertyValues(parentId);  //产品Bom的所有属性值
                foreach (var value in propertyValues)
                {
                    var result = values.FirstOrDefault(p => p.DefinitionId == value.DefinitionId && p.Value == value.Value);
                    if (result == null)
                    {
                        var propertyValue = RT.Service.Resolve<ProductBomController>().GetProductBomDetailPropertyValue(value.DefinitionId, value.Value, parentId);
                        if (propertyValue != null) //删除移除的属性值
                        {
                            propertyValue.PersistenceStatus = PersistenceStatus.Deleted;
                            RF.Save(propertyValue);
                        }
                        else
                            values.Remove(result);
                    }
                }

                if (values.Count > 0)
                {
                    foreach (var value in values)
                    {
                        var propertyValue = RT.Service.Resolve<ProductBomController>().GetProductBomDetailPropertyValue(value.DefinitionId, value.Value, parentId);
                        if (propertyValue == null)
                        {
                            ProductBomDetailPropertyValue bomPropertyValue = new ProductBomDetailPropertyValue()
                            {
                                Definition = value.Definition,
                                Value = value.Value,
                                DetailId = parentId
                            };
                            bomPropertyValue.PersistenceStatus = PersistenceStatus.New;
                            RF.Save(bomPropertyValue);
                        }
                    }
                }

                var bomDetail = RF.Find<ProductBomDetail>().GetById(parentId) as ProductBomDetail;
                var resultValues = bomDetail.PropertyValueList.GroupBy(p => p.DefinitionId).Select(f => new PropertyValueViewModel { ItemId = bomDetail.ItemId, DefinitionId = f.Key, Values = f.Select(p => p.Value).ToList(), Type = f.Select(p => p.Detail).FirstOrDefault().GetType(), ParentId = f.Select(p => p.DetailId).FirstOrDefault() });
                var itemList = new EntityList<PropertyValueViewModel>();
                itemList.AddRange(resultValues);
                view.Data = itemList;
                view.Data.MarkSaved();
                view.RefreshControl();
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
    public class BomDetailPropertyValueDeleteCommand : ListDeleteCommand
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
            if (!CRT.MessageService.AskQuestion(string.Format("你确认删除选择的{0}条数据吗？".L10N(), view.SelectedEntities.Count)))
            {
                return;
            }

            var selectedItems = view.SelectedEntities.OfType<PropertyValueViewModel>();
            foreach (var item in selectedItems)
            {
                foreach (var value in item.Values)
                {
                    var propertyValue = RT.Service.Resolve<ProductBomController>().GetProductBomDetailPropertyValue(item.DefinitionId, value, item.ParentId);
                    if (propertyValue != null)
                    {
                        propertyValue.PersistenceStatus = PersistenceStatus.Deleted;
                        RF.Save(propertyValue);
                    }
                }
            }

            var detailId = selectedItems.FirstOrDefault().ParentId;
            var detail = RF.Find<ProductBomDetail>().GetById(detailId) as ProductBomDetail;
            var resultValues = detail.PropertyValueList.GroupBy(p => p.DefinitionId).Select(f => new PropertyValueViewModel { DefinitionId = f.Key, Values = f.Select(p => p.Value).ToList(), Type = f.Select(p => p.Detail).FirstOrDefault().GetType(), ParentId = f.Select(p => p.DetailId).FirstOrDefault() });
            var vm = new EntityList<PropertyValueViewModel>();
            vm.AddRange(resultValues);
            view.Data = vm;
            view.RefreshControl();
        }
    }
}
