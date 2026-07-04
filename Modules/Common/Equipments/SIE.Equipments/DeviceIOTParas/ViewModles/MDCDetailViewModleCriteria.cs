using SIE.Domain;
using SIE.Equipments.DeviceIOTParas.Controllers;
using SIE.ObjectModel;
using System;

namespace SIE.Equipments.DeviceIOTParas.ViewModles
{
    /// <summary>
    /// MDC清单查询器
    /// </summary>
    [QueryEntity, Serializable]
    [Label("MDC清单查询器")]
    public class MDCDetailViewModleCriteria : Criteria
    {
        #region 资产编码 AssetsCode
        /// <summary>
        /// 资产编码
        /// </summary>
        [Label("资产编码")]
        public static readonly Property<string> AssetsCodeProperty = P<MDCDetailViewModleCriteria>.Register(e => e.AssetsCode);

        /// <summary>
        /// 资产编码
        /// </summary>
        public string AssetsCode
        {
            get { return this.GetProperty(AssetsCodeProperty); }
            set { this.SetProperty(AssetsCodeProperty, value); }
        }
        #endregion

        #region 型号 MesModel
        /// <summary>
        /// 型号
        /// </summary>
        [Label("型号")]
        public static readonly Property<string> MesModelProperty = P<MDCDetailViewModleCriteria>.Register(e => e.MesModel);

        /// <summary>
        /// 型号
        /// </summary>
        public string MesModel
        {
            get { return this.GetProperty(MesModelProperty); }
            set { this.SetProperty(MesModelProperty, value); }
        }
        #endregion

        #region mes设备名 MesDeviceName
        /// <summary>
        /// mes设备名
        /// </summary>
        [Label("mes设备名")]
        public static readonly Property<string> MesDeviceNameProperty = P<MDCDetailViewModleCriteria>.Register(e => e.MesDeviceName);

        /// <summary>
        /// mes设备名
        /// </summary>
        public string MesDeviceName
        {
            get { return this.GetProperty(MesDeviceNameProperty); }
            set { this.SetProperty(MesDeviceNameProperty, value); }
        }
        #endregion

        /// <summary>
        /// 查询
        /// </summary>
        /// <returns></returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<DeviceIOTParaController>().GetMDCDetail(this);
        }
    }
}
