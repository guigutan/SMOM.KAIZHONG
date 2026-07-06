using SIE.Domain;
using SIE.Items;
using SIE.MES.SpcFromMesDetails;
using SIE.MES.WorkOrders;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using SIE.Resources.WipResources;
using System;
using System.Collections.Generic;
using System.Linq;


namespace SIE.MES.SpcFromMess
{
    /// <summary>
    /// 统计过程控制-过程监控类
    /// </summary>
    public enum SpcCategory‌A
    {

        /// <summary>
        /// Xbar-R
        /// </summary>
        [Label("Xbar-R")]
        XbarR = 10,

        /// <summary>
        /// Xbar-S
        /// </summary>
        [Label("Xbar-S")]
        XbarS = 11,

        /// <summary>
        /// 单值移动极差
        /// </summary>
        [Label("单值移动极差")]
        IMRChart = 12,


        /// <summary>
        /// P 图
        /// </summary>
        [Label("P 图")]
        PChart = 13,


        /// <summary>
        /// C 图
        /// </summary>
        [Label("C 图")]
        CChart = 14,


    }

    /// <summary>
    /// 统计过程控制-过程能力分析类
    /// </summary>
    public enum SpcCategory‌B
    {

        /// <summary>
        /// Cp
        /// </summary>
        [Label("Cp")]
        Cp = 20,

        /// <summary>
        /// Cpk
        /// </summary>
        [Label("Cpk")]
        Cpk = 21,

        /// <summary>
        /// Pp
        /// </summary>
        [Label("Pp")]
        Pp = 22,


        /// <summary>
        ///Ppk
        /// </summary>
        [Label("Ppk")]
        Ppk = 23,


        /// <summary>
        ///Ca
        /// </summary>
        [Label("Ca")]
        Ca = 24,

    }


    /// <summary>
    /// 统计过程控制
    /// </summary>
    [RootEntity, Serializable]
    [Label("统计过程控制")]
    [ConditionQueryType(typeof(SpcFromMesCriteria))]
    public class SpcFromMes:DataEntity
    {
        #region 管制编号 No       
        /// <summary>
        /// 管制编号
        /// </summary>
        [Label("管制编号")]
        [Required]
        [NotDuplicate]
        public static readonly Property<string> NoProperty = P<SpcFromMes>.Register(e => e.No);

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
        public static readonly Property<string> NameProperty = P<SpcFromMes>.Register(e => e.Name);

        /// <summary>
        /// 管制名称
        /// </summary>
        public string Name
        {
            get { return this.GetProperty(NameProperty); }
            set { this.SetProperty(NameProperty, value); }
        }
        #endregion

        #region 产品 Item
        /// <summary>
        /// 产品ID
        /// </summary>
        [Label("产品编码")]
        public static readonly IRefIdProperty ItemIdProperty = P<SpcFromMes>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Item> ItemProperty = P<SpcFromMes>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 产品
        /// </summary>
        public Item Item
        {
            get { return this.GetRefEntity(ItemProperty); }
            set { this.SetRefEntity(ItemProperty, value); }
        }
        #endregion

        #region 产品名称 ItemName ------视图
        /// <summary>
        /// 产品名称
        /// </summary>
        [Label("产品名称")]
        public static readonly Property<string> ItemNameProperty = P<SpcFromMes>.RegisterView(e => e.ItemName, p => p.Item.Name);

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ItemName
        {
            get { return this.GetProperty(ItemNameProperty); }
            set { this.SetProperty(ItemNameProperty, value); }
        }
        #endregion      

        #region 管制项目 Project
        /// <summary>
        /// 管制项目
        /// </summary>
        [Label("管制项目")]
        public static readonly Property<string> ProjectProperty = P<SpcFromMes>.Register(e => e.Project);

        /// <summary>
        /// 管制项目
        /// </summary>
        public string Project
        {
            get { return this.GetProperty(ProjectProperty); }
            set { this.SetProperty(ProjectProperty, value); }
        }
        #endregion

        #region 测量单位 Unit
        /// <summary>
        /// 测量单位ID
        /// </summary>
        [Label("测量单位")]
        public static readonly IRefIdProperty UnitIdProperty = P<SpcFromMes>.RegisterRefId(e => e.UnitId, ReferenceType.Normal);

        /// <summary>
        /// 测量单位ID
        /// </summary>
        public double? UnitId
        {
            get { return (double?)this.GetRefNullableId(UnitIdProperty); }
            set { this.SetRefNullableId(UnitIdProperty, value); }
        }

        /// <summary>
        /// 测量单位
        /// </summary>
        [Label("测量单位")]
        public static readonly RefEntityProperty<Unit> UnitProperty = P<SpcFromMes>.RegisterRef(e => e.Unit, UnitIdProperty);

        /// <summary>
        /// 测量单位
        /// </summary>
        public Unit Unit
        {
            get { return this.GetRefEntity(UnitProperty); }
            set { this.SetRefEntity(UnitProperty, value); }
        }
        #endregion

        #region 测量单位编码 UnitName ------视图
        /// <summary>
        /// 测量单位编码
        /// </summary>
        [Label("测量单位编码")]
        public static readonly Property<string> UnitCodeProperty = P<SpcFromMes>.RegisterView(e => e.UnitCode, p => p.Unit.Code);

        /// <summary>
        /// 测量单位编码
        /// </summary>
        public string UnitCode
        {
            get { return this.GetProperty(UnitCodeProperty); }
            set { this.SetProperty(UnitCodeProperty, value); }
        }
        #endregion      

        #region 标准上限 StandardUsl
        /// <summary>
        /// 标准上限
        /// </summary>
        [Label("标准上限")]
        public static readonly Property<decimal> StandardUslProperty = P<SpcFromMes>.Register(e => e.StandardUsl);

        /// <summary>
        /// 标准上限
        /// </summary>
        public decimal StandardUsl
        {
            get { return this.GetProperty(StandardUslProperty); }
            set { this.SetProperty(StandardUslProperty, value); }
        }
        #endregion

        #region 标准中心限 StandardSl
        /// <summary>
        /// 标准中心限
        /// </summary>
        [Label("标准中心限")]
        public static readonly Property<decimal> StandardSlProperty = P<SpcFromMes>.Register(e => e.StandardSl);

        /// <summary>
        /// 标准中心限
        /// </summary>
        public decimal StandardSl
        {
            get { return this.GetProperty(StandardSlProperty); }
            set { this.SetProperty(StandardSlProperty, value); }
        }
        #endregion

        #region 标准下限 StandardLsl
        /// <summary>
        /// 标准下限
        /// </summary>
        [Label("标准下限")]
        public static readonly Property<decimal> StandardLslProperty = P<SpcFromMes>.Register(e => e.StandardLsl);

        /// <summary>
        /// 标准下限
        /// </summary>
        public decimal StandardLsl
        {
            get { return this.GetProperty(StandardLslProperty); }
            set { this.SetProperty(StandardLslProperty, value); }
        }
        #endregion

        #region 子组大小 GroupCount
        /// <summary>
        /// 子组大小
        /// </summary>
        [Label("子组大小")]
        public static readonly Property<int> GroupCountProperty = P<SpcFromMes>.Register(e => e.GroupCount);

        /// <summary>
        /// 子组大小
        /// </summary>
        public int GroupCount
        {
            get { return this.GetProperty(GroupCountProperty); }
            set { this.SetProperty(GroupCountProperty, value); }
        }
        #endregion

        #region 生产部门 ProductionDept
        /// <summary>
        /// 生产部门
        /// </summary>
        [Label("生产部门")]
        public static readonly Property<string> ProductionDeptProperty = P<SpcFromMes>.Register(e => e.ProductionDept);

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
        [Label("机台编码")]
        public static readonly IRefIdProperty ResourceIdProperty = P<SpcFromMes>.RegisterRefId(e => e.ResourceId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<WipResource> ResourceProperty = P<SpcFromMes>.RegisterRef(e => e.Resource, ResourceIdProperty);

        /// <summary>
        /// 机台
        /// </summary>
        public WipResource Resource
        {
            get { return this.GetRefEntity(ResourceProperty); }
            set { this.SetRefEntity(ResourceProperty, value); }
        }
        #endregion

        #region 机台名称 ResourceName ------视图
        /// <summary>
        /// 机台名称
        /// </summary>
        [Label("机台名称")]
        public static readonly Property<string> ResourceNameProperty = P<SpcFromMes>.RegisterView(e => e.ResourceName, p => p.Resource.Name);

        /// <summary>
        /// 机台名称
        /// </summary>
        public string ResourceName
        {
            get { return this.GetProperty(ResourceNameProperty); }
            set { this.SetProperty(ResourceNameProperty, value); }
        }
        #endregion

        #region 测量人员 Inspector
        /// <summary>
        /// 测量人员ID
        /// </summary>
        [Label("测量人员工号")]
        public static readonly IRefIdProperty InspectorIdProperty = P<SpcFromMes>.RegisterRefId(e => e.InspectorId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Employee> InspectorProperty = P<SpcFromMes>.RegisterRef(e => e.Inspector, InspectorIdProperty);

        /// <summary>
        /// 测量人员
        /// </summary>
        public Employee Inspector
        {
            get { return this.GetRefEntity(InspectorProperty); }
            set { this.SetRefEntity(InspectorProperty, value); }
        }
        #endregion

        #region 测量人员 InspectorName ------视图
        /// <summary>
        /// 测量人员
        /// </summary>
        [Label("测量人员")]
        public static readonly Property<string> InspectorNameProperty = P<SpcFromMes>.RegisterView(e => e.InspectorName, p => p.Inspector.Name);

        /// <summary>
        /// 测量人员
        /// </summary>
        public string InspectorName
        {
            get { return this.GetProperty(InspectorNameProperty); }
            set { this.SetProperty(InspectorNameProperty, value); }
        }
        #endregion

        #region 过程分析 ProcessAnalysis
        /// <summary>
        /// 过程分析
        /// </summary>
        [Label("过程分析")]
        public static readonly Property<string> ProcessAnalysisProperty = P<SpcFromMes>.Register(e => e.ProcessAnalysis);

        /// <summary>
        /// 过程分析
        /// </summary>
        public string ProcessAnalysis
        {
            get { return this.GetProperty(ProcessAnalysisProperty); }
            set { this.SetProperty(ProcessAnalysisProperty, value); }
        }
        #endregion

        #region 原因及改善措施 ReasonsAndSolutions
        /// <summary>
        /// 原因及改善措施
        /// </summary>
        [Label("原因及改善措施")]
        public static readonly Property<string> ReasonsAndSolutionsProperty = P<SpcFromMes>.Register(e => e.ReasonsAndSolutions);

        /// <summary>
        /// 原因及改善措施
        /// </summary>
        public string ReasonsAndSolutions
        {
            get { return this.GetProperty(ReasonsAndSolutionsProperty); }
            set { this.SetProperty(ReasonsAndSolutionsProperty, value); }
        }
        #endregion




        #region 统计过程控制明细表----父子关系
        /// <summary>
        /// 统计过程控制明细表
        /// </summary>
        public static readonly ListProperty<EntityList<SpcFromMesDetail>> SpcDetailListProperty = P<SpcFromMes>.RegisterList(e => e.SpcDetailList);

        /// <summary>
        /// 统计过程控制明细表
        /// </summary>
        public EntityList<SpcFromMesDetail> SpcDetailList
        {
            get { return this.GetLazyList(SpcDetailListProperty); }
        }
        #endregion



        #region 均值上限 AverageUsl------只读属性
        /// <summary>
        /// 均值上限
        /// </summary>
        [Label("均值上限")]
        public static readonly Property<decimal> AverageUslProperty = P<SpcFromMes>.RegisterReadOnly(
            e => e.AverageUsl,
            e => CalcAverageUcl(e.SpcDetailList, e.GroupCount),
            SpcDetailListProperty, GroupCountProperty
        );

        /// <summary>
        /// 均值上限
        /// </summary>
        public decimal AverageUsl
        {
            get { return this.GetProperty(AverageUslProperty); }
        }
        #endregion

        #region 均值中心限 AverageSl------只读属性
        /// <summary>
        /// 均值中心限
        /// </summary>
        [Label("均值中心限")]
        public static readonly Property<decimal> AverageSlProperty = P<SpcFromMes>.RegisterReadOnly(
            e => e.AverageSl,
            e => CalcXBar(e.SpcDetailList, e.GroupCount),
            SpcDetailListProperty, GroupCountProperty
        );

        /// <summary>
        /// 均值中心限
        /// </summary>
        public decimal AverageSl
        {
            get { return this.GetProperty(AverageSlProperty); }
        }
        #endregion

        #region 均值下限 AverageLsl------只读属性
        /// <summary>
        /// 均值下限
        /// </summary>
        [Label("均值下限")]
        public static readonly Property<decimal> AverageLslProperty = P<SpcFromMes>.RegisterReadOnly(
            e => e.AverageLsl,
            e => CalcAverageLcl(e.SpcDetailList, e.GroupCount),
            SpcDetailListProperty, GroupCountProperty
        );

        /// <summary>
        /// 均值下限
        /// </summary>
        public decimal AverageLsl
        {
            get { return this.GetProperty(AverageLslProperty); }
        }
        #endregion

        #region 极差上限 RangeUsl------只读属性
        /// <summary>
        /// 极差上限
        /// </summary>
        [Label("极差上限")]
        public static readonly Property<decimal> RangeUslProperty = P<SpcFromMes>.RegisterReadOnly(
            e => e.RangeUsl,
            e => CalcRangeUcl(e.SpcDetailList, e.GroupCount),
            SpcDetailListProperty, GroupCountProperty
        );

        /// <summary>
        /// 极差上限
        /// </summary>
        public decimal RangeUsl
        {
            get { return this.GetProperty(RangeUslProperty); }
        }
        #endregion

        #region 极差中心限 RangeSl------只读属性
        /// <summary>
        /// 极差中心限
        /// </summary>
        [Label("极差中心限")]
        public static readonly Property<decimal> RangeSlProperty = P<SpcFromMes>.RegisterReadOnly(
            e => e.RangeSl,
            e => CalcRBar(e.SpcDetailList, e.GroupCount),
            SpcDetailListProperty, GroupCountProperty
        );

        /// <summary>
        /// 极差中心限
        /// </summary>
        public decimal RangeSl
        {
            get { return this.GetProperty(RangeSlProperty); }
        }
        #endregion

        #region 极差下限 RangeLsl------只读属性
        /// <summary>
        /// 极差下限
        /// </summary>
        [Label("极差下限")]
        public static readonly Property<decimal> RangeLslProperty = P<SpcFromMes>.RegisterReadOnly(
            e => e.RangeLsl,
            e => CalcRangeLcl(e.SpcDetailList, e.GroupCount),
            SpcDetailListProperty, GroupCountProperty
        );

        /// <summary>
        /// 极差下限
        /// </summary>
        public decimal RangeLsl
        {
            get { return this.GetProperty(RangeLslProperty); }
        }
        #endregion

        #region 样本组数 Qty------只读属性
        /// <summary>
        /// 样本组数（子表记录数）
        /// </summary>
        [Label("样本组数")]
        public static readonly Property<int> QtyProperty = P<SpcFromMes>.RegisterReadOnly(
            e => e.Qty,
            e => e.SpcDetailList?.Count ?? 0,
            SpcDetailListProperty
        );

        /// <summary>
        /// 样本组数
        /// </summary>
        public int Qty
        {
            get { return this.GetProperty(QtyProperty); }
        }
        #endregion

        #region 总组数 TotalQty------只读属性
        /// <summary>
        /// 总组数
        /// </summary>
        [Label("总组数")]
        public static readonly Property<int> TotalQtyProperty = P<SpcFromMes>.RegisterReadOnly(
            e => e.TotalQty,
            e => e.GroupCount * e.Qty,
            GroupCountProperty, QtyProperty
        );

        /// <summary>
        /// 总组数
        /// </summary>
        public int TotalQty
        {
            get { return this.GetProperty(TotalQtyProperty); }
        }
        #endregion


        #region SPC过程能力分析字段（父级汇总，与明细表SumX/AvgX/RangeX区分）

        #region 观测值总和 TotalSumX------只读属性
        /// <summary>
        /// ∑X（父级）：所有样本组中GroupCount对应数量的ObservedValue值的总和
        /// </summary>
        [Label("∑X(总)")]
        public static readonly Property<decimal> TotalSumXProperty = P<SpcFromMes>.RegisterReadOnly(
            e => e.TotalSumX,
            e => CalcTotalSumX(e.SpcDetailList, e.GroupCount),
            SpcDetailListProperty, GroupCountProperty
        );

        /// <summary>
        /// ∑X（父级）：所有样本组中GroupCount对应数量的ObservedValue值的总和
        /// </summary>
        public decimal TotalSumX
        {
            get { return this.GetProperty(TotalSumXProperty); }
        }
        #endregion

        #region 极差总和 TotalSumR------只读属性
        /// <summary>
        /// ∑R（父级）：所有样本组极差(R)之和
        /// </summary>
        [Label("∑R(总)")]
        public static readonly Property<decimal> TotalSumRProperty = P<SpcFromMes>.RegisterReadOnly(
            e => e.TotalSumR,
            e => CalcTotalSumR(e.SpcDetailList, e.GroupCount),
            SpcDetailListProperty, GroupCountProperty
        );

        /// <summary>
        /// ∑R（父级）：所有样本组极差(R)之和
        /// </summary>
        public decimal TotalSumR
        {
            get { return this.GetProperty(TotalSumRProperty); }
        }
        #endregion

        #region 总均值 TotalAvgX------只读属性
        /// <summary>
        /// X̄（父级）：∑X(总) / 总组数TotalQty
        /// </summary>
        [Label("X̄(总)")]
        public static readonly Property<decimal> TotalAvgXProperty = P<SpcFromMes>.RegisterReadOnly(
            e => e.TotalAvgX,
            e => e.TotalQty > 0 ? Math.Round(e.TotalSumX / e.TotalQty, 6) : 0m,
            TotalSumXProperty, TotalQtyProperty
        );

        /// <summary>
        /// X̄（父级）：∑X(总) / 总组数TotalQty
        /// </summary>
        public decimal TotalAvgX
        {
            get { return this.GetProperty(TotalAvgXProperty); }
        }
        #endregion

        #region 平均极差 TotalAvgR------只读属性
        /// <summary>
        /// R̄（父级）：∑R(总) / 样本组数Qty
        /// </summary>
        [Label("R̄(总)")]
        public static readonly Property<decimal> TotalAvgRProperty = P<SpcFromMes>.RegisterReadOnly(
            e => e.TotalAvgR,
            e => e.Qty > 0 ? Math.Round(e.TotalSumR / e.Qty, 6) : 0m,
            TotalSumRProperty, QtyProperty
        );

        /// <summary>
        /// R̄（父级）：∑R(总) / 样本组数Qty
        /// </summary>
        public decimal TotalAvgR
        {
            get { return this.GetProperty(TotalAvgRProperty); }
        }
        #endregion

        #region 过程标准差 Sigma------只读属性
        /// <summary>
        /// σ = R̄ / d2，d2为SPC常数（按子组大小查表）
        /// </summary>
        [Label("σ")]
        public static readonly Property<decimal> SigmaProperty = P<SpcFromMes>.RegisterReadOnly(
            e => e.Sigma,
            e => CalcSigma(e.TotalAvgR, e.GroupCount),
            TotalAvgRProperty, GroupCountProperty
        );

        /// <summary>
        /// σ：过程标准差，σ = R̄ / d2（d2为SPC常数，按子组大小查表）
        /// </summary>
        public decimal Sigma
        {
            get { return this.GetProperty(SigmaProperty); }
        }
        #endregion

        #region CPU------只读属性
        /// <summary>
        /// CPU = (标准上限USL - X̄) / (3σ)
        /// </summary>
        [Label("CPU")]
        public static readonly Property<decimal> CpuProperty = P<SpcFromMes>.RegisterReadOnly(
            e => e.Cpu,
            e => e.Sigma > 0 ? Math.Round((e.StandardUsl - e.TotalAvgX) / (3m * e.Sigma), 6) : 0m,
            StandardUslProperty, TotalAvgXProperty, SigmaProperty
        );

        /// <summary>
        /// CPU：过程能力指数（上限），CPU = (标准上限USL - X̄) / (3σ)
        /// </summary>
        public decimal Cpu
        {
            get { return this.GetProperty(CpuProperty); }
        }
        #endregion

        #region CPL------只读属性
        /// <summary>
        /// CPL = (X̄ - 标准下限LSL) / (3σ)
        /// </summary>
        [Label("CPL")]
        public static readonly Property<decimal> CplProperty = P<SpcFromMes>.RegisterReadOnly(
            e => e.Cpl,
            e => e.Sigma > 0 ? Math.Round((e.TotalAvgX - e.StandardLsl) / (3m * e.Sigma), 6) : 0m,
            TotalAvgXProperty, StandardLslProperty, SigmaProperty
        );

        /// <summary>
        /// CPL：过程能力指数（下限），CPL = (X̄ - 标准下限LSL) / (3σ)
        /// </summary>
        public decimal Cpl
        {
            get { return this.GetProperty(CplProperty); }
        }
        #endregion

        #region STDEV------只读属性
        /// <summary>
        /// STDEV：各样本组SumX值的样本标准差
        /// </summary>
        [Label("STDEV")]
        public static readonly Property<decimal> StdevProperty = P<SpcFromMes>.RegisterReadOnly(
            e => e.Stdev,
            e => CalcStdev(e.SpcDetailList, e.GroupCount),
            SpcDetailListProperty, GroupCountProperty
        );

        /// <summary>
        /// STDEV：各样本组∑X值的样本标准差
        /// </summary>
        public decimal Stdev
        {
            get { return this.GetProperty(StdevProperty); }
        }
        #endregion

        #region PPK------只读属性
        /// <summary>
        /// PPK = (X̄ - 标准下限LSL) / (3σ)，同CPL
        /// </summary>
        [Label("PPK")]
        public static readonly Property<decimal> PpkProperty = P<SpcFromMes>.RegisterReadOnly(
            e => e.Ppk,
            e => e.Cpl,
            CplProperty
        );

        /// <summary>
        /// PPK：过程性能指数，PPK = (X̄ - 标准下限LSL) / (3σ)，同CPL
        /// </summary>
        public decimal Ppk
        {
            get { return this.GetProperty(PpkProperty); }
        }
        #endregion

        #region CPK------只读属性
        /// <summary>
        /// CPK = CPL值
        /// </summary>
        [Label("CPK")]
        public static readonly Property<decimal> CpkProperty = P<SpcFromMes>.RegisterReadOnly(
            e => e.Cpk,
            e => e.Cpl,
            CplProperty
        );

        /// <summary>
        /// CPK：综合过程能力指数，CPK = CPL值
        /// </summary>
        public decimal Cpk
        {
            get { return this.GetProperty(CpkProperty); }
        }
        #endregion

        #endregion


        #region SPC计算常数和方法（Xbar-R控制图）

        // SPC常数表，索引为子组大小n（n=2~25）
        // A2: 用于计算均值控制限，公式：UCL=X̄+A2×R̄, LCL=X̄-A2×R̄
        private static readonly decimal[] SPC_A2 = { 0m, 0m, 1.880m, 1.023m, 0.729m, 0.577m, 0.483m, 0.419m, 0.373m, 0.337m, 0.308m, 0.285m, 0.266m, 0.249m, 0.235m, 0.223m, 0.212m, 0.203m, 0.194m, 0.187m, 0.180m, 0.173m, 0.167m, 0.162m, 0.157m, 0.153m };
        // D3: 用于计算极差下限，公式：LCL=D3×R̄（n<7时D3=0，即无下限）
        private static readonly decimal[] SPC_D3 = { 0m, 0m, 0m, 0m, 0m, 0m, 0m, 0.076m, 0.136m, 0.184m, 0.223m, 0.256m, 0.283m, 0.307m, 0.328m, 0.347m, 0.363m, 0.378m, 0.391m, 0.403m, 0.415m, 0.425m, 0.434m, 0.443m, 0.451m, 0.459m };
        // D4: 用于计算极差上限，公式：UCL=D4×R̄
        private static readonly decimal[] SPC_D4 = { 0m, 0m, 3.267m, 2.574m, 2.282m, 2.114m, 2.004m, 1.924m, 1.864m, 1.816m, 1.777m, 1.744m, 1.717m, 1.693m, 1.672m, 1.653m, 1.637m, 1.622m, 1.608m, 1.597m, 1.585m, 1.575m, 1.566m, 1.557m, 1.548m, 1.541m };

        /// <summary>
        /// 根据索引获取明细行中对应子组的观测值（ObservedValue1~30）
        /// </summary>
        private static decimal GetObservedValue(SpcFromMesDetail detail, int index)
        {
            switch (index)
            {
                case 1: return detail.ObservedValue1;
                case 2: return detail.ObservedValue2;
                case 3: return detail.ObservedValue3;
                case 4: return detail.ObservedValue4;
                case 5: return detail.ObservedValue5;
                case 6: return detail.ObservedValue6;
                case 7: return detail.ObservedValue7;
                case 8: return detail.ObservedValue8;
                case 9: return detail.ObservedValue9;
                case 10: return detail.ObservedValue10;
                case 11: return detail.ObservedValue11;
                case 12: return detail.ObservedValue12;
                case 13: return detail.ObservedValue13;
                case 14: return detail.ObservedValue14;
                case 15: return detail.ObservedValue15;
                case 16: return detail.ObservedValue16;
                case 17: return detail.ObservedValue17;
                case 18: return detail.ObservedValue18;
                case 19: return detail.ObservedValue19;
                case 20: return detail.ObservedValue20;
                case 21: return detail.ObservedValue21;
                case 22: return detail.ObservedValue22;
                case 23: return detail.ObservedValue23;
                case 24: return detail.ObservedValue24;
                case 25: return detail.ObservedValue25;
                case 26: return detail.ObservedValue26;
                case 27: return detail.ObservedValue27;
                case 28: return detail.ObservedValue28;
                case 29: return detail.ObservedValue29;
                case 30: return detail.ObservedValue30;
                default: return 0m;
            }
        }

        /// <summary>
        /// 计算所有样本组的均值的平均值 X̄（X-bar）
        /// 公式：X̄ = Σ(每组观测值之和/子组大小) / 样本组数
        /// 用途：作为均值控制图的中心线（CL）
        /// </summary>
        private static decimal CalcXBar(EntityList<SpcFromMesDetail> details, int groupCount)
        {
            if (details == null || details.Count == 0 || groupCount < 2 || groupCount > 25) return 0m;
            decimal sumMean = 0m;
            int count = 0;
            foreach (SpcFromMesDetail detail in details)
            {
                decimal sum = 0m;
                for (int i = 1; i <= groupCount; i++)
                    sum += GetObservedValue(detail, i);
                sumMean += sum / groupCount;
                count++;
            }
            return count > 0 ? Math.Round(sumMean / count, 6) : 0m;
        }

        /// <summary>
        /// 计算所有样本组的极差的平均值 R̄（R-bar）
        /// 公式：R̄ = Σ(每组最大值-最小值) / 样本组数
        /// 用途：作为极差控制图的中心线（CL）
        /// </summary>
        private static decimal CalcRBar(EntityList<SpcFromMesDetail> details, int groupCount)
        {
            if (details == null || details.Count == 0 || groupCount < 2 || groupCount > 25) return 0m;
            decimal sumRange = 0m;
            int count = 0;
            foreach (SpcFromMesDetail detail in details)
            {
                decimal min = GetObservedValue(detail, 1);
                decimal max = min;
                for (int i = 2; i <= groupCount; i++)
                {
                    decimal val = GetObservedValue(detail, i);
                    if (val < min) min = val;
                    if (val > max) max = val;
                }
                sumRange += max - min;
                count++;
            }
            return count > 0 ? Math.Round(sumRange / count, 6) : 0m;
        }

        /// <summary>
        /// 计算均值控制图上控制限（UCL）
        /// 公式：UCL = X̄ + A2 × R̄
        /// </summary>
        private static decimal CalcAverageUcl(EntityList<SpcFromMesDetail> details, int groupCount)
        {
            if (groupCount < 2 || groupCount > 25) return 0m;
            decimal xBar = CalcXBar(details, groupCount);
            decimal rBar = CalcRBar(details, groupCount);
            return Math.Round(xBar + SPC_A2[groupCount] * rBar, 6);
        }

        /// <summary>
        /// 计算均值控制图下控制限（LCL）
        /// 公式：LCL = X̄ - A2 × R̄
        /// </summary>
        private static decimal CalcAverageLcl(EntityList<SpcFromMesDetail> details, int groupCount)
        {
            if (groupCount < 2 || groupCount > 25) return 0m;
            decimal xBar = CalcXBar(details, groupCount);
            decimal rBar = CalcRBar(details, groupCount);
            return Math.Round(xBar - SPC_A2[groupCount] * rBar, 6);
        }

        /// <summary>
        /// 计算极差控制图上控制限（UCL）
        /// 公式：UCL = D4 × R̄
        /// </summary>
        private static decimal CalcRangeUcl(EntityList<SpcFromMesDetail> details, int groupCount)
        {
            if (groupCount < 2 || groupCount > 25) return 0m;
            decimal rBar = CalcRBar(details, groupCount);
            return Math.Round(SPC_D4[groupCount] * rBar, 6);
        }

        /// <summary>
        /// 计算极差控制图下控制限（LCL）
        /// 公式：LCL = D3 × R̄（当n&lt;7时D3=0，LCL为0）
        /// </summary>
        private static decimal CalcRangeLcl(EntityList<SpcFromMesDetail> details, int groupCount)
        {
            if (groupCount < 2 || groupCount > 25) return 0m;
            decimal rBar = CalcRBar(details, groupCount);
            return Math.Round(SPC_D3[groupCount] * rBar, 6);
        }

        // d2常数表：用于由R̄估计过程标准差σ，公式：σ = R̄ / d2
        // 来源：GB/T 4091附表 / AIAG SPC手册 / ISO 7870
        // 索引对应子组大小n（n=2~25）
        private static readonly decimal[] SPC_d2 = { 0m, 0m, 1.128m, 1.693m, 2.059m, 2.326m, 2.534m, 2.704m, 2.847m, 2.970m, 3.078m, 3.173m, 3.258m, 3.336m, 3.407m, 3.472m, 3.532m, 3.588m, 3.640m, 3.689m, 3.735m, 3.778m, 3.819m, 3.858m, 3.895m, 3.931m };

        /// <summary>
        /// 计算∑X(总)：所有样本组中前GroupCount个ObservedValue之和的总计
        /// </summary>
        private static decimal CalcTotalSumX(EntityList<SpcFromMesDetail> details, int groupCount)
        {
            if (details == null || details.Count == 0 || groupCount <= 0) return 0m;
            decimal total = 0m;
            foreach (SpcFromMesDetail detail in details)
            {
                for (int i = 1; i <= groupCount; i++)
                    total += GetObservedValue(detail, i);
            }
            return total;
        }

        /// <summary>
        /// 计算∑R(总)：所有样本组极差之和
        /// </summary>
        private static decimal CalcTotalSumR(EntityList<SpcFromMesDetail> details, int groupCount)
        {
            if (details == null || details.Count == 0 || groupCount <= 0) return 0m;
            decimal total = 0m;
            foreach (SpcFromMesDetail detail in details)
            {
                decimal min = GetObservedValue(detail, 1);
                decimal max = min;
                for (int i = 2; i <= groupCount; i++)
                {
                    decimal val = GetObservedValue(detail, i);
                    if (val < min) min = val;
                    if (val > max) max = val;
                }
                total += max - min;
            }
            return total;
        }

        /// <summary>
        /// 计算σ = R̄ / d2
        /// d2为SPC常数，按子组大小n查表（来源：GB/T 4091 / AIAG SPC手册）
        /// </summary>
        private static decimal CalcSigma(decimal totalAvgR, int groupCount)
        {
            if (groupCount < 2 || groupCount > 25) return 0m;
            decimal d2 = SPC_d2[groupCount];
            if (d2 == 0m) return 0m;
            return Math.Round(totalAvgR / d2, 6);
        }

        /// <summary>
        /// 计算STDEV：各样本组SumX的样本标准差
        /// 公式：STDEV = sqrt(Σ(xi - x̄)² / (n-1))
        /// </summary>
        private static decimal CalcStdev(EntityList<SpcFromMesDetail> details, int groupCount)
        {
            if (details == null || details.Count < 2 || groupCount <= 0) return 0m;
            var sumXList = new List<decimal>();
            foreach (SpcFromMesDetail detail in details)
            {
                decimal sum = 0m;
                for (int i = 1; i <= groupCount; i++)
                    sum += GetObservedValue(detail, i);
                sumXList.Add(sum);
            }
            if (sumXList.Count < 2) return 0m;
            decimal mean = sumXList.Sum() / sumXList.Count;
            decimal sumSqDiff = 0m;
            foreach (decimal x in sumXList)
                sumSqDiff += (x - mean) * (x - mean);
            return Math.Round((decimal)Math.Sqrt((double)(sumSqDiff / (sumXList.Count - 1))), 6);
        }

        #endregion


    }


    /// <summary>
    /// 配置数据库映射
    /// </summary>
    internal class SpcFromMesConfig : EntityConfig<SpcFromMes>
    {
        /// <summary>
        /// 配置数据库映射
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("SPC_FROM_MES").MapAllProperties();
            Meta.EnablePhantoms();
            Meta.EnableInvOrg();
        }
    }

}
