using SIE.Domain;
using SIE.Items;
using SIE.MES.Fixture;
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
    /// 工序属性维护查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("工序属性维护查询实体")]
    public class ProcessPtyCriterial:Criteria
    {
        #region 产品名称 ProductLine
        /// <summary>
        /// 产品名称
        /// </summary>
        [Required]
        [Label("产品名称")]
        public static readonly Property<string> ProductLineProperty = P<ProcessPtyCriterial>.Register(e => e.ProductLine);

        /// <summary>
        /// 产品名称
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
        [Required]
        [Label("产品类型")]
        public static readonly Property<string> ProductTypeProperty = P<ProcessPtyCriterial>.Register(e => e.ProductType);

        /// <summary>
        /// 产品类型
        /// </summary>
        public string ProductType
        {
            get { return this.GetProperty(ProductTypeProperty); }
            set { this.SetProperty(ProductTypeProperty, value); }
        }
        #endregion

        #region 工序名称 Process
        /// <summary>
        /// 工序名称Id
        /// </summary>
        [Label("工序名称")]
        public static readonly IRefIdProperty ProcessIdProperty = P<ProcessPtyCriterial>.RegisterRefId(e => e.ProcessId, ReferenceType.Normal);

        /// <summary>
        /// 工序名称Id
        /// </summary>
        public double? ProcessId
        {
            get { return (double?)GetRefNullableId(ProcessIdProperty); }
            set { SetRefNullableId(ProcessIdProperty, value); }
        }

        /// <summary>
        /// 工序名称
        /// </summary>
        public static readonly RefEntityProperty<Process> ProcessProperty = P<ProcessPtyCriterial>.RegisterRef(e => e.Process, ProcessIdProperty);

        /// <summary>
        /// 工序名称
        /// </summary>
        public Process Process
        {
            get { return GetRefEntity(ProcessProperty); }
            set { SetRefEntity(ProcessProperty, value); }
        }
        #endregion

        #region 工序编码 ProcessCode
        /// <summary>
        /// 工序编码
        /// </summary>
        [Label("工序编码")]
        public static readonly Property<string> ProcessCodeProperty = P<ProcessPtyCriterial>.Register(e => e.ProcessCode);

        /// <summary>
        /// 工序编码
        /// </summary>
        public string ProcessCode
        {
            get { return this.GetProperty(ProcessCodeProperty); }
            set { this.SetProperty(ProcessCodeProperty, value); }
        }
        #endregion


        #region 类型 Type
        /// <summary>
        /// 类型
        /// </summary>
        [Label("类型")]
        public static readonly Property<ItemType?> TypeProperty = P<ProcessPtyCriterial>.Register(e => e.Type);

        /// <summary>
        /// 类型
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
        public static readonly Property<ProcessState?> SchedulingProperty = P<ProcessPtyCriterial>.Register(e => e.Scheduling);

        /// <summary>
        /// 排程点
        /// </summary>
        public ProcessState? Scheduling
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
        public static readonly Property<ProcessState?> DispatchWorkProperty = P<ProcessPtyCriterial>.Register(e => e.DispatchWork);

        /// <summary>
        /// 派工点
        /// </summary>
        public ProcessState? DispatchWork
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
        public static readonly Property<ProcessState?> IsPrepareProperty = P<ProcessPtyCriterial>.Register(e => e.IsPrepare);

        /// <summary>
        /// 产前准备
        /// </summary>
        public ProcessState? IsPrepare
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
        public static readonly Property<ProcessState?> IsTransferProperty = P<ProcessPtyCriterial>.Register(e => e.IsTransfer);

        /// <summary>
        /// 是否转入
        /// </summary>
        public ProcessState? IsTransfer
        {
            get { return this.GetProperty(IsTransferProperty); }
            set { this.SetProperty(IsTransferProperty, value); }
        }
        #endregion

        /// <summary>
        /// 查询方法
        /// </summary>
        /// <returns></returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<ProcessPtyController>().CriterialProcessPty(this);
        }
    }

    public enum ProcessState
    {
        /// <summary>
        /// 空白
        /// </summary>
        [Label("")]
        k = 10,
        /// <summary>
        /// 是
        /// </summary>
        [Label("是")]
        s = 20,
        /// <summary>
        /// 否
        /// </summary>
        [Label("否")]
        f = 30,
    }
}
