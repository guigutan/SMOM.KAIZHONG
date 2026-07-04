using SIE.Domain;
using SIE.Items;
using SIE.MES.SpcFromMesDetails;
using SIE.MES.WorkOrders;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using SIE.Resources.WipResources;
using System;


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

        #region 均值上限 AverageUsl
        /// <summary>
        /// 均值上限
        /// </summary>
        [Label("均值上限")]
        public static readonly Property<decimal> AverageUslProperty = P<SpcFromMes>.Register(e => e.AverageUsl);

        /// <summary>
        /// 均值上限
        /// </summary>
        public decimal AverageUsl
        {
            get { return this.GetProperty(AverageUslProperty); }
            set { this.SetProperty(AverageUslProperty, value); }
        }
        #endregion

        #region 均值中心限 AverageSl
        /// <summary>
        /// 均值中心限
        /// </summary>
        [Label("均值中心限")]
        public static readonly Property<decimal> AverageSlProperty = P<SpcFromMes>.Register(e => e.AverageSl);

        /// <summary>
        /// 均值中心限
        /// </summary>
        public decimal AverageSl
        {
            get { return this.GetProperty(AverageSlProperty); }
            set { this.SetProperty(AverageSlProperty, value); }
        }
        #endregion

        #region 均值下限 AverageLsl
        /// <summary>
        /// 均值下限
        /// </summary>
        [Label("均值下限")]
        public static readonly Property<decimal> AverageLslProperty = P<SpcFromMes>.Register(e => e.AverageLsl);

        /// <summary>
        /// 均值下限
        /// </summary>
        public decimal AverageLsl
        {
            get { return this.GetProperty(AverageLslProperty); }
            set { this.SetProperty(AverageLslProperty, value); }
        }
        #endregion

        #region 极差上限 RangeUsl
        /// <summary>
        /// 极差上限
        /// </summary>
        [Label("极差上限")]
        public static readonly Property<decimal> RangeUslProperty = P<SpcFromMes>.Register(e => e.RangeUsl);

        /// <summary>
        /// 极差上限
        /// </summary>
        public decimal RangeUsl
        {
            get { return this.GetProperty(RangeUslProperty); }
            set { this.SetProperty(RangeUslProperty, value); }
        }
        #endregion

        #region 极差中心限 RangeSl
        /// <summary>
        /// 极差中心限
        /// </summary>
        [Label("极差中心限")]
        public static readonly Property<decimal> RangeSlProperty = P<SpcFromMes>.Register(e => e.RangeSl);

        /// <summary>
        /// 极差中心限
        /// </summary>
        public decimal RangeSl
        {
            get { return this.GetProperty(RangeSlProperty); }
            set { this.SetProperty(RangeSlProperty, value); }
        }
        #endregion

        #region 极差下限 RangeLsl
        /// <summary>
        /// 极差下限
        /// </summary>
        [Label("极差下限")]
        public static readonly Property<decimal> RangeLslProperty = P<SpcFromMes>.Register(e => e.RangeLsl);

        /// <summary>
        /// 极差下限
        /// </summary>
        public decimal RangeLsl
        {
            get { return this.GetProperty(RangeLslProperty); }
            set { this.SetProperty(RangeLslProperty, value); }
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
            GroupCountProperty, QtyProperty  // 依赖这两个属性，当它们变化时重新计算
        );

        /// <summary>
        /// 总组数
        /// </summary>
        public int TotalQty
        {
            get { return this.GetProperty(TotalQtyProperty); }
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
