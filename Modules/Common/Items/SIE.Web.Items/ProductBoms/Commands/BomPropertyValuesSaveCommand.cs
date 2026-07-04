using SIE.Web.Command;

namespace SIE.Web.Items.ProductBoms.Commands
{
    /// <summary>
    /// 保存
    /// </summary>
    [JsCommand("SIE.Web.Items.ProductBoms.Commands.BomPropertyValuesSaveCommand")]
    public class BomPropertyValuesSaveCommand : ViewCommand<ViewArgs>
    {
        /// <summary>
        /// 之前的属性值保存方法，暂时废弃
        /// </summary>
        /// <param name="args"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            ////var list = args.Data.ToJsonObject<List<PropertyValueViewModel>>(); 
            ////try
            ////{
            ////    Validate(list);
            ////    List<ItemPropertyValue> values = new List<ItemPropertyValue>();
            ////    double parentId = 0;
            ////    var parent = RF.GetById<ProductBom>(list[0].ParentId);
            ////    if (parent != null)
            ////        parentId = parent.Id;
            ////    foreach (PropertyValueViewModel item in list)
            ////    {
            ////        foreach (var value in item.Values)
            ////        {
            ////            ItemPropertyValue propertyValue = new ItemPropertyValue()
            ////            {
            ////                DefinitionId = item.DefinitionId,
            ////                Value = value,
            ////            };
            ////            values.Add(propertyValue);
            ////        }
            ////    }

            ////    var propertyValues = RT.Service.Resolve<ItemController>().GetProductBomPropertyValues(parentId);  //产品Bom的所有属性值
            ////    foreach (var value in propertyValues)
            ////    {
            ////        var result = values.Where(p => p.DefinitionId == value.DefinitionId && p.Value == value.Value).FirstOrDefault() as ItemPropertyValue;
            ////        if (result == null)   //属性值已删除
            ////        {
            ////            var propertyValue = RT.Service.Resolve<ItemController>().GetProductBomPropertyValue(value.DefinitionId, value.Value, parentId);
            ////            if (propertyValue != null) //删除移除的属性值
            ////            {
            ////                propertyValue.PersistenceStatus = PersistenceStatus.Deleted;
            ////                RF.Save(propertyValue);
            ////            }
            ////        }
            ////        else
            ////            values.Remove(result); //移除已经存在的属性值
            ////    }

            ////    if (values.Count > 0)
            ////    {
            ////        foreach (var value in values)  //新增属性值
            ////        {
            ////            var propertyValue = RT.Service.Resolve<ItemController>().GetProductBomPropertyValue(value.DefinitionId, value.Value, parentId);
            ////            if (propertyValue == null)
            ////            {
            ////                ProductBomPropertyValue bomPropertyValue = new ProductBomPropertyValue()
            ////                {
            ////                    Definition = value.Definition,
            ////                    Value = value.Value,
            ////                    ProductBomId = parentId
            ////                };
            ////                bomPropertyValue.PersistenceStatus = PersistenceStatus.New;
            ////                RF.Save(bomPropertyValue);
            ////            }
            ////        }
            ////    }

            ////    var productBom = RF.Find<ProductBom>().GetById(parentId) as ProductBom;
            ////    var resultValues = productBom.PropertyValueList.GroupBy(p => p.DefinitionId).Select(f => new PropertyValueViewModel { DefinitionId = f.Key, Values = f.Select(p => p.Value).ToList(), Type = f.Select(p => p.ProductBom).FirstOrDefault().GetType(), ParentId = f.Select(p => p.ProductBomId).FirstOrDefault(), ItemId = productBom.ProductId });
            ////    var itemList = new EntityList<PropertyValueViewModel>();
            ////    itemList.AddRange(resultValues);
            ////    return itemList;
            ////}
            ////catch (Exception exc)
            ////{
            ////    if (exc is ValidationException)
            ////        throw new ValidationException("验证不通过".L10N() + ":\r\n" + exc.Message);
            ////}


            return true;
        }


        /////// <summary>
        /////// 验证属性名称与属性值
        /////// </summary>
        /////// <param name="vm">PropertyValueViewModel列表</param>
        ////private void Validate(List<PropertyValueViewModel> vm)
        ////{
        ////    foreach (var item in vm)
        ////    {
        ////        if (item.Definition == null)
        ////            throw new ValidationException("属性名称不能为空".L10N());
        ////        if (item.Values.Count == 0)
        ////            throw new ValidationException("属性值不能为空".L10N());
        ////    }
        ////}
    }
}
