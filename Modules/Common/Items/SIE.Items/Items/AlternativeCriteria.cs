using SIE.Common;
using SIE.Domain;
using System;

namespace SIE.Items.Items
{
    /// <summary>
    /// 替代料查询实体
    /// </summary>
    [QueryEntity, Serializable]
    public class AlternativeCriteria : Criteria
    {
        #region 排除替代料Id FilterId
        /// <summary>
        /// 排除替代料Id
        /// </summary>
        public static readonly Property<double[]> FilterIdProperty = P<AlternativeCriteria>.Register(e => e.FilterId);

        /// <summary>
        /// 排除替代料Id
        /// </summary>
        public double[] FilterId
        {
            get { return this.GetProperty(FilterIdProperty); }
            set { this.SetProperty(FilterIdProperty, value); }
        }
        #endregion

        #region 编码 Code

        /// <summary>
        /// 编码
        /// </summary>
        public static readonly Property<string> CodeProperty = P<AlternativeCriteria>.Register(e => e.Code);

        /// <summary>
        /// 编码
        /// </summary>
        public string Code
        {
            get { return GetProperty(CodeProperty); }
            set { SetProperty(CodeProperty, value); }
        }

        #endregion

        #region 名称 Name

        /// <summary>
        /// 名称
        /// </summary>
        public static readonly Property<string> NameProperty = P<AlternativeCriteria>.Register(e => e.Name);

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }

        #endregion

        #region 描述 Description

        /// <summary>
        /// 描述
        /// </summary>
        public static readonly Property<string> DescriptionProperty = P<AlternativeCriteria>.Register(e => e.Description);

        /// <summary>
        /// 描述
        /// </summary>
        public string Description
        {
            get { return GetProperty(DescriptionProperty); }
            set { SetProperty(DescriptionProperty, value); }
        }

        #endregion

        #region Type 类型

        /// <summary>
        /// 类型
        /// </summary>
        public static readonly Property<ItemType?> TypeProperty = P<AlternativeCriteria>.Register(e => e.Type);

        /// <summary>
        /// 类型
        /// </summary>
        public ItemType? Type
        {
            get { return GetProperty(TypeProperty); }
            set { SetProperty(TypeProperty, value); }
        }

        #endregion

        #region State 状态

        /// <summary>
        /// 状态
        /// </summary>
        public static readonly Property<State?> StateProperty = P<AlternativeCriteria>.Register(e => e.State);

        /// <summary>
        /// 状态
        /// </summary>
        public State? State
        {
            get { return GetProperty(StateProperty); }
            set { SetProperty(StateProperty, value); }
        }

        #endregion

        #region MeasurementUnit 计量单位

        /// <summary>
        /// 计量单位
        /// </summary>
        public static readonly Property<string> UnitProperty = P<AlternativeCriteria>.Register(e => e.Unit);

        /// <summary>
        /// 计量单位
        /// </summary>
        public string Unit
        {
            get { return GetProperty(UnitProperty); }
            set { SetProperty(UnitProperty, value); }
        }

        #endregion

        #region DrawingNo 图号

        /// <summary>
        /// 图号
        /// </summary>
        public static readonly Property<string> DrawingNoProperty = P<AlternativeCriteria>.Register(e => e.DrawingNo);

        /// <summary>
        /// 图号
        /// </summary>
        public string DrawingNo
        {
            get { return GetProperty(DrawingNoProperty); }
            set { SetProperty(DrawingNoProperty, value); }
        }

        #endregion

        #region Model 机型

        /// <summary>
        /// 机型
        /// </summary>
        public static readonly Property<string> ModelProperty = P<AlternativeCriteria>.Register(e => e.Model);

        /// <summary>
        /// 机型
        /// </summary>
        public string Model
        {
            get { return GetProperty(ModelProperty); }
            set { SetProperty(ModelProperty, value); }
        }

        #endregion

        #region Person 责任人
        /// <summary>
        /// 责任人
        /// </summary>
        public static readonly Property<string> PersonProperty = P<AlternativeCriteria>.Register(e => e.Person);

        /// <summary>
        /// 责任人
        /// </summary>
        public string Person
        {
            get { return GetProperty(PersonProperty); }
            set { SetProperty(PersonProperty, value); }
        }

        #endregion

        #region DataSource 数据来源
        /// <summary>
        /// 数据来源
        /// </summary>
        public static readonly Property<SourceType?> DataSourceProperty = P<AlternativeCriteria>.Register(e => e.DataSource);

        /// <summary>
        /// 数据来源
        /// </summary>
        public SourceType? DataSource
        {
            get { return this.GetProperty(DataSourceProperty); }
            set { this.SetProperty(DataSourceProperty, value); }
        }
        #endregion

        #region 物料标签类型
        /// <summary>
        /// 物料标签类型
        /// </summary>
        public static readonly Property<ItemLabelType?> ItemLabelTypeProperty = P<AlternativeCriteria>.Register(e => e.ItemLabelType);

        /// <summary>
        /// 物料标签类型
        /// </summary>
        public ItemLabelType? ItemLabelType
        {
            get { return this.GetProperty(ItemLabelTypeProperty); }
            set { this.SetProperty(ItemLabelTypeProperty, value); }
        }
        #endregion

        /// <summary>
        /// 查询实体默认查询方法
        /// </summary>
        /// <returns>EntityList</returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<ItemController>().GetItems(this, FilterId);
        }
    }
}
