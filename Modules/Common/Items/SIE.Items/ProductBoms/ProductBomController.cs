using SIE.Core.ApiModels;
using SIE.Core.Common;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EventMessages.Common.Items;
using SIE.Items.ProductBoms.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Items.ProductBoms
{
    /// <summary>
    /// 产品机型控制器
    /// </summary>
    public partial class ProductBomController : DomainController
    {
        /// <summary>
        /// 查询产品BOM
        /// </summary>
        /// <param name="criteria">查询实体</param>
        /// <returns>ProductBom列数</returns>
        /// <exception cref="ArgumentNullException">ProductBom</exception>
        public virtual EntityList<ProductBom> GetProductBoms(ProductBomCriteria criteria)
        {
            if (criteria == null)
                throw new ArgumentNullException(nameof(criteria));
            var query = Query<ProductBom>();
            if (criteria.Code.IsNotEmpty())
                query.Where(e => e.Code.Contains(criteria.Code));
            if (criteria.Name.IsNotEmpty())
                query.Where(e => e.Name.Contains(criteria.Name));
            if (criteria.ProductCode.IsNotEmpty())
                query.Where(e => e.Product.Code.Contains(criteria.ProductCode));
            if (criteria.ProductName.IsNotEmpty())
                query.Where(e => e.Product.Name.Contains(criteria.ProductName));
            if (criteria.ProductId.IsNotEmpty())
            {
                List<double> nameList = criteria.ProductId.Split(',').Select(s =>double.Parse(s)).ToList();
                query.Where(e => nameList.Contains(e.ProductId));
            }
            if (criteria.SpecificationModel.IsNotEmpty())
                query.Where(e => e.Product.SpecificationModel.Contains(criteria.SpecificationModel));

            return query.OrderBy(criteria.OrderInfoList)
                .ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty()); //贪婪加载，把创建人等一块加载进来
        }

        /// <summary>
        /// 获取产品BOM数据
        /// </summary>
        /// <param name="codes">编码</param>
        /// <returns>产品BOM数据</returns>
        public virtual EntityList<ProductBom> GetProductBoms(List<string> codes)
        {
            Check.NotNull(codes, "产品BOM编码集合不能为空".L10N());
            var exp = codes.CreateContainsExpression<ProductBom>("x", "Code");
            if (exp == null)
                return new EntityList<ProductBom>();
            return Query<ProductBom>().Where(exp).ToList(null, new EagerLoadOptions().LoadWithViewProperty().LoadWith(ProductBom.ProductProperty).LoadWith(ProductBom.DetailListProperty));
        }

        /// <summary>
        /// 根据物料ID获取产品BOM表
        /// </summary>
        /// <param name="itemIds">物料ID</param>
        /// <returns>产品BOM列表</returns>
        public virtual EntityList<ProductBom> GetProductBomsByItemIds(List<double> itemIds)
        {
            Check.NotNull(itemIds, "物料ID集合不能为空".L10N());

            return itemIds.SplitContains((tempTaskIds) =>
            {
                return Query<ProductBom>().Where(p => tempTaskIds.Contains(p.ProductId)).ToList(null, new EagerLoadOptions().LoadWith(ProductBom.DetailListProperty));
            });
        }

        /// <summary>
        /// 根据物料ID查询产品bom列表
        /// </summary>
        /// <param name="itemId">物料ID</param>
        /// <param name="pagingInfo">分页</param>
        /// <param name="keyword">关键字</param>
        /// <returns>产品BOM列表</returns>
        public virtual EntityList<ProductBom> GetProductBomsByItemId(double itemId, PagingInfo pagingInfo, string keyword)
        {
            var returns = Query<ProductBom>().Where(p => p.ProductId == itemId);
            if (keyword.IsNotEmpty())
            {
                returns.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            }
            return returns.ToList(pagingInfo);
        }

        /// <summary>
        /// 根据产品ID找到对应的默认产品BOM
        /// </summary>
        /// <param name="productId">产品ID</param>
        /// <returns>产品BOM</returns>
        /// <exception cref="ArgumentNullException">产品ID不存在</exception>
        public virtual ProductBom FindProductBom(double productId)
        {
            if (productId <= 0)
                throw new ArgumentNullException(nameof(productId));
            var query = Query<ProductBom>();
            query.Where(p => p.ProductId == productId && p.IsDefault);
            return query.FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据编码找到对应的产品BOM
        /// </summary>
        /// <param name="productBomCode">产品BOM编码</param>
        /// <returns>产品BOM，对应物料编码</returns>
        public virtual Tuple<ProductBom, string> FindProductBomByCode(string productBomCode)
        {
            var productBom = Query<ProductBom>().Where(p => p.Code == productBomCode).FirstOrDefault(new EagerLoadOptions().LoadWith(ProductBom.ProductProperty));
            if (productBom == null)
            {
                return new Tuple<ProductBom, string>(null, string.Empty);
            }
            else
            {
                return new Tuple<ProductBom, string>(productBom, productBom.Product.Code);
            }
        }
        /// <summary>
        /// 根据编码找到对应的产品BOM
        /// </summary>
        /// <param name="productBomCode">产品BOM编码</param>
        /// <returns>产品BOM，对应物料编码</returns>
        public virtual ProductBom GetProductBomByCode(string productBomCode)
        {
            return Query<ProductBom>().Where(p => p.Code == productBomCode).FirstOrDefault(new EagerLoadOptions().LoadWith(ProductBom.ProductProperty));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="productBomCodes"></param>
        /// <returns></returns>
        public virtual EntityList<ProductBom> GetProductBomByCodes(List<string> productBomCodes)
        {
            return Query<ProductBom>().Where(p => productBomCodes.Contains(p.Code)).ToList();
        }


        /// <summary>
        /// 设置产品产品bom为缺省
        /// </summary>
        /// <param name="prodcutId">产品ID</param>
        /// <param name="bomId">产品BOM ID</param>
        public virtual void SetDefaultProductBom(double prodcutId, double bomId)
        {
            using (var tran = DB.TransactionScope(ItemEntityDataProvider.ConnectionStringName))
            {
                DB.Update<ProductBom>().Set(p => p.IsDefault, true).Where(p => p.Id == bomId).Execute();
                DB.Update<ProductBom>().Set(p => p.IsDefault, false).Where(p => p.ProductId == prodcutId && p.Id != bomId).Execute();
                tran.Complete();
            }
        }

        /// <summary>
        /// 设置产品产品bom为缺省（考虑物料扩展属性）
        /// </summary>
        /// <param name="prodcutId">产品ID</param>
        /// <param name="bomId">产品BOM ID</param>
        /// <param name="extPropName">物料扩展属性</param>
        public virtual void SetDefaultProductBomWithExtProp(double prodcutId, double bomId, string extPropName)
        {
            using (var tran = DB.TransactionScope(ItemEntityDataProvider.ConnectionStringName))
            {
                DB.Update<ProductBom>().Set(p => p.IsDefault, true).Where(p => p.Id == bomId).Execute();
                //物料、扩展属性对应，且不是刚设为默认的，设为非默认
                DB.Update<ProductBom>().Set(p => p.IsDefault, false)
                    .Where(p => p.ProductId == prodcutId && p.Id != bomId && p.IsDefault && p.ItemExtPropName == extPropName)
                    .Execute();
                tran.Complete();
            }
        }

        /// <summary>
        /// 判断产品是否存缺省bom
        /// </summary>
        /// <param name="productId">产品ID</param>
        /// <returns>存在返回true，否则返回false</returns>
        public virtual bool IsExistDefaultProductBom(double productId)
        {
            return Query<ProductBom>().Where(p => p.ProductId == productId && p.IsDefault).Count() > 0;
        }

        /// <summary>
        /// 判断产品是否存缺省bom（考虑物料扩展属性）
        /// </summary>
        /// <param name="productId">产品ID</param>
        /// <param name="itemExtProp">产品扩展属性</param>
        /// <returns>存在返回true，否则返回false</returns>
        public virtual bool IsExistDefaultProductBomWithExtProp(double productId, string itemExtProp)
        {
            return Query<ProductBom>()
                .Where(p => p.ProductId == productId && p.IsDefault && p.ItemExtProp == itemExtProp)
                .Count() > 0;
        }

        /// <summary>
        /// 获取产品BOM明细数据
        /// </summary>
        /// <param name="bomIds">产品BOM Id</param>
        /// <param name="elo">贪婪加载对象</param>
        /// <returns>产品BOM明细数据</returns>
        public virtual EntityList<ProductBomDetail> GetProductBomDetails(List<double> bomIds, EagerLoadOptions elo)
        {
            Check.NotNull(bomIds, "产品BOM ID集合不能为空".L10N());
            if (elo == null)
            {
                elo = new EagerLoadOptions();
                elo.LoadWithViewProperty();
                elo.LoadWith(ProductBomDetail.ProductBomProperty);
                elo.LoadWith(ProductBomDetail.ItemProperty);
            }

            return bomIds.SplitContains((tmpIds) =>
            {
                return Query<ProductBomDetail>().Where(p => tmpIds.Contains(p.ProductBomId)).ToList(null, elo);
            });
        }

        /// <summary>
        /// 获取产品
        /// </summary>
        /// <param name="bomId">产品明细BomId</param>
        /// <returns>ProductBomDetail列表</returns>
        /// <exception cref="ArgumentNullException">参数空引用异常</exception>
        public virtual EntityList<ProductBomDetail> GetProductBomDetails(double bomId)
        {
            if (bomId <= 0)
            {
                throw new ArgumentNullException(nameof(bomId));
            }

            return Query<ProductBomDetail>().Where(p => p.ProductBomId == bomId).ToList();
        }

        /// <summary>
        /// 获取产品BOM明细
        /// </summary>
        /// <param name="productBomId">产品BOM Id</param>
        /// <returns>BOM明细</returns>
        public virtual EntityList<ProductBomDetail> GetProductBomDetail(double productBomId)
        {
            return Query<ProductBomDetail>().Where(p => p.ProductBomId == productBomId).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 产品明细BomId
        /// </summary>
        /// <param name="itemId">产品ID</param>
        /// <returns>产品BOM明细</returns>
        public virtual EntityList<ProductBomDetail> GetProductBomDetailsByItemId(double itemId)
        {
            if (itemId <= 0)
            {
                return new EntityList<ProductBomDetail>();
            }

            var q = DB.Query<ProductBomDetail>();
            q.Join<ProductBom>((x, y) => x.ProductBomId == y.Id && y.ProductId == itemId && y.IsDefault);

            return q.ToList();
        }

        /// <summary>
        /// 获取产品bom明细(排除原材料)
        /// </summary>
        /// <param name="itemId">产品Id</param>
        /// <returns></returns>
        public virtual List<BaseDataInfo> GetProductBomDetailsByProductId(double itemId)
        {
            List<BaseDataInfo> baseDataInfos = new List<BaseDataInfo>();
            List<ItemType> types = new List<ItemType> { ItemType.Product, ItemType.SemiFinished };
            var q = Query<ProductBomDetail>().Join<ProductBom>((pbd, pb) => pbd.ProductBomId == pb.Id && pb.ProductId == itemId && pb.IsDefault)
                .LeftJoin<Item>((pbd, i) => pbd.ItemId == i.Id)
                .Where<Item>((pbd, i) => types.Contains(i.Type))
                .Select<Item>((pbd, i) => new
                {
                    Id = pbd.ItemId,
                    Code = i.Code,
                    Name = i.Name,
                }).ToList<BaseDataInfo>();
            baseDataInfos.AddRange(q);
            return baseDataInfos;
        }

        /// <summary>
        /// 获取产品BOM属性值列表
        /// </summary>
        /// <param name="productBomId">产品BOM ID</param>
        /// <param name="definitionId">物料定义ID</param>
        /// <returns>ProductBomPropertyValue列表</returns>
        /// <exception cref="ArgumentNullException">参数空引用</exception>
        public virtual EntityList<ProductBomPropertyValue> GetProductBomPropertyValues(double productBomId, double definitionId = 0)
        {
            if (productBomId <= 0)
            {
                throw new ArgumentNullException(nameof(productBomId));
            }
            var query = Query<ProductBomPropertyValue>().Where(p => p.ProductBomId == productBomId);
            if (definitionId > 0)
                query.Where(p => p.DefinitionId == definitionId);
            return query.ToList();
        }

        /// <summary>
        /// 获取产品BOM属性值
        /// </summary>
        /// <param name="definitionId">属性定义ID</param>
        /// <param name="value">属性值</param>
        /// <param name="productBomId">产品BOM</param>
        /// <returns>产品Bom物料属性值</returns>
        public virtual ProductBomPropertyValue GetProductBomPropertyValue(double definitionId, string value, double productBomId)
        {
            if (definitionId <= 0)
                throw new ArgumentNullException(nameof(definitionId));
            if (value == null)
                throw new ArgumentNullException(nameof(value));
            if (productBomId <= 0)
                throw new ArgumentNullException(nameof(productBomId));
            return Query<ProductBomPropertyValue>().Where(p => p.DefinitionId == definitionId && p.Value == value && p.ProductBomId == productBomId).FirstOrDefault();
        }
        /// <summary>
        /// 删除已删除，保存新增属性
        /// </summary>
        /// <param name="values">当前所有属性值</param>
        /// <param name="bomId">产品BomId</param>
        public virtual void SaveBomPropertyValues(List<ItemPropertyValue> values, double bomId)
        {
            using (var tran = DB.AutonomousTransactionScope(ItemEntityDataProvider.ConnectionStringName))
            {
                var propertyValues = RT.Service.Resolve<ProductBomController>().GetProductBomPropertyValues(bomId);  //产品Bom的所有属性值

                //删除属性
                propertyValues.Where(p => !values.Any(q => q.DefinitionId == p.DefinitionId && q.Value == p.Value)).ForEach(p =>
                {
                    p.PersistenceStatus = PersistenceStatus.Deleted;

                    var result = values.FirstOrDefault(q => q.DefinitionId == p.DefinitionId && q.Value == p.Value);
                    values.Remove(result);
                });

                propertyValues.ForEach(p =>
                {
                    var result = values.FirstOrDefault(q => q.DefinitionId == p.DefinitionId && q.Value == p.Value);
                    values.Remove(result); //移除已经存在的属性值
                });

                if (values.Count > 0)
                {
                    foreach (var value in values)  //新增属性值
                    {
                        ProductBomPropertyValue bomPropertyValue = new ProductBomPropertyValue()
                        {
                            Definition = value.Definition,
                            Value = value.Value,
                            ProductBomId = bomId
                        };

                        bomPropertyValue.PersistenceStatus = PersistenceStatus.New;
                        propertyValues.Add(bomPropertyValue);
                    }
                }

                RF.Save(propertyValues);
                tran.Complete();
            }
        }

        /// <summary>
        /// 获取某个产品Bom明细所有扩展属性（返回产品BOM明细id，和扩展属性）
        /// </summary>
        /// <param name="detailIds">产品Bom明细Id列表</param>
        public virtual List<(double, (string prop, string propName))> GetBomDetailExtProps(List<double> detailIds)
        {
            Check.NotNull(detailIds, "产品BOM明细ID集合不能为空".L10N());
            return DataProcessEx.SplitContains<(double, (string prop, string propName)), double>(detailIds, (ids) =>
             {
                 var temp = Query<ProductBomDetail>().Where(p => ids.Contains(p.Id)).ToList();
                 List<(double, (string prop, string propName))> results = new List<(double, (string prop, string propName))>();
                 temp.ForEach(p =>
                 {
                     results.Add((p.Id, (p.ItemExtProp, p.ItemExtPropName)));
                 });
                 return results;
             });
        }

        //该方法中的物料扩展属性对象先不更改，如果引用该方法的js控件确认可以删除，就直接删除控件和该方法
        /// <summary>
        /// 获取某个产品Bom明细的物料的所有属性值
        /// </summary>
        /// <param name="bomDetailId">产品Bom明细Id</param>
        /// <param name="itemId">物料Id</param>
        /// <returns>物料的所有属性值</returns>
        public virtual EntityList<ProductBomDetailPropertyValue> GetBomDetailPropertyValues(double bomDetailId, double itemId)
        {
            if (bomDetailId <= 0)
                throw new ArgumentException("产品BOM明细ID不合法".L10N());
            if (itemId <= 0)
                throw new ArgumentException("物料ID不合法".L10N());
            var selectedPropertyValues = GetProductBomDetailPropertyValues(bomDetailId);
            var selectPropertyValues = GetProdBomDetailPropertyValuesByItemId(itemId);
            foreach (var selectPropertyValue in selectPropertyValues)
            {
                foreach (var selectedPropertyValue in selectedPropertyValues)
                {
                    if (selectPropertyValue.DefinitionId == selectedPropertyValue.DefinitionId && selectPropertyValue.Value == selectedPropertyValue.Value)
                    {
                        selectPropertyValue.Id = selectedPropertyValue.Id;
                    }
                }

                selectPropertyValue.DetailId = bomDetailId;
            }

            return selectPropertyValues;
        }

        /// <summary>
        /// 获取产品BOM明细属性值列表
        /// </summary>
        /// <param name="detailId">属性定义ID</param>
        /// <returns>ProductBomDetailPropertyValue列表</returns>
        /// <exception cref="ArgumentNullException">参数空引用</exception>
        public virtual EntityList<ProductBomDetailPropertyValue> GetProductBomDetailPropertyValues(double detailId)
        {
            if (detailId <= 0)
            {
                throw new ArgumentNullException(nameof(detailId));
            }

            return Query<ProductBomDetailPropertyValue>().Where(p => p.DetailId == detailId).ToList();
        }

        /// <summary>
        /// 根据物料获取产品BOM明细属性值列表
        /// </summary>
        /// <param name="itemId">物料Id</param>
        /// <returns>ProductBomDetailPropertyValue列表</returns>
        /// <exception cref="ArgumentNullException">参数空引用</exception>
        public virtual EntityList<ProductBomDetailPropertyValue> GetProdBomDetailPropertyValuesByItemId(double itemId)
        {
            if (itemId <= 0)
            {
                throw new ValidationException("物料不能为空，请先选择物料！".L10N());
            }

            var itemPropValueList = RT.Service.Resolve<ItemController>().GetItemPropertys(itemId);

            var prodBomDetailValues = new EntityList<ProductBomDetailPropertyValue>();
            itemPropValueList.ForEach(p =>
            {
                var prodBomDetailValue = new ProductBomDetailPropertyValue
                {
                    DefinitionId = p.DefinitionId,
                    Definition = p.Definition,
                    Value = p.Value,
                    PropertyGroup = p.PropertyGroup,
                };

                prodBomDetailValues.Add(prodBomDetailValue);
            });

            prodBomDetailValues.SetTotalCount(itemPropValueList.Count);
            return prodBomDetailValues;
        }

        /// <summary>
        /// 获取产品BOM明细属性值
        /// </summary>
        /// <param name="definitionId">属性定义ID</param>
        /// <param name="value">value</param>
        /// <param name="detailId">产品BOM明细ID</param>
        /// <returns>ProductBomDetailPropertyValue</returns>
        /// <exception cref="ArgumentNullException">ArgumentNullException</exception>
        public virtual ProductBomDetailPropertyValue GetProductBomDetailPropertyValue(double definitionId, string value, double detailId)
        {
            if (definitionId <= 0)
                throw new ArgumentNullException(nameof(definitionId));
            if (value == null)
                throw new ArgumentNullException(nameof(value));
            if (detailId <= 0)
                throw new ArgumentNullException(nameof(detailId));
            return Query<ProductBomDetailPropertyValue>().Where(p => p.DefinitionId == definitionId && p.Value == value && p.DetailId == detailId).FirstOrDefault();
        }

        /// <summary>
        /// 保存产品Bom明细物料属性（弃用，因为更新为用字符串保存扩展属性，而不是用对象）
        /// </summary>
        /// <param name="selectedPropertyValues">选择的属性</param>
        /// <param name="detailId">明细Id</param>
        public virtual void SaveProductBomDetailPropertyValues(List<ProductBomDetailPropertyValue> selectedPropertyValues, double detailId)
        {
            Check.NotNull(selectedPropertyValues, "产品BOM明细属性值列表不能为空".L10N());
            if (detailId <= 0)
                throw new ArgumentException("物料ID不合法".L10N());
            EntityList<ProductBomDetailPropertyValue> propertyValueList = new EntityList<ProductBomDetailPropertyValue>();

            var orgPropertyValues = GetProductBomDetailPropertyValues(detailId);
            var existedPropertyIds = new List<double>();
            foreach (var selectedValue in selectedPropertyValues)
            {
                bool flag = false;
                foreach (var orgValue in orgPropertyValues)
                {
                    if (selectedValue.DefinitionId == orgValue.DefinitionId && selectedValue.Value == orgValue.Value)
                    {
                        flag = true;
                        if (!existedPropertyIds.Contains(orgValue.Id))
                            existedPropertyIds.Add(orgValue.Id);
                        break;
                    }
                }

                if (!flag)
                {
                    var newPropertyValue = new ProductBomDetailPropertyValue()
                    {
                        DetailId = detailId,
                        DefinitionId = selectedValue.DefinitionId,
                        Value = selectedValue.Value,
                        PropertyGroup = selectedValue.PropertyGroup,
                        PersistenceStatus = PersistenceStatus.New
                    };

                    propertyValueList.Add(newPropertyValue);
                }
            }

            var deletedPropertyValues = orgPropertyValues.Where(p => !existedPropertyIds.Contains(p.Id));
            deletedPropertyValues.ForEach(p =>
            {
                p.PersistenceStatus = PersistenceStatus.Deleted;
                propertyValueList.Add(p);
            });

            RF.Save(propertyValueList);
        }

        /// <summary>
        /// 获取相同物料相同工艺路线的BOM明细
        /// </summary>
        /// <param name="detail">BOM明细</param>
        /// <returns>true存在BOM明细，否则返回false</returns>
        public virtual int CountProductBomDetail(ProductBomDetail detail)
        {
            return Query<ProductBomDetail>().Where(p => p.Id != detail.Id && p.ProductBomId == detail.ProductBomId && p.ItemId == detail.ItemId && p.ProcessSegmentId == detail.ProcessSegmentId).Count();
        }

        /// <summary>
        /// 获取相同物料相同工艺路线的BOM明细（且相同扩展属性）
        /// </summary>
        /// <param name="detail">BOM明细</param>
        /// <returns>true存在BOM明细，否则返回false</returns>
        public virtual int CountProductBomDetailWithExtProp(ProductBomDetail detail)
        {
            //先选出相同物料且相同工艺路线的
            EntityList<ProductBomDetail> temp = Query<ProductBomDetail>().Where(p => p.Id != detail.Id && p.ProductBomId == detail.ProductBomId && p.ItemId == detail.ItemId && p.ProcessSegmentId == detail.ProcessSegmentId).ToList();
            if (temp.Count == 0)
                return 0;
            //再从里面选出扩展属性也一样的
            List<ItemPropertyInfo> detailPropertyInfo = GetPlanExExtPropInfo(detail.ItemExtProp, detail.ItemExtPropName);
            int result = 0;
            temp.ForEach(p =>
            {
                if (IsExtPropSame(detailPropertyInfo, p.ItemExtProp, p.ItemExtPropName))
                    result++;
            });
            return result;
        }

        #region 对于物料扩展属性的字符串字段，转化为对象，并判断是否相等的方法
        /// <summary>
        /// 根据物料扩展属性的两个字段，转化成对象返回
        /// </summary>
        /// <returns>扩展属性对象</returns>
        public virtual List<ItemPropertyInfo> GetPlanExExtPropInfo(string itemExtProp, string itemExtPropName)
        {
            List<ItemPropertyInfo> results = new List<ItemPropertyInfo>();
            if (itemExtProp.IsNullOrEmpty())
                return results;
            else
            {
                string[] idAndValues = itemExtProp.Split(';');
                string[] nameAndValues = itemExtPropName.Split(';');
                int index = -1;
                foreach (string idAndVaule in idAndValues)
                {
                    index++;
                    ItemPropertyInfo result = new ItemPropertyInfo();
                    result.RelationId = 0;
                    result.DetailId = "";
                    result.Value = idAndVaule.Split(':')[1];
                    result.DefinitionId = Double.Parse(idAndVaule.Split(':')[0]);
                    result.DefinitionName = nameAndValues[index].Split(':')[0];
                    result.PropertyGroup = "";
                    results.Add(result);
                }
                return results;
            }
        }

        /// <summary>
        /// 判断属性对象的List和属性字符串代表的扩展属性是否相同
        /// </summary>
        /// <param name="pvList">属性对象List</param>
        /// <param name="extProp">扩展属性</param>
        /// <param name="extPropName">扩展属性值</param>
        /// <returns></returns>
        public virtual bool IsExtPropSame(List<ItemPropertyInfo> pvList, string extProp, string extPropName)
        {
            if (pvList.IsNullOrEmpty())
            {
                pvList = new List<ItemPropertyInfo>();
            }
            var pvList2 = GetPlanExExtPropInfo(extProp, extPropName);
            if (IntersectPropertyGroup(pvList, pvList2, new List<int>() { pvList.Count, pvList2.Count }.Max()))
                return true;
            else
                return false;
        }

        /// <summary>
        /// 两个扩展属性列表取交集的属性组数量是否等于指定属性组数量
        /// </summary>
        /// <param name="pvList1">扩展属性列表1</param>
        /// <param name="pvList2">扩展属性列表2</param>
        /// <param name="propertyGroupCount">属性组数量</param>
        /// <returns>返回是否等于指定属性组数量</returns>
        public virtual bool IntersectPropertyGroup(List<ItemPropertyInfo> pvList1, List<ItemPropertyInfo> pvList2, int propertyGroupCount)
        {
            if (pvList1.IsNullOrEmpty() && pvList2.IsNullOrEmpty())
            {
                return true;
            }

            if ((pvList1.Count == 0 && pvList2.Count != 0) || (pvList1.Count != 0 && pvList2.Count == 0))
            {
                return false;
            }

            int samePropertyGroup = pvList1.Where(p => pvList2.Any(q => q.DefinitionId == p.DefinitionId && q.Value == p.Value && q.PropertyGroup == p.PropertyGroup))
                    .GroupBy(p => p.PropertyGroup).Count();

            return samePropertyGroup == propertyGroupCount;
        }
        #endregion

        /// <summary>
        /// 获取替代料列表
        /// </summary>
        /// <param name="bomDetailId">产品BOM明细id</param>
        /// <returns>替代料列表</returns>
        public virtual EntityList<Item> GetItemAlternative(double bomDetailId)
        {
            var alternative = Query<ProductBomDetailAlternative>().Where(p => p.BomDetailId == bomDetailId).Select(p => p.ItemId).ToList<double>().ToList();
            var itemList = RT.Service.Resolve<ItemController>().GetItemList(alternative);
            return itemList;
        }



        /// <summary>
        /// 保存替代料列表
        /// </summary>
        /// <param name="bomDetailId">产品BOM明细id</param>
        /// <param name="codes">替代料物料编码</param>
        public virtual void SaveAlternative(double bomDetailId, List<string> codes)
        {
            var items = RT.Service.Resolve<ItemController>().GetItems(codes);
            var oldAlternatives = Query<ProductBomDetailAlternative>().Where(p => p.BomDetailId == bomDetailId).ToList();
            foreach (var alternative in oldAlternatives)
            {
                if (!items.Any(p => p.Id == alternative.ItemId))
                    alternative.PersistenceStatus = PersistenceStatus.Deleted;
            }
            var newAlternatives = new EntityList<ProductBomDetailAlternative>();
            foreach (var item in items)
            {
                if (oldAlternatives.Any(p => p.ItemId == item.Id))
                    continue;
                var newAlternative = new ProductBomDetailAlternative();
                newAlternative.ItemId = item.Id;
                newAlternative.BomDetailId = bomDetailId;
                newAlternatives.Add(newAlternative);
            }
            using (var tran = DB.TransactionScope(ItemEntityDataProvider.ConnectionStringName))
            {
                RF.Save(oldAlternatives);
                RF.Save(newAlternatives);
                tran.Complete();
            }
        }


        /// <summary>
        /// 根据产品BOM的ID获取产品BOM列表
        /// </summary>
        /// <param name="bomIds">产品BOMID</param>
        /// <returns>产品BOM列表</returns>
        public virtual List<ProductBom> GetProductBomsByIds(List<double> bomIds)
        {
            List<ProductBom> boms = new List<ProductBom>();
            bomIds.SplitDataExecute(ids =>
            {
                boms.AddRange(Query<ProductBom>().Where(p => ids.Contains(p.Id))
                .ToList(null, new EagerLoadOptions().LoadWith(ProductBom.DetailListProperty).LoadWith(ProductBomDetail.ItemProperty)));
            });
            return boms;
        }

        /// <summary>
        /// 获取产品BOM数据
        /// </summary>
        /// <param name="ids">产品BOM ID</param>
        /// <returns>产品BOM数据</returns>
        public virtual EntityList<ProductBom> GetProductBomsByIds(double[] ids)
        {
            Check.NotNull(ids, "产品BOM ID数组不能空".L10N());
            return Query<ProductBom>().Where(p => ids.Contains(p.Id)).ToList();
        }

        #region 产品BOM下的组合替代
        /// <summary>
        /// 根据产品BOMID获取物料清单维护过的物料
        /// </summary>
        /// <param name="bomId">产品BOM id</param>
        /// <param name="pagInfo">分页对象</param>
        /// <param name="keyword">关键字</param>
        /// <returns>返回产品BOM对应的物料清单维护过的物料</returns>
        public virtual EntityList<Item> GetBomMainItemList(double bomId, PagingInfo pagInfo, string keyword)
        {
            var result = new EntityList<Item>();
            var allEntityList = GetProductBomDetailList(bomId, null);
            if (allEntityList.Any())//找不到数据
            {
                var entityList = new EntityList<ProductBomDetail>();

                var virtualPart = allEntityList.Where(m => m.ItemIsVirtualPart).ToList();//虚拟件
                var vitruItemBoms = GetVitruItemBom(virtualPart);
                if (vitruItemBoms.Any())
                    entityList.AddRange(vitruItemBoms);

                var bomList = allEntityList.Where(m => !m.ItemIsVirtualPart).ToList();

                if (bomList.Any())
                {
                    entityList.AddRange(bomList);
                }

                if (entityList.Any())
                {
                    var itemIds = entityList.Select(m => m.ItemId).ToList();
                    result = Query<Item>().Where(m => itemIds.Contains(m.Id) && m.State == State.Enable)
                        .WhereIf(!string.IsNullOrEmpty(keyword), (p => p.Code.Contains(keyword) || p.Name.Contains(keyword))).ToList(pagInfo);
                }
            }
            return result;
        }

        /// <summary>
        /// 获取虚拟BOM
        /// </summary>
        /// <param name="productBomDetails"></param>
        /// <returns></returns>
        public virtual List<ProductBomDetail> GetVitruItemBom(List<ProductBomDetail> productBomDetails)
        {
            List<ProductBomDetail> returnProductBomDetails = new List<ProductBomDetail>();

            var itemIds = productBomDetails.Select(m => m.ItemId).ToList();
            var entitys = itemIds.SplitContains((tempIds) => { return Query<ProductBom>().Where(w => tempIds.Contains(w.ProductId) && w.IsDefault).ToList(); });
            foreach (var entity in entitys)
            {
                var entityDtelList = Query<ProductBomDetail>().Where(w => w.ProductBomId == entity.Id).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
                if (entityDtelList.Any())
                {
                    var virtualPart = new List<ProductBomDetail>();//继续循环的虚拟件
                    var groups = entityDtelList.GroupBy(m => m.ItemIsVirtualPart).ToDictionary(p => p.Key, p => p.ToList());

                    if (groups.ContainsKey(true) && groups[true] != null && groups[true].Any())
                        virtualPart.AddRange(groups[true]);
                    if (groups.ContainsKey(false) && groups[false] != null && groups[false].Any())
                        returnProductBomDetails.AddRange(groups[false]);

                    if (virtualPart.Any())
                    {
                        var res = GetVitruItemBom(virtualPart);
                        if (res.Any())
                        { returnProductBomDetails.AddRange(res); }
                    }
                }

            }
            return returnProductBomDetails;
        }

        /// <summary>
        /// 获取Bom明细
        /// </summary>
        /// <param name="bomId"></param>
        /// <param name="processSegmentId"></param>
        /// <returns></returns>
        public virtual EntityList<ProductBomDetail> GetProductBomDetailList(double bomId, double? processSegmentId)
        {
            var query = Query<ProductBomDetail>().Where(w => w.ProductBomId == bomId);
            if (processSegmentId != null)
                query.Where(w => w.ProcessSegmentId == processSegmentId);
            return query.ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据产品BOMID获取物料清单维护过的替代料
        /// </summary>
        /// <param name="itemId">主料ID</param>
        /// <param name="type">物料类型</param>
        /// <param name="pagInfo">分页对象</param>
        /// <param name="keyword">关键字</param>
        /// <returns>返回产品BOM对应的物料清单维护过的替代料</returns>
        public virtual EntityList<Item> GetBomReplateItemList(double itemId, ItemType type, PagingInfo pagInfo, string keyword)
        {
            List<double> definitionIds = Query<ItemPropertyValue>().Where(p => p.ItemId == itemId).Select(p => p.DefinitionId).Distinct().ToList<double>().ToList();
            // 如果没有启用物料扩展属性
            if (definitionIds.Count == 0)
            {
                var query1 = Query<Item>().NotExists<ItemPropertyValue>((a, b) => b.Where(c => c.ItemId == a.Id)).Where(p => p.Type == type);
                if (!string.IsNullOrEmpty(keyword))
                {
                    query1.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
                }

                return query1.ToList(pagInfo);
            }

            // 如果启用物料扩展属性
            List<double> relItemIds = new List<double>();
            List<ItemPropertyValueViewModel> tmpItemDatas = Query<ItemPropertyValueViewModel>().Where(p => definitionIds.Contains(p.DefinitionId))
                .GroupBy(p => p.ItemId).Select(p => new { Item_Id = p.ItemId, Definition_Id = p.DefinitionId.COUNT() })
                .ToList<ItemPropertyValueViewModel>().ToList();
            relItemIds.AddRange(tmpItemDatas.Where(p => p.DefinitionId == definitionIds.Count).Select(p => p.ItemId).ToList());
            if (relItemIds.Count == 0)
                return new EntityList<Item>();

            var query = Query<Item>().Where(p => p.Type == type && relItemIds.Contains(p.Id));
            if (!string.IsNullOrEmpty(keyword))
            {
                query.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            }

            return query.ToList(pagInfo);
        }

        /// <summary>
        /// 获取产品BOM组合替代数据
        /// </summary>
        /// <param name="bomIds">产品BOM Id</param>
        /// <param name="elo">贪婪加载对象</param>
        /// <returns>产品BOM组合替代数据</returns>
        public virtual EntityList<CombinationReplate> GetCombinationReplates(List<double> bomIds, EagerLoadOptions elo)
        {
            Check.NotNull(bomIds, "产品BOM ID集合不能为空".L10N());
            return bomIds.SplitContains((tmpIds) =>
            {
                return Query<CombinationReplate>().Where(p => tmpIds.Contains(p.ProductBomId)).ToList(null, elo);
            });
        }

        /// <summary>
        /// 根据组合替代ID获取产品BOM组合替代的物料属性数据
        /// </summary>
        /// <param name="comReplateIds">组合替代 Id集合</param>
        /// <param name="elo">贪婪加载对象</param>
        /// <returns>产品BOM组合替代的物料属性数据</returns>
        public virtual EntityList<CombinationReplatePropertyValue> GetCombinationReplatePropertyValues(List<double> comReplateIds, EagerLoadOptions elo)
        {
            Check.NotNull(comReplateIds, "产品BOM组合替代 ID集合不能为空".L10N());
            return comReplateIds.SplitContains((tmpIds) =>
            {
                return Query<CombinationReplatePropertyValue>().Where(p => tmpIds.Contains(p.CombinationReplateId)).ToList(null, elo);
            });
        }

        /// <summary>
        /// 获取产品BOM下组合替代的物料属性列表
        /// </summary>
        /// <param name="comReplateId">属性定义ID</param>
        /// <returns>CombinationReplatePropertyValue列表</returns>
        /// <exception cref="ArgumentNullException">参数空引用</exception>
        protected virtual EntityList<CombinationReplatePropertyValue> GetCombinationReplatePropertyValues(double comReplateId)
        {
            return Query<CombinationReplatePropertyValue>().Where(p => p.CombinationReplateId == comReplateId).ToList();
        }

        /// <summary>
        /// 获取某个产品Bom下组合替代的物料属性
        /// </summary>
        /// <param name="comReplateId">产品Bom下组合替代Id</param>
        /// <param name="itemId">物料Id</param>
        /// <returns>物料的所有属性值</returns>
        public virtual EntityList<CombinationReplatePropertyValue> GetComReplatePropertyValues(double comReplateId, double itemId)
        {
            if (comReplateId <= 0)
                throw new ArgumentException("产品BOM明细ID不合法".L10N());
            if (itemId <= 0)
                throw new ArgumentException("物料ID不合法".L10N());

            var selPropertyValues = GetCombinationReplatePropertyValues(comReplateId);
            var showPropertyValues = GetComReplatePropertyValuesByItemId(itemId);
            foreach (var showPropertyValue in showPropertyValues)
            {
                showPropertyValue.CombinationReplateId = comReplateId;
                var tmpValue = selPropertyValues.FirstOrDefault(p => showPropertyValue.DefinitionId == p.DefinitionId && showPropertyValue.Value == p.Value && showPropertyValue.PropertyGroup == p.PropertyGroup);
                if (tmpValue != null)
                    showPropertyValue.Id = tmpValue.Id;
            }

            return showPropertyValues;
        }

        /// <summary>
        /// 根据物料获取产品BOM下组合替代的属性列表
        /// </summary>
        /// <param name="itemId">物料Id</param>
        /// <returns>CombinationReplatePropertyValue列表</returns>
        /// <exception cref="ArgumentNullException">参数空引用</exception>
        protected virtual EntityList<CombinationReplatePropertyValue> GetComReplatePropertyValuesByItemId(double itemId)
        {
            EagerLoadOptions elo = new EagerLoadOptions();
            elo.LoadWith(ItemPropertyValue.DefinitionProperty);
            var itemPropValueList = RT.Service.Resolve<ItemController>().GetItemPropertys(itemId, elo);
            var prodBomDetailValues = new EntityList<CombinationReplatePropertyValue>();
            itemPropValueList.ForEach(p =>
            {
                CombinationReplatePropertyValue prodBomDetailValue = new CombinationReplatePropertyValue
                {
                    DefinitionId = p.DefinitionId,
                    Definition = p.Definition,
                    Value = p.Value,
                    PropertyGroup = p.PropertyGroup
                };

                prodBomDetailValues.Add(prodBomDetailValue);
            });

            prodBomDetailValues.SetTotalCount(itemPropValueList.Count);
            return prodBomDetailValues;
        }

        /// <summary>
        /// 保存产品Bom下组合替代的物料属性
        /// </summary>
        /// <param name="selPropertyValues">选择的属性</param>
        /// <param name="comReplateId">组合替代ID</param>
        public virtual void SaveComReplatePropertyValues(List<CombinationReplatePropertyValue> selPropertyValues, double comReplateId)
        {
            Check.NotNull(selPropertyValues, "产品BOM组合替代属性值列表不能为空".L10N());
            if (comReplateId <= 0)
                throw new ArgumentException("组合替代ID不合法".L10N());
            EntityList<CombinationReplatePropertyValue> propertyValueList = new EntityList<CombinationReplatePropertyValue>();

            // 根据组合替代ID获取组合替代的物料属性
            var oldPropertyValues = GetCombinationReplatePropertyValues(comReplateId);
            var existedPropertyIds = new List<double>();
            foreach (var selValue in selPropertyValues)
            {
                var tmpValue = oldPropertyValues.FirstOrDefault(p => selValue.DefinitionId == p.DefinitionId && selValue.Value == p.Value && selValue.PropertyGroup == p.PropertyGroup);
                if (tmpValue != null)
                {
                    existedPropertyIds.Add(tmpValue.Id);
                }
                else
                {
                    var newPropertyValue = new CombinationReplatePropertyValue()
                    {
                        CombinationReplateId = comReplateId,
                        DefinitionId = selValue.DefinitionId,
                        Value = selValue.Value,
                        PropertyGroup = selValue.PropertyGroup,
                        PersistenceStatus = PersistenceStatus.New
                    };

                    propertyValueList.Add(newPropertyValue);
                }
            }

            using (var tran = DB.TransactionScope(ItemEntityDataProvider.ConnectionStringName))
            {
                List<double> valueIds = oldPropertyValues.Where(p => !existedPropertyIds.Contains(p.Id)).Select(p => p.Id).ToList();
                DB.Delete<CombinationReplatePropertyValue>().Where(p => valueIds.Contains(p.Id)).Execute();
                RF.Save(propertyValueList);

                tran.Complete();
            }
        }
        /// <summary>
        /// 保存前检验不同替代组的主料必须完全一致
        /// </summary>
        /// <param name="data"></param>
        public virtual void ValidateSumit(EntityList data)
        {
            EntityList<ProductBom> proBomList = data as EntityList<ProductBom>;
            List<string> bomNameList = proBomList.Where(p => p.DetailList.Any(q => q.ItemId <= 0)).Select(p => p.Name).ToList();
            if (bomNameList.Any())
            {
                throw new ValidationException("产品BOM[{0}]下的物料清单没有录入物料信息".L10nFormat(string.Join("、", bomNameList)));
            }

            List<double> itemIds = proBomList.SelectMany(p => p.DetailList).Where(p => string.IsNullOrEmpty(p.ItemExtProp))
                .Select(p => p.ItemId).Distinct().ToList();

            List<double> replateItemIds = proBomList.SelectMany(p => p.CombinationReplateList).Where(p => string.IsNullOrEmpty(p.PropertyValueJson))
                .Select(p => p.ItemId).Distinct().ToList();

            itemIds.AddRange(replateItemIds);
            if (itemIds.Count == 0)
                return;

            List<string> itemNames = Query<Item>().Where(p => p.EnableExtendProperty && itemIds.Contains(p.Id))
              .Select(p => p.Name).ToList<string>().ToList();
            if (itemNames.Count > 0)
            {
                throw new ValidationException("物料[{0}]启用了扩展属性，请录入物料属性！".L10nFormat(string.Join("、", itemNames)));
            }
        }

        /// <summary>
        /// 验证主物料、物料、替代组是否有重复数据
        /// </summary>
        /// <param name="comReplate">BOM明细</param>
        /// <returns>true存在BOM明细，否则返回false</returns>
        public virtual bool ValidateCombinationReplate(CombinationReplate comReplate)
        {
            if (comReplate.PropertyValueList.Count == 0)
            {
                return Query<CombinationReplate>().NotExists<CombinationReplatePropertyValue>((x, y) => y.Where(p => p.CombinationReplateId == x.Id))
                        .Where(p => p.Id != comReplate.Id && p.ProductBomId == comReplate.ProductBomId
                        && p.ItemId == comReplate.ItemId && p.MainMaterialId == comReplate.MainMaterialId && p.ReplateGroup == comReplate.ReplateGroup).Count() > 0;
            }

            bool result = true;
            foreach (var comReplateValue in comReplate.PropertyValueList)
            {
                if (!ValidateCombinationReplateValue(comReplateValue))
                {
                    result = false;
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// 验证主物料、物料、替代组是否有重复数据
        /// </summary>
        /// <param name="comReplateValue">BOM明细</param>
        /// <returns>true存在BOM明细，否则返回false</returns>
        public virtual bool ValidateCombinationReplateValue(CombinationReplatePropertyValue comReplateValue)
        {
            CombinationReplate comReplate = comReplateValue.CombinationReplate;
            return Query<CombinationReplatePropertyValue>().Join<CombinationReplate>((x, y) => x.CombinationReplateId == y.Id)
                        .Where<CombinationReplate>((x, y) => x.DefinitionId == comReplateValue.DefinitionId && x.Value == comReplateValue.Value
                        && y.MainMaterialId == comReplate.MainMaterialId && y.ItemId == comReplate.ItemId && y.ReplateGroup == comReplate.ReplateGroup).Count() > 0;
        }
        #endregion

        /// <summary>
        /// 校验：产品BOM被生产订单或需求管理引用为指定BOM，不允许修改产品
        /// </summary>
        /// <param name="bomId"></param>
        public virtual void ValidateApsRef(double bomId)
        {
            var dm = RT.Service.Resolve<IAppointedDemandManagement>().ExsitAppointedProductBomDemandManagement(bomId);
            var po = RT.Service.Resolve<IAppointedProductOrder>().ExsitAppointedProductBomProductOrder(bomId);
            if (dm || po)
            {
                throw new ArgumentNullException("产品BOM被生产订单或需求管理引用为指定BOM，不允许修改产品".L10N());
            }
        }

        /// <summary>
        /// 获取物料类型为成品的BOM列表
        /// </summary> 
        /// <param name="keyword">关键字</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <returns>BOM列表</returns>
        public virtual EntityList<ProductBom> GetItemDataList(PagingInfo pagingInfo, string keyword = null)
        {
            var query = Query<ProductBom>().Join<Item>((bom, it) => bom.ProductId == it.Id);
            if (!keyword.IsNullOrEmpty())
            {
                query.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            }
            query.Where<Item>((bom, it) => it.Type == ItemType.Product);
            return query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据产品BOM明细ID找产品BOM替代料
        /// </summary>
        /// <param name="detailIds">BOM明细ID</param>
        /// <returns>BOM替代料列表</returns>
        public virtual EntityList<ProductBomDetailAlternative> GetProductBomDetailAlternativeList(List<double> detailIds)
        {
            return detailIds.SplitContains(tempIds =>
            {
                return Query<ProductBomDetailAlternative>()
                    .Where(w => tempIds.Contains(w.BomDetailId)).ToList();
            });
        }

        /// <summary>
        /// 获取默认版本的产品Bom
        /// </summary>
        /// <param name="productIds">产品Id</param>
        /// <returns></returns>
        public virtual List<ProBomDtlInfo> GetDefaultProductBomDtls(IEnumerable<double> productIds)
        {
            List<ProBomDtlInfo> proBomDtlInfos = new List<ProBomDtlInfo>();
            productIds.SplitDataExecute(tempIds =>
            {
                var list = Query<ProductBomDetail>().Join<ProductBom>((pbd, pb) => pbd.ProductBomId == pb.Id && tempIds.Contains(pb.ProductId) && pb.IsDefault)
                .Select<ProductBom>((pbd, pb) => new
                {
                    ItemId = pbd.ItemId,
                    UnitQty = pbd.UnitQty,
                    LossRate = pbd.LossRate,
                    IsRecoilItem = pbd.IsRecoilItem,
                    ProductId = pb.ProductId,
                    BomCode = pb.Code,
                    BomName = pb.Name,
                    Version = pb.Version,
                }).ToList<ProBomDtlInfo>();
                proBomDtlInfos.AddRange(list);
            });
            return proBomDtlInfos;
        }

        /// <summary>
        /// 获取产品bom明细 key:产品编码 value:bom明细信息
        /// </summary>
        /// <param name="productCodes"></param>
        /// <returns></returns>
        public virtual ILookup<string, ProBomDtlInfo> GetProductBomDtls(IEnumerable<string> productCodes)
        {
            List<ProBomDtlInfo> proBomDtlInfos = new List<ProBomDtlInfo>();
            productCodes.SplitDataExecute(temps =>
            {
                var list = Query<ProductBomDetail>().Join<ProductBom>((pbd, pb) => pbd.ProductBomId == pb.Id)
                .LeftJoin<ProductBom, Item>("i", (pb, i) => pb.ProductId == i.Id)
                .Where<Item>((pbd, i) => temps.Contains(i.Code))
                .LeftJoin<ProductBomDetail, Item>("ii", (pbd, ii) => pbd.ItemId == ii.Id)
                .Select<ProductBom, Item, Item>((pbd, pb, i, ii) => new
                {
                    ItemId = pbd.ItemId,
                    ItemCode = ii.Code,
                    ProductId = pb.ProductId,
                    ProductCode = i.Code,
                }).ToList<ProBomDtlInfo>();
                proBomDtlInfos.AddRange(list);
            });
            return proBomDtlInfos.ToLookup(p => p.ProductCode);
        }

        /// <summary>
        /// 查询产品+项目号产品Bom key: 产品Id value: 产品BomId
        /// </summary>
        /// <param name="productIds">产品Ids</param>
        /// <param name="projectMaintainId">项目Id</param>
        /// <returns></returns>
        public virtual Dictionary<double, double> GetProductBomDicByProjectMaintain(IEnumerable<double> productIds, double projectMaintainId)
        {
            var list = productIds.SplitContains(tempIds => { return Query<ProductBom>().Where(p => tempIds.Contains(p.ProductId) && p.ProjectMaintainId == projectMaintainId && !p.IsDefault).ToList(); });
            return list.ToDictionary(p => p.ProductId, p => p.Id);
        }
    }
}