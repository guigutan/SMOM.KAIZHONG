using SIE.Domain;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.Engrave
{
    /// <summary>
    /// 刻码替换记录查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("刻码替换记录查询实体")]
    public class EngraveLabelSnReplaceLogCriteria : Criteria
    {
        #region 原刻码 OldSn
        /// <summary>
        /// 原刻码
        /// </summary>
        [Label("原刻码")]
        public static readonly Property<string> OldSnProperty = P<EngraveLabelSnReplaceLogCriteria>.Register(e => e.OldSn);

        /// <summary>
        /// 原刻码
        /// </summary>
        public string OldSn
        {
            get { return this.GetProperty(OldSnProperty); }
            set { this.SetProperty(OldSnProperty, value); }
        }
        #endregion

        #region 新刻码 NewSn
        /// <summary>
        /// 新刻码
        /// </summary>
        [Label("新刻码")]
        public static readonly Property<string> NewSnProperty = P<EngraveLabelSnReplaceLogCriteria>.Register(e => e.NewSn);

        /// <summary>
        /// 新刻码
        /// </summary>
        public string NewSn
        {
            get { return this.GetProperty(NewSnProperty); }
            set { this.SetProperty(NewSnProperty, value); }
        }
        #endregion

        #region 产品编码 ProductCode
        /// <summary>
        /// 产品编码
        /// </summary>
        [Label("产品编码")]
        public static readonly Property<string> ProductCodeProperty = P<EngraveLabelSnReplaceLogCriteria>.Register(e => e.ProductCode);

        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProductCode
        {
            get { return this.GetProperty(ProductCodeProperty); }
            set { this.SetProperty(ProductCodeProperty, value); }
        }
        #endregion

        #region 产品名称 ProductName
        /// <summary>
        /// 产品名称
        /// </summary>
        [Label("产品名称")]
        public static readonly Property<string> ProductNameProperty = P<EngraveLabelSnReplaceLogCriteria>.Register(e => e.ProductName);

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName
        {
            get { return this.GetProperty(ProductNameProperty); }
            set { this.SetProperty(ProductNameProperty, value); }
        }
        #endregion

        #region 旧料号 ShortDescription
        /// <summary>
        /// 旧料号
        /// </summary>
        [Label("旧料号")]
        public static readonly Property<string> ShortDescriptionProperty = P<EngraveLabelSnReplaceLogCriteria>.Register(e => e.ShortDescription);

        /// <summary>
        /// 旧料号
        /// </summary>
        public string ShortDescription
        {
            get { return this.GetProperty(ShortDescriptionProperty); }
            set { this.SetProperty(ShortDescriptionProperty, value); }
        }
        #endregion

        #region 标签号 BatchNo
        /// <summary>
        /// 标签号
        /// </summary>
        [Label("标签号")]
        public static readonly Property<string> BatchNoProperty = P<EngraveLabelSnReplaceLogCriteria>.Register(e => e.BatchNo);

        /// <summary>
        /// 标签号
        /// </summary>
        public string BatchNo
        {
            get { return this.GetProperty(BatchNoProperty); }
            set { this.SetProperty(BatchNoProperty, value); }
        }
        #endregion

        #region 蓝标 BlueLabel
        /// <summary>
        /// 蓝标
        /// </summary>
        [Label("蓝标")]
        public static readonly Property<string> BlueLabelProperty = P<EngraveLabelSnReplaceLogCriteria>.Register(e => e.BlueLabel);

        /// <summary>
        /// 蓝标
        /// </summary>
        public string BlueLabel
        {
            get { return this.GetProperty(BlueLabelProperty); }
            set { this.SetProperty(BlueLabelProperty, value); }
        }
        #endregion

        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<EngraveLabelController>().CriteriaEngraveLabelSnReplaceLog(this);
        }
    }
}
