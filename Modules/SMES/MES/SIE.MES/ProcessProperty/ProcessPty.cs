using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Items;
using SIE.Items.KzItemCategorys;
using SIE.MES.Fixture;
using SIE.MES.ItemLine;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Tech.Processs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.ProcessProperty
{
    /// <summary>
    /// 工序属性维护
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(ProcessPtyCriterial))]
    [Label("工序属性维护")]
    public class ProcessPty : DataEntity
    {
        #region 快码
        /// <summary>
        /// 工序熟悉基础维护状态快码字符串
        /// </summary>
        public const string LineCatalogType = "PROCESS_LINE";

        /// <summary>
        /// 工序熟悉基础维护类型快码字符串
        /// </summary>
        public const string TypeCatalogType = "PROCESS_TYPE";
        #endregion

        #region 产品线 ProductLine
        /// <summary>
        /// 产品线
        /// </summary>
        [Label("产品线")]
        public static readonly Property<string> ProductLineProperty = P<ProcessPty>.Register(e => e.ProductLine);

        /// <summary>
        /// 产品线
        /// </summary>
        public string ProductLine
        {
            get { return this.GetProperty(ProductLineProperty); }
            set { this.SetProperty(ProductLineProperty, value); }
        }
        #endregion

        #region 产品类型 ProductType
        /// <summary>
        /// 产品类型
        /// </summary>
        [Label("产品类型")]
        public static readonly Property<string> ProductTypeProperty = P<ProcessPty>.Register(e => e.ProductType);

        /// <summary>
        /// 产品类型
        /// </summary>
        public string ProductType
        {
            get { return this.GetProperty(ProductTypeProperty); }
            set { this.SetProperty(ProductTypeProperty, value); }
        }
        #endregion

        #region 工艺属性分类 KzCategory
        /// <summary>
        /// 工艺属性分类Id
        /// </summary>
        [Label("工艺属性分类")]
        public static readonly IRefIdProperty KzCategoryIdProperty =
            P<ProcessPty>.RegisterRefId(e => e.KzCategoryId, ReferenceType.Normal);

        /// <summary>
        /// 工艺属性分类Id
        /// </summary>
        public double? KzCategoryId
        {
            get { return (double?)this.GetRefNullableId(KzCategoryIdProperty); }
            set { this.SetRefNullableId(KzCategoryIdProperty, value); }
        }

        /// <summary>
        /// 工艺属性分类
        /// </summary>
        public static readonly RefEntityProperty<KzCategory> KzCategoryProperty =
            P<ProcessPty>.RegisterRef(e => e.KzCategory, KzCategoryIdProperty);

        /// <summary>
        /// 工艺属性分类
        /// </summary>
        public KzCategory KzCategory
        {
            get { return this.GetRefEntity(KzCategoryProperty); }
            set { this.SetRefEntity(KzCategoryProperty, value); }
        }
        #endregion

        #region 工序名称 Process
        /// <summary>
        /// 工序名称Id
        /// </summary>
        [Label("工序名称")]
        public static readonly IRefIdProperty ProcessIdProperty = P<ProcessPty>.RegisterRefId(e => e.ProcessId, ReferenceType.Normal);

        /// <summary>
        /// 工序名称Id
        /// </summary>
        public double ProcessId
        {
            get { return (double)GetRefNullableId(ProcessIdProperty); }
            set { SetRefNullableId(ProcessIdProperty, value); }
        }

        /// <summary>
        /// 工序名称
        /// </summary>
        public static readonly RefEntityProperty<Process> ProcessProperty = P<ProcessPty>.RegisterRef(e => e.Process, ProcessIdProperty);

        /// <summary>
        /// 工序名称
        /// </summary>
        public Process Process
        {
            get { return GetRefEntity(ProcessProperty); }
            set { SetRefEntity(ProcessProperty, value); }
        }
        #endregion

        #region 物料类型 Type
        /// <summary>
        /// 物料类型
        /// </summary>
        [Label("物料类型")]
        public static readonly Property<ItemType?> TypeProperty = P<ProcessPty>.Register(e => e.Type);

        /// <summary>
        /// 物料类型
        /// </summary>
        public ItemType? Type
        {
            get { return GetProperty(TypeProperty); }
            set { SetProperty(TypeProperty, value); }
        }
        #endregion

        #region 排程点 Scheduling
        /// <summary>
        /// 排程点
        /// </summary>
        [Label("排程点")]
        public static readonly Property<bool> SchedulingProperty = P<ProcessPty>.Register(e => e.Scheduling);

        /// <summary>
        /// 排程点
        /// </summary>
        public bool Scheduling
        {
            get { return this.GetProperty(SchedulingProperty); }
            set { this.SetProperty(SchedulingProperty, value); }
        }
        #endregion

        #region 派工点 DispatchWork
        /// <summary>
        /// 派工点
        /// </summary>
        [Label("派工点")]
        public static readonly Property<bool> DispatchWorkProperty = P<ProcessPty>.Register(e => e.DispatchWork);

        /// <summary>
        /// 派工点
        /// </summary>
        public bool DispatchWork
        {
            get { return this.GetProperty(DispatchWorkProperty); }
            set { this.SetProperty(DispatchWorkProperty, value); }
        }
        #endregion

        #region 产前准备 IsPrepare
        /// <summary>
        /// 产前准备
        /// </summary>
        [Label("产前准备")]
        public static readonly Property<bool?> IsPrepareProperty = P<ProcessPty>.Register(e => e.IsPrepare);

        /// <summary>
        /// 产前准备
        /// </summary>
        public bool? IsPrepare
        {
            get { return this.GetProperty(IsPrepareProperty); }
            set { this.SetProperty(IsPrepareProperty, value); }
        }
        #endregion

        #region 是否转入 IsTransfer
        /// <summary>
        /// 是否转入
        /// </summary>
        [Label("是否转入")]
        public static readonly Property<bool?> IsTransferProperty = P<ProcessPty>.Register(e => e.IsTransfer);

        /// <summary>
        /// 是否转入
        /// </summary>
        public bool? IsTransfer
        {
            get { return this.GetProperty(IsTransferProperty); }
            set { this.SetProperty(IsTransferProperty, value); }
        }
        #endregion

        #region 非首工序自动生成任务单 IsUnFirstGenerateTask
        /// <summary>
        /// 非首工序自动生成任务单
        /// </summary>
        [Label("非首工序自动生成任务单")]
        public static readonly Property<bool?> IsUnFirstGenerateTaskProperty = P<ProcessPty>.Register(e => e.IsUnFirstGenerateTask);

        /// <summary>
        /// 非首工序自动生成任务单
        /// </summary>
        public bool? IsUnFirstGenerateTask
        {
            get { return this.GetProperty(IsUnFirstGenerateTaskProperty); }
            set { this.SetProperty(IsUnFirstGenerateTaskProperty, value); }
        }
        #endregion

        #region 产前项目准备 ProcessPtyDetailList
        /// <summary>
        /// 产前项目准备
        /// </summary>
        [Label("产前项目准备")]
        public static readonly ListProperty<EntityList<ProcessPtyDetail>> ProcessPtyDetailListProperty = P<ProcessPty>.RegisterList(e => e.ProcessPtyDetailList);

        /// <summary>
        /// 产前项目准备
        /// </summary>
        public EntityList<ProcessPtyDetail> ProcessPtyDetailList
        {
            get { return this.GetLazyList(ProcessPtyDetailListProperty); }
        }
        #endregion

        #region 视图属性

        #region 工序名称 ProcessName
        /// <summary>
        /// 工序名称
        /// </summary>
        [Label("工序名称")]
        public static readonly Property<string> ProcessNameProperty = P<ProcessPty>.RegisterView(e => e.ProcessName, p => p.Process.Name);

        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcessName
        {
            get { return this.GetProperty(ProcessNameProperty); }
        }
        #endregion

        #region 工序编码 ProcessCode
        /// <summary>
        /// 工序编码
        /// </summary>
        [Label("工序编码")]
        public static readonly Property<string> ProcessCodeProperty = P<ProcessPty>.RegisterView(e => e.ProcessCode, p => p.Process.Code);

        /// <summary>
        /// 工序编码
        /// </summary>
        public string ProcessCode
        {
            get { return this.GetProperty(ProcessCodeProperty); }
        }

        #endregion

        #region 工艺属性分类编码 KzCategoryCode
        /// <summary>
        /// 工艺属性分类编码
        /// </summary>
        [Label("工艺属性分类编码")]
        public static readonly Property<string> KzCategoryCodeProperty = P<ProcessPty>.RegisterView(e => e.KzCategoryCode, p => p.KzCategory.Code);

        /// <summary>
        /// 工艺属性分类编码
        /// </summary>
        public string KzCategoryCode
        {
            get { return this.GetProperty(KzCategoryCodeProperty); }
        }
        #endregion

        #region 工艺属性分类名称 KzCategoryName
        /// <summary>
        /// 工艺属性分类名称
        /// </summary>
        [Label("工艺属性分类名称")]
        public static readonly Property<string> KzCategoryNameProperty = P<ProcessPty>.RegisterView(e => e.KzCategoryName, p => p.KzCategory.Name);

        /// <summary>
        /// 工艺属性分类名称
        /// </summary>
        public string KzCategoryName
        {
            get { return this.GetProperty(KzCategoryNameProperty); }
        }
        #endregion

        #endregion

        #region 按工序BOM生成派工单数量 IsPbcd
        /// <summary>
        /// 按工序BOM生成派工单数量
        /// </summary>
        [Label("按工序BOM生成派工单数量")]
        public static readonly Property<bool?> IsPbcdProperty = P<ProcessPty>.Register(e => e.IsPbcd);

        /// <summary>
        /// 按工序BOM生成派工单数量
        /// </summary>
        public bool? IsPbcd
        {
            get { return this.GetProperty(IsPbcdProperty); }
            set { this.SetProperty(IsPbcdProperty, value); }
        }
        #endregion

        #region 是否自动派工 IsAutoDispatch
        /// <summary>
        /// 是否自动派工
        /// </summary>
        [Label("是否自动派工")]
        public static readonly Property<bool?> IsAutoDispatchProperty = P<ProcessPty>.Register(e => e.IsAutoDispatch);

        /// <summary>
        /// 是否自动派工
        /// </summary>
        public bool? IsAutoDispatch
        {
            get { return this.GetProperty(IsAutoDispatchProperty); }
            set { this.SetProperty(IsAutoDispatchProperty, value); }
        }
        #endregion

        #region 报工校验 IsReportValid
        /// <summary>
        /// 报工校验
        /// </summary>
        [Label("报工校验")]
        public static readonly Property<bool?> IsReportValidProperty = P<ProcessPty>.Register(e => e.IsReportValid);

        /// <summary>
        /// 报工校验
        /// </summary>
        public bool? IsReportValid
        {
            get { return this.GetProperty(IsReportValidProperty); }
            set { this.SetProperty(IsReportValidProperty, value); }
        }
        #endregion

    }
    /// <summary>
    /// 工序属性实体配置
    /// </summary>
    public class AndonConfig : EntityConfig<ProcessPty>
    {
        protected override void AddValidations(IValidationDeclarer rules)
        {
            rules.AddRule(new NotDuplicateRule()
            {
                Properties =
                {
                     ProcessPty.ProcessIdProperty,
                     ProcessPty.KzCategoryIdProperty
                },
                MessageBuilder = (e) =>
                {
                    return "数据已存在!".L10N();
                }
            });
            base.AddValidations(rules);
        }
        /// <summary>
        /// 元数据配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("PROCESS_PTY").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
