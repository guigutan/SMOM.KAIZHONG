using SIE.Domain;
using SIE.MES.QTimes.Services;
using SIE.MES.TaskManagement.SuspectProductLabels;
using SIE.MES.TaskManagement.SuspectProductLabels.ViewModels;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.TaskManagement.SuspectProductLabels
{
    /// <summary>
    /// 报废明细查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("报废明细")]
    public class ScrapDetailCriteria:Criteria
    {
        #region 可疑品标签 BatchNo
        /// <summary>
        /// 可疑品标签
        /// </summary>
        [Label("可疑品标签")]
        public static readonly Property<string> BatchNoProperty = P<ScrapDetailCriteria>.Register(e => e.BatchNo);

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
        public static readonly Property<string> SubBatchNoProperty = P<ScrapDetailCriteria>.Register(e => e.SubBatchNo);

        /// <summary>
        /// 可疑品子标签
        /// </summary>
        public string SubBatchNo
        {
            get { return GetProperty(SubBatchNoProperty); }
            set { SetProperty(SubBatchNoProperty, value); }
        }
        #endregion

        #region 物料名称 ProductName
        /// <summary>
        /// 物料名称
        /// </summary>
        [Label("物料名称")]
        public static readonly Property<string> ProductNameProperty = P<ScrapDetailCriteria>.Register(e => e.ProductName);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ProductName
        {
            get { return GetProperty(ProductNameProperty); }
            set { SetProperty(ProductNameProperty, value); }
        }
        #endregion

        #region 资源名称 LineType
        /// <summary>
        /// 资源名称
        /// </summary>
        [Label("资源名称")]
        public static readonly Property<string> LineTypeProperty = P<ScrapDetailCriteria>.Register(e => e.LineType);

        /// <summary>
        /// 资源名称
        /// </summary>
        public string LineType
        {
            get { return GetProperty(LineTypeProperty); }
            set { SetProperty(LineTypeProperty, value); }
        }
        #endregion

        #region 工序名称 ProcessName
        /// <summary>
        /// 工序名称
        /// </summary>
        [Label("工序名称")]
        public static readonly Property<string> ProcessNameProperty = P<ScrapDetailCriteria>.Register(e => e.ProcessName);

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
        public static readonly Property<ClassesScrapType?> ClassTypeProperty = P<ScrapDetailCriteria>.Register(e => e.ClassType);

        /// <summary>
        /// 班别
        /// </summary>
        public ClassesScrapType? ClassType
        {
            get { return GetProperty(ClassTypeProperty); }
            set { SetProperty(ClassTypeProperty, value); }
        }
        #endregion

        #region 物料编码 ItemName
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("物料编码")]
        public static readonly Property<string> ItemNameProperty = P<ScrapDetailCriteria>.Register(e => e.ItemName);

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemName
        {
            get { return GetProperty(ItemNameProperty); }
            set { SetProperty(ItemNameProperty, value); }
        }
        #endregion

        #region 站点 ItemType
        /// <summary>
        /// 站点
        /// </summary>
        [Label("站点")]
        public static readonly Property<string> ItemTypeProperty = P<ScrapDetailCriteria>.Register(e => e.ItemType);

        /// <summary>
        /// 站点
        /// </summary>
        public string ItemType
        {
            get { return GetProperty(ItemTypeProperty); }
            set { SetProperty(ItemTypeProperty, value); }
        }
        #endregion

        #region 不良代码 BadCode
        /// <summary>
        /// 不良代码
        /// </summary>
        [Label("不良代码")]
        public static readonly Property<string> BadCodeProperty = P<ScrapDetailCriteria>.Register(e => e.BadCode);

        /// <summary>
        /// 不良代码
        /// </summary>
        public string BadCode
        {
            get { return GetProperty(BadCodeProperty); }
            set { SetProperty(BadCodeProperty, value); }
        }
        #endregion

        #region 日期 ScrapDate
        /// <summary>
        /// 日期
        /// </summary>
        [Label("日期")]
        public static readonly Property<DateRange> ScrapDateProperty = P<ScrapDetailCriteria>.Register(e => e.ScrapDate);

        /// <summary>
        /// 日期
        /// </summary>
        public DateRange ScrapDate
        {
            get { return GetProperty(ScrapDateProperty); }
            set { SetProperty(ScrapDateProperty, value); }
        }
        #endregion

        #region 处理人 HandleName
        /// <summary>
        /// 处理人
        /// </summary>
        [Label("处理人")]
        public static readonly Property<string> HandleNameProperty = P<ScrapDetailCriteria>.Register(e => e.HandleName);

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
        public static readonly Property<DateRange> HandleDateProperty = P<ScrapDetailCriteria>.Register(e => e.HandleDate);

        /// <summary>
        /// 处理时间
        /// </summary>
        public DateRange HandleDate
        {
            get { return GetProperty(HandleDateProperty); }
            set { SetProperty(HandleDateProperty, value); }
        }
        #endregion

        #region Mrp控制者 MrpController
        /// <summary>
        /// Mrp控制者
        /// </summary>
        [Label("Mrp控制者")]
        public static readonly Property<string> MrpControllerProperty = P<ScrapDetailCriteria>.Register(e => e.MrpController);

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
        public static readonly Property<string> ShortDescriptionProperty = P<ScrapDetailCriteria>.Register(e => e.ShortDescription);

        /// <summary>
        /// 旧物料号
        /// </summary>
        public string ShortDescription
        {
            get { return this.GetProperty(ShortDescriptionProperty); }
            set { this.SetProperty(ShortDescriptionProperty, value); }
        }
        #endregion


        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<SuspectProductLabelController>().GetScrapDetail(this);
        }
    }

    public enum ClassesScrapType
    {
        /// <summary>
        /// 白班
        /// </summary>
        [Label("白班")]
        Day = 1,

        /// <summary>
        /// 晚班
        /// </summary>
        [Label("晚班")]
        Night = 2,

        /// <summary>
        /// 空白
        /// </summary>
        [Label("空白")]
        kb = 3

    }
}
