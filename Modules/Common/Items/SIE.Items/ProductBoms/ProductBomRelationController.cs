using SIE.Data;
using SIE.Domain;
using SIE.Items.ProductBoms.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.Items.ProductBoms
{
    /// <summary>
    /// 产品BOM关系控制器
    /// </summary>
    public class ProductBomRelationController : DomainController
    {
        /// <summary>
        /// 获取下层产品BOM
        /// </summary>
        /// <param name="itemIds">物料ID</param>
        /// <param name="typeList">来源类型集合</param>
        /// <returns>返回下层产品BOM</returns>
        public virtual List<ProductBomRelationViewModel> GetChildProductBomList(List<double> itemIds, List<ItemSourceType> typeList)
        {
            List<ProductBomRelationViewModel> orderRelList = GetChildProductBoms(itemIds, typeList);

            return orderRelList;
        }

        /// <summary>
        /// 获取下层产品BOM
        /// </summary>
        /// <param name="itemIds">物料ID</param>
        /// <param name="typeList">类型集合</param>
        /// <returns>返回下层产品BOM</returns>
        public virtual List<ProductBomRelationViewModel> GetChildProductBomList(List<double> itemIds, List<ItemType> typeList)
        {
            List<ProductBomRelationViewModel> orderRelList = GetChildProductBoms(itemIds, typeList);

            return orderRelList;
        }

        /// <summary>
        /// 获取下层产品BOM
        /// </summary>
        /// <param name="itemIds">物料ID</param>
        /// <param name="typeList">来源类型集合</param>
        /// <returns>返回下层产品BOM</returns>
        protected virtual List<ProductBomRelationViewModel> GetChildProductBoms(List<double> itemIds, List<ItemSourceType> typeList)
        {
            itemIds = itemIds.Distinct().ToList();
            HashSet<double> topItemIds = new HashSet<double>(itemIds);
            List<ProductBomRelationViewModel> orderRelList = new List<ProductBomRelationViewModel>();
            using (var db = DbAccesserFactory.Create(ItemEntityDataProvider.ConnectionStringName))
            {
                List<double> bottomItemIds = new List<double>();
                bottomItemIds.AddRange(itemIds);
                while (bottomItemIds.Count > 0)
                {
                    string sql = GetProductBomSql(bottomItemIds);
                    using (System.Data.IDataReader dr = db.ExecuteReader(sql))
                    {
                        var relOrders = GetProductBomRelationViewModel(dr);
                        relOrders = relOrders.Where(p => p.ChildItemSourceType.HasValue && typeList.Contains(p.ChildItemSourceType.Value)).ToList();
                        orderRelList.AddRange(relOrders);

                        var searchItemIds = relOrders.Where(p => !topItemIds.Contains(p.ItemId)).Select(p => p.ItemId).Distinct().ToList();
                        searchItemIds.ForEach(p => topItemIds.Add(p)); // 记录上层物料信息，防止循环嵌套

                        bottomItemIds = relOrders.Where(p => p.ChildItemId.HasValue && !topItemIds.Contains(p.ChildItemId.Value)).Select(p => p.ChildItemId.Value).Distinct().ToList();
                    }
                }
            }

            return orderRelList;
        }

        /// <summary>
        /// 获取下层产品BOM
        /// </summary>
        /// <param name="itemIds">物料ID</param>
        /// <param name="typeList">类型集合</param>
        /// <returns>返回下层产品BOM</returns>
        protected virtual List<ProductBomRelationViewModel> GetChildProductBoms(List<double> itemIds, List<ItemType> typeList)
        {
            itemIds = itemIds.Distinct().ToList();
            HashSet<double> topItemIds = new HashSet<double>(itemIds);
            List<ProductBomRelationViewModel> orderRelList = new List<ProductBomRelationViewModel>();
            using (var db = DbAccesserFactory.Create(ItemEntityDataProvider.ConnectionStringName))
            {
                List<double> bottomItemIds = new List<double>();
                bottomItemIds.AddRange(itemIds);
                while (bottomItemIds.Count > 0)
                {
                    string sql = GetProductBomSql(bottomItemIds);
                    using (System.Data.IDataReader dr = db.ExecuteReader(sql))
                    {
                        var relOrders = GetProductBomRelationViewModel(dr);
                        relOrders = relOrders.Where(p => p.ChildType.HasValue && typeList.Contains(p.ChildType.Value)).ToList();
                        orderRelList.AddRange(relOrders);

                        var searchItemIds = relOrders.Where(p => !topItemIds.Contains(p.ItemId)).Select(p => p.ItemId).Distinct().ToList();
                        searchItemIds.ForEach(p => topItemIds.Add(p)); // 记录上层物料信息，防止循环嵌套

                        bottomItemIds = relOrders.Where(p => p.ChildItemId.HasValue && !topItemIds.Contains(p.ChildItemId.Value)).Select(p => p.ChildItemId.Value).Distinct().ToList();
                    }
                }
            }

            return orderRelList;
        }

        /// <summary>
        /// 设置物料扩展属性
        /// </summary>
        /// <param name="orderRelList">订单关系列表</param>
        protected virtual void SetItemExtProp(List<ProductBomRelationViewModel> orderRelList)
        {
            //// List<double> bomIds = orderRelList.Select(p => p.BomId).Distinct().ToList();

        }

        /// <summary>
        /// 根据产品BOMID获取产品BOM产品属性
        /// </summary>
        /// <param name="bomIds">产品BOMid</param>
        /// <returns>返回产品BOM产品属性</returns>
        protected virtual EntityList<ProductBomPropertyValue> GetProductBomPropertyValues(List<double> bomIds)
        {
            return bomIds.SplitContains(tmpIds =>
            {
                return Query<ProductBomPropertyValue>().Where(p => tmpIds.Contains(p.ProductBomId)).ToList();
            });
        }

        /// <summary>
        /// 获取产品BOM关系数据
        /// </summary>
        /// <param name="dr">数据提供者</param>
        /// <returns>返回产品BOM关系数据</returns>
        private List<ProductBomRelationViewModel> GetProductBomRelationViewModel(System.Data.IDataReader dr)
        {
            List<ProductBomRelationViewModel> orderRelList = new List<ProductBomRelationViewModel>();
            while (dr.Read())
            {
                ProductBomRelationViewModel bomRel = new ProductBomRelationViewModel();
                bomRel.BomId = Convert.ToDouble(dr[0]);
                bomRel.ItemId = Convert.ToDouble(dr[1]);
                if (dr[2] != null)
                    bomRel.ItemExtProp = dr[2].ToString();
                if (dr[3] != null)
                    bomRel.ItemExtPropName = dr[3].ToString();

                if (dr[4] != null)
                    bomRel.ChildItemId = Convert.ToDouble(dr[4]);
                if (dr[5] != null)
                    bomRel.ChildItemExtProp = dr[5].ToString();
                if (dr[6] != null)
                    bomRel.ChildItemExtPropName = dr[6].ToString();
                if (dr[7] != null)
                    bomRel.UnitQty = Convert.ToDecimal(dr[7]);

                if (dr[8] != null)
                    bomRel.ChildItemCode = dr[8].ToString();
                if (dr[9] != null)
                    bomRel.ChildItemSourceType = (ItemSourceType?)Convert.ToInt32(dr[9]);
                if (dr[10] != null)
                    bomRel.ProcessSegmentId = Convert.ToDouble(dr[10]);
                if (dr[11] != null)
                    bomRel.ChildType = (ItemType?)Convert.ToInt32(dr[11]);
                if (dr[12] != null)
                    bomRel.LossRate = Convert.ToDecimal(dr[12]);
                if (dr[13] != null)
                    bomRel.IsRecoilItem = Convert.ToInt32(dr[13]) == 1;

                bomRel.ItemPropertyInfoList = bomRel.ItemExtProp.ItemExtPropToList(bomRel.ItemId);
                if (bomRel.ChildItemId.HasValue)
                    bomRel.ChildItemPropertyInfoList = bomRel.ChildItemExtProp.ItemExtPropToList(bomRel.ChildItemId.Value);

                orderRelList.Add(bomRel);
            }

            return orderRelList;
        }

        /// <summary>
        /// 获取查询产品BOM数据的SQL
        /// </summary>
        /// <param name="itemIds">物料ID集合</param>
        /// <returns>返回查询产品BOM数据的SQL</returns>
        private string GetProductBomSql(List<double> itemIds)
        {
            var bomMeta = RF.Find<ProductBom>().EntityMeta;
            var bomTableName = bomMeta.TableMeta.TableName;
            var bomDetailMeta = RF.Find<ProductBomDetail>().EntityMeta;
            var bomDetailTableName = bomDetailMeta.TableMeta.TableName;
            var itemMeta = RF.Find<Item>().EntityMeta;
            var itemTableName = itemMeta.TableMeta.TableName;

            string idStr = bomMeta.Property(Entity.IdProperty).ColumnMeta.ColumnName;                           // 产品BOM-BOM ID
            string productIdStr = bomMeta.Property(ProductBom.ProductIdProperty).ColumnMeta.ColumnName;         // 产品BOM-产品 ID
            string itemExtPropStr = bomMeta.Property(ProductBom.ItemExtPropProperty).ColumnMeta.ColumnName;         // 产品BOM-扩展属性
            string itemExtPropNameStr = bomMeta.Property(ProductBom.ItemExtPropNameProperty).ColumnMeta.ColumnName;         // 产品BOM-扩展属性

            string itemIdStr = bomDetailMeta.Property(ProductBomDetail.ItemIdProperty).ColumnMeta.ColumnName;   // 产品BOM明细-物料 ID
            string childItemExtPropStr = bomDetailMeta.Property(ProductBomDetail.ItemExtPropProperty).ColumnMeta.ColumnName;   // 产品BOM明细-物料扩展属性
            string childItemExtPropNameStr = bomDetailMeta.Property(ProductBomDetail.ItemExtPropNameProperty).ColumnMeta.ColumnName;   // 产品BOM明细-物料扩展属性
            string unitQtyStr = bomDetailMeta.Property(ProductBomDetail.UnitQtyProperty).ColumnMeta.ColumnName; // 产品BOM明细-单位用量
            string codeStr = itemMeta.Property(Item.CodeProperty).ColumnMeta.ColumnName;                        // 物料 - 编号
            string sourceTypeStr = itemMeta.Property(Item.ItemSourceTypeProperty).ColumnMeta.ColumnName;        // 物料 - 来源类型
            string typeStr = itemMeta.Property(Item.TypeProperty).ColumnMeta.ColumnName;                        // 物料-物料类型
            string segmentStr = bomDetailMeta.Property(ProductBomDetail.ProcessSegmentIdProperty).ColumnMeta.ColumnName; // 产品BOM明细 - 工段ID

            string proBomIdStr = bomDetailMeta.Property(ProductBomDetail.ProductBomIdProperty).ColumnMeta.ColumnName;   // 产品明细 - 产品BOM ID
            string defaultStr = bomMeta.Property(ProductBom.IsDefaultProperty).ColumnMeta.ColumnName;                   // 产品BOM - 是否默认
            string phantomStr = bomDetailMeta.Property(PhantomEntityExtension.IS_PHANTOMProperty).ColumnMeta.ColumnName;// 产品BOM 假删除

            string detailLossRateStr = bomDetailMeta.Property(ProductBomDetail.LossRateProperty).ColumnMeta.ColumnName;//产品BOM明细 - 损耗率

            string detailIsRecoilItemStr = bomDetailMeta.Property(ProductBomDetail.IsRecoilItemProperty).ColumnMeta.ColumnName;//产品BOM明细 - 是否反冲物料

            //string orderIdsParam = string.Concat("'", string.Join("','", itemIds), "'");

            //string conditionStr = string.Empty;
            //conditionStr = string.Format(" tmp.{0} IN ({1})", productIdStr, orderIdsParam);

            //string sql = string.Format(@"WITH tmp AS
            //                     (SELECT t.{0}
            //                            ,t.{1}
            //                            ,t.{2}
            //                            ,t.{3}
            //                            ,t2.{4}
            //                            ,t2.{5} {5}1
            //                            ,t2.{6} {6}1
            //                            ,t2.{7}
            //                            ,t3.{8}
            //                            ,t3.{9}
            //                            ,t2.{10}
            //                            ,t3.{11}
            //                        FROM {12} t
            //                        LEFT JOIN {13} t2
            //                          ON t.{0} = t2.{14}
            //                        LEFT JOIN {15} t3
            //                          ON t2.{4} = t3.{0}
            //                       WHERE t.{16} = '0'
            //                         AND t2.{16} != '1'
            //                         AND t.{17} = '1')
            //                    SELECT tmp.* FROM tmp WHERE {18}",
            //        idStr, productIdStr, itemExtPropStr, itemExtPropNameStr, // BOM ID， 产品ID，产品扩展属性, 产品扩展属性,
            //        itemIdStr, childItemExtPropStr, childItemExtPropNameStr, unitQtyStr, codeStr, sourceTypeStr, segmentStr, typeStr, // 物料ID，物料扩展属性，物料扩展属性， 单位用量，物料编号，物料来源类型, 工段ID, 物料类型，物料属性Json，物料属性值
            //        bomTableName, bomDetailTableName, proBomIdStr, itemTableName, // BOM表名，BOM明细表名，产品明细-产品BOMID， 物料表名
            //        phantomStr, defaultStr, conditionStr); // 假删除，产品BOM-是否默认，ID条件

            List<string> inParamList = new List<string>();
            itemIds.SplitDataExecute(ids =>
            {
                string orderIdsParam = string.Concat("'", string.Join("','", ids), "'");
                inParamList.Add(orderIdsParam);
            });

            StringBuilder sb = new StringBuilder();
            foreach (string param in inParamList)
            {
                if (sb.Length > 0)
                    sb.Append(" or ");
                sb.Append(string.Format(" tmp.{0} IN ({1})", productIdStr, param));
            }

            string conditionStr = sb.ToString();
            string sql = $@"WITH tmp AS
                                 (SELECT t.{idStr}
                                        ,t.{productIdStr}
                                        ,t.{itemExtPropStr}
                                        ,t.{itemExtPropNameStr}
                                        ,t2.{itemIdStr}
                                        ,t2.{childItemExtPropStr} {childItemExtPropStr}1
                                        ,t2.{childItemExtPropNameStr} {childItemExtPropNameStr}1
                                        ,t2.{unitQtyStr}
                                        ,t3.{codeStr}
                                        ,t3.{sourceTypeStr}
                                        ,t2.{segmentStr}
                                        ,t3.{typeStr}
                                        ,t2.{detailLossRateStr}
                                        ,t2.{detailIsRecoilItemStr}
                                    FROM {bomTableName} t
                                    LEFT JOIN {bomDetailTableName} t2
                                      ON t.{idStr} = t2.{proBomIdStr}
                                    LEFT JOIN {itemTableName} t3
                                      ON t2.{itemIdStr} = t3.{idStr}
                                   WHERE t.{phantomStr} = '0'
                                     AND t2.{phantomStr} != '1'
                                     AND t.{defaultStr} = '1')
                                SELECT tmp.* FROM tmp WHERE {conditionStr}";

            return sql;
        }
    }
}