using SIE.Domain;
using SIE.Items;
using SIE.ObjectModel;
using SIE.Tech.Processs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.BarcodeProcesses
{
    /// <summary>
    /// 单体工序查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("单体工序查询实体")]
    public class SingleProcessCriteria : Criteria
    {
        #region 编码 Code
        /// <summary>
        /// 编码
        /// </summary>
        [Label("编码")]
        public static readonly Property<string> CodeProperty = P<SingleProcessCriteria>.Register(e => e.Code);

        /// <summary>
        /// 编码
        /// </summary>
        public string Code
        {
            get { return this.GetProperty(CodeProperty); }
            set { this.SetProperty(CodeProperty, value); }
        }
        #endregion

        #region 名称 Name
        /// <summary>
        /// 名称
        /// </summary>
        [Label("名称")]
        public static readonly Property<string> NameProperty = P<SingleProcessCriteria>.Register(e => e.Name);

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return this.GetProperty(NameProperty); }
            set { this.SetProperty(NameProperty, value); }
        }
        #endregion

        #region 工序类型 ProcessType
        /// <summary>
        /// 工序类型
        /// </summary>
        [Label("工序类型")]
        public static readonly Property<ProcessType?> ProcessTypeProperty = P<SingleProcessCriteria>.Register(e => e.ProcessType);

        /// <summary>
        /// 工序类型
        /// </summary>
        public ProcessType? ProcessType
        {
            get { return this.GetProperty(ProcessTypeProperty); }
            set { this.SetProperty(ProcessTypeProperty, value); }
        }
        #endregion

        #region 产品族 ProductFamily
        /// <summary>
        /// 产品族Id
        /// </summary>
        [Label("产品族")]
        public static readonly IRefIdProperty ProductFamilyIdProperty =
            P<SingleProcessCriteria>.RegisterRefId(e => e.ProductFamilyId, ReferenceType.Normal);

        /// <summary>
        /// 产品族Id
        /// </summary>
        public double? ProductFamilyId
        {
            get { return (double?)this.GetRefNullableId(ProductFamilyIdProperty); }
            set { this.SetRefNullableId(ProductFamilyIdProperty, value); }
        }

        /// <summary>
        /// 产品族
        /// </summary>
        public static readonly RefEntityProperty<ProductFamily> ProductFamilyProperty =
            P<SingleProcessCriteria>.RegisterRef(e => e.ProductFamily, ProductFamilyIdProperty);

        /// <summary>
        /// 产品族
        /// </summary>
        public ProductFamily ProductFamily
        {
            get { return this.GetRefEntity(ProductFamilyProperty); }
            set { this.SetRefEntity(ProductFamilyProperty, value); }
        }
        #endregion

        /// <summary>
        /// 查询
        /// </summary>
        /// <returns></returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<BarcodeProcessController>().QuerySingleProcess(this);
        }
    }
}
