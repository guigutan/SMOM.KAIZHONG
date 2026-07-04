using SIE.Domain;
using SIE.Domain.Query;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.SO.SaleOrders;
using System;

namespace SIE.Kit.APS.FactoryConfirms
{
    /// <summary>
    /// 厂别确认 用于智能分厂时的目标产能计算
    /// </summary>
    [RootEntity, Serializable]
    [CriteriaQuery]
    [Label("厂别确认")]
    public class FactoryConfirmsViewModel2 : Entity<double>
    {
        #region 行号 LineNo
        /// <summary>
        /// 行号
        /// </summary>
        public static readonly Property<string> LineNoProperty = P<FactoryConfirmsViewModel2>.Register(e => e.LineNo);
        /// <summary>
        /// 行号
        /// </summary>
        public string LineNo
        {
            get { return GetProperty(LineNoProperty); }
            set { SetProperty(LineNoProperty, value); }
        }

        #endregion

        #region 库存组织 Enterprise

        /// <summary>
        /// 库存组织Id
        /// </summary>
        public static readonly Property<double> EnterpriseIdProperty = P<FactoryConfirmsViewModel2>.Register(e => e.EnterpriseId);

        /// <summary>
        /// 库存组织Id
        /// </summary>
        public double EnterpriseId
        {
            get { return GetProperty(EnterpriseIdProperty); }
            set { SetProperty(EnterpriseIdProperty, value); }

        }

        #endregion

        #region 行状态 LineState
        /// <summary>
        /// 行状态
        /// </summary>
        [Label("行状态")]
        public static readonly Property<LineState> LineStateProperty = P<FactoryConfirmsViewModel2>.Register(e => e.LineState);

        /// <summary>
        /// 行状态
        /// </summary>
        public LineState LineState
        {
            get { return GetProperty(LineStateProperty); }
            set { SetProperty(LineStateProperty, value); }
        }
        #endregion

        #region 面积M2 Area
        /// <summary>
        /// 面积M2
        /// </summary>
        [Required]
        [Label("面积M2")]
        public static readonly Property<decimal> AreaProperty = P<FactoryConfirmsViewModel2>.Register(e => e.Area);

        /// <summary>
        /// 面积M2
        /// </summary>
        public decimal Area
        {
            get { return GetProperty(AreaProperty); }
            set { SetProperty(AreaProperty, value); }
        }
        #endregion

        #region 物料ID ItemId
        /// <summary>
        /// 物料ID
        /// </summary>
        [Required]
        [Label("物料ID")]
        public static readonly Property<double> ItemIdProperty = P<FactoryConfirmsViewModel2>.Register(e => e.ItemId);

        /// <summary>
        /// 面积M2
        /// </summary>
        public double ItemId
        {
            get { return GetProperty(ItemIdProperty); }
            set { SetProperty(ItemIdProperty, value); }
        }
        #endregion

        #region 客户交期 RequireDelivery
        /// <summary>
        /// 客户交期
        /// </summary>
        [Required]
        [Label("客户交期")]
        public static readonly Property<DateTime> RequireDeliveryProperty = P<FactoryConfirmsViewModel2>.Register(e => e.RequireDelivery);

        /// <summary>
        /// 客户交期
        /// </summary>
        public DateTime RequireDelivery
        {
            get { return GetProperty(RequireDeliveryProperty); }
            set { SetProperty(RequireDeliveryProperty, value); }
        }
        #endregion

        #region 承诺交期 PromiseDelivery
        /// <summary>
        /// 承诺交期
        /// </summary>
        [Label("承诺交期")]
        public static readonly Property<DateTime?> PromiseDeliveryProperty = P<FactoryConfirmsViewModel2>.Register(e => e.PromiseDelivery);

        /// <summary>
        /// 承诺交期
        /// </summary>
        public DateTime? PromiseDelivery
        {
            get { return GetProperty(PromiseDeliveryProperty); }
            set { SetProperty(PromiseDeliveryProperty, value); }
        }
        #endregion
    }

    /// <summary>
    ///  实体配置
    /// </summary>
    internal class SalesOrderDetailConfig2 : EntityConfig<FactoryConfirmsViewModel2>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Func<IQuery> view = () => DB.Query<SaleOrderDetail>("SaleOrderDetail")
            .Select(x =>
                        new
                        {
                            x.Id,
                            Item_Id = x.ItemId,
                            Line_No = x.LineNo,
                            Line_State = x.LineState,
                            Enterprise_Id = x.EnterpriseId,
                            Area = x.Area,
                            Require_Delivery = x.RequireDelivery,
                            Promise_Delivery = x.PromiseDelivery
                        })
                .ToQuery();
            Meta.MapView(view).MapAllProperties();
            Meta.IsTreeEntity = false;
        }
    }
}