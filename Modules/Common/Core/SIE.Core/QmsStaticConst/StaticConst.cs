using SIE;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Core.QmsStaticConst
{
    /// <summary>
    /// 质量统计常用参数
    /// </summary>
    [RootEntity, Serializable]
    [CriteriaQuery]
    [Label("质量统计常用参数")]
    [DisplayMember(nameof(Name))]
    public partial class StaticConst : DataEntity
    {
        #region 编码 Code
        /// <summary>
        /// 编码
        /// </summary>
        [Label("编码")]
        [Required]
        [NotDuplicate]
        public static readonly Property<string> CodeProperty = P<StaticConst>.Register(e => e.Code);

        /// <summary>
        /// 编码
        /// </summary>
        public string Code
        {
            get { return GetProperty(CodeProperty); }
            set { SetProperty(CodeProperty, value); }
        }
        #endregion

        #region 参数（表）名称 Name
        /// <summary>
        /// 参数（表）名称
        /// </summary>
        [Label("参数（表）名称")]
        [Required]
        [NotDuplicate]
        public static readonly Property<string> NameProperty = P<StaticConst>.Register(e => e.Name);

        /// <summary>
        /// 参数（表）名称
        /// </summary>
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }
        #endregion

        #region d2表集合 ListD2
        /// <summary>
        /// d2表集合
        /// </summary>
        public static readonly ListProperty<EntityList<StaticConstD2>> ListD2Property = P<StaticConst>.RegisterList(e => e.ListD2);
        /// <summary>
        /// d2表集合
        /// </summary>
        public EntityList<StaticConstD2> ListD2
        {
            get { return this.GetLazyList(ListD2Property); }
        }
        #endregion

        #region 控制图参数集合 ListControlChart
        /// <summary>
        /// 控制图参数集合
        /// </summary>
        public static readonly ListProperty<EntityList<ControlChartConst>> ListControlChartProperty = P<StaticConst>.RegisterList(e => e.ListControlChart);
        /// <summary>
        /// 控制图参数集合
        /// </summary>
        public EntityList<ControlChartConst> ListControlChart
        {
            get { return this.GetLazyList(ListControlChartProperty); }
        }
        #endregion

        #region K1集合 ListK1
        /// <summary>
        /// K1集合
        /// </summary>
        public static readonly ListProperty<EntityList<StaticConstK1>> ListK1Property = P<StaticConst>.RegisterList(e => e.ListK1);
        /// <summary>
        /// K1集合
        /// </summary>
        public EntityList<StaticConstK1> ListK1
        {
            get { return this.GetLazyList(ListK1Property); }
        }
        #endregion

        #region K2集合 ListK2
        /// <summary>
        /// K2集合
        /// </summary>
        public static readonly ListProperty<EntityList<StaticConstK2>> ListK2Property = P<StaticConst>.RegisterList(e => e.ListK2);
        /// <summary>
        /// K2集合
        /// </summary>
        public EntityList<StaticConstK2> ListK2
        {
            get { return this.GetLazyList(ListK2Property); }
        }
        #endregion

        #region t值集合 ListT
        /// <summary>
        /// t值集合
        /// </summary>
        public static readonly ListProperty<EntityList<StaticConstT>> ListTProperty = P<StaticConst>.RegisterList(e => e.ListT);
        /// <summary>
        /// t值集合
        /// </summary>
        public EntityList<StaticConstT> ListT
        {
            get { return this.GetLazyList(ListTProperty); }
        }
        #endregion

        #region K3集合 ListK3
        /// <summary>
        /// K3集合
        /// </summary>
        public static readonly ListProperty<EntityList<StaticConstK3>> ListK3Property = P<StaticConst>.RegisterList(e => e.ListK3);
        /// <summary>
        /// K3集合
        /// </summary>
        public EntityList<StaticConstK3> ListK3
        {
            get { return this.GetLazyList(ListK3Property); }
        }
        #endregion
    }

    /// <summary>
    /// MAS常用参数 实体配置
    /// </summary>
    internal class MsaConstConfig : EntityConfig<StaticConst>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("MSA_CONST").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}