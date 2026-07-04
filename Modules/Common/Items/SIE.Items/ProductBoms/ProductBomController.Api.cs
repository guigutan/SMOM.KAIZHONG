using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EventMessages;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Items.ProductBoms
{
    /// <summary>
    /// 产品BOM控制器
    /// </summary>
    public partial class ProductBomController : DomainController
    {
        /// <summary>
        /// ERP保存产品BOM数据
        /// </summary>
        /// <param name="datas">通信数据类</param>
        /// <returns>错误信息</returns>
        public virtual List<ErpErrorData> SaveProductBoms(List<ProductBomData> datas)
        {
            var errors = new List<ErpErrorData>();
            if (datas.Count == 0)
                return errors;

            //获取BOM数据
            var boms = GetProductBoms(datas.Select(p => p.Code).Distinct().ToList());
            var dicBom = boms.ToDictionary(p => p.Code);    //<bom编码,bom>

            //获取BOM明细数据
            var bomDetails = GetProductBomDetails(boms.Select(p => p.Id).ToList(), null);
            var dicBomDetails = bomDetails.GroupBy(p => p.ProductBom.Code).ToDictionary(p => p.Key, p => p.ToList());    //<bom编码,bom明细列表>

            //物料字典数据
            var itemCodes = datas.Select(p => p.ItemCode).ToList();
            itemCodes.AddRange(datas.SelectMany(p => p.DetailData).Select(d => d.ItemCode));
            var items = RT.Service.Resolve<ItemController>().GetItems(itemCodes.Distinct().ToList());
            var dicItem = items.ToDictionary(p => p.Code);

            //按顺序处理数据
            foreach (var p in datas)
            {
                try
                {
                    SaveProductBom(p, dicBom, dicItem, dicBomDetails);
                }
                catch (Exception ex)
                {
                    errors.Add(new ErpErrorData() { ErrMsg = ex.Message, Infkey = p.Infkey });
                }
            }

            return errors;
        }

        /// <summary>
        /// 执行数据保存
        /// </summary>
        /// <param name="data">数据实体</param>
        /// <param name="dic">数据字典</param>
        /// <param name="dicItem">数据字典</param>
        /// <param name="dicBomDetails">数据字典</param>
        private void SaveProductBom(ProductBomData data, Dictionary<string, ProductBom> dic, Dictionary<string, Item> dicItem, Dictionary<string, List<ProductBomDetail>> dicBomDetails)
        {
            //启用事务，保存主从数据完整性
            using (var trans = DB.TransactionScope(ItemEntityDataProvider.ConnectionStringName))
            {
                ProductBom entity;
                var key = data.Code;
                if (key.IsNullOrEmpty())
                    throw new ValidationException("产品BOM编码为空".L10nFormat(key));
                if (!dic.ContainsKey(key))
                    dic.Add(key, new ProductBom());
                entity = dic[key];
                //处理待删除数据
                if (data.IsDelete)
                {
                    DeleteEntity(dic, key, entity);
                    dicBomDetails.Remove(key);
                    return;
                }
                if (data.ItemCode.IsNullOrEmpty() || !dicItem.ContainsKey(data.ItemCode))
                    throw new ValidationException("产品编码[{0}]不存在".L10nFormat(data.ItemCode));

                entity.Product = dicItem[data.ItemCode];
                entity.Code = data.Code;
                entity.Name = data.Name;
                entity.Version = data.Version;
                RF.Save(entity);

                //处理明细
                var detailDatas = data.DetailData;
                if (!dicBomDetails.ContainsKey(key))
                    dicBomDetails.Add(key, new List<ProductBomDetail>());
                var dicDetail = dicBomDetails[key].ToDictionary(p => p.Item.Code);
                foreach (var detail in detailDatas)
                {
                    SaveProductBomDetail(detail, dicDetail, dicItem, entity);
                }
                trans.Complete();
            }
        }

        /// <summary>
        /// 保存产品BOM明细
        /// </summary>
        /// <param name="datas">ERP数据</param>
        /// <returns>错误信息</returns>
        public virtual List<ErpErrorData> SaveProductBomDetails(List<ProductBomDetailData> datas)
        {
            var errors = new List<ErpErrorData>();
            if (datas.Count == 0)
                return errors;

            //获取BOM数据
            var boms = GetProductBoms(datas.Select(p => p.ProductBomCode).Distinct().ToList());
            var dicBom = boms.ToDictionary(p => p.Code);    //<bom编码,bom>

            //获取BOM明细数据
            var bomDetails = GetProductBomDetails(boms.Select(p => p.Id).ToList(), null);
            var dicBomDetails = bomDetails.GroupBy(p => p.ProductBom.Code).ToDictionary(p => p.Key, p => p.ToList());    //<bom编码,bom明细列表>

            //物料字典数据
            var itemCodes = datas.Select(p => p.ItemCode).ToList();
            var items = RT.Service.Resolve<ItemController>().GetItems(itemCodes.Distinct().ToList());
            var dicItem = items.ToDictionary(p => p.Code);

            //按顺序处理数据
            foreach (var data in datas)
            {
                try
                {
                    var key = data.ProductBomCode;  //产品BOM编码为主键
                    if (!dicBom.ContainsKey(key))
                        throw new ValidationException("产品BOM[{0}]不存在".L10nFormat(key));
                    var bom = dicBom[key];
                    if (!dicBomDetails.ContainsKey(key))
                        dicBomDetails.Add(key, new List<ProductBomDetail>());
                    var dicDetail = dicBomDetails[key].ToDictionary(p => p.Item.Code);

                    SaveProductBomDetail(data, dicDetail, dicItem, bom);
                }
                catch (Exception ex)
                {
                    errors.Add(new ErpErrorData() { ErrMsg = ex.Message, Infkey = data.Infkey });
                }
            }

            return errors;
        }

        /// <summary>
        /// 执行数据保存
        /// </summary>
        /// <param name="data">数据实体</param>
        /// <param name="dic">数据字典</param>
        /// <param name="dicItem">数据字典</param>
        /// <param name="bom">产品BOM</param>
        private void SaveProductBomDetail(ProductBomDetailData data, Dictionary<string, ProductBomDetail> dic, Dictionary<string, Item> dicItem, ProductBom bom)
        {

            ProductBomDetail entity;
            var key = data.ItemCode;
            if (key.IsNullOrEmpty())
                throw new ValidationException("产品BOM[{0}]的明细行物料编码为空".L10nFormat(bom.Code));
            if (!dic.ContainsKey(key))
                dic.Add(key, new ProductBomDetail());
            entity = dic[key];
            //处理待删除数据
            if (data.IsDelete)
            {
                DeleteEntity(dic, key, entity);
                return;
            }

            entity.Item = dicItem[data.ItemCode];
            entity.UnitQty = data.UnitQty;
            entity.LossRate = data.LossRate;
            entity.Remark = data.Remark;
            entity.ProductBom = bom;
            RF.Save(entity);
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <typeparam name="E">实体类型</typeparam>
        /// <param name="dic">实体数据字典</param>
        /// <param name="key">待删除主键</param>
        /// <param name="entity">待删除实体</param>
        private void DeleteEntity<E>(Dictionary<string, E> dic, string key, E entity) where E : Entity
        {
            if (entity.PersistenceStatus != PersistenceStatus.New)
            {
                entity.PersistenceStatus = PersistenceStatus.Deleted;
                RF.Save(entity);
            }
            if (dic.ContainsKey(key))
                dic.Remove(key);
        }
    }
}
