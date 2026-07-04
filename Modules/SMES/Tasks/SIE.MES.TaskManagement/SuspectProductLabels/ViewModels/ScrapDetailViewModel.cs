using SIE.Domain;
using SIE.MES.QTimes.ViewModels;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.TaskManagement.SuspectProductLabels.ViewModels
{
    /// <summary>
    /// 报废明细查询实体
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(ScrapDetailCriteria))]
    [Label("报废明细查询实体")]
    public class ScrapDetailViewModel:ViewModel
    {
        #region 可疑品标签 BatchNo
        /// <summary>
        /// 可疑品标签
        /// </summary>
        [Label("可疑品标签")]
        public static readonly Property<string> BatchNoProperty = P<ScrapDetailViewModel>.Register(e => e.BatchNo);

        /// <summary>
        /// 可疑品标签
        /// </summary>
        public string BatchNo
        {
            get { return GetProperty(BatchNoProperty); }
            set { SetProperty(BatchNoProperty, value); }
        }
        #endregion

        #region 物料名称 ProductName
        /// <summary>
        /// 物料名称
        /// </summary>
        [Label("物料名称")]
        public static readonly Property<string> ProductNameProperty = P<ScrapDetailViewModel>.Register(e => e.ProductName);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ProductName
        {
            get { return GetProperty(ProductNameProperty); }
            set { SetProperty(ProductNameProperty, value); }
        }
        #endregion

        #region 资源编码 LineTypeCode
        /// <summary>
        /// 资源编码
        /// </summary>
        [Label("资源编码")]
        public static readonly Property<string> LineTypeCodeProperty = P<ScrapDetailViewModel>.Register(e => e.LineTypeCode);

        /// <summary>
        /// 资源编码
        /// </summary>
        public string LineTypeCode
        {
            get { return GetProperty(LineTypeCodeProperty); }
            set { SetProperty(LineTypeCodeProperty, value); }
        }
        #endregion

        #region 资源名称 LineTypeName
        /// <summary>
        /// 资源名称
        /// </summary>
        [Label("资源名称")]
        public static readonly Property<string> LineTypeNameProperty = P<ScrapDetailViewModel>.Register(e => e.LineTypeName);

        /// <summary>
        /// 资源名称
        /// </summary>
        public string LineTypeName
        {
            get { return GetProperty(LineTypeNameProperty); }
            set { SetProperty(LineTypeNameProperty, value); }
        }
        #endregion

        #region 工序编码 ProcessCode
        /// <summary>
        /// 工序编码
        /// </summary>
        [Label("工序编码")]
        public static readonly Property<string> ProcessCodeProperty = P<ScrapDetailViewModel>.Register(e => e.ProcessCode);

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
        public static readonly Property<string> ProcessNameProperty = P<ScrapDetailViewModel>.Register(e => e.ProcessName);

        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcessName
        {
            get { return GetProperty(ProcessNameProperty); }
            set { SetProperty(ProcessNameProperty, value); }
        }
        #endregion

        #region 班别 ClassType
        /// <summary>
        /// 班别
        /// </summary>
        [Label("班别")]
        public static readonly Property<string> ClassTypeProperty = P<ScrapDetailViewModel>.Register(e => e.ClassType);

        /// <summary>
        /// 班别
        /// </summary>
        public string ClassType
        {
            get { return GetProperty(ClassTypeProperty); }
            set { SetProperty(ClassTypeProperty, value); }
        }
        #endregion

        #region 物料编码 ItemCode
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("物料编码")]
        public static readonly Property<string> ItemCodeProperty = P<ScrapDetailViewModel>.Register(e => e.ItemCode);

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode
        {
            get { return GetProperty(ItemCodeProperty); }
            set { SetProperty(ItemCodeProperty, value); }
        }
        #endregion

        #region 基本计量单位 UnitCode
        /// <summary>
        /// 基本计量单位
        /// </summary>
        [Label("基本计量单位")]
        public static readonly Property<string> UnitCodeProperty = P<ScrapDetailViewModel>.Register(e => e.UnitCode);

        /// <summary>
        /// 基本计量单位
        /// </summary>
        public string UnitCode
        {
            get { return GetProperty(UnitCodeProperty); }
            set { SetProperty(UnitCodeProperty, value); }
        }
        #endregion

        #region 基本分类 ItemType
        /// <summary>
        /// 基本分类
        /// </summary>
        [Label("基本分类")]
        public static readonly Property<string> ItemTypeProperty = P<ScrapDetailViewModel>.Register(e => e.ItemType);

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
        public static readonly Property<string> MtartProperty = P<ScrapDetailViewModel>.Register(e => e.Mtart);

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
        public static readonly Property<string> MrpControllerProperty = P<ScrapDetailViewModel>.Register(e => e.MrpController);

        /// <summary>
        /// Mrp控制者
        /// </summary>
        public string MrpController
        {
            get { return GetProperty(MrpControllerProperty); }
            set { SetProperty(MrpControllerProperty, value); }
        }
        #endregion

        #region 不良代码 BadCode
        /// <summary>
        /// 不良代码
        /// </summary>
        [Label("不良代码")]
        public static readonly Property<string> BadCodeProperty = P<ScrapDetailViewModel>.Register(e => e.BadCode);

        /// <summary>
        /// 不良代码
        /// </summary>
        public string BadCode
        {
            get { return GetProperty(BadCodeProperty); }
            set { SetProperty(BadCodeProperty, value); }
        }
        #endregion

        #region 不良现象 BadName
        /// <summary>
        /// 不良现象
        /// </summary>
        [Label("不良现象")]
        public static readonly Property<string> BadNameProperty = P<ScrapDetailViewModel>.Register(e => e.BadName);

        /// <summary>
        /// 不良现象
        /// </summary>
        public string BadName
        {
            get { return GetProperty(BadNameProperty); }
            set { SetProperty(BadNameProperty, value); }
        }
        #endregion

        #region 数量 ScrapNum
        /// <summary>
        /// 数量
        /// </summary>
        [Label("数量")]
        public static readonly Property<string> ScrapNumProperty = P<ScrapDetailViewModel>.Register(e => e.ScrapNum);

        /// <summary>
        /// 数量
        /// </summary>
        public string ScrapNum
        {
            get { return GetProperty(ScrapNumProperty); }
            set { SetProperty(ScrapNumProperty, value); }
        }
        #endregion

        #region 创建时间 ScrapDate
        /// <summary>
        /// 创建时间
        /// </summary>
        [Label("创建时间")]
        public static readonly Property<string> ScrapDateProperty = P<ScrapDetailViewModel>.Register(e => e.ScrapDate);

        /// <summary>
        /// 创建时间
        /// </summary>
        public string ScrapDate
        {
            get { return GetProperty(ScrapDateProperty); }
            set { SetProperty(ScrapDateProperty, value); }
        }
        #endregion

        #region 创建人 CreateName
        /// <summary>
        /// 创建人
        /// </summary>
        [Label("创建人")]
        public static readonly Property<string> CreateNameProperty = P<ScrapDetailViewModel>.Register(e => e.CreateName);

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
        public static readonly Property<string> HandleNameProperty = P<ScrapDetailViewModel>.Register(e => e.HandleName);

        /// <summary>
        /// 处理人
        /// </summary>
        public string HandleName
        {
            get { return GetProperty(HandleNameProperty); }
            set { SetProperty(HandleNameProperty, value); }
        }
        #endregion

        #region 处理时间 HandleDate
        /// <summary>
        /// 处理时间
        /// </summary>
        [Label("处理时间")]
        public static readonly Property<string> HandleDateProperty = P<ScrapDetailViewModel>.Register(e => e.HandleDate);

        /// <summary>
        /// 处理时间
        /// </summary>
        public string HandleDate
        {
            get { return GetProperty(HandleDateProperty); }
            set { SetProperty(HandleDateProperty, value); }
        }
        #endregion
    }
}
