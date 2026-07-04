using SIE.Domain;
using SIE.ESop.EngDocuments.Enums;
using SIE.ESop.EngDocuments.Services;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.ESop.EngDocuments
{
    /// <summary>
    /// 工程文件维护查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("工程文件维护查询实体")]
    public class EngDocCriteria : Criteria
    {
        #region 产品编码 ProductCode
        /// <summary>
        /// 产品编码
        /// </summary>
        [Label("产品编码")]
        public static readonly Property<string> ProductCodeProperty = P<EngDocCriteria>.Register(e => e.ProductCode);

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
        public static readonly Property<string> ProductNameProperty = P<EngDocCriteria>.Register(e => e.ProductName);

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName
        {
            get { return this.GetProperty(ProductNameProperty); }
            set { this.SetProperty(ProductNameProperty, value); }
        }
        #endregion

        #region 工单号 WoNo
        /// <summary>
        /// 工单号
        /// </summary>
        [Label("工单号")]
        public static readonly Property<string> WoNoProperty = P<EngDocCriteria>.Register(e => e.WoNo);

        /// <summary>
        /// 工单号
        /// </summary>
        public string WoNo
        {
            get { return this.GetProperty(WoNoProperty); }
            set { this.SetProperty(WoNoProperty, value); }
        }
        #endregion

        #region 类型 Type
        /// <summary>
        /// 类型
        /// </summary>
        [Label("类型")]
        public static readonly Property<EngDocType?> TypeProperty = P<EngDocCriteria>.Register(e => e.Type);

        /// <summary>
        /// 类型
        /// </summary>
        public EngDocType? Type
        {
            get { return this.GetProperty(TypeProperty); }
            set { this.SetProperty(TypeProperty, value); }
        }
        #endregion

        #region 文件编码 DocCode
        /// <summary>
        /// 文件编码
        /// </summary>
        [Label("文件编码")]
        public static readonly Property<string> DocCodeProperty = P<EngDocCriteria>.Register(e => e.DocCode);

        /// <summary>
        /// 文件编码
        /// </summary>
        public string DocCode
        {
            get { return this.GetProperty(DocCodeProperty); }
            set { this.SetProperty(DocCodeProperty, value); }
        }
        #endregion

        #region 文件名称 DocName
        /// <summary>
        /// 文件名称
        /// </summary>
        [Label("文件名称")]
        public static readonly Property<string> DocNameProperty = P<EngDocCriteria>.Register(e => e.DocName);

        /// <summary>
        /// 文件名称
        /// </summary>
        public string DocName
        {
            get { return this.GetProperty(DocNameProperty); }
            set { this.SetProperty(DocNameProperty, value); }
        }
        #endregion

        /// <summary>
        /// 查询
        /// </summary>
        /// <returns></returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<EngDocumentService>().EngDocCriteriaFetch(this);
        }
    }
}
