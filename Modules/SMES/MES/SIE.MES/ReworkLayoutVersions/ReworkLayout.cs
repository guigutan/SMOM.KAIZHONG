using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.ReworkLayoutVersions
{
    /// <summary>
    /// 工艺路线明细
    /// </summary>
    [ChildEntity, Serializable]
    [Label("工艺路线明细")]
    public class ReworkLayout : DataEntity
    {
        #region 返工工艺路线版本 ReworkLayoutVersion
        /// <summary>
        /// 返工工艺路线版本Id
        /// </summary>
        [Label("返工工艺路线版本")]
        public static readonly IRefIdProperty ReworkLayoutVersionIdProperty =
            P<ReworkLayout>.RegisterRefId(e => e.ReworkLayoutVersionId, ReferenceType.Parent);

        /// <summary>
        /// 返工工艺路线版本Id
        /// </summary>
        public double ReworkLayoutVersionId
        {
            get { return (double)this.GetRefId(ReworkLayoutVersionIdProperty); }
            set { this.SetRefId(ReworkLayoutVersionIdProperty, value); }
        }

        /// <summary>
        /// 返工工艺路线版本
        /// </summary>
        public static readonly RefEntityProperty<ReworkLayoutVersion> ReworkLayoutVersionProperty =
            P<ReworkLayout>.RegisterRef(e => e.ReworkLayoutVersion, ReworkLayoutVersionIdProperty);

        /// <summary>
        /// 返工工艺路线版本
        /// </summary>
        public ReworkLayoutVersion ReworkLayoutVersion
        {
            get { return this.GetRefEntity(ReworkLayoutVersionProperty); }
            set { this.SetRefEntity(ReworkLayoutVersionProperty, value); }
        }
        #endregion

        #region 操作活动编号 Vornr
        /// <summary>
        /// 操作活动编号
        /// </summary>
        [Label("操作活动编号")]
        public static readonly Property<string> VornrProperty = P<ReworkLayout>.Register(e => e.Vornr);

        /// <summary>
        /// 操作活动编号
        /// </summary>
        public string Vornr
        {
            get { return this.GetProperty(VornrProperty); }
            set { this.SetProperty(VornrProperty, value); }
        }
        #endregion

        #region 标准文本码 ProcessCode
        /// <summary>
        /// 标准文本码
        /// </summary>
        [Label("标准文本码")]
        public static readonly Property<string> ProcessCodeProperty = P<ReworkLayout>.Register(e => e.ProcessCode);

        /// <summary>
        /// 标准文本码
        /// </summary>
        public string ProcessCode
        {
            get { return this.GetProperty(ProcessCodeProperty); }
            set { this.SetProperty(ProcessCodeProperty, value); }
        }
        #endregion

        #region 工作中心编码 WorkCenterCode
        /// <summary>
        /// 工作中心编码
        /// </summary>
        [Label("工作中心编码")]
        public static readonly Property<string> WorkCenterCodeProperty = P<ReworkLayout>.Register(e => e.WorkCenterCode);

        /// <summary>
        /// 工作中心编码
        /// </summary>
        public string WorkCenterCode
        {
            get { return this.GetProperty(WorkCenterCodeProperty); }
            set { this.SetProperty(WorkCenterCodeProperty, value); }
        }
        #endregion

        #region 控制码(工序控制码) Steus
        /// <summary>
        /// 控制码(工序控制码)
        /// </summary>
        [Label("控制码(工序控制码)")]
        public static readonly Property<string> SteusProperty = P<ReworkLayout>.Register(e => e.Steus);

        /// <summary>
        /// 控制码(工序控制码)
        /// </summary>
        public string Steus
        {
            get { return this.GetProperty(SteusProperty); }
            set { this.SetProperty(SteusProperty, value); }
        }
        #endregion

        #region 工厂 Factory
        /// <summary>
        /// 工厂
        /// </summary>
        [Label("工厂")]
        public static readonly Property<string> FactoryProperty = P<ReworkLayout>.Register(e => e.Factory);

        /// <summary>
        /// 工厂
        /// </summary>
        public string Factory
        {
            get { return this.GetProperty(FactoryProperty); }
            set { this.SetProperty(FactoryProperty, value); }
        }
        #endregion

        #region 工序数量 ProcessQty
        /// <summary>
        /// 工序数量
        /// </summary>
        [Label("工序数量")]
        public static readonly Property<decimal> ProcessQtyProperty = P<ReworkLayout>.Register(e => e.ProcessQty);

        /// <summary>
        /// 工序数量
        /// </summary>
        public decimal ProcessQty
        {
            get { return this.GetProperty(ProcessQtyProperty); }
            set { this.SetProperty(ProcessQtyProperty, value); }
        }
        #endregion

        #region 分单数量 Zcode
        /// <summary>
        /// 分单数量
        /// </summary>
        [Label("分单数量")]
        public static readonly Property<decimal> ZcodeProperty = P<ReworkLayout>.Register(e => e.Zcode);

        /// <summary>
        /// 分单数量
        /// </summary>
        public decimal Zcode
        {
            get { return this.GetProperty(ZcodeProperty); }
            set { this.SetProperty(ZcodeProperty, value); }
        }
        #endregion
    }

    internal class ReworkLayoutConfig : EntityConfig<ReworkLayout>
    {
        protected override void ConfigMeta()
        {
            Meta.MapTable("REWORK_LAYOUT").MapAllProperties();
            Meta.EnableInvOrg();
            Meta.EnablePhantoms();
        }
    }
}
