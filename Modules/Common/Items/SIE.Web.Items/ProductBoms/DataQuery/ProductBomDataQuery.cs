using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Items;
using SIE.Items.ProductBoms;
using SIE.Web.Data;
using SIE.Web.Items.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.Items.ProductBoms.DataQuery
{
    /// <summary>
    /// 产品BOM数据操作类
    /// </summary>
    public class ProductBomDataQuery : DataQueryer
    {
        /// <summary>
        /// 自定义保存配送管理的属性值标签
        /// </summary>
        /// <param name="proModel">属性实体</param>
        /// <param name="productBomId">产品BOM ID</param>
        public void SaveProductBomProValue(List<PropertyValueViewModel> proModel, double productBomId)
        {
            if (productBomId != -1)
            {
                var productBom = RF.GetById<ProductBom>(productBomId);
                var productBomValues = new EntityList<PropertyValueViewModel>();
                if (proModel.Any(p => p.DefinitionId == 0))
                    throw new ValidationException("属性值的物料属性不能为空。".L10N());
                ////工单属性值
                productBomValues.AddRange(proModel);
                if (productBom.PropertyValueList != null)
                {
                    productBom.PropertyValueList.Clear(); //清空工单属性值列表                 
                }
                if (productBomValues.Count > 0)
                {
                    foreach (var productBomValue in productBomValues)  //工单属性值
                    {
                        foreach (var value in productBomValue.Values)
                        {
                            ProductBomPropertyValue item = new ProductBomPropertyValue()
                            {
                                Definition = productBomValue.Definition,
                                Value = value,
                                ProductBomId = productBom.Id
                            };
                            item.GenerateId();
                            productBom.PropertyValueList.Add(item);
                        }
                    }
                }
                RF.Save(productBom);
            }
        }

        /// <summary>
        /// 删除属性值
        /// </summary>
        /// <param name="deleteBomId">产品bomid</param>
        public void DeleteProductBomProValue(string deleteBomId)
        {
            if (!string.IsNullOrEmpty(deleteBomId))
            {
                string[] stringIdArray = deleteBomId.Split(',');
                double[] deleteIdArray = new double[stringIdArray.Length];
                for (int i = 0; i < deleteIdArray.Length; i++)
                {
                    double tmp;
                    if (double.TryParse(stringIdArray[i], out tmp))
                    {
                        deleteIdArray[i] = tmp;
                    }
                    else
                    {
                        deleteIdArray[i] = -1;
                    }

                }
                var bomList = RT.Service.Resolve<ProductBomController>().GetProductBomsByIds(deleteIdArray);
                foreach (var item in bomList)
                {
                    if (item.PropertyValueList != null)
                    {
                        item.PropertyValueList.Clear(); //清空属性值列表                 
                    }
                }
                RF.Save(bomList);
            }
        }

        /// <summary>
        /// 设置产品产品bom为缺省
        /// </summary>
        /// <param name="prodcutId">产品ID</param>
        /// <param name="bomId">产品BOM ID</param>
        public void SetDefaultProductBom(double prodcutId, double bomId)
        {
            RT.Service.Resolve<ItemController>().SetDefaultProductBom(prodcutId, bomId);
        }

        /// <summary>
        /// 设置产品产品bom为缺省（考虑物料扩展属性）
        /// </summary>
        /// <param name="prodcutId">产品ID</param>
        /// <param name="bomId">产品BOM ID</param>
        /// <param name="extPropName">物料扩展属性</param>
        public virtual void SetDefaultProductBomWithExtProp(double prodcutId, double bomId, string extPropName)
        {
            RT.Service.Resolve<ProductBomController>().SetDefaultProductBomWithExtProp(prodcutId, bomId, extPropName);
        }

        /// <summary>
        /// 获取物料下所有产品BOM明细属性值
        /// </summary>
        /// <param name="bomDetailId">产品BOM明细Id</param>
        /// <param name="itemId">物料Id</param>
        /// <returns>所有产品BOM明细属性值</returns>
        public EntityList<ProductBomDetailPropertyValue> GetPropertyValueList(double bomDetailId, double itemId)
        {
            return RT.Service.Resolve<ProductBomController>().GetBomDetailPropertyValues(bomDetailId, itemId);
        }

        /// <summary>
        /// 获取物料下所有产品BOM下组合替代属性值
        /// </summary>
        /// <param name="comReplateId">产品BOM下组合替代Id</param>
        /// <param name="itemId">物料Id</param>
        /// <returns>所有产品BOM组合替代的属性值</returns>
        public EntityList<CombinationReplatePropertyValue> GetComReplatePropertyValues(double comReplateId, double itemId)
        {
            return RT.Service.Resolve<ProductBomController>().GetComReplatePropertyValues(comReplateId, itemId);
        }

        /// <summary>
        /// 获取全部物料
        /// </summary>
        /// <returns>物料</returns>
        public EntityList<Item> GetAllItems(string key, PagingInfo pagingInfo)
        {
            return RT.Service.Resolve<ItemController>().GetItemDatas(pagingInfo, key);
        }

        /// <summary>
        /// 获取替代料列表
        /// </summary>
        /// <param name="bomDetailId">产品BOM明细id</param>
        /// <returns>替代料列表</returns>
        public EntityList<Item> GetItemAlternative(double bomDetailId)
        {
            return RT.Service.Resolve<ProductBomController>().GetItemAlternative(bomDetailId);
        }

        /// <summary>
        /// 保存替代料列表
        /// </summary>
        /// <param name="bomDetailId">产品BOM明细id</param>
        /// <param name="codes">替代料物料编码</param>
        public void SaveAlternative(double bomDetailId, List<string> codes)
        {
            RT.Service.Resolve<ProductBomController>().SaveAlternative(bomDetailId, codes);
        }

        /// <summary>
        /// 校验：产品BOM被生产订单或需求管理引用为指定BOM，不允许修改产品
        /// </summary>
        /// <param name="bomId"></param>
        public void ValidateApsRef(double bomId)
        {
            RT.Service.Resolve<ProductBomController>().ValidateApsRef(bomId);
        }
    }
}
