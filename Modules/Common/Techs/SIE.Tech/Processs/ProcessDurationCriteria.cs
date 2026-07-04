using SIE.Domain;
using SIE.Items;
using SIE.ObjectModel;
using System;

namespace SIE.Tech.Processs
{
    /// <summary>
    /// 采购订单查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("采购订单查询")]    
    public class ProcessDurationCriteria : Criteria
    {
        #region 产品 Product
        /// <summary>
        /// 产品Id
        /// </summary>
        [Label("产品")]
        public static readonly IRefIdProperty ProductIdProperty =
            P<ProcessDurationCriteria>.RegisterRefId(e => e.ProductId, ReferenceType.Normal);

        /// <summary>
        /// 产品Id
        /// </summary>
        public double? ProductId
        {
            get { return (double?)this.GetRefNullableId(ProductIdProperty); }
            set { this.SetRefNullableId(ProductIdProperty, value); }
        }

        /// <summary>
        /// 产品
        /// </summary>
        public static readonly RefEntityProperty<Item> ProductProperty =
            P<ProcessDurationCriteria>.RegisterRef(e => e.Product, ProductIdProperty);

        /// <summary>
        /// 产品
        /// </summary>
        public Item Product
        {
            get { return this.GetRefEntity(ProductProperty); }
            set { this.SetRefEntity(ProductProperty, value); }
        }
        #endregion

        #region 工序 Process
        /// <summary>
        /// 注释Id
        /// </summary>
        [Label("工序")]
        public static readonly IRefIdProperty ProcessIdProperty =
            P<ProcessDurationCriteria>.RegisterRefId(e => e.ProcessId, ReferenceType.Normal);

        /// <summary>
        /// 注释Id
        /// </summary>
        public double? ProcessId
        {
            get { return (double?)this.GetRefNullableId(ProcessIdProperty); }
            set { this.SetRefNullableId(ProcessIdProperty, value); }
        }

        /// <summary>
        /// 注释
        /// </summary>
        public static readonly RefEntityProperty<Process> ProcessProperty =
            P<ProcessDurationCriteria>.RegisterRef(e => e.Process, ProcessIdProperty);

        /// <summary>
        /// 注释
        /// </summary>
        public Process Process
        {
            get { return this.GetRefEntity(ProcessProperty); }
            set { this.SetRefEntity(ProcessProperty, value); }
        }
        #endregion

        #region 产品名称 ProductName
        /// <summary>
        /// 产品名称
        /// </summary>
        [Label("产品名称")]
        public static readonly Property<string> ProductNameProperty = P<ProcessDurationCriteria>.Register(e => e.ProductName);

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName
        {
            get { return this.GetProperty(ProductNameProperty); }
            set { this.SetProperty(ProductNameProperty, value); }
        }
        #endregion

        #region 工序名称 ProcessName
        /// <summary>
        /// 工序名称
        /// </summary>
        [Label("工序名称")]
        public static readonly Property<string> ProcessNameProperty = P<ProcessDurationCriteria>.Register(e => e.ProcessName);

        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcessName
        {
            get { return this.GetProperty(ProcessNameProperty); }
            set { this.SetProperty(ProcessNameProperty, value); }
        }
        #endregion


        /// <summary>
        /// 查询
        /// </summary>
        /// <returns></returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<ProcessDurationController>().QueryDatas(this);
        }
    }
}