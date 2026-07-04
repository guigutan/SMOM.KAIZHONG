using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.WipResources;
using System;
using System.Linq;

namespace SIE.Kit.MES.CallMaterials.Configs
{
    /// <summary>
    /// 资源仓库
    /// </summary>
    [RootEntity, Serializable]
    [Label("资源仓库")]
    [DisplayMember(nameof(Name))]
    public class ResourceWarehouse : StringEntity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ResourceWarehouse() 
        {
            // 构造函数
        }

        /// <summary>
        /// 根据企业模型获取资源仓库
        /// </summary>
        /// <param name="resourceId">企业模型</param>
        /// <returns>返回资源仓库</returns>
        public static ResourceWarehouse Find(double resourceId)
        {
            return RF.Concrete<ResourceWarehouseRepository>().AllData.FirstOrDefault(p => p.GetId().ToString() == resourceId.ToString());
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="resource">资源对象</param>       
        internal ResourceWarehouse(WipResource resource)
        {
            Id = resource.Id.ToString();
            Code = resource.Code;
            Name = resource.Name;
        }

        #region 代码
        /// <summary>
        /// 代码
        /// </summary>
        [Label("编码")]
        public static readonly Property<string> CodeProperty = P<ResourceWarehouse>.Register(e => e.Code);

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
        public static readonly Property<string> NameProperty = P<ResourceWarehouse>.Register(e => e.Name);

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return this.GetProperty(NameProperty); }
            set { this.SetProperty(NameProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 资源仓库
    /// </summary>
    [IgnoreProxy]
    class ResourceWarehouseRepository : EntityRepository
    {

        /// <summary>
        /// 库存组织
        /// </summary>
        int? currentInvOrg = 0;

        /// <summary>
        /// 员工Id
        /// </summary>
        double currentEmpId;

        /// <summary>
        /// 资源仓库集合
        /// </summary>
        public virtual EntityList<ResourceWarehouse> AllData
        {
            get
            {
                if (currentInvOrg != RT.InvOrg || currentEmpId != RT.IdentityId)
                {
                    currentInvOrg = RT.InvOrg;
                    currentEmpId = RT.IdentityId;
                }
                return GetResourceWarehouseTree();//商议后决定去掉缓存
            }
        }

        /// <summary>
        /// 将企业模型与工位拼接成树形结构
        /// </summary>
        /// <returns>返回资源仓库列表</returns>
        private EntityList<ResourceWarehouse> GetResourceWarehouseTree()
        {
            EntityList<ResourceWarehouse> allDataResult = new EntityList<ResourceWarehouse>();

            // 1、将企业模型数据先插入到资源仓库树形结构中
            var resoucres = RT.Service.Resolve<WipResourceController>().GetWipResourcesByEmp(null, string.Empty);
            foreach (var resource in resoucres)
            {
                var res = new ResourceWarehouse(resource);
                allDataResult.Add(res);
            }

            return allDataResult;
        }

        /// <summary>
        /// 获取资源仓库集合
        /// </summary>
        /// <param name="paging">分页对象</param>
        /// <param name="eagerLoad">贪婪加载项</param>
        /// <returns>返回资源仓库集合</returns>
        public override EntityList GetAll(PagingInfo paging = null, EagerLoadOptions eagerLoad = null)
        {
            return AllData;
        }

        /// <summary>
        /// 返回显示值
        /// </summary>
        /// <param name="id">资源Id</param>
        /// <param name="eagerLoad">eagerLoad</param>
        /// <returns>当前实体</returns>
        public override Entity GetById(object id, EagerLoadOptions eagerLoad = null)
        {
            if (id != null && !id.ToString().IsNullOrWhiteSpace())
            {
                return new ResourceWarehouse(RF.GetById<WipResource>(double.Parse(id.ToString())));
            }
            return null;
        }
    }

    /// <summary>
    /// 资源仓库实体配置类
    /// </summary>
    class ResourceWarehouseConfig : EntityConfig<ResourceWarehouse>
    {
        /// <summary>
        /// 完成对 Meta 属性的配置
        /// </summary>
        protected override void ConfigMeta()
        {
            // 完成对 Meta 属性的配置
        }
    }
}
