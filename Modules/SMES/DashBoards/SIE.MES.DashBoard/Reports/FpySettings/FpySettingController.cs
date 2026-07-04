using SIE.Domain;
using SIE.Items;
using SIE.Resources.Enterprises;
using SIE.Resources.WipResources;
using System.Collections.Generic;
using System.Linq;

namespace SIE.MES.DashBoard.Reports.FpySettings
{
    /// <summary>
    /// 直通率设置控制器
    /// </summary>
    public class FpySettingController : DomainController
    {
        /// <summary>
        /// 获取车间直通率设置列表
        /// </summary>
        /// <param name="criteria">车间直通率设置查询实体</param>
        /// <returns>车间直通率设置列表</returns>
        public virtual EntityList<ShopFpySetting> GetShopFpySettings(ShopFpySettingCriteria criteria)
        {
            var query = Query<ShopFpySetting>();
            if (criteria.ShopId.HasValue)
            {
                query.Where(p => p.ShopId == criteria.ShopId);
            }

            if (criteria.ResourceId.HasValue)
            {
                query.Exists<LineFpySetting>((x, y) => y.Where(p => p.ShopFpySettingId == x.Id && p.ResourceId == criteria.ResourceId));
            }

            return query.ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取车间直通率设置列表
        /// </summary>
        /// <returns>车间直通率设置列表</returns>
        public virtual Dictionary<string, FpySetting> GetShopFpySettings(List<string> lineNames)
        {
            Dictionary<string, FpySetting> dicLineNameSettings = new Dictionary<string, FpySetting>();
            var resources = Query<WipResource>()
                .Exists<Enterprise>((x, y) => y.Where(f => f.Id == x.WorkShopId && lineNames.Contains(x.Name)))
                .ToList();
            var dicShopSettings = Query<ShopFpySetting>()
                .Exists<Enterprise>((x, y) => y.Join<WipResource>((c, d) => c.Id == d.WorkShopId && lineNames.Contains(d.Name))
                .Where(p => p.Id == x.ShopId))
                .ToList().ToDictionary(p => p.ShopId);
            var dicLineSettings = Query<LineFpySetting>()
                .Exists<WipResource>((x, y) => y.Where(f => f.Id == x.ResourceId && lineNames.Contains(f.Name)))
                .ToList().ToDictionary(p => p.ResourceId);

            foreach (var resource in resources)
            {
                LineFpySetting lineSetting = null;
                if (dicLineSettings.TryGetValue(resource.Id, out lineSetting))
                {
                    FpySetting fpySetting = new FpySetting();
                    fpySetting.Alarm = lineSetting.Alarm;
                    fpySetting.Desired = lineSetting.Desired;
                    dicLineNameSettings.Add(resource.Name, fpySetting);
                }
                else if (resource.WorkShop != null)
                {
                    ShopFpySetting shopSetting = null;
                    if (dicShopSettings.TryGetValue(resource.WorkShopId.Value, out shopSetting))
                    {
                        FpySetting fpySetting = new FpySetting();
                        fpySetting.Alarm = shopSetting.Alarm;
                        fpySetting.Desired = shopSetting.Desired;
                        dicLineNameSettings.Add(resource.Name, fpySetting);
                    }
                }
                else
                {
                    //
                }
            }

            return dicLineNameSettings;
        }

        /// <summary>
        /// 根据车间Id集合获取
        /// 车间直通率设置列表
        /// </summary>
        /// <param name="shopIds">车间Id集合</param>
        /// <returns>车间直通率设置列表</returns>
        public virtual EntityList<ShopFpySetting> GetShopFpySettingsByShopIds(List<double> shopIds)
        {
            var query = Query<ShopFpySetting>();
            if (shopIds.Count > 0)
                query.Where(p => shopIds.Contains(p.ShopId));
            return query.ToList();
        }

        /// <summary>
        /// 根据产线Id集合获取
        /// 产线直通率设置列表
        /// </summary>
        /// <param name="lineIds">产线Id集合</param>
        /// <returns>产线直通率设置列表</returns>
        public virtual EntityList<LineFpySetting> GetLineFpySettingsByLineIds(List<double> lineIds)
        {
            var query = Query<LineFpySetting>();
            if (lineIds.Count > 0)
            {
                query.Where(p => lineIds.Contains(p.ResourceId));
            }

            return query.ToList();
        }

        /// <summary>
        /// 通过资源名称获取产线直通率设置
        /// </summary>
        /// <param name="lineNames">资源名称列表</param>
        /// <returns>产线直通率设置</returns>
        public virtual EntityList<LineFpySetting> GetLineFpySettingByLineName(List<string> lineNames)
        {
            EntityList<LineFpySetting> lineFpySettings = null;
            lineFpySettings = Query<LineFpySetting>()
                .Exists<WipResource>((x, y) => y.Where(f => f.Id == x.ResourceId && lineNames.Contains(f.Name)))
                .ToList();

            if (lineFpySettings != null)
            {
                return lineFpySettings;
            }

            lineFpySettings = Query<LineFpySetting>().Exists<WipResource>((x, y) => y.Join<Enterprise>((c, d) => c.WorkShopId == d.Id)
                     .Where(p => p.Id == x.ResourceId && lineNames.Contains(p.Name)))
                     .ToList();

            return lineFpySettings;
        }

        /// <summary>
        /// 获取产品直通率设置列表
        /// </summary>
        /// <returns>产品直通率设置列表</returns>
        public virtual Dictionary<string, FpySetting> GetAllProductFpySetting(List<string> productNames)
        {
            Dictionary<string, FpySetting> dicLineNameSettings = new Dictionary<string, FpySetting>();
            var products = Query<Item>().Exists<ProductModel>((x, y) => y.Where(p => p.Id == x.Model.Id)).Where(p => productNames.Contains(p.Name))
                .ToList();
            var dicModelSettings = Query<ProductModelFpySetting>()
                .Exists<ProductModel>((x, y) => y.Where(p => p.Id == x.Model.Id))
                .ToList().ToDictionary(p => p.ModelId);
            var dicProductSettings = Query<ProductFpySetting>()
                .Exists<Item>((x, y) => y.Where(f => f.Id == x.Product.Id && productNames.Contains(f.Name)))
                .ToList().ToDictionary(p => p.ProductId);

            foreach (var product in products)
            {
                ProductModelFpySetting modelSetting = null;
                ProductFpySetting productSetting = null;
                if (dicModelSettings.TryGetValue(product.Model.Id, out modelSetting))
                {
                    FpySetting fpySetting = new FpySetting();
                    fpySetting.Alarm = modelSetting.Alarm;
                    fpySetting.Desired = modelSetting.Desired;
                    dicLineNameSettings.Add(product.Name, fpySetting);
                }
                else if (dicProductSettings.TryGetValue(product.Id, out productSetting))
                {
                    FpySetting fpySetting = new FpySetting();
                    fpySetting.Alarm = productSetting.Alarm;
                    fpySetting.Desired = productSetting.Desired;
                    dicLineNameSettings.Add(product.Name, fpySetting);
                }
                else
                {
                    //
                }
            }

            return dicLineNameSettings;
        }

        /// <summary>
        /// 根据产线Id集合获取,如果产线Id无法找到，改为找车间直通率
        /// 产线直通率设置列表
        /// </summary>
        /// <param name="lineId">产线Id</param>
        /// <returns>产线直通率设置列表</returns>
        public virtual LineFpySetting GetLineFpySettingsByLineId(double lineId)
        {
            if (lineId == 0)
            {
                return null;
            }
            var query = Query<LineFpySetting>().Where(p => p.ResourceId == lineId);
            if (query.ToList().Count > 0)
            {
                return query.ToList()[0];
            }
            else
            {
                var resource = RF.GetById<WipResource>(lineId);
                var queryShop = Query<ShopFpySetting>().Where(p => p.ShopId == resource.WorkShopId);
                if (queryShop.ToList().Count > 0)
                {
                    LineFpySetting lfs = new LineFpySetting();
                    lfs.Alarm = queryShop.ToList()[0].Alarm;
                    lfs.Desired = queryShop.ToList()[0].Desired;
                    lfs.ResourceId = resource.Id;
                    lfs.Resource = resource;
                    return lfs;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 获取产品机型直通率设置列表
        /// </summary>
        /// <param name="criteria">产品机型直通率查询实体</param>
        /// <returns>产品机型直通率设置列表</returns>
        public virtual EntityList<ProductModelFpySetting> GetProductModelFpySettings(ProductModelFpySettingCriteria criteria)
        {
            var query = Query<ProductModelFpySetting>();
            if (criteria.ModelId.HasValue)
            {
                query.Where(p => p.ModelId == criteria.ModelId);
            }

            if (criteria.ProductId.HasValue)
            {
                query.Exists<ProductFpySetting>((x, y) => y.Where(p => p.ProductModelFpySettingId == x.Id && p.ProductId == criteria.ProductId));
            }

            return query.ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据产品机型Id集合获取
        /// 产品机型直通率设置列表
        /// </summary>
        /// <param name="modelIds">产品机型Id集合</param>
        /// <returns>产品机型直通率设置列表</returns>
        public virtual EntityList<ProductModelFpySetting> GetProductModelFpySettingsByModelIds(List<double> modelIds)
        {
            var query = Query<ProductModelFpySetting>();
            if (modelIds.Count > 0)
            {
                query.Where(p => modelIds.Contains(p.ModelId));
            }

            return query.ToList();
        }


        /// <summary>
        /// 根据引用判断是否可以删除企业模型和设备模型
        /// </summary>
        /// <param name="id">来源Id</param>
        /// <param name="sourceType">来源类型</param>
        /// <returns>true,false</returns>
        public virtual bool IsHasUsedResourse(double id, SyncSourceType sourceType)
        {
            //根据企业模型或设备中获取资源ID
            //判断该资源ID是否有被使用
            var res = RT.Service.Resolve<WipResourceController>().GetWipResource(id, sourceType);
            if (res == null)
            {
                return true;
            }

            return Query<LineFpySetting>().Where(p => p.ResourceId == res.Id).FirstOrDefault() == null;
        }

        /// <summary>
        /// 判断产线直通率设置是否引用指定的生产资源
        /// </summary>
        /// <param name="wipResourceId">生产资源Id</param>
        /// <returns>bool: false--工单未引用生产资源；true--工单已引用生产资源</returns>
        public virtual bool LineFpySettingHasUsedWipResource(double wipResourceId)
        {
            var shopFpySetting = Query<LineFpySetting>().Where(x => x.ResourceId == wipResourceId).FirstOrDefault();
            return shopFpySetting == null;
        }
    }
}