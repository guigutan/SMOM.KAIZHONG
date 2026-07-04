using SIE.Domain;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.OrgLevels
{

    /// <summary>
    /// 组织架构查询实体
    /// </summary>
    [QueryEntity, Serializable]    
    public class OrgLevelCriteria : Criteria
    {

        /// <summary>
        /// 组织架构ID
        /// </summary>
        [Label("组织架构ID")]
        [RequiredAttribute]
        public static readonly Property<string> OrgCodeProperty = P<OrgLevelCriteria>.Register(e => e.OrgCode);

        /// <summary>
        /// 组织架构ID
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
        public static readonly Property<string> OrgNameProperty = P<OrgLevelCriteria>.Register(e => e.OrgName);

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
        [RequiredAttribute]
        public static readonly Property<string> ParentLevelProperty = P<OrgLevelCriteria>.Register(e => e.ParentLevel);

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
        [RequiredAttribute]
        public static readonly Property<string> TheLevelProperty = P<OrgLevelCriteria>.Register(e => e.TheLevel);

        /// <summary>
        /// 组织层级
        /// </summary>
        public string TheLevel
        {
            get { return this.GetProperty(TheLevelProperty); }
            set { this.SetProperty(TheLevelProperty, value); }
        }









        /// <summary>
        /// 查询方法
        /// </summary>
        /// <returns></returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<OrgLevelController>().Fetch(this);
        }
    }
}
