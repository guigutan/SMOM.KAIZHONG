using SIE.Domain;
using SIE.Items;
using SIE.ObjectModel;
using SIE.Resources;
using SIE.Resources.WipResources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.SpcFromMess
{
    /// <summary>
    /// 统计过程控制-查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("统计过程控制-查询实体")]
    public class SpcFromMesCriteria:Criteria
    {
        #region 管制编号 No
        /// <summary>
        /// 管制编号
        /// </summary>
        [Label("管制编号")]
        public static readonly Property<string> NoProperty = P<SpcFromMesCriteria>.Register(e => e.No);

        /// <summary>
        /// 管制编号
        /// </summary>
        public string No
        {
            get { return this.GetProperty(NoProperty); }
            set { this.SetProperty(NoProperty, value); }
        }
        #endregion

        #region 管制名称 Name
        /// <summary>
        /// 管制名称
        /// </summary>
        [Label("管制名称")]
        public static readonly Property<string> NameProperty = P<SpcFromMesCriteria>.Register(e => e.Name);

        /// <summary>
        /// 管制名称
        /// </summary>
        public string Name
        {
            get { return this.GetProperty(NameProperty); }
            set { this.SetProperty(NameProperty, value); }
        }
        #endregion

        #region 产品  Item
        /// <summary>
        /// 产品ID
        /// </summary>
        [Label("产品")]
        public static readonly IRefIdProperty ItemIdProperty = P<SpcFromMesCriteria>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

        /// <summary>
        /// 产品ID
        /// </summary>
        public double? ItemId
        {
            get { return (double?)this.GetRefNullableId(ItemIdProperty); }
            set { this.SetRefNullableId(ItemIdProperty, value); }
        }


        /// <summary>
        /// 产品
        /// </summary>
        [Label("产品")]
        public static readonly RefEntityProperty<Item> ItemProperty = P<SpcFromMesCriteria>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 产品
        /// </summary>
        public Item Item
        {
            get { return this.GetRefEntity(ItemProperty); }
            set { this.SetRefEntity(ItemProperty, value); }
        }
        #endregion

        #region 管制项目 Project
        /// <summary>
        /// 管制项目
        /// </summary>
        [Label("管制项目")]
        public static readonly Property<string> ProjectProperty = P<SpcFromMesCriteria>.Register(e => e.Project);

        /// <summary>
        /// 管制项目
        /// </summary>
        public string Project
        {
            get { return this.GetProperty(ProjectProperty); }
            set { this.SetProperty(ProjectProperty, value); }
        }
        #endregion

        #region 生产部门 ProductionDept
        /// <summary>
        /// 生产部门
        /// </summary>
        [Label("生产部门")]
        public static readonly Property<string> ProductionDeptProperty = P<SpcFromMesCriteria>.Register(e => e.ProductionDept);

        /// <summary>
        /// 生产部门
        /// </summary>
        public string ProductionDept
        {
            get { return this.GetProperty(ProductionDeptProperty); }
            set { this.SetProperty(ProductionDeptProperty, value); }
        }
        #endregion

        #region 机台 Resource
        /// <summary>
        /// 机台ID
        /// </summary>
        [Label("机台")]
        public static readonly IRefIdProperty ResourceIdProperty = P<SpcFromMesCriteria>.RegisterRefId(e => e.ResourceId, ReferenceType.Normal);

        /// <summary>
        /// 机台ID
        /// </summary>
        public double? ResourceId
        {
            get { return (double?)this.GetRefNullableId(ResourceIdProperty); }
            set { this.SetRefNullableId(ResourceIdProperty, value); }
        }

        /// <summary>
        /// 机台
        /// </summary>
        [Label("机台")]
        public static readonly RefEntityProperty<WipResource> ResourceProperty = P<SpcFromMesCriteria>.RegisterRef(e => e.Resource, ResourceIdProperty);

        /// <summary>
        /// 机台
        /// </summary>
        public WipResource Resource
        {
            get { return this.GetRefEntity(ResourceProperty); }
            set { this.SetRefEntity(ResourceProperty, value); }
        }
        #endregion

        #region 测量人员 Inspector
        /// <summary>
        /// 测量人员ID
        /// </summary>
        [Label("测量人员")]
        public static readonly IRefIdProperty InspectorIdProperty = P<SpcFromMesCriteria>.RegisterRefId(e => e.InspectorId, ReferenceType.Normal);

        /// <summary>
        /// 测量人员ID
        /// </summary>
        public double? InspectorId
        {
            get { return (double?)this.GetRefNullableId(InspectorIdProperty); }
            set { this.SetRefNullableId(InspectorIdProperty, value); }
        }

        /// <summary>
        /// 测量人员
        /// </summary>
        [Label("测量人员")]
        public static readonly RefEntityProperty<Employee> InspectorProperty = P<SpcFromMesCriteria>.RegisterRef(e => e.Inspector, InspectorIdProperty);

        /// <summary>
        /// 测量人员
        /// </summary>
        public Employee Inspector
        {
            get { return this.GetRefEntity(InspectorProperty); }
            set { this.SetRefEntity(InspectorProperty, value); }
        }
        #endregion

        #region 创建时间 CreateTime
        /// <summary>
        /// 创建时间
        /// </summary>
        [Label("创建时间")]
        public static readonly Property<DateRange> CreateTimeProperty = P<SpcFromMesCriteria>.Register(e => e.CreateTime);

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateRange CreateTime
        {
            get { return this.GetProperty(CreateTimeProperty); }
            set { this.SetProperty(CreateTimeProperty, value); }
        }
        #endregion




        /// <summary>
        /// 查询方法
        /// </summary>
        /// <returns></returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<SpcFromMesController>().Fetch(this);
        }
    }


}
