using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources;
using System;

namespace SIE.MES.WIP.Products
{
    /// <summary>
    /// 产品工艺路线变更事件
    /// </summary>
    [RootEntity, Serializable]
    [Label("产品工艺路线变更事件")]
    public partial class WipProductRoutingEvent : DataEntity
    {
        #region 旧Layout OldLayout
        /// <summary>
        /// 旧Layout
        /// </summary>
        [Label("旧Layout")]
        public static readonly Property<string> OldLayoutProperty = P<WipProductRoutingEvent>.Register(e => e.OldLayout);

        /// <summary>
        /// 旧Layout
        /// </summary>
        public string OldLayout
        {
            get { return GetProperty(OldLayoutProperty); }
            set { SetProperty(OldLayoutProperty, value); }
        }
        #endregion

        #region 新Layout NewLayout
        /// <summary>
        /// 新Layout
        /// </summary>
        [Label("新Layout")]
        public static readonly Property<string> NewLayoutProperty = P<WipProductRoutingEvent>.Register(e => e.NewLayout);

        /// <summary>
        /// 新Layout
        /// </summary>
        public string NewLayout
        {
            get { return GetProperty(NewLayoutProperty); }
            set { SetProperty(NewLayoutProperty, value); }
        }
        #endregion

        #region 更改时间 ChangeDate
        /// <summary>
        /// 更改时间
        /// </summary>
        [Label("更改时间")]
        public static readonly Property<DateTime> ChangeDateProperty = P<WipProductRoutingEvent>.Register(e => e.ChangeDate);

        /// <summary>
        /// 更改时间
        /// </summary>
        public DateTime ChangeDate
        {
            get { return GetProperty(ChangeDateProperty); }
            set { SetProperty(ChangeDateProperty, value); }
        }
        #endregion

        #region 产品工艺路线 Routing
        /// <summary>
        /// 产品工艺路线Id
        /// </summary>
        [Label("产品工艺路线")]
        public static readonly IRefIdProperty RoutingIdProperty = P<WipProductRoutingEvent>.RegisterRefId(e => e.RoutingId, ReferenceType.Normal);

        /// <summary>
        /// 产品工艺路线Id
        /// </summary>
        public double RoutingId
        {
            get { return (double)GetRefId(RoutingIdProperty); }
            set { SetRefId(RoutingIdProperty, value); }
        }

        /// <summary>
        /// 产品工艺路线
        /// </summary>
        [Label("产品工艺路线")]
        public static readonly RefEntityProperty<WipProductRouting> RoutingProperty = P<WipProductRoutingEvent>.RegisterRef(e => e.Routing, RoutingIdProperty);

        /// <summary>
        /// 产品工艺路线
        /// </summary>
        public WipProductRouting Routing
        {
            get { return GetRefEntity(RoutingProperty); }
            set { SetRefEntity(RoutingProperty, value); }
        }
        #endregion

        #region SN RoutingVersionSn
        /// <summary>
        /// SN
        /// </summary>
        [Label("SN")]
        public static readonly Property<string> RoutingVersionSnProperty = P<WipProductRoutingEvent>.RegisterView(e => e.RoutingVersionSn, p => p.Routing.Version.Sn);

        /// <summary>
        /// SN
        /// </summary>
        public string RoutingVersionSn
        {
            get { return this.GetProperty(RoutingVersionSnProperty); }
        }
        #endregion 

        #region 变更用户 ChangeUser
        /// <summary>
        /// 变更用户Id
        /// </summary>
        [Label("变更用户")]
        public static readonly IRefIdProperty ChangeUserIdProperty = P<WipProductRoutingEvent>.RegisterRefId(e => e.ChangeUserId, ReferenceType.Normal);

        /// <summary>
        /// 变更用户Id
        /// </summary>
        public double ChangeUserId
        {
            get { return (double)GetRefId(ChangeUserIdProperty); }
            set { SetRefId(ChangeUserIdProperty, value); }
        }

        /// <summary>
        /// 变更用户
        /// </summary>
        [Label("变更用户")]
        public static readonly RefEntityProperty<Employee> ChangeUserProperty = P<WipProductRoutingEvent>.RegisterRef(e => e.ChangeUser, ChangeUserIdProperty);

        /// <summary>
        /// 变更用户
        /// </summary>
        public Employee ChangeUser
        {
            get { return GetRefEntity(ChangeUserProperty); }
            set { SetRefEntity(ChangeUserProperty, value); }
        }
        #endregion

        #region 备注 Remark
        /// <summary>
        /// 备注
        /// </summary>
        [Label("备注")]
        public static readonly Property<string> RemarkProperty = P<WipProductRoutingEvent>.Register(e => e.Remark);

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return this.GetProperty(RemarkProperty); }
            set { this.SetProperty(RemarkProperty, value); }
        }
        #endregion

        #region 视图属性
        #region 修改人 ChangeUserName
        /// <summary>
        /// 修改人
        /// </summary>
        [Label("修改人")]
        public static readonly Property<string> ChangeUserNameProperty = P<WipProductRoutingEvent>.RegisterView(e => e.ChangeUserName, p => p.ChangeUser.Name);

        /// <summary>
        /// 修改人
        /// </summary>
        public string ChangeUserName
        {
            get { return this.GetProperty(ChangeUserNameProperty); }
        }
        #endregion
        #endregion
    }

    /// <summary>
    /// 产品工艺路线变更事件 实体配置
    /// </summary>
    internal class WipProductRoutingEventConfig : EntityConfig<WipProductRoutingEvent>
    {
        /// <summary>
        /// 元数据配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("WIP_PROD_ROUT_EVENT").MapAllProperties();
            Meta.Property(WipProductRoutingEvent.NewLayoutProperty).ColumnMeta.HasLength("MAX");
            Meta.Property(WipProductRoutingEvent.OldLayoutProperty).ColumnMeta.HasLength("MAX");
            Meta.Property(WipProductRoutingEvent.RoutingIdProperty).ColumnMeta.HasIndex();
            Meta.EnablePhantoms();
        }
    }
}