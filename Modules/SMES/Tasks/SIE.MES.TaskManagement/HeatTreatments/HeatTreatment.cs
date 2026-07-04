using SIE.Barcodes.WipBatchs;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.MES.TaskManagement.HeatTreatments
{
    /// <summary>
    /// 老化炉标签进出炉记录
    /// </summary>
    [RootEntity, Serializable]
    [CriteriaQuery]
    //[ConditionQueryType(typeof(HeatTreatmentCriteria))]
    [Label("老化炉标签进出炉记录")]
    public class HeatTreatment : DataEntity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public HeatTreatment()
        {
            IsReported = false;
        }

        #region 工序标签 WipBatch
        /// <summary>
        /// 工序标签Id
        /// </summary>
        [Label("工序标签")]
        public static readonly IRefIdProperty WipBatchIdProperty =
            P<HeatTreatment>.RegisterRefId(e => e.WipBatchId, ReferenceType.Normal);

        /// <summary>
        /// 工序标签Id
        /// </summary>
        public double? WipBatchId
        {
            get { return (double?)this.GetRefNullableId(WipBatchIdProperty); }
            set { this.SetRefNullableId(WipBatchIdProperty, value); }
        }

        /// <summary>
        /// 工序标签
        /// </summary>
        public static readonly RefEntityProperty<WipBatch> WipBatchProperty =
            P<HeatTreatment>.RegisterRef(e => e.WipBatch, WipBatchIdProperty);

        /// <summary>
        /// 工序标签
        /// </summary>
        public WipBatch WipBatch
        {
            get { return this.GetRefEntity(WipBatchProperty); }
            set { this.SetRefEntity(WipBatchProperty, value); }
        }
        #endregion

        #region 条码号 Barcode
        /// <summary>
        /// 条码号
        /// </summary>
        [Label("条码号")]
        public static readonly Property<string> BarcodeProperty = P<HeatTreatment>.Register(e => e.Barcode);

        /// <summary>
        /// 条码号
        /// </summary>
        public string Barcode
        {
            get { return this.GetProperty(BarcodeProperty); }
            set { this.SetProperty(BarcodeProperty, value); }
        }
        #endregion

        #region 旧料号 Model
        /// <summary>
        /// 旧料号
        /// </summary>
        [Label("旧料号")]
        public static readonly Property<string> ModelProperty = P<HeatTreatment>.Register(e => e.Model);

        /// <summary>
        /// 旧料号
        /// </summary>
        public string Model
        {
            get { return this.GetProperty(ModelProperty); }
            set { this.SetProperty(ModelProperty, value); }
        }
        #endregion

        #region 物料编码 MaterialCode
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("物料编码")]
        public static readonly Property<string> MaterialCodeProperty = P<HeatTreatment>.Register(e => e.MaterialCode);

        /// <summary>
        /// 物料编码
        /// </summary>
        public string MaterialCode
        {
            get { return this.GetProperty(MaterialCodeProperty); }
            set { this.SetProperty(MaterialCodeProperty, value); }
        }
        #endregion

        #region 作业类型 OperationType
        /// <summary>
        /// 作业类型(1=入炉，2=出炉)
        /// </summary>
        [Label("作业类型")]
        public static readonly Property<OperationType?> OperationTypeProperty = P<HeatTreatment>.Register(e => e.OperationType);

        /// <summary>
        /// 作业类型(1=入炉，2=出炉)
        /// </summary>
        public OperationType? OperationType
        {
            get { return this.GetProperty(OperationTypeProperty); }
            set { this.SetProperty(OperationTypeProperty, value); }
        }
        #endregion

        #region 作业时间 EnableTime1
        /// <summary>
        /// 作业时间(入炉时间)
        /// </summary>
        [Label("作业时间")]
        public static readonly Property<DateTime?> EnableTime1Property = P<HeatTreatment>.Register(e => e.EnableTime1);

        /// <summary>
        /// 作业时间(入炉时间)
        /// </summary>
        public DateTime? EnableTime1
        {
            get { return this.GetProperty(EnableTime1Property); }
            set { this.SetProperty(EnableTime1Property, value); }
        }
        #endregion

        #region 设备ID DevId
        /// <summary>
        /// 设备ID
        /// </summary>
        [Label("设备ID")]
        public static readonly Property<string> DevIdProperty = P<HeatTreatment>.Register(e => e.DevId);

        /// <summary>
        /// 设备ID
        /// </summary>
        public string DevId
        {
            get { return this.GetProperty(DevIdProperty); }
            set { this.SetProperty(DevIdProperty, value); }
        }
        #endregion

        #region 数量 Count00
        /// <summary>
        /// 数量
        /// </summary>
        [Label("数量")]
        public static readonly Property<decimal?> Count00Property = P<HeatTreatment>.Register(e => e.Count00);

        /// <summary>
        /// 数量
        /// </summary>
        public decimal? Count00
        {
            get { return this.GetProperty(Count00Property); }
            set { this.SetProperty(Count00Property, value); }
        }
        #endregion

        #region 存储时间 SvTime
        /// <summary>
        /// 存储时间
        /// </summary>
        [Label("存储时间")]
        public static readonly Property<DateTime?> SvTimeProperty = P<HeatTreatment>.Register(e => e.SvTime);

        /// <summary>
        /// 存储时间
        /// </summary>
        public DateTime? SvTime
        {
            get { return this.GetProperty(SvTimeProperty); }
            set { this.SetProperty(SvTimeProperty, value); }
        }
        #endregion

        #region 时间整形字 SvTimeMs
        /// <summary>
        /// 时间整形字
        /// </summary>
        [Label("时间整形字")]
        public static readonly Property<int?> SvTimeMsProperty = P<HeatTreatment>.Register(e => e.SvTimeMs);

        /// <summary>
        /// 时间整形字
        /// </summary>
        public int? SvTimeMs
        {
            get { return this.GetProperty(SvTimeMsProperty); }
            set { this.SetProperty(SvTimeMsProperty, value); }
        }
        #endregion

        #region 设备实际名称 DevName
        /// <summary>
        /// 设备实际名称
        /// </summary>
        [Label("设备实际名称")]
        public static readonly Property<string> DevNameProperty = P<HeatTreatment>.Register(e => e.DevName);

        /// <summary>
        /// 设备实际名称
        /// </summary>
        public string DevName
        {
            get { return this.GetProperty(DevNameProperty); }
            set { this.SetProperty(DevNameProperty, value); }
        }
        #endregion

        #region 设备异常代码 ErrNum
        /// <summary>
        /// 设备异常代码
        /// </summary>
        [Label("设备异常代码")]
        public static readonly Property<int?> ErrNumProperty = P<HeatTreatment>.Register(e => e.ErrNum);

        /// <summary>
        /// 设备异常代码
        /// </summary>
        public int? ErrNum
        {
            get { return this.GetProperty(ErrNumProperty); }
            set { this.SetProperty(ErrNumProperty, value); }
        }
        #endregion

        #region 运行段 RunPro
        /// <summary>
        /// 运行段
        /// </summary>
        [Label("运行段")]
        public static readonly Property<int?> RunProProperty = P<HeatTreatment>.Register(e => e.RunPro);

        /// <summary>
        /// 运行段
        /// </summary>
        public int? RunPro
        {
            get { return this.GetProperty(RunProProperty); }
            set { this.SetProperty(RunProProperty, value); }
        }
        #endregion

        #region 运行状态 State
        /// <summary>
        /// 运行状态
        /// </summary>
        [Label("运行状态")]
        public static readonly Property<int?> StateProperty = P<HeatTreatment>.Register(e => e.State);

        /// <summary>
        /// 运行状态
        /// </summary>
        public int? State
        {
            get { return this.GetProperty(StateProperty); }
            set { this.SetProperty(StateProperty, value); }
        }
        #endregion

        #region 工艺代码 Rec
        /// <summary>
        /// 工艺代码
        /// </summary>
        [Label("工艺代码")]
        public static readonly Property<string> RecProperty = P<HeatTreatment>.Register(e => e.Rec);

        /// <summary>
        /// 工艺代码
        /// </summary>
        public string Rec
        {
            get { return this.GetProperty(RecProperty); }
            set { this.SetProperty(RecProperty, value); }
        }
        #endregion

        #region 保存号 SvId
        /// <summary>
        /// 保存号
        /// </summary>
        [Label("保存号")]
        public static readonly Property<string> SvIdProperty = P<HeatTreatment>.Register(e => e.SvId);

        /// <summary>
        /// 保存号
        /// </summary>
        public string SvId
        {
            get { return this.GetProperty(SvIdProperty); }
            set { this.SetProperty(SvIdProperty, value); }
        }
        #endregion

        #region 温度允许上限值 TmpH
        /// <summary>
        /// 温度允许上限值
        /// </summary>
        [Label("温度允许上限值")]
        public static readonly Property<decimal?> TmpHProperty = P<HeatTreatment>.Register(e => e.TmpH);

        /// <summary>
        /// 温度允许上限值
        /// </summary>
        public decimal? TmpH
        {
            get { return this.GetProperty(TmpHProperty); }
            set { this.SetProperty(TmpHProperty, value); }
        }
        #endregion

        #region 温度允许下限值 TmpL
        /// <summary>
        /// 温度允许下限值
        /// </summary>
        [Label("温度允许下限值")]
        public static readonly Property<decimal?> TmpLProperty = P<HeatTreatment>.Register(e => e.TmpL);

        /// <summary>
        /// 温度允许下限值
        /// </summary>
        public decimal? TmpL
        {
            get { return this.GetProperty(TmpLProperty); }
            set { this.SetProperty(TmpLProperty, value); }
        }
        #endregion

        #region 目标温度值 Tmp
        /// <summary>
        /// 目标温度值
        /// </summary>
        [Label("目标温度值")]
        public static readonly Property<decimal?> TmpProperty = P<HeatTreatment>.Register(e => e.Tmp);

        /// <summary>
        /// 目标温度值
        /// </summary>
        public decimal? Tmp
        {
            get { return this.GetProperty(TmpProperty); }
            set { this.SetProperty(TmpProperty, value); }
        }
        #endregion

        #region CH1温度值 Tmp1
        /// <summary>
        /// CH1温度值
        /// </summary>
        [Label("CH1温度值")]
        public static readonly Property<decimal?> Tmp1Property = P<HeatTreatment>.Register(e => e.Tmp1);

        /// <summary>
        /// CH1温度值
        /// </summary>
        public decimal? Tmp1
        {
            get { return this.GetProperty(Tmp1Property); }
            set { this.SetProperty(Tmp1Property, value); }
        }
        #endregion

        #region CH2温度值 Tmp2
        /// <summary>
        /// CH2温度值
        /// </summary>
        [Label("CH2温度值")]
        public static readonly Property<decimal?> Tmp2Property = P<HeatTreatment>.Register(e => e.Tmp2);

        /// <summary>
        /// CH2温度值
        /// </summary>
        public decimal? Tmp2
        {
            get { return this.GetProperty(Tmp2Property); }
            set { this.SetProperty(Tmp2Property, value); }
        }
        #endregion

        #region CH3温度值 Tmp3
        /// <summary>
        /// CH3温度值
        /// </summary>
        [Label("CH3温度值")]
        public static readonly Property<decimal?> Tmp3Property = P<HeatTreatment>.Register(e => e.Tmp3);

        /// <summary>
        /// CH3温度值
        /// </summary>
        public decimal? Tmp3
        {
            get { return this.GetProperty(Tmp3Property); }
            set { this.SetProperty(Tmp3Property, value); }
        }
        #endregion

        #region CH4温度值 Tmp4
        /// <summary>
        /// CH4温度值
        /// </summary>
        [Label("CH4温度值")]
        public static readonly Property<decimal?> Tmp4Property = P<HeatTreatment>.Register(e => e.Tmp4);

        /// <summary>
        /// CH4温度值
        /// </summary>
        public decimal? Tmp4
        {
            get { return this.GetProperty(Tmp4Property); }
            set { this.SetProperty(Tmp4Property, value); }
        }
        #endregion

        #region 运行时间 RunTime
        /// <summary>
        /// 运行时间
        /// </summary>
        [Label("运行时间")]
        public static readonly Property<int?> RunTimeProperty = P<HeatTreatment>.Register(e => e.RunTime);

        /// <summary>
        /// 运行时间
        /// </summary>
        public int? RunTime
        {
            get { return this.GetProperty(RunTimeProperty); }
            set { this.SetProperty(RunTimeProperty, value); }
        }
        #endregion

        #region 运行号 RunId
        /// <summary>
        /// 运行号
        /// </summary>
        [Label("运行号")]
        public static readonly Property<string> RunIdProperty = P<HeatTreatment>.Register(e => e.RunId);

        /// <summary>
        /// 运行号
        /// </summary>
        public string RunId
        {
            get { return this.GetProperty(RunIdProperty); }
            set { this.SetProperty(RunIdProperty, value); }
        }
        #endregion

        #region 层号 Layer00
        /// <summary>
        /// 层号
        /// </summary>
        [Label("层号")]
        public static readonly Property<int?> Layer00Property = P<HeatTreatment>.Register(e => e.Layer00);

        /// <summary>
        /// 层号
        /// </summary>
        public int? Layer00
        {
            get { return this.GetProperty(Layer00Property); }
            set { this.SetProperty(Layer00Property, value); }
        }
        #endregion

        #region 卡号 Card00
        /// <summary>
        /// 卡号
        /// </summary>
        [Label("卡号")]
        public static readonly Property<string> Card00Property = P<HeatTreatment>.Register(e => e.Card00);

        /// <summary>
        /// 卡号
        /// </summary>
        public string Card00
        {
            get { return this.GetProperty(Card00Property); }
            set { this.SetProperty(Card00Property, value); }
        }
        #endregion

        #region 规格 Type00
        /// <summary>
        /// 规格
        /// </summary>
        [Label("规格")]
        public static readonly Property<string> Type00Property = P<HeatTreatment>.Register(e => e.Type00);

        /// <summary>
        /// 规格
        /// </summary>
        public string Type00
        {
            get { return this.GetProperty(Type00Property); }
            set { this.SetProperty(Type00Property, value); }
        }
        #endregion

        #region 允许温度上偏差值 TmpH1
        /// <summary>
        /// 允许温度上偏差值
        /// </summary>
        [Label("允许温度上偏差值")]
        public static readonly Property<decimal?> TmpH1Property = P<HeatTreatment>.Register(e => e.TmpH1);

        /// <summary>
        /// 允许温度上偏差值
        /// </summary>
        public decimal? TmpH1
        {
            get { return this.GetProperty(TmpH1Property); }
            set { this.SetProperty(TmpH1Property, value); }
        }
        #endregion

        #region 允许温度上极限值 TmpH2
        /// <summary>
        /// 允许温度上极限值
        /// </summary>
        [Label("允许温度上极限值")]
        public static readonly Property<decimal?> TmpH2Property = P<HeatTreatment>.Register(e => e.TmpH2);

        /// <summary>
        /// 允许温度上极限值
        /// </summary>
        public decimal? TmpH2
        {
            get { return this.GetProperty(TmpH2Property); }
            set { this.SetProperty(TmpH2Property, value); }
        }
        #endregion

        #region CH1校正值 Ch1
        /// <summary>
        /// CH1校正值
        /// </summary>
        [Label("CH1校正值")]
        public static readonly Property<decimal?> Ch1Property = P<HeatTreatment>.Register(e => e.Ch1);

        /// <summary>
        /// CH1校正值
        /// </summary>
        public decimal? Ch1
        {
            get { return this.GetProperty(Ch1Property); }
            set { this.SetProperty(Ch1Property, value); }
        }
        #endregion

        #region CH2校正值 Ch2
        /// <summary>
        /// CH2校正值
        /// </summary>
        [Label("CH2校正值")]
        public static readonly Property<decimal?> Ch2Property = P<HeatTreatment>.Register(e => e.Ch2);

        /// <summary>
        /// CH2校正值
        /// </summary>
        public decimal? Ch2
        {
            get { return this.GetProperty(Ch2Property); }
            set { this.SetProperty(Ch2Property, value); }
        }
        #endregion

        #region CH3校正值 Ch3
        /// <summary>
        /// CH3校正值
        /// </summary>
        [Label("CH3校正值")]
        public static readonly Property<decimal?> Ch3Property = P<HeatTreatment>.Register(e => e.Ch3);

        /// <summary>
        /// CH3校正值
        /// </summary>
        public decimal? Ch3
        {
            get { return this.GetProperty(Ch3Property); }
            set { this.SetProperty(Ch3Property, value); }
        }
        #endregion

        #region CH4校正值 Ch4
        /// <summary>
        /// CH4校正值
        /// </summary>
        [Label("CH4校正值")]
        public static readonly Property<decimal?> Ch4Property = P<HeatTreatment>.Register(e => e.Ch4);

        /// <summary>
        /// CH4校正值
        /// </summary>
        public decimal? Ch4
        {
            get { return this.GetProperty(Ch4Property); }
            set { this.SetProperty(Ch4Property, value); }
        }
        #endregion

        #region 标记字段 Flag
        /// <summary>
        /// 标记字段
        /// </summary>
        [Label("标记字段")]
        public static readonly Property<int> FlagProperty = P<HeatTreatment>.Register(e => e.Flag);

        /// <summary>
        /// 标记字段
        /// </summary>
        public int Flag
        {
            get { return this.GetProperty(FlagProperty); }
            set { this.SetProperty(FlagProperty, value); }
        }
        #endregion

        #region 生效时间 EnableTime
        /// <summary>
        /// 生效时间
        /// </summary>
        [Label("生效时间")]
        public static readonly Property<DateTime?> EnableTimeProperty = P<HeatTreatment>.Register(e => e.EnableTime);

        /// <summary>
        /// 生效时间
        /// </summary>
        public DateTime? EnableTime
        {
            get { return this.GetProperty(EnableTimeProperty); }
            set { this.SetProperty(EnableTimeProperty, value); }
        }
        #endregion

        #region 计划号 PlanNum
        /// <summary>
        /// 计划号
        /// </summary>
        [Label("计划号")]
        public static readonly Property<string> PlanNumProperty = P<HeatTreatment>.Register(e => e.PlanNum);

        /// <summary>
        /// 计划号
        /// </summary>
        public string PlanNum
        {
            get { return this.GetProperty(PlanNumProperty); }
            set { this.SetProperty(PlanNumProperty, value); }
        }
        #endregion

        #region 生产号 ProductNum
        /// <summary>
        /// 生产号
        /// </summary>
        [Label("生产号")]
        public static readonly Property<string> ProductNumProperty = P<HeatTreatment>.Register(e => e.ProductNum);

        /// <summary>
        /// 生产号
        /// </summary>
        public string ProductNum
        {
            get { return this.GetProperty(ProductNumProperty); }
            set { this.SetProperty(ProductNumProperty, value); }
        }
        #endregion

        #region 产部名称 WorkId
        /// <summary>
        /// 产部名称
        /// </summary>
        [Label("产部名称")]
        public static readonly Property<string> WorkIdProperty = P<HeatTreatment>.Register(e => e.WorkId);

        /// <summary>
        /// 产部名称
        /// </summary>
        public string WorkId
        {
            get { return this.GetProperty(WorkIdProperty); }
            set { this.SetProperty(WorkIdProperty, value); }
        }
        #endregion

        #region 工厂 Factory
        /// <summary>
        /// 工厂
        /// </summary>
        [Label("工厂")]
        public static readonly Property<string> FactoryProperty = P<HeatTreatment>.Register(e => e.Factory);

        /// <summary>
        /// 工厂
        /// </summary>
        public string Factory
        {
            get { return this.GetProperty(FactoryProperty); }
            set { this.SetProperty(FactoryProperty, value); }
        }
        #endregion


        #region 是否已报工 IsReported
        /// <summary>
        /// 是否已报工
        /// </summary>
        [Label("是否已报工")]
        public static readonly Property<bool?> IsReportedProperty = P<HeatTreatment>.Register(e => e.IsReported);

        /// <summary>
        /// 是否已报工
        /// </summary>
        public bool? IsReported
        {
            get { return this.GetProperty(IsReportedProperty); }
            set { this.SetProperty(IsReportedProperty, value); }
        }
        #endregion

        #region 报工数量 ReportQty
        /// <summary>
        /// 报工数量
        /// </summary>
        [Label("报工数量")]
        public static readonly Property<decimal?> ReportQtyProperty = P<HeatTreatment>.Register(e => e.ReportQty);

        /// <summary>
        /// 报工数量
        /// </summary>
        public decimal? ReportQty
        {
            get { return this.GetProperty(ReportQtyProperty); }
            set { this.SetProperty(ReportQtyProperty, value); }
        }
        #endregion

        #region 备注 Remark
        /// <summary>
        /// 备注
        /// </summary>
        [Label("备注")]
        public static readonly Property<string> RemarkProperty = P<HeatTreatment>.Register(e => e.Remark);

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return this.GetProperty(RemarkProperty); }
            set { this.SetProperty(RemarkProperty, value); }
        }
        #endregion

        #region 属性性情
        #region 物料编码 ProductCode
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("物料编码")]
        public static readonly Property<string> ProductCodeProperty = P<HeatTreatment>.RegisterView(e => e.ProductCode, p => p.WipBatch.WorkOrder.Product.Code);

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ProductCode
        {
            get { return this.GetProperty(ProductCodeProperty); }
        }

        #endregion
        #region 旧料号 ShortDescription
        /// <summary>
        /// 旧料号
        /// </summary>
        [Label("旧料号")]
        public static readonly Property<string> ShortDescriptionProperty = P<HeatTreatment>.RegisterView(e => e.ShortDescription, p => p.WipBatch.WorkOrder.Product.ShortDescription);

        /// <summary>
        /// 旧料号
        /// </summary>
        public string ShortDescription
        {
            get { return this.GetProperty(ShortDescriptionProperty); }
        }

        #endregion

        #endregion
    }

    internal class HeatTreatmentEntityConfig : EntityConfig<HeatTreatment>
    {
        protected override void ConfigMeta()
        {
            Meta.MapTable("HEAT_TREATMENT").MapAllProperties();
            Meta.Property(HeatTreatment.RemarkProperty).ColumnMeta.HasLength(1000);
            Meta.EnablePhantoms();
        }
    }
}
