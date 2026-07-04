using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.ProjectDesigns
{
    /// <summary>
    /// 项目号需求设计详情
    /// </summary>
    [RootEntity, Serializable]
    [Label("项目号需求设计详情")]
    public class ProjectDesignDetail : ProjectDesignBase
    {
        #region 视图属性
        #region 产品名称 ProductName
        /// <summary>
        /// 产品名称
        /// </summary>
        [Label("产品名称")]
        public static readonly Property<string> ProductNameProperty = P<ProjectDesignDetail>.RegisterView(e => e.ProductName, p => p.Product.Name);

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName
        {
            get { return this.GetProperty(ProductNameProperty); }
        }
        #endregion

        #endregion
    }

    /// <summary>
    /// 实体配置
    /// </summary>
    public class ProjectDesignDetailConfig : EntityConfig<ProjectDesignDetail>
    {
        /// <summary>
        /// 数据库配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("MES_PRO_DESIGN").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
