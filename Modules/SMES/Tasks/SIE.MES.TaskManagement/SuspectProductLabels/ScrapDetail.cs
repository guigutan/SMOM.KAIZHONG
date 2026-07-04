using SIE.Defects;
using SIE.Domain;
using SIE.Items;
using SIE.MES.TaskManagement.Dispatchs;
using SIE.MES.TaskManagement.SuspectProductLabels;
using SIE.MES.WorkOrders;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources;
using SIE.Resources.WipResources;
using SIE.Tech.Processs;
using System;

namespace SIE.MES.TaskManagement.SuspectProductLabels
{
    /// <summary>
    /// 报废明细查询实体 - 视图实体
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(ScrapDetailCriteria))]
    [Label("报废明细查询实体")]
    public class ScrapDetail : DataEntity  // 改为继承DataEntity
    {
        #region 可疑品标签 BatchNo
        /// <summary>
        /// 可疑品标签
        /// </summary>
        [Label("可疑品标签")]
        public static readonly Property<string> BatchNoProperty = P<ScrapDetail>.Register(e => e.BatchNo);

        /// <summary>
        /// 可疑品标签
        /// </summary>
        public string BatchNo
        {
            get { return GetProperty(BatchNoProperty); }
            set { SetProperty(BatchNoProperty, value); }
        }
        #endregion

        #region 可疑品子标签 SubBatchNo
        /// <summary>
        /// 可疑品子标签
        /// </summary>
        [Label("可疑品子标签")]
        public static readonly Property<string> SubBatchNoProperty = P<ScrapDetail>.Register(e => e.SubBatchNo);

        /// <summary>
        /// 可疑品子标签
        /// </summary>
        public string SubBatchNo
        {
            get { return GetProperty(SubBatchNoProperty); }
            set { SetProperty(SubBatchNoProperty, value); }
        }
        #endregion

        #region 资源编码 WipCode
        /// <summary>
        /// 资源编码
        /// </summary>
        [Label("资源编码")]
        public static readonly Property<string> WipCodeProperty = P<ScrapDetail>.Register(e => e.WipCode);

        /// <summary>
        /// 资源编码
        /// </summary>
        public string WipCode
        {
            get { return GetProperty(WipCodeProperty); }
            set { SetProperty(WipCodeProperty, value); }
        }
        #endregion

        #region 资源名称 WipName
        /// <summary>
        /// 资源名称
        /// </summary>
        [Label("资源名称")]
        public static readonly Property<string> WipNameProperty = P<ScrapDetail>.Register(e => e.WipName);

        /// <summary>
        /// 资源名称
        /// </summary>
        public string WipName
        {
            get { return GetProperty(WipNameProperty); }
            set { SetProperty(WipNameProperty, value); }
        }
        #endregion

        #region 物料编码 ItemCode
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("物料编码")]
        public static readonly Property<string> ItemCodeProperty = P<ScrapDetail>.Register(e => e.ItemCode);

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode
        {
            get { return GetProperty(ItemCodeProperty); }
            set { SetProperty(ItemCodeProperty, value); }
        }
        #endregion

        #region 物料名称 ItemName
        /// <summary>
        /// 物料名称
        /// </summary>
        [Label("物料名称")]
        public static readonly Property<string> ItemNameProperty = P<ScrapDetail>.Register(e => e.ItemName);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName
        {
            get { return GetProperty(ItemNameProperty); }
            set { SetProperty(ItemNameProperty, value); }
        }
        #endregion

        #region 基本分类 ItemType
        /// <summary>
        /// 基本分类
        /// </summary>
        [Label("基本分类")]
        public static readonly Property<string> ItemTypeProperty = P<ScrapDetail>.Register(e => e.ItemType);

        /// <summary>
        /// 基本分类
        /// </summary>
        public string ItemType
        {
            get { return GetProperty(ItemTypeProperty); }
            set { SetProperty(ItemTypeProperty, value); }
        }
        #endregion

        #region 物料类型 Mtart
        /// <summary>
        /// 物料类型
        /// </summary>
        [Label("物料类型")]
        public static readonly Property<string> MtartProperty = P<ScrapDetail>.Register(e => e.Mtart);

        /// <summary>
        /// 物料类型
        /// </summary>
        public string Mtart
        {
            get { return this.GetProperty(MtartProperty); }
            set { this.SetProperty(MtartProperty, value); }
        }
        #endregion

        #region Mrp控制者 MrpController
        /// <summary>
        /// Mrp控制者
        /// </summary>
        [Label("Mrp控制者")]
        public static readonly Property<string> MrpControllerProperty = P<ScrapDetail>.Register(e => e.MrpController);

        /// <summary>
        /// Mrp控制者
        /// </summary>
        public string MrpController
        {
            get { return GetProperty(MrpControllerProperty); }
            set { SetProperty(MrpControllerProperty, value); }
        }
        #endregion

        #region 旧物料号 ShortDescription
        /// <summary>
        /// 旧物料号
        /// </summary>
        [Label("旧物料号")]
        public static readonly Property<string> ShortDescriptionProperty = P<ScrapDetail>.Register(e => e.ShortDescription);

        /// <summary>
        /// 旧物料号
        /// </summary>
        public string ShortDescription
        {
            get { return this.GetProperty(ShortDescriptionProperty); }
            set { this.SetProperty(ShortDescriptionProperty, value); }
        }
        #endregion
/*
        #region 处理时间 HandleDate
        /// <summary>
        /// 处理时间
        /// </summary>
        [Label("处理时间")]
        public static readonly Property<string> HandleDateProperty = P<ScrapDetail>.Register(e => e.HandleDate);

        /// <summary>
        /// 处理时间
        /// </summary>
        public string HandleDate
        {
            get { return GetProperty(HandleDateProperty); }
            set { SetProperty(HandleDateProperty, value); }
        }
        #endregion*/

        #region 数量 ScrapNum
        /// <summary>
        /// 数量
        /// </summary>
        [Label("数量")]
        public static readonly Property<string> ScrapNumProperty = P<ScrapDetail>.Register(e => e.ScrapNum);

        /// <summary>
        /// 数量
        /// </summary>
        public string ScrapNum
        {
            get { return GetProperty(ScrapNumProperty); }
            set { SetProperty(ScrapNumProperty, value); }
        }
        #endregion

        #region 创建人 CreateName
        /// <summary>
        /// 创建人
        /// </summary>
        [Label("创建人")]
        public static readonly Property<string> CreateNameProperty = P<ScrapDetail>.Register(e => e.CreateName);

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreateName
        {
            get { return GetProperty(CreateNameProperty); }
            set { SetProperty(CreateNameProperty, value); }
        }
        #endregion

        #region 处理人 HandleName
        /// <summary>
        /// 处理人
        /// </summary>
        [Label("处理人")]
        public static readonly Property<string> HandleNameProperty = P<ScrapDetail>.Register(e => e.HandleName);

        /// <summary>
        /// 处理人
        /// </summary>
        public string HandleName
        {
            get { return GetProperty(HandleNameProperty); }
            set { SetProperty(HandleNameProperty, value); }
        }
        #endregion

        #region 不良代码 DefectCode
        /// <summary>
        /// 不良代码
        /// </summary>
        [Label("不良代码")]
        public static readonly Property<string> DefectCodeProperty = P<ScrapDetail>.Register(e => e.DefectCode);

        /// <summary>
        /// 不良代码
        /// </summary>
        public string DefectCode
        {
            get { return GetProperty(DefectCodeProperty); }
            set { SetProperty(DefectCodeProperty, value); }
        }
        #endregion

        #region 不良现象 DefectDes
        /// <summary>
        /// 不良现象
        /// </summary>
        [Label("不良现象")]
        public static readonly Property<string> DefectDesProperty = P<ScrapDetail>.Register(e => e.DefectDes);

        /// <summary>
        /// 不良现象
        /// </summary>
        public string DefectDes
        {
            get { return GetProperty(DefectDesProperty); }
            set { SetProperty(DefectDesProperty, value); }
        }
        #endregion

        #region 班别 ClassName
        /// <summary>
        /// 班别
        /// </summary>
        [Label("班别")]
        public static readonly Property<string> ClassNameProperty = P<ScrapDetail>.Register(e => e.ClassName);

        /// <summary>
        /// 班别
        /// </summary>
        public string ClassName
        {
            get { return GetProperty(ClassNameProperty); }
            set { SetProperty(ClassNameProperty, value); }
        }
        #endregion

        #region 基本计量单位 UnitCode
        /// <summary>
        /// 基本计量单位
        /// </summary>
        [Label("基本计量单位")]
        public static readonly Property<string> UnitCodeProperty = P<ScrapDetail>.Register(e => e.UnitCode);

        /// <summary>
        /// 基本计量单位
        /// </summary>
        public string UnitCode
        {
            get { return GetProperty(UnitCodeProperty); }
            set { SetProperty(UnitCodeProperty, value); }
        }
        #endregion

        #region 工序编码 ProcessCode
        /// <summary>
        /// 工序编码
        /// </summary>
        [Label("工序编码")]
        public static readonly Property<string> ProcessCodeProperty = P<ScrapDetail>.Register(e => e.ProcessCode);

        /// <summary>
        /// 工序编码
        /// </summary>
        public string ProcessCode
        {
            get { return GetProperty(ProcessCodeProperty); }
            set { SetProperty(ProcessCodeProperty, value); }
        }
        #endregion

        #region 工序名称 ProcessName
        /// <summary>
        /// 工序名称
        /// </summary>
        [Label("工序名称")]
        public static readonly Property<string> ProcessNameProperty = P<ScrapDetail>.Register(e => e.ProcessName);

        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcessName
        {
            get { return GetProperty(ProcessNameProperty); }
            set { SetProperty(ProcessNameProperty, value); }
        }
        #endregion
        /*
                #region 创建时间 ScrapDate
                /// <summary>
                /// 创建时间
                /// </summary>
                [Label("创建时间")]
                public static readonly Property<string> ScrapDateProperty = P<ScrapDetail>.Register(e => e.ScrapDate);

                /// <summary>
                /// 创建时间
                /// </summary>
                public string ScrapDate
                {
                    get { return GetProperty(ScrapDateProperty); }
                    set { SetProperty(ScrapDateProperty, value); }
                }
                #endregion*/
        // 修改为DateTime?类型
        #region 处理时间 HandleDate
        [Label("处理时间")]
        public static readonly Property<DateTime?> HandleDateProperty = P<ScrapDetail>.Register(e => e.HandleDate);
        public DateTime? HandleDate
        {
            get { return GetProperty(HandleDateProperty); }
            set { SetProperty(HandleDateProperty, value); }
        }
        #endregion

        #region 创建时间 ScrapDate
        [Label("创建时间")]
        public static readonly Property<DateTime?> ScrapDateProperty = P<ScrapDetail>.Register(e => e.ScrapDate);
        public DateTime? ScrapDate
        {
            get { return GetProperty(ScrapDateProperty); }
            set { SetProperty(ScrapDateProperty, value); }
        }
        #endregion
        // 移除所有的RefIdProperty和RefEntityProperty，因为视图实体不需要这些
        // 只需要保留上面这些简单的属性
    }

    /// <summary>
    /// 报废明细查询实体 实体配置
    /// </summary>
    internal class ScrapDetailEntityConfig : EntityConfig<ScrapDetail>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            // 映射到视图而不是表
            Meta.MapTable("VW_SCRAP_DETAIL").MapAllProperties();

            // 或者使用SQL视图定义（如果使用SQL视图）
            // Meta.IsView = true;
            // Meta.ViewSql = @"SELECT ... FROM ..."; // 在这里定义视图的SQL
            Meta.EnableSort();
            /*Meta.EnablePhantoms();*/
            // 禁用幽灵记录功能
           // Meta.DisablePhantoms();
        }
    }
}