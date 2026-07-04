using SIE.Defects;
using SIE.Defects.Measures;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.BatchWIP.Products;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Wpf.MES.BatchWIP.Repairs
{
    /// <summary>
    /// 不良信息视图模型
    /// </summary>
    [RootEntity, Serializable]
    [Label("不良信息")]
    public class BatchRepairDefectViewModel : ViewModel
    {

        #region 产品缺陷记录 WipProductDefect 
        /// <summary>
        /// 产品缺陷记录ID
        /// </summary>
        [Label("产品缺陷记录")]
        public static readonly IRefIdProperty WipProductDefectIdProperty =
            P<BatchRepairDefectViewModel>.RegisterRefId(e => e.WipProductDefectId, ReferenceType.Normal);

        /// <summary>
        /// 产品缺陷记录ID
        /// </summary>
        public double WipProductDefectId
        {
            get { return (double)this.GetRefId(WipProductDefectIdProperty); }
            set { this.SetRefId(WipProductDefectIdProperty, value); }
        }

        /// <summary>
        /// 产品缺陷记录
        /// </summary>
        public static readonly RefEntityProperty<BatchWipProductDefect> WipProductDefectProperty =
            P<BatchRepairDefectViewModel>.RegisterRef(e => e.WipProductDefect, WipProductDefectIdProperty);

        /// <summary>
        /// 产品缺陷记录
        /// </summary>
        public BatchWipProductDefect WipProductDefect
        {
            get { return this.GetRefEntity(WipProductDefectProperty); }
            set { this.SetRefEntity(WipProductDefectProperty, value); }
        }
        #endregion

        #region 工序 ProcessName
        /// <summary>
        /// 工序
        /// </summary>
        [Label("工序")]
        public static readonly Property<string> ProcessNameProperty = P<BatchRepairDefectViewModel>.RegisterView(e => e.ProcessName, p => p.WipProductDefect.Process.Name);

        /// <summary>
        /// 工序
        /// </summary>
        public string ProcessName
        {
            get { return this.GetProperty(ProcessNameProperty); }
        }
        #endregion

        #region 不良数量 Qty
        /// <summary>
        /// 批次数量
        /// </summary>
        [Label("不良数量")]
        public static readonly Property<decimal> QtyProperty = P<BatchRepairDefectViewModel>.Register(e => e.Qty);

        /// <summary>
        /// 不良数量
        /// </summary>
        public decimal Qty
        {
            get { return GetProperty(QtyProperty); }
            set { SetProperty(QtyProperty, value); }
        }
        #endregion

        #region 备注 Remark
        /// <summary>
        /// 备注
        /// </summary>
        [Label("备注")]
        public static readonly Property<string> RemarkProperty = P<BatchRepairDefectViewModel>.Register(e => e.Remark);

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return this.GetProperty(RemarkProperty); }
            set { this.SetProperty(RemarkProperty, value); }
        }
        #endregion

        #region 报废数量 ScrapQty
        /// <summary>
        /// 报废数量
        /// </summary>
        [Label("报废数量")]
        public static readonly Property<decimal> ScrapQtyProperty = P<BatchRepairDefectViewModel>.Register(e => e.ScrapQty);

        /// <summary>
        /// 报废数量
        /// </summary>
        public decimal ScrapQty
        {
            get { return this.GetProperty(ScrapQtyProperty); }
            set { this.SetProperty(ScrapQtyProperty, value); }
        }
        #endregion

        #region 报废原因 ScrapReason
        /// <summary>
        /// 报废原因
        /// </summary>
        [Label("报废原因")]
        public static readonly Property<string> ScrapReasonProperty = P<BatchRepairDefectViewModel>.Register(e => e.ScrapReason);

        /// <summary>
        /// 报废原因
        /// </summary>
        public string ScrapReason
        {
            get { return this.GetProperty(ScrapReasonProperty); }
            set { this.SetProperty(ScrapReasonProperty, value); }
        }
        #endregion 

        #region 维修措施 MeasureList
        /// <summary>
        /// 维修措施
        /// </summary> 
        [Label("维修措施")]
        public static readonly ListProperty<EntityList<RepairMeasure>> MeasureListProperty = P<BatchRepairDefectViewModel>.RegisterList(e => e.MeasureList, new ListPropertyMeta
        {
            HasManyType = HasManyType.Aggregation,
            DataProvider = e => new EntityList<RepairMeasure>()
        });

        /// <summary>
        /// 维修措施
        /// </summary>
        public EntityList<RepairMeasure> MeasureList
        {
            get { return this.GetLazyList(MeasureListProperty); }
        }
        #endregion

        #region 缺陷责任 ResponsibilityList
        /// <summary>
        /// 缺陷责任
        /// </summary> 
        [Label("缺陷责任")]
        public static readonly ListProperty<EntityList<DefectResponsibility>> ResponsibilityListProperty = P<BatchRepairDefectViewModel>.RegisterList(e => e.ResponsibilityList, new ListPropertyMeta
        {
            HasManyType = HasManyType.Aggregation,
            DataProvider = e => new EntityList<DefectResponsibility>()
        });

        /// <summary>
        /// 缺陷责任
        /// </summary>
        public EntityList<DefectResponsibility> ResponsibilityList
        {
            get { return this.GetLazyList(ResponsibilityListProperty); }
        }
        #endregion 
    }

    /// <summary>
    /// 实体配置
    /// </summary>
    internal class BatchRepairDefectModelConfig : EntityConfig<BatchRepairDefectViewModel>
    {
        /// <summary>
        /// 添加验证规则
        /// </summary>
        /// <param name="rules">验证规则</param>
        protected override void AddValidations(IValidationDeclarer rules)
        {
            base.AddValidations(rules);
            rules.AddRule(BatchRepairDefectViewModel.ScrapQtyProperty, new HandlerRule
            {
                Handler = (s, e) =>
                {
                    var model = s as BatchRepairDefectViewModel;
                    if (model.ScrapQty > model.Qty) e.BrokenDescription = "报废数量不能大于不良数量[{0}]".L10nFormat(model.Qty);
                }
            });
        }
    }
}
