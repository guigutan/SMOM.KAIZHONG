using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.OrgLevels
{

    /// <summary>
    /// 人员组织架构信息
    /// </summary>
    [RootEntity, Serializable]
    [Label("人员组织架构信息")]
    [ConditionQueryType(typeof(OrgLevelCriteria))]
    public class OrgLevel : DataEntity
    {

        /// <summary>
        /// 组织架构代码
        /// </summary>
        [Label("组织架构代码")]
        [RequiredAttribute]
        [NotDuplicateAttribute]
        public static readonly Property<string> OrgCodeProperty = P<OrgLevel>.Register(e => e.OrgCode);
        /// <summary>
        /// 组织架构代码
        /// </summary>
        public string OrgCode
        {
            get { return this.GetProperty(OrgCodeProperty); }
            set { this.SetProperty(OrgCodeProperty, value); }
        }

        /// <summary>
        /// 组织架构名称
        /// </summary>
        [Label("组织架构名称")]
        [RequiredAttribute]      
        public static readonly Property<string> OrgNameProperty = P<OrgLevel>.Register(e => e.OrgName);
        /// <summary>
        /// 组织架构名称
        /// </summary>
        public string OrgName
        {
            get { return this.GetProperty(OrgNameProperty); }
            set { this.SetProperty(OrgNameProperty, value); }
        }

        /// <summary>
        /// 上级组织
        /// </summary>
        [Label("上级组织")]      
        public static readonly Property<string> ParentLevelProperty = P<OrgLevel>.Register(e => e.ParentLevel);
        /// <summary>
        /// 上级组织
        /// </summary>
        public string ParentLevel
        {
            get { return this.GetProperty(ParentLevelProperty); }
            set { this.SetProperty(ParentLevelProperty, value); }
        }

        /// <summary>
        /// 组织层级
        /// </summary>
        [Label("组织层级")]      
        public static readonly Property<string> OrgLevelProperty = P<OrgLevel>.Register(e => e.TheLevel);
        /// <summary>
        /// 组织层级
        /// </summary>
        public string TheLevel
        {
            get { return this.GetProperty(OrgLevelProperty); }
            set { this.SetProperty(OrgLevelProperty, value); }
        }


        internal class OrgLevelConfig : EntityConfig<OrgLevel>
        {
            /// <summary>
            /// 配置数据库映射
            /// </summary>
            protected override void ConfigMeta()
            {
                Meta.MapTable("ORG_LEVEL").MapAllProperties();
                Meta.EnablePhantoms();
                Meta.DisableInvOrg();//不启用库存组织
            }

        }
    }
}
