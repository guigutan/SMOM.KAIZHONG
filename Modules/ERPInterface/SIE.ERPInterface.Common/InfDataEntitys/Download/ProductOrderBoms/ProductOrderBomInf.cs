using System;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;

namespace SIE.ERPInterface.Common.InfDataEntitys.Download
{
    /// <summary>
    /// 生产订单BOM中间表
    /// </summary>
    [RootEntity, Serializable]
    [CriteriaQuery]
    [Label("生产订单BOM中间表")]
    public class ProductOrderBomInf : DownloadBaseEntity
    {
        #region 生产订单编号 Code
        /// <summary>
        /// 生产订单编号
        /// </summary>
        [Label("生产订单编号")]
        public static readonly Property<string> CodeProperty = P<ProductOrderBomInf>.Register(e => e.Code);

        /// <summary>
        /// 生产订单编号
        /// </summary>
        public string Code
        {
            get { return GetProperty(CodeProperty); }
            set { SetProperty(CodeProperty, value); }
        }
        #endregion

        #region 物料编号 ItemCode
        /// <summary>
        /// 物料编号
        /// </summary>
        [Label("物料编号")]
        public static readonly Property<string> ItemCodeProperty = P<ProductOrderBomInf>.Register(e => e.ItemCode);

        /// <summary>
        /// 物料编号
        /// </summary>
        public string ItemCode
        {
            get { return GetProperty(ItemCodeProperty); }
            set { SetProperty(ItemCodeProperty, value); }
        }
        #endregion

        #region 规格描述 SpecificationDesc
        /// <summary>
        /// 规格描述
        /// </summary>
        [Label("规格描述")]
        public static readonly Property<string> SpecificationDescProperty = P<ProductOrderBomInf>.Register(e => e.SpecificationDesc);

        /// <summary>
        /// 规格描述
        /// </summary>
        public string SpecificationDesc
        {
            get { return GetProperty(SpecificationDescProperty); }
            set { SetProperty(SpecificationDescProperty, value); }
        }
        #endregion

        //#region 替代料值 ReplateItemType
        ///// <summary>
        ///// 替代料值 0--主料，1--替代料
        ///// </summary>
        //[Label("替代料值")]
        //public static readonly Property<ReplateItemType> ReplateItemTypeProperty = P<ProductOrderBomInf>.Register(e => e.ReplateItemType);

        ///// <summary>
        ///// 替代料值
        ///// </summary>
        //public ReplateItemType ReplateItemType
        //{
        //    get { return GetProperty(ReplateItemTypeProperty); }
        //    set { SetProperty(ReplateItemTypeProperty, value); }
        //}
        //#endregion

        #region 主物料编码 MainMaterialCode
        /// <summary>
        /// 主物料编码
        /// </summary>
        [Label("主物料编码")]
        public static readonly Property<string> MainMaterialCodeProperty = P<ProductOrderBomInf>.Register(e => e.MainMaterialCode);

        /// <summary>
        /// 主物料编码
        /// </summary>
        public string MainMaterialCode
        {
            get { return GetProperty(MainMaterialCodeProperty); }
            set { SetProperty(MainMaterialCodeProperty, value); }
        }
        #endregion

        #region 元件位号 ElementNo
        /// <summary>
        /// 元件位号
        /// </summary>
        [Label("元件位号")]
        public static readonly Property<string> ElementNoProperty = P<ProductOrderBomInf>.Register(e => e.ElementNo);

        /// <summary>
        /// 元件位号
        /// </summary>
        public string ElementNo
        {
            get { return GetProperty(ElementNoProperty); }
            set { SetProperty(ElementNoProperty, value); }
        }
        #endregion

        #region 需求量 RequireQty
        /// <summary>
        /// 需求量
        /// </summary>
        [Label("需求量")]
        public static readonly Property<decimal> RequireQtyProperty = P<ProductOrderBomInf>.Register(e => e.RequireQty);

        /// <summary>
        /// 需求量
        /// </summary>
        public decimal RequireQty
        {
            get { return GetProperty(RequireQtyProperty); }
            set { SetProperty(RequireQtyProperty, value); }
        }
        #endregion

        #region 制程 ProcessTech
        /// <summary>
        /// 制程
        /// </summary>
        [Label("制程")]
        public static readonly Property<string> ProcessTechProperty = P<ProductOrderBomInf>.Register(e => e.ProcessTech);

        /// <summary>
        /// 制程
        /// </summary>
        public string ProcessTech
        {
            get { return GetProperty(ProcessTechProperty); }
            set { SetProperty(ProcessTechProperty, value); }
        }
        #endregion

        #region 备注 Remark
        /// <summary>
        /// 备注
        /// </summary>
        [Label("备注")]
        public static readonly Property<string> RemarkProperty = P<ProductOrderBomInf>.Register(e => e.Remark);

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return GetProperty(RemarkProperty); }
            set { SetProperty(RemarkProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 生产订单BOM中间表 实体配置
    /// </summary>
    internal class ProductOrderBomInfConfig : EntityConfig<ProductOrderBomInf>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("INF_PRODUCT_ORDER_BOM").MapAllProperties();
            Meta.Property(ProductOrderBomInf.RemarkProperty).ColumnMeta.HasLength(2000);
            Meta.EnablePhantoms();
        }
    }
}