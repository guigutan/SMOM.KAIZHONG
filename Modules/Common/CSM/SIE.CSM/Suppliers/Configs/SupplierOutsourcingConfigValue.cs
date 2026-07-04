using SIE.Common.Configs;
using SIE.Common.NumberRules;
using SIE.Core.Enums;
using SIE.Domain;
using SIE.ObjectModel;
using SIE.Warehouses;
using System;

namespace SIE.CSM.Suppliers.Configs
{
    /// <summary>
    /// 委外参数配置
    /// </summary>
    [RootEntity, Serializable]
    [Label("委外参数配置")]
    public class SupplierOutsourcingConfigValue : ConfigValue
    {
        
        /// <summary>
        /// 显示接口配置
        /// </summary>
        /// <returns>返回接口配置</returns>
        public override string Display()
        {
            var inLocCode = string.Empty;
            var OutLocCode = string.Empty;
            var OutsourcingReceiveStr = string.Empty;
            var OutsourcingUseTimeStr = string.Empty;
            var IsHasStorerStr = string.Empty;
            if (OutsourcingInLoc != null)
            {
                inLocCode = OutsourcingInLoc.Code;
            }
            if (OutsourcingOutLoc != null)
            {
                OutLocCode = OutsourcingInLoc.Code;
            }
            OutsourcingReceiveStr = OutsourcingReceive.ToLabel().L10N();
            OutsourcingUseTimeStr = OutsourcingUseTime.ToLabel().L10N();
            IsHasStorerStr = IsHasStorer.ToString().L10N();
            return "委外发料调入库位:{0};委外扣料库位:{1};委外收货扣料处理:{2};委外扣料时点:{3};委外库存带货主管理:{4};".L10nFormat(inLocCode,OutLocCode, OutsourcingReceiveStr, OutsourcingUseTimeStr, IsHasStorerStr);
        }
       

        #region 委外发料调入库位 OutsourcingInLoc
        /// <summary>
        /// 委外发料调入库位ID
        /// </summary>
        [Label("委外发料调入库位")]
        public static readonly IRefIdProperty OutsourcingInLocIdProperty =
              P<SupplierOutsourcingConfigValue>.RegisterRefId(e => e.OutsourcingInLocId, ReferenceType.Normal);

        /// <summary>
        /// 委外发料调入库位ID
        /// </summary>
        public double? OutsourcingInLocId
        {
            get { return (double?)this.GetRefNullableId(OutsourcingInLocIdProperty); }
            set { this.SetRefNullableId(OutsourcingInLocIdProperty, value); }
        }

        /// <summary>
        /// 委外发料调入库位
        /// </summary>
        public static readonly RefEntityProperty<StorageLocation> OutsourcingInLocProperty =
            P<SupplierOutsourcingConfigValue>.RegisterRef(e => e.OutsourcingInLoc, OutsourcingInLocIdProperty);

        /// <summary>
        /// 委外发料调入库位
        /// </summary>
        public StorageLocation OutsourcingInLoc
        {
            get { return this.GetRefEntity(OutsourcingInLocProperty); }
            set { this.SetRefEntity(OutsourcingInLocProperty, value); }
        }
        #endregion

        #region 委外扣料库位 OutsourcingInLoc
        /// <summary>
        /// 委外扣料库位ID
        /// </summary>
        [Label("委外扣料库位")]
        public static readonly IRefIdProperty OutsourcingOutLocIdProperty =
              P<SupplierOutsourcingConfigValue>.RegisterRefId(e => e.OutsourcingOutLocId, ReferenceType.Normal);

        /// <summary>
        /// 委外扣料库位ID
        /// </summary>
        public double? OutsourcingOutLocId
        {
            get { return (double?)this.GetRefNullableId(OutsourcingOutLocIdProperty); }
            set { this.SetRefNullableId(OutsourcingOutLocIdProperty, value); }
        }

        /// <summary>
        /// 委外扣料库位
        /// </summary>
        public static readonly RefEntityProperty<StorageLocation> OutsourcingOutLocProperty =
            P<SupplierOutsourcingConfigValue>.RegisterRef(e => e.OutsourcingOutLoc, OutsourcingOutLocIdProperty);

        /// <summary>
        /// 委外扣料库位
        /// </summary>
        public StorageLocation OutsourcingOutLoc
        {
            get { return this.GetRefEntity(OutsourcingOutLocProperty); }
            set { this.SetRefEntity(OutsourcingOutLocProperty, value); }
        }
        #endregion

        #region 委外收货扣料处理 OutsourcingReceive
        /// <summary>
        /// 委外收货扣料处理
        /// </summary>
        [Label("委外收货扣料处理")]
        public static readonly Property<OutsourcingReceiveType> OutsourcingReceiveProperty = P<SupplierOutsourcingConfigValue>.Register(e => e.OutsourcingReceive);

        /// <summary>
        /// 委外收货扣料处理
        /// </summary>
        public OutsourcingReceiveType OutsourcingReceive
        {
            get { return GetProperty(OutsourcingReceiveProperty); }
            set { SetProperty(OutsourcingReceiveProperty, value); }
        }
        #endregion

        #region 委外扣料时点 OutsourcingUseTime
        /// <summary>
        /// 委外扣料时点
        /// </summary>
        [Label("委外扣料时点")]
        public static readonly Property<OutsourcingTimeType> OutsourcingUseTimeProperty = P<SupplierOutsourcingConfigValue>.Register(e => e.OutsourcingUseTime);

        /// <summary>
        /// 委外扣料时点
        /// </summary>
        public OutsourcingTimeType OutsourcingUseTime
        {
            get { return GetProperty(OutsourcingUseTimeProperty); }
            set { SetProperty(OutsourcingUseTimeProperty, value); }
        }
        #endregion

        #region 委外库存带货主管理 IsHasStorer
        /// <summary>
        /// 委外库存带货主管理
        /// </summary>
        [Label("委外库存带货主管理")]
        public static readonly Property<bool> IsHasStorerProperty = P<SupplierOutsourcingConfigValue>.Register(e => e.IsHasStorer);

        /// <summary>
        /// 委外库存带货主管理
        /// </summary>
        public bool IsHasStorer
        {
            get { return this.GetProperty(IsHasStorerProperty); }
            set { this.SetProperty(IsHasStorerProperty, value); }
        }
        #endregion

    }
}
