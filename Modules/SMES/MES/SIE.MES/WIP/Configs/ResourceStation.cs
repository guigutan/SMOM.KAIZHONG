using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Enterprises;
using SIE.Resources.WipResources;
using SIE.Tech.Stations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.MES.WIP.Configs
{
    /// <summary>
    /// 工位资源
    /// </summary>
    [RootEntity, Serializable]
    [Label("工位组织")]
    [DisplayMember(nameof(Name))]
    public class ResourceStation : StringEntity
    {
        /// <summary>
        /// 企业模型树形结构前缀 "r"
        /// </summary>
        public static readonly string EnterprisePre = "e";

        /// <summary>
        /// 企业模型树形结构前缀 "r"
        /// </summary>
        public static readonly string ScheduleResPre = "r";

        /// <summary>
        /// 工位树形结构前缀 "s"
        /// </summary>
        public static readonly string StationPre = "s";

        /// <summary>
        /// 构造函数
        /// </summary>
        public ResourceStation() { }

        /// <summary>
        /// 根据工位对象货区工位资源
        /// </summary>
        /// <param name="station">工位对象</param>
        /// <returns>返回工位资源</returns>
        public static ResourceStation Find(Station station)
        {
            return RF.Concrete<ResourceStationRepository>().AllData.EachNode(p => p.GetId().ToString() == StationPre + station.Id) as ResourceStation;
        }

        /// <summary>
        /// 根据企业模型获取工位资源
        /// </summary>
        /// <param name="resource">企业模型</param>
        /// <returns>返回工位资源</returns>
        public static ResourceStation Find(Enterprise resource)
        {
            return RF.Concrete<ResourceStationRepository>().AllData.EachNode(p => p.GetId().ToString() == EnterprisePre + resource.Id) as ResourceStation;
        }

        /// <summary>
        /// 根据企业模型获取工位资源
        /// </summary>
        /// <param name="resource">企业模型</param>
        /// <returns>返回工位资源</returns>
        public static ResourceStation Find(WipResource resource)
        {
            return RF.Concrete<ResourceStationRepository>().AllData.EachNode(p => p.GetId().ToString() == ScheduleResPre + resource.Id) as ResourceStation;
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="station">工位对象</param>
        /// <param name="treePid">treePid</param>
        internal ResourceStation(Station station, string treePid)
        {
            Id = StationPre + station.Id;
            Code = station.Code;
            Name = station.Name;
            Level = "工位".L10N();
            this.TreePId = treePid;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="resource">企业模型</param>
        internal ResourceStation(Enterprise resource)
        {
            Id = EnterprisePre + resource.Id;
            Code = resource.Code;
            Name = resource.Name;
            if (resource.Level != null)
                Level = resource.Level.Name;
            if (resource.TreePId != null)
            {
                this.TreePId = EnterprisePre + resource.TreePId;
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="resource">生产资源</param>
        internal ResourceStation(WipResource resource)
        {
            Id = ScheduleResPre + resource.Id;
            Code = resource.Code;
            Name = resource.Name;
            Level = "资源".L10N();
            if (resource.WorkShopId != null)
            {
                this.TreePId = EnterprisePre + resource.WorkShopId;
            }
        }

        #region 代码
        /// <summary>
        /// 代码
        /// </summary>
        [Label("代码")]
        public static readonly Property<string> CodeProperty = P<ResourceStation>.Register(e => e.Code);

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
        public static readonly Property<string> NameProperty = P<ResourceStation>.Register(e => e.Name);

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
        public static readonly Property<string> LevelProperty = P<ResourceStation>.Register(e => e.Level);

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
    class ResourceStationRepository : EntityRepository
    {
        /// <summary>
        /// 工位资源集合
        /// </summary>
        EntityList<ResourceStation> allData;

        /// <summary>
        /// 工位资源集合
        /// </summary>
        public virtual EntityList<ResourceStation> AllData
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
        /// 将企业模型与工位拼接成树形结构
        /// </summary>
        /// <returns>返回工位资源列表</returns>
        ////private EntityList<ResourceStation> GetResourceStationTree()
        ////{
        ////    EntityList<ResourceStation> allDataResult = new EntityList<ResourceStation>();

        ////    // 1、将企业模型数据先插入到工位资源树形结构中
        ////    var resoucres = RT.Service.Resolve<EnterpriseController>().GetEnterprises();
        ////    foreach (var resource in resoucres)
        ////    {
        ////        var station = new ResourceStation(resource);
        ////        allDataResult.Add(station);
        ////    }

        ////    // 2、将所有工位插入到工位资源树形结构中下（根据工位与资源来找对应位置）
        ////    EntityList<ResourceStation> stationsResult = new EntityList<ResourceStation>();
        ////    EntityList<Station> stations = RT.Service.Resolve<StationController>().GetStations();
        ////    foreach (var s in stations)
        ////    {
        ////        if (s.Resource != null)
        ////        {
        ////            string tempResID = string.Concat(ResourceStation.EnterprisePre, s.ResourceId.ToString());
        ////            foreach (ResourceStation r in allDataResult)
        ////            {
        ////                if (r.Id == tempResID)
        ////                {
        ////                    var station = new ResourceStation(s, r.Id);
        ////                    stationsResult.Add(station);
        ////                    break;
        ////                }
        ////            }
        ////        }
        ////        else
        ////        {
        ////            var station = new ResourceStation(s, string.Empty);
        ////            stationsResult.Add(station);
        ////        }
        ////    }

        ////    allDataResult.AddRange(stationsResult);

        ////    return allDataResult;
        ////}

        /// <summary>
        /// 将生产资源与工位拼接成树形结构
        /// </summary>
        /// <returns>返回工位资源列表</returns>
        private EntityList<ResourceStation> GetScheResStationTree()
        {
            //用于存储是否创建车间对象
            Dictionary<string, bool> dics = new Dictionary<string, bool>();

            EntityList<ResourceStation> allDataResult = new EntityList<ResourceStation>();

            // 1、将企业模型数据先插入到工位资源树形结构中
            var elo = new EagerLoadOptions();
            elo.LoadWith(WipResource.WorkShopProperty);
            elo.LoadWith(Enterprise.LevelProperty);
            var resoucres = RF.GetAll<WipResource>(eagerLoad: elo);
            foreach (var res in resoucres)
            {
                if (res.WorkShopId != null && !dics.ContainsKey(string.Concat(ResourceStation.EnterprisePre, res.WorkShopId.Value)))
                {
                    var shopStation = new ResourceStation(res.WorkShop);
                    allDataResult.Add(shopStation);
                    dics[shopStation.Id] = true;
                }

                var resStation = new ResourceStation(res);
                allDataResult.Add(resStation);
                dics[resStation.Id] = true;
            }

            // 2、将所有工位插入到工位资源树形结构中下（根据工位与资源来找对应位置）
            EntityList<ResourceStation> stationsResult = new EntityList<ResourceStation>();
            EntityList<Station> stations = RT.Service.Resolve<StationController>().GetStations();
            foreach (var s in stations)
            {
                if (s.ResourceId != 0)
                {
                    string tempResID = string.Concat(ResourceStation.ScheduleResPre, s.ResourceId.ToString());
                    if (!dics.ContainsKey(tempResID))
                        tempResID = string.Empty;
                    var station = new ResourceStation(s, tempResID);
                    stationsResult.Add(station);
                }
                else
                {
                    var station = new ResourceStation(s, string.Empty);
                    stationsResult.Add(station);
                }
            }

            allDataResult.AddRange(stationsResult);

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
            if (sid.IsNullOrWhiteSpace()) return null;
            if (sid.StartsWith(ResourceStation.StationPre))
            {
                var sidTemp = double.Parse(sid.Replace(ResourceStation.StationPre, string.Empty));
                var m = RF.Find<Station>().GetById(sidTemp) as Station;
                if (m == null) return null;
                return new ResourceStation(m, ResourceStation.EnterprisePre + m.ResourceId);
            }
            else if (sid.StartsWith(ResourceStation.EnterprisePre))
            {
                var sidTemp = double.Parse(sid.Replace(ResourceStation.EnterprisePre, string.Empty));
                var m = RF.Find<Enterprise>().GetById(sidTemp) as Enterprise;
                if (m == null) return null;
                return new ResourceStation(m);
            }
            else if (sid.StartsWith(ResourceStation.ScheduleResPre))
            {
                var sidTemp = double.Parse(sid.Replace(ResourceStation.ScheduleResPre, string.Empty));
                var m = RF.Find<WipResource>().GetById(sidTemp) as WipResource;
                if (m == null) return null;
                return new ResourceStation(m);
            }

            return null;
        }
    }

    /// <summary>
    /// 工位资源实体配置类
    /// </summary>
    class ResourceStationConfig : EntityConfig<ResourceStation>
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
