using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Enterprises;
using SIE.Resources.WipResources;
using System;
using System.Collections.Generic;

namespace SIE.MES.WIP.Configs
{
    /// <summary>
    /// 车间产线
    /// </summary>
    [RootEntity, Serializable]
    [Label("车间产线")]
    [DisplayMember(nameof(Name))]
    public class ShopResource : StringEntity
    {
        /// <summary>
        /// 系统级结构ID
        /// </summary>
        public static readonly string SystemId = "s001";

        /// <summary>
        /// 系统级结构前缀 "s"
        /// </summary>
        public static readonly string SystemPre = "s";

        /// <summary>
        /// 企业模型车间结构前缀 "s"
        /// </summary> 
        public static readonly string EnterprisePre = "s";

        /// <summary>
        /// 企业模型产线结构前缀 "r"
        /// </summary>
        public static readonly string WipResourcePre = "r";

        /// <summary>
        /// 构造函数
        /// </summary>
        public ShopResource() { }

        /// <summary>
        /// 根据企业模型获取工位资源
        /// </summary>
        /// <param name="resource">资源</param>
        /// <returns>返回工位资源</returns>
        public static ShopResource Find(WipResource resource)
        {
            var datas = RF.Concrete<ShopResourceRepository>().AllData;
            return datas.EachNode(p => p.GetId().ToString() == WipResourcePre + resource.Id) as ShopResource;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="resource">车间</param>
        internal ShopResource(Enterprise resource)
        {
            Id = EnterprisePre + resource.Id;
            Code = resource.Code;
            Name = resource.Name;
            if (resource.Level != null)
                Level = resource.Level.Name;
            TreePId = SystemId;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="resource">生产资源</param>
        internal ShopResource(WipResource resource)
        {
            Id = WipResourcePre + resource.Id;
            Code = resource.Code;
            Name = resource.Name;
            Level = "资源".L10N();
            if (resource.WorkShopId != null)
            {
                this.TreePId = EnterprisePre + resource.WorkShopId;
            }
            else
                this.TreePId = SystemId;
        }

        #region 代码
        /// <summary>
        /// 代码
        /// </summary>
        [Label("代码")]
        public static readonly Property<string> CodeProperty = P<ShopResource>.Register(e => e.Code);

        /// <summary>
        /// 代码
        /// </summary>
        public string Code
        {
            get { return this.GetProperty(CodeProperty); }
            set { this.SetProperty(CodeProperty, value); }
        }
        #endregion

        #region 名称
        /// <summary>
        /// 名称
        /// </summary>
        [Label("名称")]
        public static readonly Property<string> NameProperty = P<ShopResource>.Register(e => e.Name);

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return this.GetProperty(NameProperty); }
            set { this.SetProperty(NameProperty, value); }
        }
        #endregion

        #region 层级
        /// <summary>
        /// 组织层级
        /// </summary>
        [Label("层级")]
        public static readonly Property<string> LevelProperty = P<ShopResource>.Register(e => e.Level);

        /// <summary>
        /// 组织层级
        /// </summary>
        public string Level
        {
            get { return this.GetProperty(LevelProperty); }
            set { this.SetProperty(LevelProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 工位资源仓库
    /// </summary>
    [IgnoreProxy]
    class ShopResourceRepository : EntityRepository
    {
        /// <summary>
        /// 工位资源集合
        /// </summary>
        EntityList<ShopResource> allData;

        /// <summary>
        /// 工位资源集合
        /// </summary>
        public virtual EntityList<ShopResource> AllData
        {
            get
            {
                if (allData == null)
                {
                    allData = GetScheResStationTree();
                }

                return allData;
            }
        }

        /// <summary>
        /// 将生产资源与工位拼接成树形结构
        /// </summary>
        /// <returns>返回工位资源列表</returns>
        private EntityList<ShopResource> GetScheResStationTree()
        {
            EntityList<ShopResource> allDataResult = new EntityList<ShopResource>();
            var systemResource = new ShopResource()
            {
                Id = ShopResource.SystemId,
                Code = "系统".L10N(),
                Name = "系统".L10N(),
                Level = "系统".L10N()
            };
            allDataResult.Add(systemResource);
            //用于存储是否创建车间对象
            Dictionary<string, bool> dics = new Dictionary<string, bool>();
            var elo = new EagerLoadOptions();
            elo.LoadWith(WipResource.WorkShopProperty);
            elo.LoadWith(Enterprise.LevelProperty);

            var resoucres = RF.GetAll<WipResource>(new PagingInfo()
            {
                PageNumber = 1,
                PageSize = int.MaxValue - 1
            }, elo);

            foreach (var res in resoucres)
            {
                if (res.WorkShopId != null && !dics.ContainsKey(string.Concat(ShopResource.EnterprisePre, res.WorkShopId.Value)))
                {
                    var shopResource = new ShopResource(res.WorkShop);
                    allDataResult.Add(shopResource);
                    dics[shopResource.Id] = true;
                }

                var resStation = new ShopResource(res);
                allDataResult.Add(resStation);
                dics[resStation.Id] = true;
            }
            return allDataResult;
        }

        /// <summary>
        /// 获取工位资源集合
        /// </summary>
        /// <param name="paging">分页对象</param>
        /// <param name="eagerLoad">贪婪加载项</param>
        /// <returns>返回工位资源集合</returns>
        public override EntityList GetAll(PagingInfo paging = null, EagerLoadOptions eagerLoad = null)
        {
            return AllData;
        }

        /// <summary>
        /// 根据工位资源ID获取对应的工位或资源
        /// </summary>
        /// <param name="id">工位资源ID</param>
        /// <param name="eagerLoad">贪婪加载项</param>
        /// <returns>返回对应的工位或资源</returns>
        public override Entity GetById(object id, EagerLoadOptions eagerLoad = null)
        {
            var sid = id.ToString();
            if (sid.IsNullOrWhiteSpace())
                return null;
            if (sid == ShopResource.SystemId)
            {
                return new ShopResource()
                {
                    Id = ShopResource.SystemId,
                    Code = "系统".L10N(),
                    Name = "系统".L10N(),
                    Level = "系统".L10N()
                };
            }
            if (sid.StartsWith(ShopResource.EnterprisePre))
            {
                var m = RF.Find<Enterprise>().GetById(double.Parse(sid.Replace(ShopResource.EnterprisePre, string.Empty))) as Enterprise;
                if (m == null) return null;
                return new ShopResource(m);
            }
            else if (sid.StartsWith(ShopResource.WipResourcePre))
            {
                var m = RF.Find<WipResource>().GetById(double.Parse(sid.Replace(ShopResource.WipResourcePre, string.Empty))) as WipResource;
                if (m == null) return null;
                return new ShopResource(m);
            }

            return null;
        }
    }

    /// <summary>
    /// 工位资源实体配置类
    /// </summary>
    class ShopResourceConfig : EntityConfig<ShopResource>
    {
        /// <summary>
        /// 完成对 Meta 属性的配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.SupportTree();
        }
    }
}