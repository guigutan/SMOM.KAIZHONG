using SIE.Domain;
using SIE.ObjectModel;
using SIE.Resources.WipResources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.TaskManagement.Dispatchs.ViewModels
{
    /// <summary>
    /// 
    /// </summary>
    [RootEntity, Serializable]
    public class DispatchTaskViewModel : ViewModel
    {
        #region 派工单Id TaskId
        /// <summary>
        /// 派工单Id
        /// </summary>
        [Label("派工单Id")]
        public static readonly Property<string> TaskIdProperty = P<DispatchTaskViewModel>.Register(e => e.TaskId);

        /// <summary>
        /// 派工单Id
        /// </summary>
        public string TaskId
        {
            get { return this.GetProperty(TaskIdProperty); }
            set { this.SetProperty(TaskIdProperty, value); }
        }
        #endregion

        #region 资源 WipResource
        /// <summary>
        /// 资源Id
        /// </summary>
        [Label("资源")]
        public static readonly IRefIdProperty WipResourceIdProperty =
            P<DispatchTaskViewModel>.RegisterRefId(e => e.WipResourceId, ReferenceType.Normal);

        /// <summary>
        /// 资源Id
        /// </summary>
        public double WipResourceId
        {
            get { return (double)this.GetRefId(WipResourceIdProperty); }
            set { this.SetRefId(WipResourceIdProperty, value); }
        }

        /// <summary>
        /// 资源
        /// </summary>
        public static readonly RefEntityProperty<WipResource> WipResourceProperty =
            P<DispatchTaskViewModel>.RegisterRef(e => e.WipResource, WipResourceIdProperty);

        /// <summary>
        /// 资源
        /// </summary>
        public WipResource WipResource
        {
            get { return this.GetRefEntity(WipResourceProperty); }
            set { this.SetRefEntity(WipResourceProperty, value); }
        }
        #endregion

        #region 工作中心编码 WorkCenterCode
        /// <summary>
        /// 工作中心编码
        /// </summary>
        [Label("工作中心编码")]
        public static readonly Property<string> WorkCenterCodeProperty = P<DispatchTaskViewModel>.Register(e => e.WorkCenterCode);

        /// <summary>
        /// 工作中心编码
        /// </summary>
        public string WorkCenterCode
        {
            get { return this.GetProperty(WorkCenterCodeProperty); }
            set { this.SetProperty(WorkCenterCodeProperty, value); }
        }
        #endregion

    }
}
