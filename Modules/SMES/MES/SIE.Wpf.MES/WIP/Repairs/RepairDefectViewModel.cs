using SIE.Defects;
using SIE.Defects.Measures;
using SIE.Domain;
using SIE.MES.WIP.Products;
using SIE.ObjectModel;
using System;

namespace SIE.Wpf.MES.WIP.Repairs
{
    //    /// <summary>
    //    /// 不良信息视图模型
    //    /// </summary>
    //    [RootEntity, Serializable]
    //    [Label("不良信息")]
    //    public class RepairDefectViewModel : ViewModel
    //    {

    //    #region 产品缺陷记录 WipProductDefect 
    //    /// <summary>
    //    /// 产品缺陷记录ID
    //    /// </summary>
    //    [Label("产品缺陷记录")]
    //    public static readonly IRefIdProperty WipProductDefectIdProperty =
    //        P<RepairDefectViewModel>.RegisterRefId(e => e.WipProductDefectId, ReferenceType.Normal);

    //    /// <summary>
    //    /// 产品缺陷记录ID
    //    /// </summary>
    //    public double WipProductDefectId
    //    {
    //        get { return (double)this.GetRefId(WipProductDefectIdProperty); }
    //        set { this.SetRefId(WipProductDefectIdProperty, value); }
    //    }

    //    /// <summary>
    //    /// 产品缺陷记录
    //    /// </summary>
    //    public static readonly RefEntityProperty<WipProductDefect> WipProductDefectProperty =
    //        P<RepairDefectViewModel>.RegisterRef(e => e.WipProductDefect, WipProductDefectIdProperty);

    //    /// <summary>
    //    /// 产品缺陷记录
    //    /// </summary>
    //    public WipProductDefect WipProductDefect
    //    {
    //        get { return this.GetRefEntity(WipProductDefectProperty); }
    //        set { this.SetRefEntity(WipProductDefectProperty, value); }
    //    }
    //    #endregion

    //    #region 工序 ProcessName
    //    /// <summary>
    //    /// 工序
    //    /// </summary>
    //    [Label("工序")]
    //    public static readonly Property<string> ProcessNameProperty = P<RepairDefectViewModel>.RegisterView(e => e.ProcessName, p => p.WipProductDefect.Process.Name);

    //    /// <summary>
    //    /// 工序
    //    /// </summary>
    //    public string ProcessName
    //    {
    //        get { return this.GetProperty(ProcessNameProperty); }
    //    }
    //    #endregion

    //    #region 检验项描述 InspectionItemName
    //    /// <summary>
    //    /// 检验项描述
    //    /// </summary>
    //    [Label("检验项描述")]
    //    public static readonly Property<string> InspectionItemNameProperty = P<RepairDefectViewModel>.RegisterView(e => e.InspectionItemName, p => p.WipProductDefect.InspectionItem.Name);

    //    /// <summary>
    //    /// 检验项描述
    //    /// </summary>
    //    public string InspectionItemName
    //    {
    //        get { return this.GetProperty(InspectionItemNameProperty); }
    //    }
    //    #endregion

    //    #region 缺陷编码 DefectCode
    //    /// <summary>
    //    /// 缺陷编码
    //    /// </summary>
    //    [Label("缺陷编码")]
    //    public static readonly Property<string> DefectCodeProperty = P<RepairDefectViewModel>.RegisterView(e => e.DefectCode, p => p.WipProductDefect.Defect.Code);

    //    /// <summary>
    //    /// 缺陷编码
    //    /// </summary>
    //    public string DefectCode
    //    {
    //        get { return this.GetProperty(DefectCodeProperty); }
    //    }
    //    #endregion

    //    #region 缺陷描述 DefectDescription
    //    /// <summary>
    //    /// 缺陷描述
    //    /// </summary>
    //    [Label("缺陷描述")]
    //    public static readonly Property<string> DefectDescriptionProperty = P<RepairDefectViewModel>.RegisterView(e => e.DefectDescription, p => p.WipProductDefect.Defect.Description);

    //    /// <summary>
    //    /// 缺陷描述
    //    /// </summary>
    //    public string DefectDescription
    //    {
    //        get { return this.GetProperty(DefectDescriptionProperty); }
    //    }
    //    #endregion

    //    #region 是否修好 IsFixed
    //    /// <summary>
    //    /// 是否修好
    //    /// </summary>
    //    [Label("是否修好")]
    //    public static readonly Property<bool> IsFixedProperty = P<RepairDefectViewModel>.Register(e => e.IsFixed);

    //    /// <summary>
    //    /// 是否修好
    //    /// </summary>
    //    public bool IsFixed
    //    {
    //        get { return this.GetProperty(IsFixedProperty); }
    //        set { this.SetProperty(IsFixedProperty, value); }
    //    }
    //    #endregion

    //    #region 备注 Remark
    //    /// <summary>
    //    /// 备注
    //    /// </summary>
    //    [Label("备注")]
    //    public static readonly Property<string> RemarkProperty = P<RepairDefectViewModel>.Register(e => e.Remark);

    //    /// <summary>
    //    /// 备注
    //    /// </summary>
    //    public string Remark
    //    {
    //        get { return this.GetProperty(RemarkProperty); }
    //        set { this.SetProperty(RemarkProperty, value); }
    //    }
    //    #endregion

    //    #region 报废数量 ScrapQty
    //    /// <summary>
    //    /// 报废数量
    //    /// </summary>
    //    [Label("报废数量")]
    //    public static readonly Property<decimal> ScrapQtyProperty = P<RepairDefectViewModel>.Register(e => e.ScrapQty);

    //    /// <summary>
    //    /// 报废数量
    //    /// </summary>
    //    public decimal ScrapQty
    //    {
    //        get { return this.GetProperty(ScrapQtyProperty); }
    //        set { this.SetProperty(ScrapQtyProperty, value); }
    //    }
    //    #endregion

    //    #region 报废原因 ScrapReason
    //    /// <summary>
    //    /// 报废原因
    //    /// </summary>
    //    [Label("报废原因")]
    //    public static readonly Property<string> ScrapReasonProperty = P<RepairDefectViewModel>.Register(e => e.ScrapReason);

    //    /// <summary>
    //    /// 报废原因
    //    /// </summary>
    //    public string ScrapReason
    //    {
    //        get { return this.GetProperty(ScrapReasonProperty); }
    //        set { this.SetProperty(ScrapReasonProperty, value); }
    //    }
    //    #endregion 

    //    #region 维修措施 MeasureList
    //    /// <summary>
    //    /// 维修措施
    //    /// </summary> 
    //    [Label("维修措施")]
    //    public static readonly ListProperty<EntityList<RepairMeasure>> MeasureListProperty = P<RepairDefectViewModel>.RegisterList(e => e.MeasureList, new ListPropertyMeta
    //    {
    //        HasManyType = HasManyType.Aggregation,
    //        DataProvider = e => new EntityList<RepairMeasure>()
    //    });

    //    /// <summary>
    //    /// 维修措施
    //    /// </summary>
    //    public EntityList<RepairMeasure> MeasureList
    //    {
    //        get { return this.GetLazyList(MeasureListProperty); }
    //    }
    //    #endregion

    //    #region 缺陷责任 ResponsibilityList
    //    /// <summary>
    //    /// 缺陷责任
    //    /// </summary> 
    //    [Label("缺陷责任")]
    //    public static readonly ListProperty<EntityList<DefectResponsibility>> ResponsibilityListProperty = P<RepairDefectViewModel>.RegisterList(e => e.ResponsibilityList, new ListPropertyMeta
    //    {
    //        HasManyType = HasManyType.Aggregation,
    //        DataProvider = e => new EntityList<DefectResponsibility>()
    //    });

    //    /// <summary>
    //    /// 缺陷责任
    //    /// </summary>
    //    public EntityList<DefectResponsibility> ResponsibilityList
    //    {
    //        get { return this.GetLazyList(ResponsibilityListProperty); }
    //    }
    //    #endregion 
    //}
}
