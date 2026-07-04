using Newtonsoft.Json;
using SIE.Core.Barcodes;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.WIP.Products;
using SIE.Tech.Processs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.MES.WIP.Runtime
{
    /// <summary>
    /// 采集运行时控制器
    /// </summary>
    public class RuntimeController : DomainController
    {
        #region Puid

        /// <summary>
        /// CreatePuidKey
        /// </summary>
        /// <param name="barcode">条码</param>
        /// <param name="type">条码类型</param>
        /// <returns>返回PUIDKEY</returns>
        public virtual string CreatePuidKey(string barcode, BarcodeType type)
        {
            return ((int)type) + ":" + barcode;
        }

        /// <summary>
        /// 查询产品Puid,找不到返回null
        /// </summary>
        /// <param name="barcode">条码</param>
        /// <returns>返回Puid</returns>
        public virtual string FindPuid(CollectBarcode barcode)
        {
            return FindPuid(barcode.Code, barcode.Type);
        }

        /// <summary>
        /// 查询产品Puid,找不到返回null
        /// </summary>
        /// <param name="barcode">条码</param>
        /// <param name="type">条码类型</param>
        /// <returns>返回Puid</returns>
        public virtual string FindPuid(string barcode, BarcodeType type)
        {
            return Query<PuidMap>().Where(x => x.Id == CreatePuidKey(barcode, type))
                .Select(t => t.Puid).FirstOrDefault<string>();
        }

        /// <summary>
        /// 不知道条码类型，查找所有类型的条码
        /// </summary>
        /// <param name="barcode">条码</param>
        /// <returns>返回条码类型</returns>
        public virtual BarcodeType? FindMapBarcodeType(string barcode)
        {
            var csn = CreatePuidKey(barcode, BarcodeType.CSN);
            var sn = CreatePuidKey(barcode, BarcodeType.SN);
            var keyLabel = CreatePuidKey(barcode, BarcodeType.KeyLabel);
            var box = CreatePuidKey(barcode, BarcodeType.TurnoverBox);
            var combinedCode = CreatePuidKey(barcode, BarcodeType.CombinedCode);
            return Query<PuidMap>().Where(x => x.Id == csn || x.Id == sn || x.Id == keyLabel || x.Id == box || x.Id == combinedCode)
                .FirstOrDefault()?.BarcodeType;
        }

        /// <summary>
        /// 解绑条码集合
        /// </summary>
        /// <param name="barcode">条码</param>
        /// <exception cref="EntityNotFoundException">条码未关联Puid</exception>
        public virtual void UnmapPuid(CollectBarcode barcode)
        {
            var map = GetById<PuidMap>(CreatePuidKey(barcode.Code, barcode.Type));
            if (map == null)
                throw new EntityNotFoundException("[{0}] 并未关联PUID".L10nFormat(barcode));
            map.PersistenceStatus = PersistenceStatus.Deleted;
            RF.Save(map);
        }

        /// <summary>
        /// 添加匹配，如果匹配不存在则新建
        /// </summary>
        /// <param name="barcode">条码</param>
        /// <param name="puid">puid</param>
        /// <returns>返回是否匹配</returns>
        public virtual bool MapPuid(CollectBarcode barcode, string puid)
        {
            return MapPuid(barcode.Code, barcode.Type, puid);
        }

        /// <summary>
        /// 添加匹配，如果匹配不存在则新建
        /// </summary>
        /// <param name="code">条码</param>
        /// <param name="type">条码类型</param>
        /// <param name="puid">puid</param>
        /// <returns>返回是否匹配</returns>
        public virtual bool MapPuid(string code, BarcodeType type, string puid)
        {
            var key = CreatePuidKey(code, type);
            var map = Query<PuidMap>().Where(p => p.Id == key).FirstOrDefault();
            if (map != null)
            {
                if (map.Puid != puid)
                {
                    var nextProcess = FindProduct(map.Puid).Routing.GetNext().Select(p => p?.Name).Concat("、");
                    throw new ValidationException("[{0}]已关联,下一站是[{1}],不能重复关联".L10nFormat("{0}:{1}".FormatArgs(type.ToLabel(), code), nextProcess));
                }
            }
            else
            {
                map = new PuidMap
                {
                    Id = key,
                    Puid = puid,
                    Barcode = code,
                    BarcodeType = type
                };
                RF.Save(map);
                return true;
            }

            return false;
        }

        /// <summary>
        /// 查找Puid关联的条码
        /// </summary>
        /// <param name="puid">puid</param>
        /// <param name="barcodeType">条码类型</param>
        /// <returns>返回生产条码</returns>
        public virtual string FindMapBarcode(string puid, BarcodeType barcodeType)
        {
            return Query<PuidMap>().Where(p => p.BarcodeType == barcodeType && p.Puid == puid).Select(p => p.Barcode).FirstOrDefault<string>();
        }
        #endregion

        #region Product

        /// <summary>
        /// 查找采集运行时产品
        /// </summary>
        /// <param name="barcode">条码</param>
        /// <returns>返回product</returns>
        public virtual product FindProduct(CollectBarcode barcode)
        {
            var puid = FindPuid(barcode);
            if (puid == null)
                return null;
            return FindProduct(puid);
        }

        /// <summary>
        /// 查找采集运行时产品
        /// </summary>
        /// <param name="barcodes">条码列表</param>
        /// <param name="type">条码类型</param>
        /// <returns>返回product</returns>
        public virtual Dictionary<string, product> FindProduct(List<string> barcodes, BarcodeType type)
        {
            Dictionary<string, product> productsDictionary = new Dictionary<string, product>();

            if (barcodes == null || !barcodes.Any())
            {
                return productsDictionary;
            }

            var puidKeys = barcodes.Select(x => CreatePuidKey(x, type));

            var puidMaps = puidKeys.SplitContains(tempPuidKeys =>
             {
                 return Query<PuidMap>().Where(x => tempPuidKeys.Contains(x.Id))
                     .ToList();
             });

            var puids = puidMaps.Select(x => x.Puid).Distinct().ToList();

            var productEntities = puids.SplitContains(tempPuids =>
            {
                return Query<ProductEntity>().Where(x => tempPuids.Contains(x.Id)).ToList();
            });

            foreach (var barcode in barcodes)
            {
                var puidKey = CreatePuidKey(barcode, type);

                var puidMap = puidMaps.FirstOrDefault(x => x.Id == puidKey);

                if (puidMap == null)
                {
                    continue;
                }

                var productEntity = productEntities.FirstOrDefault(x => x.Id == puidMap.Puid);
                if (productEntity == null)
                {
                    continue;
                }

                string jsonProduct = CombineProduct(productEntity);

                productsDictionary.Add(barcode, JsonConvert.DeserializeObject<product>(jsonProduct));
            }

            return productsDictionary;
        }

        /// <summary>
        /// 查找采集运行时产品
        /// </summary>
        /// <param name="barcode">条码</param>
        /// <param name="type">条码类型</param>
        /// <returns>返回product</returns>
        public virtual product FindProduct(string barcode, BarcodeType type)
        {
            var puid = FindPuid(barcode, type);
            if (puid == null)
                return null;
            return FindProduct(puid);
        }

        /// <summary>
        /// 查找采集运行时产品
        /// </summary>
        /// <param name="puid">产品唯一主键</param>
        /// <returns>返回product</returns>
        public virtual product FindProduct(string puid)
        {
            var product = GetById<ProductEntity>(puid);
            if (product == null)
                return null;
            string jsonProduct = CombineProduct(product);
            return JsonConvert.DeserializeObject<product>(jsonProduct);
        }

        /// <summary>
        /// 保存采集运行时产品信息
        /// </summary>
        /// <param name="product">产品对象</param>
        public virtual void Save(product product)
        {
            var entity = GetById<ProductEntity>(product.Puid);
            if (entity == null)
            {
                entity = new ProductEntity()
                {
                    Id = product.Puid,
                    PersistenceStatus = PersistenceStatus.New
                };
                var jsonProduct = JsonConvert.SerializeObject(product);
                if (!jsonProduct.IsNullOrEmpty())
                {
                    var products = SplitProduct(jsonProduct, 2000);
                    SetProduct(entity, products);
                }
            }
            else
            {
                var jsonProduct = JsonConvert.SerializeObject(product);
                if (!jsonProduct.IsNullOrEmpty())
                {
                    var products = SplitProduct(jsonProduct, 2000);
                    SetProduct(entity, products);
                }

                entity.PersistenceStatus = PersistenceStatus.Modified;
            }

            RF.Save(entity);
        }

        /// <summary>
        /// 删除所有采集运行时数据
        /// </summary>
        /// <param name="product">产品对象</param>
        public virtual void RemoveProduct(product product)
        {
            using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                DB.Delete<PuidMap>().Where(p => p.Puid == product.Puid).Execute();

                DB.Delete<ProductEntity>().Where(p => p.Id == product.Puid).Execute();

                tran.Complete();
            }
        }

        /// <summary>
        /// 合并产品信息
        /// </summary>
        /// <param name="entity">产品信息</param>
        /// <returns>产品信息字符串</returns>
        public virtual string CombineProduct(ProductEntity entity)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(entity.Product1);
            sb.Append(entity.Product2);
            sb.Append(entity.Product3);
            sb.Append(entity.Product4);
            sb.Append(entity.Product5);
            sb.Append(entity.Product6);
            sb.Append(entity.Product7);
            sb.Append(entity.Product8);
            sb.Append(entity.Product9);
            sb.Append(entity.Product10);
            sb.Append(entity.Product11);
            sb.Append(entity.Product12);
            sb.Append(entity.Product13);
            sb.Append(entity.Product14);
            sb.Append(entity.Product15);
            sb.Append(entity.Product16);
            sb.Append(entity.Product17);
            sb.Append(entity.Product18);
            sb.Append(entity.Product19);
            sb.Append(entity.Product20);
            sb.Append(entity.Product21);
            sb.Append(entity.Product22);
            sb.Append(entity.Product23);
            sb.Append(entity.Product24);
            sb.Append(entity.Product25);
            sb.Append(entity.Product26);
            sb.Append(entity.Product27);
            sb.Append(entity.Product28);
            sb.Append(entity.Product29);
            sb.Append(entity.Product30);
            sb.Append(entity.Product31);
            sb.Append(entity.Product32);
            sb.Append(entity.Product33);
            sb.Append(entity.Product34);
            sb.Append(entity.Product35);
            sb.Append(entity.Product36);
            sb.Append(entity.Product37);
            sb.Append(entity.Product38);
            sb.Append(entity.Product39);
            sb.Append(entity.Product40);
            sb.Append(entity.Product41);
            sb.Append(entity.Product42);
            sb.Append(entity.Product43);
            sb.Append(entity.Product44);
            sb.Append(entity.Product45);
            sb.Append(entity.Product46);
            sb.Append(entity.Product47);
            sb.Append(entity.Product48);
            sb.Append(entity.Product49);
            sb.Append(entity.Product50);
            return sb.ToString();
        }

        /// <summary>
        /// 分割产品信息
        /// </summary>
        /// <param name="product">产品JSon字符串</param>
        /// <param name="num">拆分长度</param>
        /// <returns>产品信息数组</returns>
        public virtual string[] SplitProduct(string product, int num)
        {
            string tempStr = product;
            string[] strList = new string[50];
            int max = Convert.ToInt32(Math.Ceiling(product.Length / (num * 1.0)));
            for (int i = 1; i <= max; i++)
            {
                string result = tempStr.Substring(0, tempStr.Length > num ? num : tempStr.Length);
                strList[i - 1] = result;
                if (tempStr.Length > num)
                {
                    tempStr = tempStr.Substring(num, tempStr.Length - num);
                }
            }

            return strList;
        }

        /// <summary>
        /// 设置产品信息
        /// </summary>
        /// <param name="entity">产品信息</param>
        /// <param name="products">运行时信息数组</param>
        public virtual void SetProduct(ProductEntity entity, string[] products)
        {
            if (entity is null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            if (products is null)
            {
                throw new ArgumentNullException(nameof(products));
            }

            entity.Product1 = products[0];
            entity.Product2 = products[1];
            entity.Product3 = products[2];
            entity.Product4 = products[3];
            entity.Product5 = products[4];
            entity.Product6 = products[5];
            entity.Product7 = products[6];
            entity.Product8 = products[7];
            entity.Product9 = products[8];
            entity.Product10 = products[9];
            entity.Product11 = products[10];
            entity.Product12 = products[11];
            entity.Product13 = products[12];
            entity.Product14 = products[13];
            entity.Product15 = products[14];
            entity.Product16 = products[15];
            entity.Product17 = products[16];
            entity.Product18 = products[17];
            entity.Product19 = products[18];
            entity.Product20 = products[19];
            entity.Product21 = products[20];
            entity.Product22 = products[21];
            entity.Product23 = products[22];
            entity.Product24 = products[23];
            entity.Product25 = products[24];
            entity.Product26 = products[25];
            entity.Product27 = products[26];
            entity.Product28 = products[27];
            entity.Product29 = products[28];
            entity.Product30 = products[29];
            entity.Product31 = products[30];
            entity.Product32 = products[31];
            entity.Product33 = products[32];
            entity.Product34 = products[33];
            entity.Product35 = products[34];
            entity.Product36 = products[35];
            entity.Product37 = products[36];
            entity.Product38 = products[37];
            entity.Product39 = products[38];
            entity.Product40 = products[39];
            entity.Product41 = products[40];
            entity.Product42 = products[41];
            entity.Product43 = products[42];
            entity.Product44 = products[43];
            entity.Product45 = products[44];
            entity.Product46 = products[45];
            entity.Product47 = products[46];
            entity.Product48 = products[47];
            entity.Product49 = products[48];
            entity.Product50 = products[49];
        }

        /// <summary>
        /// 判断产品是否在胜制局中
        /// </summary>
        /// <param name="barcode">条码</param> 
        /// <returns>在局中返回true，否则返回false</returns>
        public virtual bool IsInInning(CollectBarcode barcode)
        {
            var product = FindProduct(barcode);
            return product?.Routing?.Current?.InInning == true;
        }
        #endregion 
    }
}
