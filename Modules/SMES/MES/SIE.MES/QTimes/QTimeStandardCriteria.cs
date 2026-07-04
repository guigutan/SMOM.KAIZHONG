using SIE.Domain;
using SIE.Items;
using SIE.MES.QTimes.Services;
using SIE.ObjectModel;
using SIE.Resources.Enterprises;
using SIE.Resources.WipResources;
using SIE.Tech.Processs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.QTimes
{
    /// <summary>
    /// QTime标准维护
    /// </summary>
    [QueryEntity, Serializable]
    [Label("QTime标准维护查询实体")]
    public class QTimeStandardCriteria : Criteria
    {
        #region 产品编码 ProductCode
        /// <summary>
        /// 产品编码
        /// </summary>
        [Label("产品编码")]
        public static readonly Property<string> ProductCodeProperty = P<QTimeStandardCriteria>.Register(e => e.ProductCode);

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
        public static readonly Property<string> ProductNameProperty = P<QTimeStandardCriteria>.Register(e => e.ProductName);

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName
        {
            get { return this.GetProperty(ProductNameProperty); }
            set { this.SetProperty(ProductNameProperty, value); }
        }
        #endregion

        #region 产线 WipResource
        /// <summary>
        /// 产线Id
        /// </summary>
        [Label("产线")]
        public static readonly IRefIdProperty WipResourceIdProperty =
            P<QTimeStandardCriteria>.RegisterRefId(e => e.WipResourceId, ReferenceType.Normal);

        /// <summary>
        /// 产线Id
        /// </summary>
        public double? WipResourceId
        {
            get { return (double?)this.GetRefNullableId(WipResourceIdProperty); }
            set { this.SetRefNullableId(WipResourceIdProperty, value); }
        }

        /// <summary>
        /// 产线
        /// </summary>
        public static readonly RefEntityProperty<WipResource> WipResourceProperty =
            P<QTimeStandardCriteria>.RegisterRef(e => e.WipResource, WipResourceIdProperty);

        /// <summary>
        /// 产线
        /// </summary>
        public WipResource WipResource
        {
            get { return this.GetRefEntity(WipResourceProperty); }
            set { this.SetRefEntity(WipResourceProperty, value); }
        }
        #endregion

        #region 开始工序 StartProcess
        /// <summary>
        /// 开始工序Id
        /// </summary>
        [Label("开始工序")]
        public static readonly IRefIdProperty StartProcessIdProperty =
            P<QTimeStandardCriteria>.RegisterRefId(e => e.StartProcessId, ReferenceType.Normal);

        /// <summary>
        /// 开始工序Id
        /// </summary>
        public double? StartProcessId
        {
            get { return (double?)this.GetRefNullableId(StartProcessIdProperty); }
            set { this.SetRefNullableId(StartProcessIdProperty, value); }
        }

        /// <summary>
        /// 开始工序
        /// </summary>
        public static readonly RefEntityProperty<Process> StartProcessProperty =
            P<QTimeStandardCriteria>.RegisterRef(e => e.StartProcess, StartProcessIdProperty);

        /// <summary>
        /// 开始工序
        /// </summary>
        public Process StartProcess
        {
            get { return this.GetRefEntity(StartProcessProperty); }
            set { this.SetRefEntity(StartProcessProperty, value); }
        }
        #endregion

        #region 结束工序 EndProcess
        /// <summary>
        /// 结束工序Id
        /// </summary>
        [Label("结束工序")]
        public static readonly IRefIdProperty EndProcessIdProperty =
            P<QTimeStandardCriteria>.RegisterRefId(e => e.EndProcessId, ReferenceType.Normal);

        /// <summary>
        /// 结束工序Id
        /// </summary>
        public double? EndProcessId
        {
            get { return (double?)this.GetRefNullableId(EndProcessIdProperty); }
            set { this.SetRefNullableId(EndProcessIdProperty, value); }
        }

        /// <summary>
        /// 结束工序
        /// </summary>
        public static readonly RefEntityProperty<Process> EndProcessProperty =
            P<QTimeStandardCriteria>.RegisterRef(e => e.EndProcess, EndProcessIdProperty);

        /// <summary>
        /// 结束工序
        /// </summary>
        public Process EndProcess
        {
            get { return this.GetRefEntity(EndProcessProperty); }
            set { this.SetRefEntity(EndProcessProperty, value); }
        }
        #endregion

        #region 是否预警 IsAlert
        /// <summary>
        /// 是否预警
        /// </summary>
        [Label("是否预警")]
        public static readonly Property<YesNo?> IsAlertProperty = P<QTimeStandardCriteria>.Register(e => e.IsAlert);

        /// <summary>
        /// 是否预警
        /// </summary>
        public YesNo? IsAlert
        {
            get { return this.GetProperty(IsAlertProperty); }
            set { this.SetProperty(IsAlertProperty, value); }
        }
        #endregion

        #region 是否启用 State
        /// <summary>
        /// 是否启用
        /// </summary>
        [Label("是否启用")]
        public static readonly Property<State?> StateProperty = P<QTimeStandardCriteria>.Register(e => e.State);

        /// <summary>
        /// 是否启用
        /// </summary>
        public State? State
        {
            get { return this.GetProperty(StateProperty); }
            set { this.SetProperty(StateProperty, value); }
        }
        #endregion

        /// <summary>
        /// 查询
        /// </summary>
        /// <returns></returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<QTimeStandardService>().QueryQTimeEntityList(this);
        }
    }
}
