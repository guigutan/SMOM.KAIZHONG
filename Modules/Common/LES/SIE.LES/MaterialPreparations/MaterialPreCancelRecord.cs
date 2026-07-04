using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.LES.MaterialPreparations
{
    /// <summary>
    /// 备料需求单取消记录
    /// </summary>
    [ChildEntity, Serializable]
    [Label("备料需求单取消记录")]
    public class MaterialPreCancelRecord : DataEntity
    {
        #region 备料需求单 MaterialPreparation
        /// <summary>
        /// 备料需求单Id
        /// </summary>
        [Label("备料需求单")]
        public static readonly IRefIdProperty MaterialPreparationIdProperty =
            P<MaterialPreCancelRecord>.RegisterRefId(e => e.MaterialPreparationId, ReferenceType.Parent);

        /// <summary>
        /// 备料需求单Id
        /// </summary>
        public double MaterialPreparationId
        {
            get { return (double)this.GetRefId(MaterialPreparationIdProperty); }
            set { this.SetRefId(MaterialPreparationIdProperty, value); }
        }

        /// <summary>
        /// 备料需求单
        /// </summary>
        public static readonly RefEntityProperty<MaterialPreparation> MaterialPreparationProperty =
            P<MaterialPreCancelRecord>.RegisterRef(e => e.MaterialPreparation, MaterialPreparationIdProperty);

        /// <summary>
        /// 备料需求单
        /// </summary>
        public MaterialPreparation MaterialPreparation
        {
            get { return this.GetRefEntity(MaterialPreparationProperty); }
            set { this.SetRefEntity(MaterialPreparationProperty, value); }
        }
        #endregion

        #region 物料编码 ItemCode
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("物料编码")]
        public static readonly Property<string> ItemCodeProperty = P<MaterialPreCancelRecord>.Register(e => e.ItemCode);

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode
        {
            get { return this.GetProperty(ItemCodeProperty); }
            set { this.SetProperty(ItemCodeProperty, value); }
        }
        #endregion

        #region 物料扩展属性 ItemExtPropName
        /// <summary>
        /// 物料扩展属性
        /// </summary>
        [Label("物料扩展属性")]
        public static readonly Property<string> ItemExtPropNameProperty = P<MaterialPreCancelRecord>.Register(e => e.ItemExtPropName);

        /// <summary>
        /// 物料扩展属性
        /// </summary>
        public string ItemExtPropName
        {
            get { return this.GetProperty(ItemExtPropNameProperty); }
            set { this.SetProperty(ItemExtPropNameProperty, value); }
        }
        #endregion

        #region 取消数 CancelQty
        /// <summary>
        /// 取消数
        /// </summary>
        [Label("取消数")]
        public static readonly Property<decimal> CancelQtyProperty = P<MaterialPreCancelRecord>.Register(e => e.CancelQty);

        /// <summary>
        /// 取消数
        /// </summary>
        public decimal CancelQty
        {
            get { return this.GetProperty(CancelQtyProperty); }
            set { this.SetProperty(CancelQtyProperty, value); }
        }
        #endregion

        #region 备料需求单号 MpNo
        /// <summary>
        /// 备料需求单号
        /// </summary>
        [Label("备料需求单号")]
        public static readonly Property<string> MpNoProperty = P<MaterialPreCancelRecord>.RegisterView(e => e.MpNo, p => p.MaterialPreparation.No);

        /// <summary>
        /// 备料需求单号
        /// </summary>
        public string MpNo
        {
            get { return this.GetProperty(MpNoProperty); }
        }
        #endregion

    }

    /// <summary>
    /// 实体配置
    /// </summary>
    public class MaterialPreCancelRecordConfig : EntityConfig<MaterialPreCancelRecord>
    {
        /// <summary>
        /// 数据库配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("LES_MATERIAL_CANCEL").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
